using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace GuideToF20
{
    internal static class StartupManager
    {
        private const string AppName = "Guide Nvidia";

        public static bool IsEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                false);

            return key?.GetValue(AppName) != null;
        }

        public static void SetEnabled(bool enabled)
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                true);

            if (key == null)
                return;

            if (enabled)
            {
                string exePath = Application.ExecutablePath;
                key.SetValue(AppName, $"\"{exePath}\"");
            }
            else
            {
                key.DeleteValue(AppName, false);
            }
        }
    }
}