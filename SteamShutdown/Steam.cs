using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SteamShutdown
{
    public static partial class Steam
    {
        static readonly Regex singleLine = new Regex("^(\\t+\".+\")\\t\\t(\".*\")$", RegexOptions.Compiled);
        static readonly Regex startOfObject = new Regex("^\\t+\".+\"$", RegexOptions.Compiled);

        public static List<App> Apps { get; private set; } = new List<App>();

        static List<FileSystemWatcher> fswList;

        const string STEAM_REG_VALUE = "InstallPath";

        static Steam()
        {
            string steamRegistryPath = GetSteamRegistryPath();
            var rg = Registry.LocalMachine.OpenSubKey(steamRegistryPath, true);
            string installationPath = rg.GetValue(STEAM_REG_VALUE, null) as string;
            if (installationPath == null)
            {
                MessageBox.Show("Steam is not installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            SteamShutdown.Log("Installation path: " + installationPath);

            if (!Directory.Exists(installationPath))
            {
                var key = Path.Combine(steamRegistryPath, STEAM_REG_VALUE);

                // TODO: change to YesNoCancel
                DialogResult mb = MessageBox.Show("Seems a registry value is wrong, probably because of moving Steam to another location." + Environment.NewLine
                    + $"I can try to fix that for you. For that I will delete this registry value: {key}" + Environment.NewLine
                    + "You have to restart Steam afterwards since this will set the correct value for this registry value." + Environment.NewLine
                    + Environment.NewLine
                    + "If you click \"Yes\", the registry value will be deleted and SteamShutdown closed. Then restart Steam first before opening SteamShutdown again." + Environment.NewLine
                    + "If you click \"No\", you can select the installation path by yourself.",
                    "Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (mb == DialogResult.Yes)
                {
                    rg.DeleteValue("InstallPath");
                    Environment.Exit(0);
                }

                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Your steam folder could not be automatically detected."
                    + Environment.NewLine
                    + "Please select the root of your steam folder."
                    + Environment.NewLine
                    + "Example: " + @"C:\Program Files (x86)\Steam";
                DialogResult re = fbd.ShowDialog();
                if (re != DialogResult.OK) return;
                installationPath = fbd.SelectedPath;
            }

            rg.Close();

            string[] libraryPaths = GetLibraryPaths(installationPath);
            SteamShutdown.Log("Detected library paths: " + string.Join(", ", libraryPaths));
            if (libraryPaths.Length == 0)
            {
                MessageBox.Show("No game library found." + Environment.NewLine + "This might appear if Steam has been installed on this machine but was uninstalled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            UpdateAppInfos(libraryPaths);

            fswList = new List<FileSystemWatcher>(libraryPaths.Length);
            foreach (string libraryFolder in libraryPaths)
            {
                FileSystemWatcher fsw = new FileSystemWatcher(libraryFolder, "*.acf");
                fsw.Disposed += Fsw_Disposed;
                fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.Size;
                fsw.Changed += Fsw_Changed;
                fsw.Created += Fsw_Created;
                fsw.Deleted += Fsw_Deleted;
                fsw.Renamed += Fsw_Renamed;
                fsw.Error += Fsw_Error;
                fsw.EnableRaisingEvents = true;
                fswList.Add(fsw);

                SteamShutdown.Log("Created FileSystemWatcher for " + libraryFolder);
            }
        }

        private static void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            SteamShutdown.Log("Fsw_Renamed: " + e.OldFullPath + " to " + e.FullPath + " ChangeType: " + e.ChangeType);

            /*
             * Steam does not change the files anymore, but creates a .acf.tmp file and overwrites the old .acf file by
             * renaming the .acf.tmp file to .acf
             */
            UpdateAppInfo(e.FullPath);
        }

        private static void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            SteamShutdown.Log("Fsw_Created: " + e.FullPath + " ChangeType: " + e.ChangeType);
        }

        private static void Fsw_Disposed(object sender, EventArgs e)
        {
            SteamShutdown.Log("A FileSystemWatcher was disposed.");
        }

        private static void Fsw_Error(object sender, ErrorEventArgs e)
        {
            SteamShutdown.Log($"Fsw_Error: {e.GetException()}");
        }

        public static int IdFromAcfFilename(string filename)
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            int loc = filenameWithoutExtension.IndexOf('_');
            return int.Parse(filenameWithoutExtension.Substring(loc + 1));
        }

        private static void UpdateAppInfos(IEnumerable<string> libraryPaths)
        {
            SteamShutdown.Log($"UpdateAppInfos for: {string.Join(", ", libraryPaths)}");
            var appInfos = new List<App>();

            foreach (string path in libraryPaths)
            {
                DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo fileInfo in di.EnumerateFiles("*.acf"))
                {
                    // Skip if file is empty
                    if (fileInfo.Length == 0)
                    {
                        SteamShutdown.Log($"UpdateAppInfos: File {fileInfo.FullName} was empty.");
                        continue;
                    }

                    App ai = FileToAppInfo(fileInfo.FullName);
                    if (ai == null)
                    {
                        SteamShutdown.Log($"UpdateAppInfos: File {fileInfo.FullName} couldn't be parsed properly.");
                        continue;
                    }

                    appInfos.Add(ai);
                }
            }


            Apps = appInfos.OrderBy(x => x.Name).ToList();
        }

        public static App FileToAppInfo(string filename)
        {
            SteamShutdown.Log($"FileToAppInfo: {filename}");

            string[] content = File.ReadAllLines(filename);

            // Skip if file contains only NULL bytes (this can happen sometimes, example: download crashes, resulting in a corrupted file)
            // or file does not start with \"AppState\".
            if (content.Length <= 1 || !content[0].StartsWith("\"AppState\""))
            {
                SteamShutdown.Log($"FileToAppInfo: {filename} is corrupt.");
                return null;
            }

            string json = AcfToJson(content);
            dynamic stuff;
            try
            {
                stuff = JsonConvert.DeserializeObject(json);
            }
            catch (JsonSerializationException ex)
            {
                SteamShutdown.Log($"FileToAppInfo: Failed to deserialize {filename}");
                SteamShutdown.Log(ex.ToString());
                return null;
            }

            if (stuff == null)
            {
                MessageBox.Show(
                    $"{filename}{Environment.NewLine}contains unexpected content.{Environment.NewLine}This game will be ignored.",
                    "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            App ai = JsonToAppInfo(stuff);
            return ai;
        }

        private static App JsonToAppInfo(dynamic json)
        {
            App newInfo = new App
            {
                ID = int.Parse((json.appid ?? json.appID ?? json.AppID).ToString()),
                Name = json.name ?? json.installdir,
                State = int.Parse(json.StateFlags.ToString())
            };

            return newInfo;
        }

        private static string AcfToJson(string[] acfLines)
        {
            StringBuilder sb = new StringBuilder(acfLines.Length - 1);

            for (int i = 1; i < acfLines.Length; i++)
            {
                Match mSingle = singleLine.Match(acfLines[i]);
                if (mSingle.Success)
                {
                    sb.Append(mSingle.Groups[1].Value);
                    sb.Append(": ");
                    sb.Append(mSingle.Groups[2].Value);

                    // Last value of object must not have a tailing comma
                    if (i + 1 < acfLines.Length && acfLines[i + 1].EndsWith("}"))
                        sb.AppendLine();
                    else
                        sb.AppendLine(",");
                }
                else if (acfLines[i].StartsWith("\t") && acfLines[i].EndsWith("}"))
                {
                    sb.Append(acfLines[i]);

                    if (i + 1 < acfLines.Length && acfLines[i + 1].EndsWith("}"))
                        sb.AppendLine();
                    else
                        sb.AppendLine(",");
                }
                else if (startOfObject.IsMatch(acfLines[i]))
                {
                    sb.Append(acfLines[i]);
                    sb.AppendLine(":");
                }
                else
                {
                    sb.AppendLine(acfLines[i]);
                }
            }

            return sb.ToString();
        }

        public static string[] InstallationPathsFromVdf(string vdfFile)
        {
            var paths = new List<string>();

            string json = AcfToJson(File.ReadAllLines(vdfFile));

            dynamic rootNode = JsonConvert.DeserializeObject(json);

            for (int i = 1; ; i++)
            {
                dynamic pathNode = rootNode[i.ToString()];

                if (pathNode == null) break;

                if (pathNode.Type != JTokenType.String)
                {
                    // New format
                    // Valve introduced a new format for the "libraryfolders.vdf" file
                    // In the new format, the node "1" not only contains a single value (the path),
                    // but multiple values: path, label, mounted, contentid

                    // If a library folder is removed in the Steam settings, the path persists, but its 'mounted' value is set to 0 (disabled)
                    // We consider only the value '1' as that the path is actually enabled.  If this key is not there, it's semantic is that it is mounted.
                    dynamic mountedNode = pathNode["mounted"];
                    if (mountedNode != null && mountedNode.ToString() != "1")
                        continue;

                    pathNode = pathNode["path"];
                }

                string path = Path.Combine(pathNode.ToString(), "SteamApps");
                paths.Add(path);
            }

            return paths.ToArray();
        }

        private static string[] GetLibraryPaths(string installationPath)
        {
            var paths = new List<string>()
                {
                    Path.Combine(installationPath, "SteamApps")
                };

            string libraryFoldersPath = Path.Combine(installationPath, "SteamApps", "libraryfolders.vdf");
            if (!File.Exists(libraryFoldersPath))
                return paths.ToArray();

            string[] readLibraryFolderPaths = InstallationPathsFromVdf(libraryFoldersPath);
            string[] existingLibraryFolderPaths = readLibraryFolderPaths.Where(Directory.Exists).ToArray();
            paths.AddRange(existingLibraryFolderPaths);
            return paths.ToArray();
        }

        private static string GetSteamRegistryPath()
        {
            string start = @"SOFTWARE\";
            if (Environment.Is64BitOperatingSystem)
            {
                start += @"Wow6432Node\";
            }

            return start + @"Valve\Steam";
        }
    }
}
