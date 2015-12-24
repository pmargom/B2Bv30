using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace B2B.Generic
{
    public static class Utils
    {

        #region Serialization
        public static T Deserialize<T>(this string xml) where T : class
        {
            XmlTextReader xr = new XmlTextReader(new StringReader(xml));
            XmlSerializer xs = new XmlSerializer(typeof(T));
            return (T)xs.Deserialize(xr);
        }

        public static string Serialize(this object obj)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            xs.Serialize(ms, obj);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static object Deserialize(this string xml, Type t)
        {
            XmlTextReader xr = new XmlTextReader(new StringReader(xml));
            XmlSerializer xs = new XmlSerializer(t);
            return xs.Deserialize(xr);
        }

        #endregion

        #region Parsing

        public static bool? ParseBool(this object obj)
        {
            if (obj == null) return null;
            bool tmp; if (bool.TryParse(obj.ToString(), out tmp)) return tmp;
            return null;
        }

        public static T Parse<T>(this object obj)
        {
            if (obj == null || obj.GetType() == typeof(System.DBNull)) return default(T);

            try
            {
                if (!typeof(T).IsEnum)
                    return (T)Convert.ChangeType(obj, typeof(T));
                else
                    return (T)Enum.Parse(typeof(T), obj.ToString());
            }
            catch { return default(T); }
        }

        public static object Parse(this object value, Type type)
        {
            if (value == null) return null;

            try
            {
                if (!type.IsEnum)
                    return Convert.ChangeType(value, type);
                else
                    return value;
            }
            catch { return null; }
        }

        public static bool ParseBool(this object obj, bool defaultValue)
        {
            if (obj == null) return defaultValue;
            bool tmp; return (bool.TryParse(obj.ToString(), out tmp) ? tmp : defaultValue);
        }
        
        #endregion

    }
}
