using System;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Represents an AGS version number in string and numerical formats. Modeled after AGS::Common::Version.
    /// </summary>
    public class AGSVersion : IEquatable<long>, IEquatable<string>, IEquatable<AGSVersion>
    {
        public AGSVersion()
            : this(0, 0, 0, 0, "")
        {
        }

        public AGSVersion(int major, int minor, int release)
            : this(major, minor, release, 0, "")
        {
        }

        public AGSVersion(int major, int minor, int release, int revision)
            : this(major, minor, release, revision, "")
        {
        }

        public AGSVersion(int major, int minor, int release, int revision, string special)
        {
            Major = major;
            Minor = minor;
            Release = release;
            Revision = revision;
            Special = special;
        }

        public AGSVersion(string version)
        {
            string[] sections = version.Split('.', ' ');
            int i;
            switch (sections.Length > 4 ? 4 : sections.Length - 1)
            {
                case 4:
                    Special = version.Substring(sections[0].Length + sections[1].Length + sections[2].Length + sections[3].Length + 4);
                    goto case 3;
                case 3:
                    Revision = int.TryParse(sections[3], out i) ? i : 0;
                    goto case 2;
                case 2:
                    Release = int.TryParse(sections[2], out i) ? i : 0;
                    goto case 1;
                case 1:
                    Minor = int.TryParse(sections[1], out i) ? i : 0;
                    goto case 0;
                case 0:
                    Major = int.TryParse(sections[0], out i) ? i : 0;
                    break;
                default:
                    break;
            }
        }

        public int Major
        {
            get;
            private set;
        }

        public int Minor
        {
            get;
            private set;
        }

        public int Release
        {
            get;
            private set;
        }

        public int Revision
        {
            get;
            private set;
        }

        public string Special
        {
            get;
            private set;
        }

        public string LongString
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}{4}{5}", Major, Minor, Release, Revision, Special.Length == 0 ? "" : " ", Special);
            }
        }

        public string ShortString
        {
            get
            {
                return string.Format("{0}.{1}", Major, Minor);
            }
        }

        public override int GetHashCode()
        {
            return ((long)this).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is long)
            {
                return Equals((long)obj);
            }
            if (!(obj is string))
            {
                long? l = (obj as long?);
                if (l.HasValue)
                {
                    return Equals(l.Value);
                }
                try
                {
                    return Equals(Convert.ToInt64(obj));
                }
                catch
                {
                }
            }
            return Equals(obj.ToString());
        }

        public bool Equals(long value)
        {
            return value == (long)this;
        }

        public bool Equals(string value)
        {
            return string.Equals(value, ShortString) || string.Equals(value, LongString) || Equals((long)new AGSVersion(value));
        }

        public bool Equals(AGSVersion value)
        {
            return Equals((long)value);
        }

        public static explicit operator short(AGSVersion version)
        {
            return (short)((version.Major * 100) + version.Minor);
        }

        public static explicit operator int(AGSVersion version)
        {
            return (version.Major * 10000) + (version.Minor * 100) + version.Release;
        }

        public static explicit operator long(AGSVersion version)
        {
            return (version.Major * 100000000L) + (version.Minor * 1000000L) + (version.Release * 10000L) + version.Revision;
        }

        public static bool operator==(AGSVersion lhs, AGSVersion rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(AGSVersion lhs, AGSVersion rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator<(AGSVersion lhs, AGSVersion rhs)
        {
            return ((long)lhs) < ((long)rhs);
        }

        public static bool operator<=(AGSVersion lhs, AGSVersion rhs)
        {
            return (lhs < rhs) || (lhs == rhs);
        }

        public static bool operator>(AGSVersion lhs, AGSVersion rhs)
        {
            return !(lhs <= rhs);
        }

        public static bool operator>=(AGSVersion lhs, AGSVersion rhs)
        {
            return !(lhs < rhs);
        }
    }
}
