namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Wrapper class to bind a template file's short path name to a full path.
    /// </summary>
    public class TemplateFilePath
    {
        public TemplateFilePath(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }

        /// <summary>
        /// The short path name (as in template.zip).
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The full path of the extracted file on disk.
        /// </summary>
        public string FullPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns whether this file represents a directory entry only.
        /// </summary>
        public bool IsDirectory
        {
            get { return Name.EndsWith("/"); }
        }

        public static bool operator==(TemplateFilePath lhs, object rhs)
        {
            if (lhs == null)
                return ((rhs is TemplateFilePath) && (rhs == null));
            return lhs.Equals(rhs);
        }

        public static bool operator!=(TemplateFilePath lhs, object rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            TemplateFilePath _obj = (obj as TemplateFilePath);
            if (_obj == null)
                return false;
            return ((_obj.Name == Name) && (_obj.FullPath == FullPath));
        }

        public override int GetHashCode()
        {
            int hash = 1009;
            hash = (hash * 9176) + Name.GetHashCode();
            hash = (hash * 9176) + FullPath.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator string(TemplateFilePath path)
        {
            return path.ToString();
        }
    }
}
