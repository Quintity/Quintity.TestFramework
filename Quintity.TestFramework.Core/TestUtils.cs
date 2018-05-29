using System;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Quintity.TestFramework.Core
{
    public static partial class TestUtils
    {
        #region Internal methods

        #endregion

        #region Public methods

        public static bool IsSerializable(object value, out string message, List<Type> knownTypes = null)
        {
            bool serializable = false;

            try
            {
                System.Runtime.Serialization.DataContractSerializer serializer =
                    new System.Runtime.Serialization.DataContractSerializer(value.GetType(), knownTypes);

                // Create a memory stream and write to it.
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, value);

                // Just to verify
                memoryStream.Flush();
                memoryStream.Position = 0;

                StreamReader reader = new StreamReader(memoryStream);
                message = reader.ReadToEnd();

                serializable = true;
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return serializable;
        }

        public static object GetDefault<T>()
        {
            return default(T);
        }

        public static bool FromString(string valueAsString, ParameterInfo parameter, out object value)
        {
            return FromString(valueAsString, parameter.ParameterType, out value);
        }

        public static bool FromString(string valueAsString, Type type, out object value)
        {
            bool success = false;

            value = null;

            switch (type.ToString().ToUpper())
            {
                case "SYSTEM.STRING":
                    value = valueAsString;
                    success = true;
                    break;

                case "SYSTEM.NULLABLE`1[SYSTEM.CHAR]":
                    char @charn;

                    if (valueAsString == null)
                    {
                        return true;
                    }
                    else if (fromString<char>(valueAsString, out @charn))
                    {
                        value = @charn;
                        success = true;
                    }

                    break;
                case "SYSTEM.CHAR":
                    char @char;

                    if (fromString<char>(valueAsString, out @char))
                    {
                        value = @char;
                        success = true;
                    }

                    break;

                case "SYSTEM.BYTE":
                    Byte @byte;

                    if (fromString<Byte>(valueAsString, out @byte))
                    {
                        value = @byte;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.BYTE]":
                    Byte @byten;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<Byte>(valueAsString, out @byten))
                    {
                        value = @byten;
                        success = true;
                    }

                    break;
                case "SYSTEM.SBYTE":
                    SByte @sbyte;

                    if (fromString<SByte>(valueAsString, out @sbyte))
                    {
                        value = @sbyte;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.SBYTE]":
                    SByte @sbyten;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<SByte>(valueAsString, out @sbyten))
                    {
                        value = @sbyten;
                        success = true;
                    }

                    break;
                case "SYSTEM.INT16":
                    short int16;

                    if (fromString<short>(valueAsString, out int16))
                    {
                        value = int16;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.INT16]":
                    short int16n;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<short>(valueAsString, out int16n))
                    {
                        value = int16n;
                        success = true;
                    }

                    break;
                case "SYSTEM.INT32":
                    int int32;

                    if (fromString<int>(valueAsString, out int32))
                    {
                        value = int32;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.INT32]":
                    int int32n;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<int>(valueAsString, out int32n))
                    {
                        value = int32n;
                        success = true;
                    }

                    break;

                case "SYSTEM.INT64":
                    long int64;

                    if (fromString<long>(valueAsString, out int64))
                    {
                        value = int64;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.INT64]":
                    long int64n;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<long>(valueAsString, out int64n))
                    {
                        value = int64n;
                        success = true;
                    }

                    break;
                case "SYSTEM.UINT16":
                    ushort uint16;

                    if (fromString<ushort>(valueAsString, out uint16))
                    {
                        value = uint16;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.UINT16]":
                    ushort uint16n;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<ushort>(valueAsString, out uint16n))
                    {
                        value = uint16n;
                        success = true;
                    }

                    break;
                case "SYSTEM.UINT32":
                    uint uint32;

                    if (fromString<uint>(valueAsString, out uint32))
                    {
                        value = uint32;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.UINT32]":
                    uint uint32n;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<uint>(valueAsString, out uint32n))
                    {
                        value = uint32n;
                        success = true;
                    }

                    break;
                case "SYSTEM.UINT64":
                    ulong uint64;

                    if (fromString<ulong>(valueAsString, out uint64))
                    {
                        value = uint64;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.UINT64]":
                    ulong uint64n;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<ulong>(valueAsString, out uint64n))
                    {
                        value = uint64n;
                        success = true;
                    }

                    break;
                case "SYSTEM.SINGLE":
                    Single single;

                    if (fromString<Single>(valueAsString, out single))
                    {
                        value = single;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.SINGLE]":
                    Single singlen;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<Single>(valueAsString, out singlen))
                    {
                        value = singlen;
                        success = true;
                    }

                    break;
                case "SYSTEM.DOUBLE":
                    Double @double;

                    if (fromString<Double>(valueAsString, out @double))
                    {
                        value = @double;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.DOUBLE]":
                    Double @doublen;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<Double>(valueAsString, out @doublen))
                    {
                        value = @doublen;
                        success = true;
                    }

                    break;
                case "SYSTEM.DECIMAL":
                    Decimal @decimal;

                    if (fromString<Decimal>(valueAsString, out @decimal))
                    {
                        value = @decimal;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.DECIMAL]":
                    Decimal @decimaln;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<Decimal>(valueAsString, out @decimaln))
                    {
                        value = @decimaln;
                        success = true;
                    }

                    break;
                case "SYSTEM.BOOLEAN":
                    Boolean boolean;

                    if (fromString<Boolean>(valueAsString, out boolean))
                    {
                        value = boolean;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.BOOLEAN]":
                    Boolean booleann;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<Boolean>(valueAsString, out booleann))
                    {
                        value = booleann;
                        success = true;
                    }

                    break;
                case "SYSTEM.DATETIME":
                    DateTime datetime;

                    if (fromString<DateTime>(valueAsString, out datetime))
                    {
                        value = datetime;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.DATETIME]":
                    DateTime datetimen;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<DateTime>(valueAsString, out datetimen))
                    {
                        value = datetimen;
                        success = true;
                    }

                    break;
                case "SYSTEM.TIMESPAN":
                    TimeSpan timespan;

                    if (fromString<TimeSpan>(valueAsString, out timespan))
                    {
                        value = timespan;
                        success = true;
                    }

                    break;
                case "SYSTEM.NULLABLE`1[SYSTEM.TIMESPAN]":
                    TimeSpan timespann;

                    if (valueAsString == null)
                    {
                        success = true;
                    }
                    else if (fromString<TimeSpan>(valueAsString, out timespann))
                    {
                        value = timespann;
                        success = true;
                    }

                    break;
                default:
                    break;
            }

            return success;
        }

        /// <summary>
        /// Safely copies the original string to a new string.
        /// </summary>
        /// <param name="original">Original string to copy.</param>
        /// <returns>Copy of original string or, if original string is null, a null value.</returns>
        public static string SafeCopyString(string original)
        {
            return original != null ? string.Copy(original) : null;
        }

        /// <summary>
        /// Converts a comma delimited string to a list of trimmed string elements.
        /// </summary>
        /// <param name="delimitedString">Delimited string</param>
        /// <returns>List of parsed string values.</returns>

        public static List<string> ToList(string delimitedString)
        {
            return ToList(delimitedString, new char[] { ',' });
        }

        /// <summary>
        /// Converts a character delimited string to a list of trimmed string elements.
        /// </summary>
        /// <param name="delimitedString">Delimited string</param>
        /// <param name="delimiters">Array of character delimiteds</param>
        /// <returns>List of parsed string values.</returns>
        public static List<string> ToList(string delimitedString, char[] delimiters)
        {
            string[] array = delimitedString.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array[i].Trim();
            }

            return new List<string>(array);
        }

        internal static string GetXPathValue(XPathNavigator navigator, string expression)
        {
            return GetXPathValue(navigator, expression, null);
        }

        internal static string GetXPathValue(XPathNavigator navigator, string expression, string defaultValue)
        {
            string result = null;

            // Execute xpath expression and move to result set.
            XPathNodeIterator iterator = navigator.Select(expression);

            // If results, get first value.
            if (iterator.Count != 0)
            {
                iterator.MoveNext();
                result = iterator.Current.Value;
            }

            // If no result value returned and default value specified, use that.
            return ((result == "" || result == null) && defaultValue != null) ? defaultValue : result;
        }

        #endregion

        #region Private methods

        private static bool fromString<T>(string valueAsString, out T value)
        {
            try
            {
                value = (T)Convert.ChangeType(valueAsString, typeof(T));//, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        #endregion
    }
}
