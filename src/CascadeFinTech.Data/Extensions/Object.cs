using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace CascadeFinTech.Data.Extensions
{
    internal static class ObjectExtensions
    {
        internal static bool IsNumeric(this object expression)
        {
            var isNum = double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _);
            return isNum;
        }

        internal static T ConvertTo<T>(this object value, Func<T> defaultValueDelegate)
        {
            T tmp = default(T);
            try
            {
                var tc = Type.GetTypeCode(typeof(T));
                switch (tc)
                {
                    case TypeCode.Boolean:
                        {
                            object val = Convert.ToBoolean(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Byte:
                        {
                            object val = Convert.ToByte(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Char:
                        {
                            object val = Convert.ToChar(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.DateTime:
                        {
                            object val = Convert.ToDateTime(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.DBNull:
                        {
                            object val = DBNull.Value;
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Decimal:
                        {
                            object val = Convert.ToDecimal(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Double:
                        {
                            object val = Convert.ToDouble(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Empty:
                        {
                            break;
                        }
                    case TypeCode.Int16:
                        {
                            object val = Convert.ToInt16(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Int32:
                        {
                            object val = Convert.ToInt32(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Int64:
                        {
                            object val = Convert.ToInt64(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Object:
                        {
                            object val = value;
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.SByte:
                        {
                            object val = Convert.ToSByte(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.Single:
                        {
                            object val = Convert.ToSingle(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.String:
                        {
                            object val = value.ToString();
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.UInt16:
                        {
                            object val = Convert.ToUInt16(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.UInt32:
                        {
                            object val = Convert.ToUInt32(value);
                            tmp = (T)val;
                            break;
                        }
                    case TypeCode.UInt64:
                        {
                            object val = Convert.ToUInt64(value);
                            tmp = (T)val;
                            break;
                        }
                    default:
                        {
                            object val = value;
                            tmp = (T)val;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                if (defaultValueDelegate != null)
                {
                    tmp = defaultValueDelegate();
                }
                else
                {
                    throw ex;
                }
            }
            return tmp;
        }
        internal static T ConvertTo<T>(this object value)
        {
            var tmp = ConvertTo<T>(value, null);
            return tmp;
        }

        internal static string XmlSerialize(this object obj)
        {
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            StringBuilder result = new StringBuilder();
            var serializer = new XmlSerializer(obj.GetType());
            using (var writer = XmlWriter.Create(result))
            {
                serializer.Serialize(writer, obj, emptyNamepsaces);
            }
            return result.ToString();
        }

        internal static string XmlSerializeUtf8(this object obj)
        {
            var result = default(string);
            var writer = default(XmlWriter);
            var reader = default(StreamReader);
            var ms = new MemoryStream();
            try
            {
                writer = XmlWriter.Create(ms, new XmlWriterSettings
                {
                    Indent = true
                });
                reader = new StreamReader(ms, Encoding.UTF8);
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj, emptyNamepsaces);
                writer.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                result = reader.ReadToEnd();
                writer.Flush();
            }
            finally
            {
                if (writer != null) writer.Dispose();
                if (reader != null) reader.Dispose();
                if (ms != null) ms.Dispose();
            }
            return result;
        }

        internal static T ConvertValue<T>(this object value)
        {
            if (value == null)
                return default(T);

            var t = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(t);

            if (underlyingType != null)
            {
                return (T)Convert.ChangeType(value, underlyingType);
            }

            if (value is IConvertible && !(typeof(T).IsEnum))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T).IsEnum && value.GetType().Name == "String")
            {
                int intValue;
                var isNumeric = int.TryParse(value.ToString(), out intValue);

                if (isNumeric)
                    value = intValue;

                return (T)value;
            }

            return (T)value;
        }
    }
}
