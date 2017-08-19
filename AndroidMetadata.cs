using System.IO;
using System.Windows.Forms;
using System.Xml;
using static AGS.Plugin.AndroidBuilder.Utility;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Miscellaneous metadata used in deploying for Android.
    /// </summary>
    public static class AndroidMetadata
    {
        private const string TAG_PACKAGE_NAME = "PackageName";
        private const string TAG_VERSION_CODE = "VersionCode";
        private const string TAG_VERSION_NAME = "VersionName";
        private const string TAG_KEY_STORE_PATH = "KeyStorePath";
        private const string TAG_KEY_STORE_PASSWORD = "KeyStorePassword";
        private const string TAG_KEY_STORE_ALIAS = "KeyStoreAlias";
        private const string TAG_KEY_STORE_ALIAS_PASSWORD = "KeyStoreAliasPassword";
        private const string TAG_APP_NAME = "AppName";
        private const string TAG_DATA_FILENAME = "DataFilename";
        private const string TAG_OBB_VERSION = "ObbVersion";
        private const string TAG_OBB_FILE_SIZE = "ObbFileSize";
        private const string TAG_OBB_PASSWORD = "ObbPassword";
        private const string TAG_RSA_PUBLIC_KEY = "RsaPublicKey";
        private const string TAG_PRIVATE_SALT = "PrivateSalt";
        private const string TAG_LAUNCHER_ICON_ZIP = "LauncherIconZip";

        private static bool hasJDK;
        private static string sdkDir;
        private static string packageName = "com.yourcompany.gamename";
        private static int versionCode = 1;
        private static string versionName = "1.0";
        private static string appName = "";
        private static string keyStorePath = "";
        private static string keyStorePassword = "";
        private static string keyStoreAlias = "";
        private static string keyStoreAliasPassword = "";
        private static int obbVersion = 1;
        private static string obbPassword = "@null";
        private static string rsaPublicKey = "@null";
        private static XmlCsvSByteArray privateSalt = XmlCsvSByteArray.CreateEmpty();
        private static string launcherIconsZip = null;

        static AndroidMetadata()
        {
            sdkDir = FindAndroidSdkPath();
            if (sdkDir == null)
                sdkDir = ""; // TODO: wizard to locate Android SDK
            hasJDK = (FindExePath("javac.exe") != null); // presence of 'javac' command indicates JDK installation
            if (!hasJDK)
            {
                MessageBox.Show("JDK not found! Android builder will not be available until the JDK is installed (requires editor restart). Please obtain the JDK from Oracle.com and ensure the JDK's location is in the environment PATH.",
                    "JDK missing!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Whether the user has the JDK installed (required).
        /// </summary>
        public static bool HasJDK
        {
            get { return hasJDK; }
        }

        /// <summary>
        /// Path to the Android SDK. Will search in default installation paths, but
        /// must be provided by user if not found.
        /// </summary>
        public static string SdkDir
        {
            get { return sdkDir; }
        }

        /// <summary>
        /// The APK file name (AppName.apk).
        /// </summary>
        public static string APKName
        {
            get { return AppName + ".apk"; }
        }

        /// <summary>
        /// The Android app's package name.
        /// </summary>
        public static string PackageName
        {
            get { return packageName; }

            internal set
            {
                if (packageName == value)
                    return;
                packageName = value;
                TemplateTag.PACKAGE_NAME.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The Android app version code.
        /// </summary>
        public static int VersionCode
        {
            get { return versionCode; }

            internal set
            {
                if (versionCode == value)
                    return;
                versionCode = value;
                TemplateTag.VERSION_CODE.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The Android app's "friendly" version name.
        /// </summary>
        public static string VersionName
        {
            get { return versionName; }

            internal set
            {
                if (versionName == value)
                    return;
                versionName = value;
                TemplateTag.VERSION_NAME.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The Android app's name.
        /// </summary>
        public static string AppName
        {
            get { return appName; }

            internal set
            {
                if (appName == value)
                    return;
                appName = value;
                TemplateTag.APP_NAME.MarkFilesDirty();
            }
        }

        private static string GameName
        {
            get { return Path.GetFileName(AndroidBuilderPlugin.AGSEditor.CurrentGame.DirectoryPath); }
        }

        /// <summary>
        /// The compiled AGS data file.
        /// </summary>
        public static string DataFileName
        {
            get { return GameName + ".ags"; }
        }

        /// <summary>
        /// The path to the Java key store used to sign the APK.
        /// </summary>
        public static string KeyStorePath
        {
            get { return keyStorePath; }

            internal set
            {
                if (keyStorePath == value)
                    return;
                keyStorePath = value;
                TemplateTag.KEY_STORE_PATH.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The Java key store password.
        /// </summary>
        public static string KeyStorePassword
        {
            get { return keyStorePassword; }

            internal set
            {
                if (keyStorePassword == value)
                    return;
                keyStorePassword = value;
                TemplateTag.KEY_STORE_PASSWORD.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The Java key store alias.
        /// </summary>
        public static string KeyStoreAlias
        {
            get { return keyStoreAlias; }

            internal set
            {
                if (keyStoreAlias == value)
                    return;
                keyStoreAlias = value;
                TemplateTag.KEY_STORE_ALIAS.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The Java key store alias' password.
        /// </summary>
        public static string KeyStoreAliasPassword
        {
            get { return keyStoreAliasPassword; }

            internal set
            {
                if (keyStoreAliasPassword == value)
                    return;
                keyStoreAliasPassword = value;
                TemplateTag.KEY_STORE_ALIAS_PASSWORD.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The version code of the APK expansion file.
        /// </summary>
        public static int ObbVersion
        {
            get { return obbVersion; }

            internal set
            {
                if (obbVersion == value)
                    return;
                obbVersion = value;
                TemplateTag.OBB_VERSION.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The password of the APK expansion file.
        /// </summary>
        public static string ObbPassword
        {
            get { return obbPassword; }

            internal set
            {
                value = string.IsNullOrEmpty(value) ? "@null" : value;
                if (obbPassword == value)
                    return;
                obbPassword = value;
                TemplateTag.OBB_PASSWORD.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The directory where the compiled game data is stored by AGS.
        /// </summary>
        public static string ObbInputDirectory
        {
            get
            {
                if (AndroidBuilderPlugin.AGS_VERSION_CURRENT < AndroidBuilderPlugin.AGS_VERSION_341)
                {
                    return BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.TMP_DATA_DIR);
                }
                return Path.Combine(BuildTargetBase.EDITOR_OUTPUT_DIRECTORY, BuildTargetBase.EDITOR_DATA_OUTPUT_DIRECTORY);
            }
        }

        /// <summary>
        /// The file name of the generated APK expansion file.
        /// </summary>
        public static string ObbFileName
        {
            get { return "main." + ObbVersion + "." + PackageName + ".obb"; }
        }

        /// <summary>
        /// Returns the full path of the APK expansion file. If no RsaPublicKey is provided, then
        /// this path will be in the app's assets folder; otherwise, this path will be in the
        /// release folder.
        /// </summary>
        public static string ObbFilePath
        {
            get
            {
                if (RsaPublicKey == "@null")
                    return BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.STUDIO_PROJECT_DIR, "app", "src", "main", "assets", ObbFileName);
                return BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.RELEASE_DIR, ObbFileName);
            }
        }

        /// <summary>
        /// Returns the full alternate path of the APK expansion file (either in assets or release,
        /// opposite to the ObbFilePath location). This is used to ensure that any old OBB files are
        /// deleted before building the Android Studio project.
        /// </summary>
        public static string AltObbFilePath
        {
            get
            {
                if (RsaPublicKey == "@null")
                    return BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.RELEASE_DIR, ObbFileName);
                return BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.STUDIO_PROJECT_DIR, "app", "src", "main", "assets", ObbFileName);
            }
        }

        /// <summary>
        /// Returns the file size (in bytes) of the APK expansion file.
        /// </summary>
        public static long ObbFileSize
        {
            get
            {
                try
                {
                    // TODO: mark template file dirty if obb file size changes
                    return new FileInfo(ObbFilePath).Length;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The RSA public key given in the Google Play Developer Console ("@null" if none).
        /// </summary>
        public static string RsaPublicKey
        {
            get { return rsaPublicKey; }

            internal set
            {
                value = string.IsNullOrEmpty(value) ? "@null" : value;
                if (rsaPublicKey == value)
                    return;
                rsaPublicKey = value;
                TemplateTag.RSA_PUBLIC_KEY.MarkFilesDirty();
            }
        }

        /// <summary>
        /// List of signed bytes ([-128, 127]) used as an encryption salt.
        /// </summary>
        public static XmlCsvSByteArray PrivateSalt
        {
            get { return privateSalt; }

            internal set
            {
                if (privateSalt.Equals(value))
                    return;
                if (!privateSalt.IsEmpty)
                {
                    DialogResult result = MessageBox.Show("You are about to change your private salt. Once your APK has been uploaded to Google Play, you should not change this value. Are you sure you want to change your private salt?",
                        "Really change salt?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                        return;
                }
                privateSalt = value;
                TemplateTag.PRIVATE_SALT.MarkFilesDirty();
            }
        }

        /// <summary>
        /// The path to a zip archive containing the app's launcher icons.
        /// </summary>
        public static string LauncherIconsZip
        {
            get { return launcherIconsZip; }

            internal set
            {
                launcherIconsZip = value;
            }
        }

        private static void ReadPrivateSettings()
        {
            string localStaticPropertiesPath = BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.STUDIO_PROJECT_DIR, "local.static.properties");
            if (File.Exists(localStaticPropertiesPath))
            {
                string[] lines = File.ReadAllLines(localStaticPropertiesPath);
                int pos;
                string rol;
                foreach (string line in lines)
                {
                    pos = line.IndexOf('=');
                    if (pos == -1)
                        continue;
                    rol = pos == (line.Length - 1) ? "" : line.Substring(pos + 1);
                    if (line.StartsWith("storeFile"))
                    {
                        if (rol == TemplateTag.KEY_STORE_PATH.Tag)
                            continue;
                        keyStorePath = rol.Replace("\\\\", "\\");
                    }
                    else if (line.StartsWith("storePassword"))
                    {
                        if (rol == TemplateTag.KEY_STORE_PASSWORD.Tag)
                            continue;
                        keyStorePassword = rol;
                    }
                    else if (line.StartsWith("keyAlias"))
                    {
                        if (rol == TemplateTag.KEY_STORE_ALIAS.Tag)
                            continue;
                        keyStoreAlias = rol;
                    }
                    else if (line.StartsWith("keyPassword"))
                    {
                        if (rol == TemplateTag.KEY_STORE_ALIAS_PASSWORD.Tag)
                            continue;
                        keyStoreAliasPassword = rol;
                    }
                }
            }
            string privateXmlPath = BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.STUDIO_PROJECT_DIR,
                "app", "src", "main", "res", "values", "private.xml");
            if (File.Exists(privateXmlPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.Load(privateXmlPath);
                XmlNode root = doc.DocumentElement;
                XmlNode rsaNode = root.SelectSingleNode("string[@name='RSA_public_key']");
                XmlNode saltNode = root.SelectSingleNode("integer-array[@name='ExpansionDownloaderServiceSALT']");
                if (rsaNode != null)
                {
                    if (rsaNode.InnerText != TemplateTag.RSA_PUBLIC_KEY.Tag)
                        rsaPublicKey = rsaNode.InnerText;
                }
                if ((saltNode != null) && (saltNode.InnerText != TemplateTag.PRIVATE_SALT.Tag))
                {
                    privateSalt = XmlCsvSByteArray.CreateFromXml(saltNode.InnerXml);
                }
            }
        }

        /// <summary>
        /// Editor is reading data from XML.
        /// </summary>
        public static void FromXml(XmlNode node)
        {
            if (node == null) // AGS game has not used plugin before
            {
                appName = GameName; // default app name to game name
                return;
            }
            // read values from XML
            // note we read directly to the backing fields, we do NOT want to mark files dirty here
            packageName = node.SelectSingleNode(TAG_PACKAGE_NAME).InnerText;
            int.TryParse(node.SelectSingleNode(TAG_VERSION_CODE).InnerText, out versionCode);
            versionName = node.SelectSingleNode(TAG_VERSION_NAME).InnerText;
            appName = node.SelectSingleNode(TAG_APP_NAME).InnerText;
            int.TryParse(node.SelectSingleNode(TAG_OBB_VERSION).InnerText, out obbVersion);
            obbPassword = node.SelectSingleNode(TAG_OBB_PASSWORD).InnerText;
            launcherIconsZip = node.SelectSingleNode(TAG_LAUNCHER_ICON_ZIP).InnerText;
            ReadPrivateSettings();
            AndroidBuilderPane.Instance.OnMetadataLoaded();
        }

        /// <summary>
        /// Editor is saving data to XML.
        /// </summary>
        public static void ToXml(XmlTextWriter writer)
        {
            // write values to XML
            writer.WriteElementString(TAG_PACKAGE_NAME, packageName);
            writer.WriteElementString(TAG_VERSION_CODE, versionCode.ToString());
            writer.WriteElementString(TAG_VERSION_NAME, versionName);
            writer.WriteElementString(TAG_APP_NAME, appName);
            writer.WriteElementString(TAG_OBB_VERSION, obbVersion.ToString());
            writer.WriteElementString(TAG_OBB_PASSWORD, obbPassword);
            writer.WriteElementString(TAG_LAUNCHER_ICON_ZIP, launcherIconsZip);
        }
    }
}
