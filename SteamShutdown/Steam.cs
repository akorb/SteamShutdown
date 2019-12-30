using Microsoft.Win32;
using Newtonsoft.Json;
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
        static string InstallationPath;
        static string[] LibraryPaths;

        static Regex singleLine = new Regex("^(\\t+\".+\")\\t\\t(\".*\")$", RegexOptions.Compiled);
        static Regex startOfObject = new Regex("^\\t+\".+\"$", RegexOptions.Compiled);

        public static List<App> Apps { get; private set; } = new List<App>();
        public static List<App> SortedApps => Apps.OrderBy(x => x.Name).ToList();

        static Steam()
        {
            InstallationPath = Registry.GetValue(GetSteamRegistryPath(), "InstallPath", "") as string;
            if (InstallationPath == null)
            {
                MessageBox.Show("Steam is not installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(InstallationPath))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Your steam folder could not be automatically detected."
                    + Environment.NewLine
                    + "Please select the root of your steam folder."
                    + Environment.NewLine
                    + "Example: " + @"C:\Program Files (x86)\Steam";
                if (fbd.ShowDialog() != DialogResult.OK) return;
                InstallationPath = fbd.SelectedPath;
            }

            LibraryPaths = GetLibraryPaths();
            if (LibraryPaths.Length == 0)
            {
                MessageBox.Show("No game library found.\r\nThis might appear if Steam has been installed on this machine but was uninstalled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateAppInfos();

            foreach (string libraryFolder in LibraryPaths)
            {
                var fsw = new FileSystemWatcher(libraryFolder, "*.acf");
                fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                fsw.Changed += Fsw_Changed;
                fsw.Deleted += Fsw_Deleted;
                fsw.EnableRaisingEvents = true;
            }
        }


        public static int IdFromAcfFilename(string filename)
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            int loc = filenameWithoutExtension.IndexOf('_');
            return int.Parse(filenameWithoutExtension.Substring(loc + 1));
        }

        private static void UpdateAppInfos()
        {
            var appInfos = new List<App>();

            foreach (string path in LibraryPaths)
            {
                DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo fileInfo in di.EnumerateFiles("*.acf"))
                {
                    // Skip if file is empty
                    if (fileInfo.Length == 0) continue;

                    App ai = FileToAppInfo(fileInfo.FullName);
                    if (ai == null) continue;

                    appInfos.Add(ai);
                }
            }

            Apps = appInfos;
        }

        public static App FileToAppInfo(string filename)
        {
            string[] content = File.ReadAllLines(filename);

            // Skip if file contains only NULL bytes (this can happen sometimes, example: download crashes, resulting in a corrupted file)
            if (content.Length == 1 && string.IsNullOrWhiteSpace(content[0].TrimStart('\0'))) return null;

            string json = AcfToJson(content);
            dynamic stuff = JsonConvert.DeserializeObject(json);

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

        private static string[] GetLibraryPaths()
        {
            List<string> paths = new List<string>()
                {
                    Path.Combine(InstallationPath, "SteamApps")
                };

            string libraryFoldersPath = Path.Combine(InstallationPath, "SteamApps", "libraryfolders.vdf");

            string json = AcfToJson(File.ReadAllLines(libraryFoldersPath));


            dynamic stuff = JsonConvert.DeserializeObject(json);

            for (int i = 1; ; i++)
            {
                dynamic path = stuff[i.ToString()];

                if (path == null) break;
                paths.Add(Path.Combine(path.ToString(), "SteamApps"));
            }

            return paths.ToArray();
        }

        private static string GetSteamRegistryPath()
        {
            const string start = @"HKEY_LOCAL_MACHINE\SOFTWARE\";
            if (Environment.Is64BitOperatingSystem)
            {
                return start + @"Wow6432Node\Valve\Steam";
            }

            // 32 bit
            return start + @"Valve\Steam";
        }
    }
}
