using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace General.DataSerialize
{
    public class XmlSerializer
    {
        public class ListElementAttribute : Attribute
        {
            public Type m_Type;
            public ListElementAttribute(Type t)
            {
                m_Type = t;
            }
        }
        public static void ParseXML(IList result, Type t, XmlNodeList list)
        {
            int len = list.Count;
            for (int i = 0; i < len; ++i)
            {
                XmlNode xmlNode = list[i];
                object node = Activator.CreateInstance(t);
                XmlAttributeCollection coll = xmlNode.Attributes;
                foreach (XmlAttribute att in coll)
                {
                    FieldInfo field = t.GetField(att.Name);
                    if (field == null)
                        throw new Exception();
                    if (field.FieldType == typeof(int))
                        field.SetValue(node, att.Value == "" ? 0 : int.Parse(att.Value));
                    else if (field.FieldType == typeof(byte))
                        field.SetValue(node, att.Value == "" ? 0 : byte.Parse(att.Value));
                    else if (field.FieldType == typeof(short))
                        field.SetValue(node, att.Value == "" ? 0 : short.Parse(att.Value));
                    else if (field.FieldType == typeof(float))
                        field.SetValue(node, att.Value == "" ? 0 : float.Parse(att.Value));
                    else if (field.FieldType == typeof(string))
                        field.SetValue(node, att.Value);
                    else
                        throw new Exception("Type Error" + field.FieldType);

                }
                if (xmlNode.ChildNodes.Count > 0)
                {
                    FieldInfo cell = t.GetField("list" + xmlNode.ChildNodes[0].Name);
                    object[] listAtt = cell.GetCustomAttributes(false);
                    if (listAtt.Length > 0)
                    {
                        ParseXML(cell.GetValue(node) as IList, ((ListElementAttribute)listAtt[0]).m_Type, xmlNode.ChildNodes);
                    }
                }
                result.Add(node);
            }
        }
    }
}
