using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SteamShutdown
{
    public static partial class Steam
    {
        static string InstallationPath;
        static string[] LibraryPaths;

        public static List<AppInfo> Apps { get; private set; } = new List<AppInfo>();

        static Steam()
        {
            InstallationPath = Registry.GetValue(GetSteamRegistryPath(), "InstallPath", "") as string;
            if (InstallationPath == null)
            {
                MessageBox.Show("Steam is not installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
                fsw.Changed += fsw_Changed;
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
            var appInfos = new List<AppInfo>();

            foreach (string path in LibraryPaths)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fileInfos = di.GetFiles("*.acf");

                foreach (FileInfo fileInfo in fileInfos)
                {
                    string json = AcfToJson(File.ReadAllLines(fileInfo.FullName).ToList());

                    if (json.Length > 0)
                    {
                        dynamic stuff = JsonConvert.DeserializeObject(json);
                        
                        AppInfo ai = JsonToAppInfo(stuff);

                        appInfos.Add(ai);
                    }
                }
            }

            Apps = appInfos;
        }

        private static AppInfo JsonToAppInfo(dynamic json)
        {
            AppInfo newInfo = new AppInfo
            {
                ID = int.Parse((json.appid ?? json.appID ?? json.AppID).ToString()),
                Name = json.name ?? json.installdir,
                State = int.Parse(json.StateFlags.ToString())
            };

            return newInfo;
        }

        private static string AcfToJson(List<string> acfLines)
        {
            acfLines.RemoveAt(0);

            for (int i = 0; i < acfLines.Count; i++)
            {
                if (acfLines[i].StartsWith("\t") && acfLines[i].EndsWith("}"))
                    acfLines[i] += ",";

                acfLines[i] = acfLines[i].TrimStart('\t');

                acfLines[i] = acfLines[i].Replace("\t\t", ":");
                if (acfLines[i].Last() == '\"' && !acfLines[i + 1].Contains('{') && !acfLines[i + 1].Contains('}'))
                    acfLines[i] += ",";

                if (i + 1 < acfLines.Count)
                    if (acfLines[i + 1].Contains('{'))
                        acfLines[i] += ":";
            }

            return string.Join("", acfLines);
        }

        private static string[] GetLibraryPaths()
        {
            List<string> paths = new List<string>()
                {
                    Path.Combine(InstallationPath, "SteamApps")
                };

            string libraryFoldersPath = Path.Combine(InstallationPath, "SteamApps", "libraryfolders.vdf");

            string json = AcfToJson(File.ReadAllLines(libraryFoldersPath).ToList());


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
