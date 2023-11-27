using AlhambraScoringAndroid.Tools;
using AndroidBase.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace AndroidBase.Options
{
    public static class SettingsManager
    {
        public static void Set<SettingsType>(SettingsType settingsType, bool value) where SettingsType : struct, IComparable, IFormattable, IConvertible
        {
            AndroidSettings<SettingsType>.Set(settingsType, value);
        }

        public static bool Get<SettingsType>(SettingsType settingsType) where SettingsType : struct, IComparable, IFormattable, IConvertible
        {
            return AndroidSettings<SettingsType>.GetBool(settingsType);
        }

        public static string GetAttributeName(PropertyInfo field)
        {
            System.Xml.Serialization.XmlAttributeAttribute xmlAttribute = field.GetFieldAttribute<System.Xml.Serialization.XmlAttributeAttribute>();
            string attributeName = xmlAttribute.AttributeName;
            if (String.IsNullOrEmpty(attributeName))
                attributeName = field.Name;

            return attributeName;
        }

        public static void Deserialize(object resultObject, XmlNode node)
        {
            foreach (PropertyInfo field in resultObject.GetType().GetProperties().Where(p => p.GetFieldAttribute<System.Xml.Serialization.XmlAttributeAttribute>() != null))
            {
                string valueString = node.SingleOrDefaultChildNode(GetAttributeName(field))?.InnerText;

                object value = null;
                Type fieldType = field.PropertyType;
                if (Nullable.GetUnderlyingType(fieldType) != null)
                    fieldType = Nullable.GetUnderlyingType(fieldType);

                if (valueString == null)
                {
                    value = fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null;
                }
                else if (fieldType.IsEnum)
                {
                    value = Enum.Parse(fieldType, valueString);
                }
                else
                {
                    switch (fieldType.Name)
                    {
                        case nameof(String):
                            value = valueString;
                            break;
                        case nameof(Boolean):
                            value = Boolean.Parse(valueString);
                            break;
                        case nameof(Int32):
                            value = Int32.Parse(valueString);
                            break;
                        case nameof(DateTime):
                            value = DateTime.Parse(valueString);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                field.SetValue(resultObject, value);
            }
        }

        public static List<TEnum> DeserializeListOfEnums<TEnum>(XmlNode node, string child1NodeName, string child2NodeName, string valueNodeName) where TEnum : struct
        {
            List<TEnum> result = new List<TEnum>();

            XmlNode elementsNode = node.SingleOrDefaultChildNode(child1NodeName);
            if (elementsNode != null)
                foreach (XmlNode module in elementsNode.GetChildNodes(child2NodeName))
                    result.Add(Enum.Parse<TEnum>(module.SingleChildNode(valueNodeName).InnerText));

            return result;
        }

        public static void Serialize(object resultObject, XmlDocument document, XmlElement node)
        {
            foreach (PropertyInfo field in resultObject.GetType().GetProperties().Where(p => p.GetFieldAttribute<System.Xml.Serialization.XmlAttributeAttribute>() != null))
            {
                XmlOperations.AddTextChild(document, node, GetAttributeName(field), field.GetValue(resultObject).ToString());
            }
        }

        public static void SerializeListOfEnums<TEnum>(List<TEnum> list, XmlElement node, XmlDocument document, string child1NodeName, string child2NodeName, string valueNodeName) where TEnum : struct
        {
            if (list != null)
            {
                XmlElement modulesElement = document.CreateElement(String.Empty, child1NodeName, String.Empty);
                node.AppendChild(modulesElement);
                foreach (TEnum module in list)
                {
                    XmlElement moduleElement = document.CreateElement(String.Empty, child2NodeName, String.Empty);
                    XmlOperations.AddTextChild(document, moduleElement, valueNodeName, module.ToString());
                    modulesElement.AppendChild(moduleElement);
                }
            }
        }
    }
}