using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SteamShutdown
{
    public static partial class Steam
    {
        static string InstallationPath;
        static string DownloadingPath;
        static string[] LibraryPaths;

        public static List<AppInfo> Apps { get; private set; }

        static Steam()
        {
            InstallationPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null).ToString();
            DownloadingPath = Path.Combine(InstallationPath, "SteamApps", "downloading");
            LibraryPaths = GetLibraryPaths();

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

                    dynamic stuff = JsonConvert.DeserializeObject(json);

                    AppInfo ai = JsonToAppInfo(stuff);

                    appInfos.Add(ai);
                }
            }

            Apps = appInfos;
        }

        private static AppInfo JsonToAppInfo(dynamic json)
        {
            AppInfo newInfo = new AppInfo();
            newInfo.ID = int.Parse(json.appID.ToString());
            newInfo.Name = json.name ?? json.installdir;
            newInfo.State = int.Parse(json.StateFlags.ToString());

            if (json.StateFlags == 1062)
            {
                newInfo.State = 1026;
            }

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

            return String.Join("", acfLines);
        }

        private static string[] GetLibraryPaths()
        {
            List<String> paths = new List<string>()
                {
                    Path.Combine(InstallationPath, "SteamApps")
                };

            String libraryFoldersPath = Path.Combine(InstallationPath, "SteamApps", "libraryfolders.vdf");

            String json = AcfToJson(File.ReadAllLines(libraryFoldersPath).ToList());


            dynamic stuff = JsonConvert.DeserializeObject(json);

            for (int i = 1; true; i++)
            {
                dynamic path = stuff[i.ToString()];

                if (path == null) break;
                paths.Add(Path.Combine(path.ToString(), "SteamApps"));
            }

            return paths.ToArray();
        }
    }
}
