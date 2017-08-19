﻿using System.Collections.Generic;
using System.Diagnostics;
using AGS.Types;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Windows.Forms;
using System;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// The IBuildTarget used for building on Android.
    /// </summary>
    public class BuildTargetAndroid : BuildTargetBase
    {
        public const string ANDROID_DIR = "Android";
        public const string STUDIO_PROJECT_DIR = "Studio";
        public const string RELEASE_DIR = "Release";
        public const string TMP_DATA_DIR = "data";
        public const string LIBS_DIR = "libs";

        private static BuildTargetAndroid _instance;

        /// <summary>
        /// Returns a Singleton instance of this class to give other classes access to
        /// GetCompiledPath.
        /// </summary>
        public static BuildTargetAndroid Instance
        {
            get
            {
                return _instance == null ? _instance = new BuildTargetAndroid() : _instance;
            }
        }

        private BuildTargetAndroid()
        {
        }

        /// <summary>
        /// Displays the wizard to collect metadata needed to build for Android.
        /// </summary>
        private bool CollectMetadata(CompileMessages errors)
        {
            AndroidBuilderPane.Instance.CheckForErrors();
            if (AndroidBuilderPane.Errors.Count == 0)
                return true;
            foreach (string error in AndroidBuilderPane.Errors)
            {
                errors.Add(new CompileError("Android build error: " + error));
            }
            AndroidBuilderPlugin.AGSEditor.GUIController.AddOrShowPane(AndroidBuilderPlugin.Pane);
            return false;
        }

        /// <summary>
        /// Create the OBB file from the game's data.
        /// </summary>
        private bool CreateObb(CompileMessages errors)
        {
            if (AndroidBuilderPlugin.AGS_VERSION_CURRENT < AndroidBuilderPlugin.AGS_VERSION_341)
            {
                Directory.CreateDirectory(AndroidMetadata.ObbInputDirectory);
                string[] files = Directory.GetFiles(EDITOR_OUTPUT_DIRECTORY);
                foreach (string file in files)
                {
                    string shortName = Path.GetFileName(file);
                    Utility.HardlinkOrCopy(Path.Combine(AndroidMetadata.ObbInputDirectory, shortName), file, true);
                }
            }
            if (File.Exists(AndroidMetadata.AltObbFilePath)) // if a previous OBB exists that WON'T be overwritten, delete it
            {
                File.Delete(AndroidMetadata.AltObbFilePath);
            }
            string obbPath = AndroidMetadata.ObbFilePath;
            Directory.CreateDirectory(Path.GetDirectoryName(obbPath)); // make sure output folder for OBB exists
            // copy jobb.jar to a temporary file (it's very small)
            string jar = Path.GetTempFileName();
            File.WriteAllBytes(jar, Properties.Resources.jobb);
            StringBuilder args = new StringBuilder("-jar "); // arguments for java command line (see jobb docs/help text)
            args.Append(jar)
                .Append(" -d ").Append(Utility.Quote(AndroidMetadata.ObbInputDirectory))
                .Append(" -o ").Append(Utility.Quote(obbPath))
                .Append(" -pn ").Append(AndroidMetadata.PackageName)
                .Append(" -pv ").Append(AndroidMetadata.ObbVersion)
                .Append(" -v");
            if (AndroidMetadata.ObbPassword != "@null") // if we have an OBB password
            {
                args.Append(" -k ").Append(AndroidMetadata.ObbPassword);
            }
            using (Process proc = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "cmd.exe", // invoke this from cmd.exe so we can pause (temporary solution until better way of handling errors and output is found)
                    Arguments = "/C \"java " + args.ToString() + "\" & pause",
                    CreateNoWindow = false // display window for user feedback in case of errors
                }
            })
            {
                proc.Start();
                proc.WaitForExit();
                // TODO: check process return code
            }
            if (AndroidBuilderPlugin.AGS_VERSION_CURRENT < AndroidBuilderPlugin.AGS_VERSION_341)
            {
                try
                {
                    Directory.Delete(AndroidMetadata.ObbInputDirectory);
                }
                catch
                {
                    try
                    {
                        Directory.Delete(AndroidMetadata.ObbInputDirectory);
                    }
                    catch
                    {
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Extract launcher icons from zip to disk.
        /// </summary>
        private bool ExtractLauncherIcons(CompileMessages errors)
        {
            if (string.IsNullOrEmpty(AndroidMetadata.LauncherIconsZip))
            {
                errors.Add(new CompileError("Launcher icons must be provided to build for Android. Consider using the Android Asset Studio if you need to generate launcher icons."));
                return false;
            }
            string rootDir = GetCompiledPath(STUDIO_PROJECT_DIR, "app", "src", "main");
            using (ZipStorer zip = ZipStorer.Open(AndroidMetadata.LauncherIconsZip, FileAccess.Read))
            {
                List<ZipStorer.ZipFileEntry> files = zip.ReadCentralDir();
                foreach (ZipStorer.ZipFileEntry entry in files)
                {
                    zip.ExtractFile(entry, Path.Combine(rootDir, entry.FilenameInZip));
                }
            }
            return true;
        }

        private bool CopyLibrariesToAndroidStudioProject(CompileMessages errors)
        {
            foreach (KeyValuePair<string, string> kvPair in GetRequiredLibraryPaths())
            {
                string library = kvPair.Value.EndsWith(kvPair.Key) ? kvPair.Value : Path.Combine(kvPair.Value, kvPair.Key);
                try
                {
                    string destPath = GetCompiledPath(STUDIO_PROJECT_DIR, LIBS_DIR, kvPair.Key);
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    File.Copy(library, destPath, true);
                }
                catch (System.Exception e)
                {
                    errors.Add(new CompileError("Error copying Android library file '" + kvPair.Key + "':" + System.Environment.NewLine + e.Message));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Invokes the gradle wrapper to build the Android Studio project.
        /// </summary>
        private bool BuildGradle(CompileMessages errors)
        {
            string localProperties = GetCompiledPath(STUDIO_PROJECT_DIR, "local.properties");
            if (!File.Exists(localProperties)) // make sure we have a local.properties file first
            {
                File.WriteAllText(localProperties, "sdk.dir=" + AndroidMetadata.SdkDir.Replace("\\", "\\\\"));
            }
            TemplateFile.WriteDirtyOrMissingFiles(); // write any dirty or missing template files
            if (!CopyLibrariesToAndroidStudioProject(errors))
            {
                return false;
            }
            // start gradle command
            using (Process proc = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "cmd.exe",
                    Arguments = "/C gradlew.bat nativeLibsToJar & gradlew.bat assembleRelease & pause",
                    CreateNoWindow = false,
                    WorkingDirectory = GetCompiledPath(STUDIO_PROJECT_DIR)
                }
            })
            {
                proc.Start();
                proc.WaitForExit();
                // TODO: check process return code
            }
            return true;
        }

        /// <summary>
        /// Deploy the APK to RELEASE_DIR.
        /// </summary>
        private bool DeployApk(CompileMessages errors)
        {
            string apk = GetCompiledPath(STUDIO_PROJECT_DIR, "app", "build", "outputs", "apk", "app-release.apk");
            if (File.Exists(apk))
            {
                File.Copy(apk, GetCompiledPath(RELEASE_DIR, AndroidMetadata.APKName), true);
                return true;
            }
            else
            {
                errors.Add(new CompileError("APK not found in expected location! Do not move or delete files during building!"));
                return false;
            }
        }

        public override bool Build(CompileMessages errors, bool forceRebuild)
        {
            return base.Build(errors, forceRebuild) && CollectMetadata(errors) && CreateObb(errors) &&
                ExtractLauncherIcons(errors) && BuildGradle(errors) && DeployApk(errors);
        }

        public override IDictionary<string, string> GetRequiredLibraryPaths()
        {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            string[] libs =
            {
                "libags_parallax.so",
                "libags_snowrain.so",
                "libagsblend.so",
                "libagsengine.so",
                "libagsflashlight.so",
                "libagslua.so",
                "libagsspritefont.so",
                "libpe.so"
            };
            string androidDir = Path.Combine(AndroidBuilderPlugin.AGSEditorDirectory, ANDROID_DIR);
            foreach (string arch in new string[] { "armeabi", "armeabi-v7a", "mips", "x86" })
            {
                foreach (string lib in libs)
                {
                    paths.Add(Path.Combine(arch, lib), androidDir);
                }
            }
            return paths;
        }

        public override bool IsAvailable
        {
            get
            {
                return base.IsAvailable && AndroidMetadata.HasJDK && (!string.IsNullOrEmpty(AndroidMetadata.SdkDir));
            }
        }

        public override string Name
        {
            get
            {
                return "Android";
            }
        }

        public override string OutputDirectory
        {
            get
            {
                return ANDROID_DIR;
            }
        }

        public override string[] GetPlatformStandardSubfolders()
        {
            return new string[] { GetCompiledPath(STUDIO_PROJECT_DIR), GetCompiledPath(RELEASE_DIR) };
        }
    }
}
