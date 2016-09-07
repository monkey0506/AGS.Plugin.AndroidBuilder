using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// A signed byte array which can be represented in either XML or CSV formats.
    /// </summary>
    public class XmlCsvSByteArray
    {
        private const string DEFAULT_XML_INDENT = "        "; // assume we're nested at least two elements deep (root element and array element)
        private const int DEFAULT_XML_BYTES_PER_ROW = 4; // default 4 bytes per row for readability

        private sbyte[] _bytes = null;
        private string _xmlIndent;
        private int _xmlBytesPerRow;

        /// <summary>
        /// Creates an empty array.
        /// </summary>
        public static XmlCsvSByteArray CreateEmpty()
        {
            return new XmlCsvSByteArray();
        }

        /// <summary>
        /// Creates an empty array with the specified XML format parameters.
        /// </summary>
        public static XmlCsvSByteArray CreateEmpty(string xmlIndent, int xmlBytesPerRow)
        {
            return new XmlCsvSByteArray(xmlIndent, xmlBytesPerRow);
        }

        /// <summary>
        /// Creates a copy of the given signed byte array.
        /// </summary>
        public static XmlCsvSByteArray CreateFromBytes(sbyte[] bytes)
        {
            return new XmlCsvSByteArray(bytes);
        }

        /// <summary>
        /// Creates a copy of the given signed byte array with the specified XML format parameters.
        /// </summary>
        public static XmlCsvSByteArray CreateFromBytes(sbyte[] bytes, string xmlIndent, int xmlBytesPerRow)
        {
            return new XmlCsvSByteArray(bytes, xmlIndent, xmlBytesPerRow);
        }

        /// <summary>
        /// Creates a signed byte array from the given XML text with the specified XML format parameters.
        /// </summary>
        public static XmlCsvSByteArray CreateFromXml(string xml, string xmlIndent, int xmlBytesPerRow)
        {
            XmlCsvSByteArray result = new XmlCsvSByteArray(xmlIndent, xmlBytesPerRow);
            result.FromXml(xml);
            return result;
        }

        /// <summary>
        /// Creates a signed byte array from the given XML text.
        /// </summary>
        public static XmlCsvSByteArray CreateFromXml(string xml)
        {
            return CreateFromXml(xml, DEFAULT_XML_INDENT, DEFAULT_XML_BYTES_PER_ROW);
        }

        /// <summary>
        /// Creates a signed byte array from the given CSV text with the specified XML format parameters.
        /// </summary>
        public static XmlCsvSByteArray CreateFromCsv(string csv, string xmlIndent, int xmlBytesPerRow)
        {
            XmlCsvSByteArray result = new XmlCsvSByteArray(xmlIndent, xmlBytesPerRow);
            result.FromCsv(csv);
            return result;
        }

        /// <summary>
        /// Creates a signed byte array from the given CSV text.
        /// </summary>
        public static XmlCsvSByteArray CreateFromCsv(string csv)
        {
            return CreateFromCsv(csv, DEFAULT_XML_INDENT, DEFAULT_XML_BYTES_PER_ROW);
        }

        private XmlCsvSByteArray() :
            this(null, DEFAULT_XML_INDENT, DEFAULT_XML_BYTES_PER_ROW)
        {
        }

        private XmlCsvSByteArray(sbyte[] bytes) :
            this(bytes, DEFAULT_XML_INDENT, DEFAULT_XML_BYTES_PER_ROW)
        {
        }

        private XmlCsvSByteArray(string xmlIndent, int xmlBytesPerRow) :
            this(null, xmlIndent, xmlBytesPerRow)
        {
        }

        private XmlCsvSByteArray(sbyte[] bytes, string xmlIndent, int xmlBytesPerRow)
        {
            _xmlIndent = xmlIndent == null ? "" : xmlIndent;
            _xmlBytesPerRow = xmlBytesPerRow < 0 ? 1 : xmlBytesPerRow;
            if ((bytes == null) || (bytes.Length == 0))
                return;
            _bytes = new sbyte[bytes.Length];
            for (int i = 0; i < _bytes.Length; ++i)
            {
                _bytes[i] = bytes[i];
            }
        }

        /// <summary>
        /// Returns an XML representation of this signed byte array.
        /// </summary>
        public string ToXml()
        {
            if ((_bytes == null) || (_bytes.Length == 0))
                return "";
            StringBuilder sb = new StringBuilder(17 * _bytes.Length); // max length of each item is 17 chars, so assume this as default capacity
            int rowByteCount = 0;
            foreach (sbyte b in _bytes)
            {
                if (++rowByteCount > _xmlBytesPerRow)
                {
                    sb.Append(Environment.NewLine).Append(_xmlIndent);
                    rowByteCount = 1; // this is always at least 1 by this point
                }
                sb.Append("<item>").Append(b).Append("</item>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a CSV representation of this signed byte array.
        /// </summary>
        public string ToCsv()
        {
            if ((_bytes == null) || (_bytes.Length == 0))
                return "";
            StringBuilder sb = new StringBuilder(5 * _bytes.Length); // max length of each item is 5 chars, so assume this as default capacity
            foreach (sbyte b in _bytes)
            {
                if (sb.Length != 0)
                    sb.Append(',');
                sb.Append(b);
            }
            return sb.ToString();
        }

        private void FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                _bytes = null;
                return;
            }
            xml = "<integer-array>" + xml + "</integer-array>"; // simulate a root element for XmlTextReader
            List<sbyte> list = new List<sbyte>();
            sbyte sb;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode root = doc.DocumentElement;
            foreach (XmlNode node in root.SelectNodes("item"))
            {
                if (sbyte.TryParse(node.InnerText, out sb))
                    list.Add(sb);
            }
            _bytes = list.Count == 0 ? null : list.ToArray();
        }

        private void FromCsv(string csv)
        {
            if (string.IsNullOrEmpty(csv))
            {
                _bytes = null;
                return;
            }
            string[] bytes = csv.Split(',');
            List<sbyte> list = new List<sbyte>();
            sbyte sb;
            foreach (string byteText in bytes)
            {
                if (sbyte.TryParse(byteText.Trim(), out sb))
                    list.Add(sb);
            }
            _bytes = list.Count == 0 ? null : list.ToArray();
        }

        /// <summary>
        /// Returns the underlying signed byte array.
        /// </summary>
        public sbyte[] Bytes
        {
            get { return _bytes; }
        }

        /// <summary>
        /// Returns whether the signed byte array is null or contains no items.
        /// </summary>
        public bool IsEmpty
        {
            get { return (_bytes == null) || (_bytes.Length == 0); }
        }

        public override bool Equals(object obj)
        {
            sbyte[] arr = null;
            if (obj is sbyte[])
                arr = (sbyte[])obj;
            else if (obj is XmlCsvSByteArray)
                arr = ((XmlCsvSByteArray)obj).Bytes;
            else
                return false;
            if (_bytes == null)
                return arr == null;
            if (arr == null)
                return false;
            if (arr.Length != _bytes.Length)
                return false;
            for (int i = 0; i < _bytes.Length; ++i)
            {
                if (_bytes[i] != arr[i])
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return _bytes == null ? 0 : _bytes.GetHashCode();
        }

        public static explicit operator sbyte[](XmlCsvSByteArray arr)
        {
            return arr.Bytes;
        }
    }
}
