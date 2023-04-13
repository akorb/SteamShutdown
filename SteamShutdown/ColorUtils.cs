using Microsoft.Win32;
using System;
using System.Drawing;

namespace SteamShutdown
{
    /// <summary>
    /// The ColorUtils class is used to determine two things: the system theme (light/dark) and the system accent color.
    /// This information can later be used to style the context menu base on the users preferences.
    /// </summary>
    public static class ColorUtils
    {
        public static bool SystemUsesLightTheme()
        {
            int sult = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);
            return sult == 1;
        }

        public static Color GetAccentColor()
        {
            const string DWM_KEY = @"Software\Microsoft\Windows\DWM";
            using (RegistryKey dwmKey = Registry.CurrentUser.OpenSubKey(DWM_KEY, RegistryKeyPermissionCheck.ReadSubTree))
            {
                if (dwmKey is null)
                {
                    const string KEY_EX_MSG = "The \"HKCU\\" + DWM_KEY + "\" registry key does not exist.";
                    throw new InvalidOperationException(KEY_EX_MSG);
                }

                object accentColorObj = dwmKey.GetValue("AccentColor");
                if (accentColorObj is int accentColorDword)
                {
                    return ParseDWordColor(accentColorDword);
                }
                else
                {
                    const string VALUE_EX_MSG = "The \"HKCU\\" + DWM_KEY + "\\AccentColor\" registry key value could not be parsed as an ARGB color.";
                    throw new InvalidOperationException(VALUE_EX_MSG);
                }
            }
        }

        private static Color ParseDWordColor(int color)
        {
            return Color.FromArgb(
                (color >> 24) & 0xFF,
                (color >> 0) & 0xFF,
                (color >> 8) & 0xFF,
                (color >> 16) & 0xFF);
        }
    }
}
