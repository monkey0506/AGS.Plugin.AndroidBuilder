using System;
using System.Diagnostics;
using System.IO;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Miscellaneous utility methods.
    /// </summary>
    public static class Utility
    {
        private static string androidSdkUniqueFile = Path.Combine("platform-tools", "adb.exe"); // look for adb in Android folders

        /// <summary>
        /// Delegate for a method which takes no parameters and returns a result.
        /// From .NET 3.5, this can be replaced with System.Func&lt;TResult&gt;.
        /// </summary>
        public delegate TResult Func<TResult>();

        /// <summary>
        /// Returns whether an Android SDK installation is found at the given path.
        /// </summary>
        public static bool IsAndroidSdkPath(string path)
        {
            return File.Exists(Path.Combine(path, androidSdkUniqueFile));
        }

        /// <summary>
        /// Attempt to look up Android SDK installation in standard folders, and return SDK path.
        /// </summary>
        public static string FindAndroidSdkPath()
        {
            string[] searchDirs =
            {
                // User/AppData/Local
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                // Program Files
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                // Program Files (x86) (it's okay if we're on 32-bit, we check if this folder exists first)
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + " (x86)",
                // User/AppData/Roaming
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            };
            foreach (string searchDir in searchDirs)
            {
                string androidDir = Path.Combine(searchDir, "Android");
                if (Directory.Exists(androidDir))
                {
                    // pattern may match "Sdk", "Sdk1", "android-sdk", or others
                    string[] subDirs = Directory.GetDirectories(androidDir, "*sdk*", SearchOption.TopDirectoryOnly);
                    foreach (string subDir in subDirs)
                    {
                        if (IsAndroidSdkPath(subDir))
                            return subDir;
                    }
                }
            }
            // no luck finding SDK! :(
            return null;
        }

        /// <summary>
        /// Find whether a command is present in PATH or other environment variable locations.
        /// </summary>
        public static bool FindCmd(string cmd, out bool hasCmd)
        {
            using (Process proc = new Process()
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "where.exe", // invoke Windows built-in 'where' command for lookup
                    Arguments = cmd,
                    CreateNoWindow = true, // hide window
                    RedirectStandardOutput = true // get response from command line
                }
            })
            {
                string path = null;
                try
                {
                    proc.Start();
                    path = proc.StandardOutput.ReadLine(); // read command line output
                }
                catch
                {
                }
                // If command is not found, prints error message such as:
                //
                //      INFO: Could not find files for the given pattern(s).
                //
                // Otherwise, we should have a path.
                return hasCmd = ((!string.IsNullOrEmpty(path)) && (!path.StartsWith("INFO: ")));
            }
        }

        /// <summary>
        /// Helper to quote paths.
        /// </summary>
        public static string Quote(string input)
        {
            return '"' + input + '"';
        }
    }
}
