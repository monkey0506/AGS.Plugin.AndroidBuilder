using System.Collections.Generic;
using static AGS.Plugin.AndroidBuilder.Utility;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Represents a tag which is to be replaced in a TemplateFile with a real value.
    /// </summary>
    public class TemplateTag
    {
        private const string TAG_PACKAGE_NAME = "$AGS_GAME_PACKAGE$";
        private const string TAG_VERSION_CODE = "$AGS_GAME_VERSION_CODE$";
        private const string TAG_VERSION_NAME = "$AGS_GAME_VERSION_NAME$";
        private const string TAG_KEY_STORE_PATH = "$AGS_GAME_KEYSTORE_PATH$";
        private const string TAG_KEY_STORE_PASSWORD = "$AGS_GAME_KEYSTORE_PASSWORD$";
        private const string TAG_KEY_STORE_ALIAS = "$AGS_GAME_KEYSTORE_ALIAS$";
        private const string TAG_KEY_STORE_ALIAS_PASSWORD = "$AGS_GAME_KEYSTORE_ALIAS_PASSWORD$";
        private const string TAG_APP_NAME = "$AGS_GAME_NAME$";
        private const string TAG_DATA_FILENAME = "$AGS_GAME_DATA_FILENAME$";
        private const string TAG_OBB_VERSION = "$AGS_GAME_OBB_VERSION$";
        private const string TAG_OBB_FILE_SIZE = "$AGS_GAME_OBB_FILE_SIZE$";
        private const string TAG_OBB_PASSWORD = "$AGS_GAME_OBB_PASSWORD$";
        private const string TAG_RSA_PUBLIC_KEY = "$AGS_GAME_RSA_PUBLIC_KEY$";
        private const string TAG_PRIVATE_SALT = "$AGS_GAME_PRIVATE_SALT$";

        private static readonly List<TemplateTag> _tags = new List<TemplateTag>();

        public static readonly TemplateTag PACKAGE_NAME = new TemplateTag(TAG_PACKAGE_NAME, () => AndroidMetadata.PackageName);
        public static readonly TemplateTag VERSION_CODE = new TemplateTag(TAG_VERSION_CODE, () => AndroidMetadata.VersionCode.ToString());
        public static readonly TemplateTag VERSION_NAME = new TemplateTag(TAG_VERSION_NAME, () => AndroidMetadata.VersionName);
        public static readonly TemplateTag KEY_STORE_PATH = new TemplateTag(TAG_KEY_STORE_PATH, () => AndroidMetadata.KeyStorePath.Replace("\\", "\\\\"));
        public static readonly TemplateTag KEY_STORE_PASSWORD = new TemplateTag(TAG_KEY_STORE_PASSWORD, () => AndroidMetadata.KeyStorePassword);
        public static readonly TemplateTag KEY_STORE_ALIAS = new TemplateTag(TAG_KEY_STORE_ALIAS, () => AndroidMetadata.KeyStoreAlias);
        public static readonly TemplateTag KEY_STORE_ALIAS_PASSWORD = new TemplateTag(TAG_KEY_STORE_ALIAS_PASSWORD, () => AndroidMetadata.KeyStoreAliasPassword);
        public static readonly TemplateTag APP_NAME = new TemplateTag(TAG_APP_NAME, () => AndroidMetadata.AppName);
        public static readonly TemplateTag DATA_FILENAME = new TemplateTag(TAG_DATA_FILENAME, () => AndroidMetadata.DataFileName);
        public static readonly TemplateTag OBB_VERSION = new TemplateTag(TAG_OBB_VERSION, () => AndroidMetadata.ObbVersion.ToString());
        public static readonly TemplateTag OBB_FILE_SIZE = new TemplateTag(TAG_OBB_FILE_SIZE, () => AndroidMetadata.ObbFileSize.ToString());
        public static readonly TemplateTag OBB_PASSWORD = new TemplateTag(TAG_OBB_PASSWORD, () => AndroidMetadata.ObbPassword);
        public static readonly TemplateTag RSA_PUBLIC_KEY = new TemplateTag(TAG_RSA_PUBLIC_KEY, () => AndroidMetadata.RsaPublicKey);
        public static readonly TemplateTag PRIVATE_SALT = new TemplateTag(TAG_PRIVATE_SALT, () => AndroidMetadata.PrivateSalt.ToXml());

        private Func<string> _value;
        private List<TemplateFile> _files;

        public static IList<TemplateTag> Tags
        {
            get
            {
                return _tags.AsReadOnly();
            }
        }

        private TemplateTag(string tag, Func<string> value)
        {
            Tag = tag;
            _files = new List<TemplateFile>();
            _value = value;
            _tags.Add(this);
        }

        /// <summary>
        /// Marks all files which use this tag as dirty, so they will be rewritten
        /// with the current RealValue of this tag at the next invocation of
        /// TemplateFile.WriteDirtyOrMissingFiles.
        /// </summary>
        public void MarkFilesDirty()
        {
            foreach (TemplateFile file in Files)
            {
                file.Dirty = true;
            }
        }

        /// <summary>
        /// The raw text of the template tag, which is to be replaced.
        /// </summary>
        public string Tag
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the actual text which is used to replace this template tag in
        /// the template files.
        /// </summary>
        public string RealValue
        {
            get
            {
                return _value();
            }
        }

        /// <summary>
        /// Returns a read-only list of files which use this tag.
        /// </summary>
        public IList<TemplateFile> Files
        {
            get
            {
                return _files.AsReadOnly();
            }
        }

        /// <summary>
        /// Adds the specified file to this tag's list of files.
        /// </summary>
        internal void AddFile(TemplateFile file)
        {
            if (!_files.Contains(file))
                _files.Add(file);
        }
    }
}
