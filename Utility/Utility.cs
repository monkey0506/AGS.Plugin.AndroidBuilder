using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

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
            // no luck finding SDK! try searching environment PATH
            return FindExePath(androidSdkUniqueFile);
        }

        /// <summary>
        /// Expands environment variables and, if unqualified, locates the exe in the working directory
        /// or the evironment's path.
        /// </summary>
        /// <param name="exe">The name of the executable file</param>
        /// <returns>The fully-qualified path to the file, or null.</returns>
        public static string FindExePath(string exe)
        {
            exe = Environment.ExpandEnvironmentVariables(exe);
            if (!File.Exists(exe))
            {
                if (Path.GetDirectoryName(exe) == "")
                {
                    foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';'))
                    {
                        string path = test.Trim();
                        if (!string.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
                        {
                            return Path.GetFullPath(path);
                        }
                    }
                }
                return null;
            }
            return Path.GetFullPath(exe);
        }

        /// <summary>
        /// Helper to quote paths.
        /// </summary>
        public static string Quote(string input)
        {
            return '"' + input + '"';
        }

        /// <summary>
        /// Copy the contents of one directory to another. (Borrowed and modified from MSDN)
        /// </summary>
        public static void DirectoryCopy(string src, string dest, bool copySubdirectories)
        {
            DirectoryInfo srcDir = new DirectoryInfo(src);
            if (!srcDir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + src);
            }
            // If the destination directory doesn't exist, create it.
            Directory.CreateDirectory(dest);
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = srcDir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(dest, file.Name), false);
            }
            // If copying subdirectories, copy them and their contents to new location.
            if (copySubdirectories)
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo[] subdirs = srcDir.GetDirectories();
                foreach (DirectoryInfo subdir in subdirs)
                {
                    DirectoryCopy(subdir.FullName, Path.Combine(dest, subdir.Name), copySubdirectories);
                }
            }
        }

        #region Borrowed from AGS.Editor.Utilities

        private const int ERROR_NO_MORE_FILES = 18;

        public static bool IsMonoRunning()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        /// <summary>
        /// Wraps Directory.GetFiles in a handler to deal with an exception
        /// erroneously being thrown on Linux network shares if no files match.
        /// </summary>
        public static string[] GetDirectoryFileList(string directory, string fileMask)
        {
            return GetDirectoryFileList(directory, fileMask, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Wraps Directory.GetFiles in a handler to deal with an exception
        /// erroneously being thrown on Linux network shares if no files match.
        /// </summary>
        public static string[] GetDirectoryFileList(string directory, string fileMask, SearchOption searchOption)
        {
            try
            {
                return Directory.GetFiles(directory, fileMask, searchOption);
            }
            catch (IOException)
            {
                if (Marshal.GetLastWin32Error() == ERROR_NO_MORE_FILES)
                {
                    // On a network share the Framework can throw this if
                    // there are no matching files (reported by RickJ)...
                    // Seems to be a Win32 FindFirstFile bug in certain
                    // circumstances.
                    return new string[0];
                }
                throw;
            }
        }

        /// <summary>
        /// Sets security permissions for a specific file.
        /// </summary>
        public static void SetFileAccess(string fileName, SecurityIdentifier sid, FileSystemRights rights, AccessControlType type)
        {
            try
            {
                FileSecurity fsec = File.GetAccessControl(fileName);
                fsec.AddAccessRule(new FileSystemAccessRule(sid, rights, type));
                File.SetAccessControl(fileName, fsec);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public static void SetDirectoryFilesAccess(string directory)
        {
            SetDirectoryFilesAccess(directory, new SecurityIdentifier(WellKnownSidType.WorldSid, null));
        }

        public static void SetDirectoryFilesAccess(string directory, SecurityIdentifier sid)
        {
            SetDirectoryFilesAccess(directory, sid, FileSystemRights.Modify);
        }

        public static void SetDirectoryFilesAccess(string directory, SecurityIdentifier sid, FileSystemRights rights)
        {
            SetDirectoryFilesAccess(directory, sid, rights, AccessControlType.Allow);
        }

        /// <summary>
        /// Sets security permissions for all files in a directory.
        /// </summary>
        public static void SetDirectoryFilesAccess(string directory, SecurityIdentifier sid, FileSystemRights rights, AccessControlType type)
        {
            try
            {
                // first we will attempt to take ownership of the file
                // this ensures (if successful) that all users can change security
                // permissions for the file without admin access
                ProcessStartInfo si = new ProcessStartInfo("cmd.exe");
                si.RedirectStandardInput = false;
                si.RedirectStandardOutput = false;
                si.RedirectStandardError = false;
                si.UseShellExecute = false;
                si.Arguments = string.Format("/c takeown /f \"{0}\" /r /d y", directory);
                si.CreateNoWindow = true;
                si.WindowStyle = ProcessWindowStyle.Hidden;
                if (IsMonoRunning())
                {
                    si.FileName = "chown";
                    si.Arguments = string.Format("-R $USER:$USER \"{0}\"", directory);
                }
                Process process = Process.Start(si);
                bool result = (process != null);
                if (result)
                {
                    process.EnableRaisingEvents = true;
                    process.WaitForExit();
                    if (process.ExitCode != 0) return;
                    process.Close();
                    // after successfully gaining ownership, parse each file in the
                    // directory (recursively) and update its access rights
                    foreach (string filename in GetDirectoryFileList(directory, "*", SearchOption.AllDirectories))
                    {
                        SetFileAccess(filename, sid, rights, type);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public static bool IsWindowsXPOrHigher()
        {
            OperatingSystem os = Environment.OSVersion;
            // Windows XP reports platform as Win32NT and version number >= 5.1
            return ((os.Platform == PlatformID.Win32NT) && ((os.Version.Major > 5) || ((os.Version.Major == 5) && (os.Version.Minor == 1))));
        }

        public static bool IsWindowsVistaOrHigher()
        {
            OperatingSystem os = Environment.OSVersion;
            // Windows Vista reports platform as Win32NT and version number >= 6
            return ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 6));
        }

        /// <summary>
        /// Creates hardlink, if failed then creates file's copy.
        /// </summary>
        /// <param name="destFileName">Destination file path, name of the created hardlink</param>
        /// <param name="sourceFileName">Source file path, what hardlink to</param>
        /// <param name="overwrite">Whether overwrite existing hardlink or not</param>
        /// <returns></returns>
        public static bool HardlinkOrCopy(string destFileName, string sourceFileName, bool overwrite)
        {
            bool res = CreateHardLink(destFileName, sourceFileName, overwrite);
            if (!res)
                File.Copy(sourceFileName, destFileName, overwrite);
            return res;
        }

        /// <summary>
        /// Creates hardlink using operating system utilities.
        /// </summary>
        /// <param name="destFileName">Destination file path, name of the created hardlink</param>
        /// <param name="sourceFileName">Source file path, what hardlink to</param>
        /// <param name="overwrite">Whether overwrite existing hardlink or not</param>
        /// <returns></returns>
        public static bool CreateHardLink(string destFileName, string sourceFileName, bool overwrite)
        {
            if (File.Exists(destFileName))
            {
                if (overwrite) File.Delete(destFileName);
                else return false;
            }
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (Path.GetFileName(destFileName).IndexOfAny(invalidFileNameChars) != -1)
            {
                throw new ArgumentException("Cannot create hard link! Invalid destination file name. (" + destFileName + ")");
            }
            if (Path.GetFileName(sourceFileName).IndexOfAny(invalidFileNameChars) != -1)
            {
                throw new ArgumentException("Cannot create hard link! Invalid source file name. (" + sourceFileName + ")");
            }
            if (!File.Exists(sourceFileName))
            {
                throw new FileNotFoundException("Cannot create hard link! Source file does not exist. (" + sourceFileName + ")");
            }
            ProcessStartInfo si = new ProcessStartInfo("cmd.exe");
            si.RedirectStandardInput = false;
            si.RedirectStandardOutput = false;
            si.RedirectStandardError = false;
            si.UseShellExecute = false;
            si.Arguments = string.Format("/c mklink /h \"{0}\" \"{1}\"", destFileName, sourceFileName);
            si.CreateNoWindow = true;
            si.WindowStyle = ProcessWindowStyle.Hidden;
            if ((!IsWindowsVistaOrHigher()) && (IsWindowsXPOrHigher())) // running Windows XP
            {
                si.Arguments = string.Format("/c fsutil hardlink create \"{0}\" \"{1}\"", destFileName, sourceFileName);
            }
            if (IsMonoRunning())
            {
                si.FileName = "ln";
                si.Arguments = string.Format("\"{0}\" \"{1}\"", sourceFileName, destFileName);
            }
            Process process = Process.Start(si);
            bool result = (process != null);
            if (result)
            {
                process.EnableRaisingEvents = true;
                process.WaitForExit();
                result = process.ExitCode == 0;
                process.Close();
            }
            return result;
        }

        #endregion // Borrowed from AGS.Editor.Utilities
    }
}
