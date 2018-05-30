using System;
using System.Runtime.Serialization;
using System.Xml.XPath;
using System.Xml;

namespace Quintity.TestFramework.Core
{
    [DataContract]
    public class TestParameter
    {
        #region Data members

        [DataMember(Order = 0)]
        public string DisplayName
        { get; set; }

        [DataMember(Order = 1)]
        public string Name
        { get; set; }

        [DataMember(Order = 2)]
        public string ValueAsString
        { get; set; }

        [DataMember(Order = 3)]
        public String TypeAsString
        { get; set; }

        #endregion

        #region Constructors

        public TestParameter()
        { }

        public TestParameter(TestParameter testParameter) : this()
        {
            Name = string.Copy(testParameter.Name);
            DisplayName = string.Copy(testParameter.DisplayName);
            ValueAsString = string.Copy(testParameter.ValueAsString);
            TypeAsString = string.Copy(testParameter.TypeAsString);
        }

        public TestParameter(string name, string displayName, object value, Type type)
        {
            TestAssert.IsFalse(string.IsNullOrEmpty(name), "The parameter name cannot be a null or empty value.");
            TestAssert.IsFalse(string.IsNullOrEmpty(displayName), "The parameter display name cannot be a null or empty value.");

            Name = name;
            DisplayName = displayName;
            ValueAsString = value != null ? value.ToString() : null;
            TypeAsString = type.FullName;
        }

        public TestParameter(string name, string displayName, string value,string type)
        {
            Name = name;
            DisplayName = displayName;
            ValueAsString = value;
            TypeAsString = type;
        }

        //[Obsolete]
        //public TestParameter(string name, string dataType, string value)
        //{
        //    Name = name;
        //    string valueToUpper = value.ToUpper();

        //    switch (dataType.ToUpper())
        //    {
        //        case "SYSTEM.STRING":
        //            _value = value;
        //            _dataType = Type.GetType("System.String");
        //            break;

        //        case "SYSTEM.CHAR":
        //            _value = Convert.ToChar(value);
        //            _dataType = Type.GetType("System.Char");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.CHAR]":

        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToChar(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Char]");
        //            break;

        //        case "SYSTEM.BYTE":
        //            _value = Convert.ToByte(value);
        //            _dataType = Type.GetType("System.Byte");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.BYTE]":

        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToByte(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Byte]");
        //            break;

        //        case "SYSTEM.SBYTE":
        //            _value = Convert.ToSByte(value);
        //            _dataType = Type.GetType("System.SByte");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.SBYTE]":

        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToSByte(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.SByte]");
        //            break;

        //        case "SYSTEM.INT16":
        //            _value = Convert.ToInt16(value);
        //            _dataType = Type.GetType("System.Int16");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.INT16]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToInt16(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Int16]");
        //            break;

        //        case "SYSTEM.UINT16":
        //            _value = Convert.ToUInt16(value);
        //            _dataType = Type.GetType("System.UInt16");
        //            break;

        //        case "SYSTEM.NULLABLE`1[SYSTEM.UINT16]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToUInt16(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.UInt16]");
        //            break;

        //        case "SYSTEM.INT32":
        //            _value = Convert.ToInt32(value);
        //            _dataType = Type.GetType("System.Int32");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.INT32]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToInt32(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Int32]");
        //            break;

        //        case "SYSTEM.UINT32":
        //            _value = Convert.ToUInt32(value);
        //            _dataType = Type.GetType("System.UInt32");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.UINT32]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToUInt32(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.UInt32]");
        //            break;

        //        case "SYSTEM.INT64":
        //            _value = Convert.ToInt64(value);
        //            _dataType = Type.GetType("System.Int64");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.INT64]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToInt64(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Int64]");
        //            break;

        //        case "SYSTEM.UINT64":
        //            _value = Convert.ToUInt64(value);
        //            _dataType = Type.GetType("System.UInt64");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.UINT64]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToUInt64(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.UInt64]");
        //            break;

        //        case "SYSTEM.SINGLE":
        //            _value = Convert.ToSingle(value);
        //            _dataType = Type.GetType("System.Single");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.SINGLE]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToSingle(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Single]");
        //            break;

        //        case "SYSTEM.DOUBLE":
        //            _value = Convert.ToDouble(value);
        //            _dataType = Type.GetType("System.Double");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.DOUBLE]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToDouble(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Double]");
        //            break;

        //        case "SYSTEM.DECIMAL":
        //            _value = Convert.ToDecimal(value);
        //            _dataType = Type.GetType("System.Decimal");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.DECIMAL]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToDecimal(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Decimal]");
        //            break;

        //        case "SYSTEM.BOOLEAN":
        //            _value = Convert.ToBoolean(value);
        //            _dataType = Type.GetType("System.Boolean");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.BOOLEAN]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = Convert.ToBoolean(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.Boolean]");
        //            break;

        //        case "SYSTEM.DATETIME":
        //            if (valueToUpper != "[NOW]")
        //            {
        //                _value = Convert.ToDateTime(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.DateTime");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.DATETIME]":
        //            if (valueToUpper != "[NULL]" && !(valueToUpper.Contains("[NOW") && valueToUpper.Contains("]")))
        //            {
        //                _value = Convert.ToDateTime(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.DateTime]");
        //            break;

        //        case "SYSTEM.TIMESPAN":
        //            _value = TimeSpan.Parse(value);
        //            _dataType = Type.GetType("System.TimeSpan");
        //            break;
        //        case "SYSTEM.NULLABLE`1[SYSTEM.TIMESPAN]":
        //            if (valueToUpper != "[NULL]")
        //            {
        //                _value = TimeSpan.Parse(value);
        //            }
        //            else
        //            {
        //                _value = valueToUpper;
        //            }

        //            _dataType = Type.GetType("System.Nullable`1[System.TimeSpan]");
        //            break;

        //        default:
        //            break;
        //    }
        //}

        #endregion

        #region Public methods

        internal static TestParameter ReadTestParameter(XPathNavigator navigator)
        {
            TestParameter testParameter = new TestParameter()
            {
                DisplayName = TestUtils.GetXPathValue(navigator, "DisplayName"),
                Name = TestUtils.GetXPathValue(navigator, "Name"),
                TypeAsString = TestUtils.GetXPathValue(navigator, "TypeAsString"),
                ValueAsString = TestUtils.GetXPathValue(navigator, "ValueAsString")
            };

            return testParameter;

        }

        internal void Write(XmlTextWriter xmlWriter)
        {
            // Writes parent element name.
            xmlWriter.WriteStartElement("TestParameter");

            // Write element attributes for name.
            xmlWriter.WriteElementString("DisplayName", "", DisplayName);
            xmlWriter.WriteElementString("Name", Name);
            xmlWriter.WriteElementString("TypeAsString", TypeAsString);
            xmlWriter.WriteElementString("ValueAsString", ValueAsString);

            // Ends parent element.
            xmlWriter.WriteEndElement();
        }

        new public Type GetType()
        {
            return Type.GetType(TypeAsString);
        }

        public object GetValue()
        {
            object value;
            TestUtils.FromString(ValueAsString, Type.GetType(TypeAsString), out value);

            return value;
        }

        public override string ToString()
        {
            return string.Format("{0}, Value:  {1}, Datatype:  {2}",
                        Name.Equals(DisplayName) ? DisplayName : 
                            string.Format("{0} ({1})", DisplayName, Name),
                        ValueAsString != null ? ValueAsString : "Null",
                        TypeAsString);
        }

        

        #endregion
    }
}
