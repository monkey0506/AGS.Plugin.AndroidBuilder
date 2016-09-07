using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Represents one of the template files used in creating the Android Studio project.
    /// File exists as a template in memory, and is written to disk with the final values
    /// replaced.
    /// </summary>
    public class TemplateFile
    {
        private static Dictionary<TemplateFilePath, TemplateFile> _files = new Dictionary<TemplateFilePath, TemplateFile>();

        private List<TemplateTag> _tags;

        static TemplateFile()
        {
            ExtractTemplateZipToMemory();
        }

        /// <summary>
        /// Return the tags associated with the specified template file.
        /// </summary>
        private static List<TemplateTag> GetTemplateTagsForFile(string shortName)
        {
            // * $AGS_GAME_PACKAGE$ must be replaced with the package name (e.g., com.bigbluecup.game) in the following files:
            //     * Java source files (app/src/main/java/*.java)
            //     * app/src/main/AndroidManifest.xml
            //     * project.properties
            //
            // * $AGS_GAME_VERSION_CODE$ and $AGS_GAME_VERSION_NAME$ in project.properties must be replaced with the game's version code and version name, respectively.
            //
            // * The following symbols are used in local.static.properties:
            //     * $AGS_GAME_KEYSTORE_PATH$ must be replaced with the path to the Java keystore
            //     * $AGS_GAME_KEYSTORE_PASSWORD$ must be replaced with the Java keystore password
            //     * $AGS_GAME_KEYSTORE_ALIAS$ must be replaced with the Java keystore alias
            //     * $AGS_GAME_KEYSTORE_ALIAS_PASSWORD$ must be replaced with the Java keystore alias password
            //
            // * The following symbols are used in app/src/main/res/values/project.xml:
            //     * $AGS_GAME_NAME$ must be replaced with the name of your AGS game (as it will appear on the device)
            //     * $AGS_GAME_DATA_FILENAME$ must be replaced with the filename of your AGS game data file (e.g., game.ags or game.exe)
            //     * $AGS_GAME_OBB_VERSION$ must be replaced with the version of your APK expansion file
            //     * $AGS_GAME_OBB_FILE_SIZE$ must be replaced with the file size (in bytes) of your APK expansion file
            //     * $AGS_GAME_OBB_PASSWORD$ must be replaced with the password of your APK expansion file, or @null for none.
            //
            // * The following symbols are used in app/src/main/res/values/private.xml:
            //     * $AGS_GAME_RSA_PUBLIC_KEY$ must be replaced with the RSA public key of your app
            //     * $AGS_GAME_PRIVATE_SALT$ must be replaced with a series of bytes formatted as <item>VALUE</item>
            List<TemplateTag> result = new List<TemplateTag>();
            if ((shortName.StartsWith("app") && shortName.EndsWith(".java")) ||
                (shortName == "app/src/main/AndroidManifest.xml"))
            {
                result.Add(TemplateTag.PACKAGE_NAME);
            }
            else if (shortName == "project.properties")
                result.AddRange(new TemplateTag[] { TemplateTag.PACKAGE_NAME, TemplateTag.VERSION_CODE, TemplateTag.VERSION_NAME });
            else if (shortName == "local.static.properties")
                result.AddRange(new TemplateTag[] { TemplateTag.KEY_STORE_PATH, TemplateTag.KEY_STORE_PASSWORD,
                    TemplateTag.KEY_STORE_ALIAS, TemplateTag.KEY_STORE_ALIAS_PASSWORD });
            else if (shortName == "app/src/main/res/values/project.xml")
                result.AddRange(new TemplateTag[] { TemplateTag.APP_NAME, TemplateTag.DATA_FILENAME, TemplateTag.OBB_VERSION,
                    TemplateTag.OBB_FILE_SIZE, TemplateTag.OBB_PASSWORD });
            else if (shortName == "app/src/main/res/values/private.xml")
                result.AddRange(new TemplateTag[] { TemplateTag.RSA_PUBLIC_KEY, TemplateTag.PRIVATE_SALT });
            return result;
        }

        /// <summary>
        /// Extracts a file's contents from the embedded zip archive into memory.
        /// </summary>
        private static void ExtractTemplateZipToMemory()
        {
            string appJavaPath = BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.STUDIO_PROJECT_DIR, "app", "src", "main", "java",
                AndroidMetadata.PackageName.Replace('.', System.IO.Path.DirectorySeparatorChar));
            using (MemoryStream zipStream = new MemoryStream(Properties.Resources.templateZip))
            using (ZipStorer zip = ZipStorer.Open(zipStream, FileAccess.Read))
            {
                List<ZipStorer.ZipFileEntry> files = zip.ReadCentralDir(); // get list of files in the zip
                foreach (ZipStorer.ZipFileEntry entry in files)
                {
                    string filenameInZip = entry.FilenameInZip;
                    string fullPath = BuildTargetAndroid.Instance.GetCompiledPath(BuildTargetAndroid.STUDIO_PROJECT_DIR, filenameInZip);
                    bool isAppJavaFile = filenameInZip.StartsWith("app") && filenameInZip.EndsWith(".java");
                    if (isAppJavaFile)
                    {
                        fullPath = System.IO.Path.Combine(appJavaPath, System.IO.Path.GetFileName(filenameInZip));
                    }
                    TemplateFilePath shortName = new TemplateFilePath(filenameInZip, fullPath);
                    using (MemoryStream stream = new MemoryStream((int)entry.FileSize))
                    {
                        zip.ExtractFile(entry, stream);
                        _files.Add(shortName, new TemplateFile(shortName, stream.ToArray()));
                    }
                }
            }
        }

        /// <summary>
        /// Writes any dirty or missing files to disk, overwriting any existing files.
        /// </summary>
        public static void WriteDirtyOrMissingFiles()
        {
            foreach (var kvPair in Files)
            {
                if ((!File.Exists(kvPair.Key.FullPath)) || (kvPair.Value.Dirty))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(kvPair.Key.FullPath));
                    if (kvPair.Key.IsDirectory)
                        continue;
                    if (kvPair.Value.Tags.Count > 0)
                    {
                        string fileText = Encoding.UTF8.GetString(kvPair.Value.Contents);
                        foreach (TemplateTag tag in kvPair.Value.Tags)
                        {
                            fileText = fileText.Replace(tag.Tag, tag.RealValue);
                        }
                        File.WriteAllText(kvPair.Key.FullPath, fileText);
                    }
                    else
                        File.WriteAllBytes(kvPair.Key.FullPath, kvPair.Value.Contents);
                    kvPair.Value.Dirty = false;
                }
            }
        }

        /// <summary>
        /// A dictionary of all known template files, with the respective
        /// TemplateFilePath as the key.
        /// </summary>
        public static IDictionary<TemplateFilePath, TemplateFile> Files
        {
            get
            {
                return new ReadOnlyDictionary<TemplateFilePath, TemplateFile>(_files);
            }
        }

        /// <summary>
        /// Create a template file from the given TemplateFilePath and contents.
        /// </summary>
        private TemplateFile(TemplateFilePath path, byte[] contents)
        {
            Path = path;
            Contents = contents;
            _tags = GetTemplateTagsForFile(path);
            foreach (TemplateTag tag in _tags)
            {
                tag.AddFile(this);
            }
        }

        /// <summary>
        /// Creates a template file from the given paths and contents.
        /// </summary>
        private TemplateFile(string shortName, string fullPath, byte[] contents) :
            this(new TemplateFilePath(shortName, fullPath), contents)
        {
        }

        /// <summary>
        /// Gets or sets whether this template file is dirty. A dirty template file
        /// is one that needs to be rewritten to disk, typically meaning that a
        /// related template tag's real value has changed (such as package name).
        /// </summary>
        public bool Dirty
        {
            get;
            set;
        }

        /// <summary>
        /// The TemplateFilePath of this template file.
        /// </summary>
        public TemplateFilePath Path
        {
            get;
            private set;
        }

        /// <summary>
        /// The contents of this file, as read from the zip archive.
        /// </summary>
        public byte[] Contents
        {
            get;
            private set;
        }

        /// <summary>
        /// A read-only list of the template tags associated with this file.
        /// </summary>
        public IList<TemplateTag> Tags
        {
            get
            {
                return _tags.AsReadOnly();
            }
        }
    }
}
