using Microsoft.Win32;
using System;
using System.Drawing;

namespace SteamShutdown
{
    public static class ColorUtils
    {
        public static bool SystemUsesLightTheme()
        {
            int sult = (Int32)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);
            return sult == 1;
        }

        public static Color GetAccentColor()
        {
            const String DWM_KEY = @"Software\Microsoft\Windows\DWM";
            using (RegistryKey dwmKey = Registry.CurrentUser.OpenSubKey(DWM_KEY, RegistryKeyPermissionCheck.ReadSubTree))
            {
                const String KEY_EX_MSG = "The \"HKCU\\" + DWM_KEY + "\" registry key does not exist.";
                if (dwmKey is null) throw new InvalidOperationException(KEY_EX_MSG);

                Object accentColorObj = dwmKey.GetValue("AccentColor");
                if (accentColorObj is Int32 accentColorDword)
                {
                    return ParseDWordColor(accentColorDword);
                }
                else
                {
                    const String VALUE_EX_MSG = "The \"HKCU\\" + DWM_KEY + "\\AccentColor\" registry key value could not be parsed as an ABGR color.";
                    throw new InvalidOperationException(VALUE_EX_MSG);
                }
            }
        }

        private static Color ParseDWordColor(Int32 color)
        {
            return Color.FromArgb((color >> 24) & 0xFF, (color >> 0) & 0xFF, (color >> 8) & 0xFF, (color >> 16) & 0xFF);
        }
    }
}
