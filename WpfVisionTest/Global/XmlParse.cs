using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;


namespace WpfVisionTest.MvdXmlParse
{
    public class CMvdNodeBase
    {
        string m_strName = string.Empty;
        string m_strDescription = string.Empty;
        string m_strDisplayName = string.Empty;
        string m_strVisibility = string.Empty;
        string m_strAccessMode = string.Empty;
        int m_nAlgorithmIndex = 0;

        public string Name
        {
            get
            {
                return m_strName;
            }
            set
            {
                m_strName = value;
            }
        }

        public string Description
        {
            get
            {
                return m_strDescription;
            }
            set
            {
                m_strDescription = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return m_strDisplayName;
            }
            set
            {
                m_strDisplayName = value;
            }
        }

        public string Visibility
        {
            get
            {
                return m_strVisibility;
            }
            set
            {
                m_strVisibility = value;
            }
        }

        public string AccessMode
        {
            get
            {
                return m_strAccessMode;
            }
            set
            {
                m_strAccessMode = value;
            }
        }

        public int AlgorithmIndex
        {
            get
            {
                return m_nAlgorithmIndex;
            }
            set
            {
                m_nAlgorithmIndex = value;
            }
        }
    }

    public class CMvdNodeInteger : CMvdNodeBase
    {
        int m_nCurValue = 0;
        int m_nDefaultValue = 0;
        int m_nMinValue = 0;
        int m_nMaxValue = 0;
        int m_nIncValue = 0;

        public int CurValue
        {
            get
            {
                return m_nCurValue;
            }
            set
            {
                m_nCurValue = value;
            }
        }

        public int DefaultValue
        {
            get
            {
                return m_nDefaultValue;
            }
            set
            {
                m_nDefaultValue = value;
            }
        }

        public int MinValue
        {
            get
            {
                return m_nMinValue;
            }
            set
            {
                m_nMinValue = value;
            }
        }

        public int MaxValue
        {
            get
            {
                return m_nMaxValue;
            }
            set
            {
                m_nMaxValue = value;
            }
        }

        public int IncValue
        {
            get
            {
                return m_nIncValue;
            }
            set
            {
                m_nIncValue = value;
            }
        }
    }

    public class CMvdNodeEnumEntry
    {
        string m_strName = string.Empty;
        string m_strDescription = string.Empty;
        string m_strDisplayName = string.Empty;
        bool m_bIsImplemented = false;
        int m_nValue = 0;

        public string Name
        {
            get
            {
                return m_strName;
            }
            set
            {
                m_strName = value;
            }
        }

        public string Description
        {
            get
            {
                return m_strDescription;
            }
            set
            {
                m_strDescription = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return m_strDisplayName;
            }
            set
            {
                m_strDisplayName = value;
            }
        }

        public bool IsImplemented
        {
            get
            {
                return m_bIsImplemented;
            }
            set
            {
                m_bIsImplemented = value;
            }
        }

        public int Value
        {
            get
            {
                return m_nValue;
            }
            set
            {
                m_nValue = value;
            }
        }
    }

    public class CMvdNodeFloat : CMvdNodeBase
    {
        float m_fCurValue = 0;
        float m_fDefaultValue = 0;
        float m_fMinValue = 0;
        float m_fMaxValue = 0;
        float m_fIncValue = 0;

        public float CurValue
        {
            get
            {
                return m_fCurValue;
            }
            set
            {
                m_fCurValue = value;
            }
        }

        public float DefaultValue
        {
            get
            {
                return m_fDefaultValue;
            }
            set
            {
                m_fDefaultValue = value;
            }
        }

        public float MinValue
        {
            get
            {
                return m_fMinValue;
            }
            set
            {
                m_fMinValue = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return m_fMaxValue;
            }
            set
            {
                m_fMaxValue = value;
            }
        }

        public float IncValue
        {
            get
            {
                return m_fIncValue;
            }
            set
            {
                m_fIncValue = value;
            }
        }
    }

    public class CMvdNodeBoolean : CMvdNodeBase
    {
        bool m_bDefaultValue = false;
        bool m_bCurValue = false;

        public bool CurValue
        {
            get
            {
                return m_bCurValue;
            }
            set
            {
                m_bCurValue = value;
            }
        }

        public bool DefaultValue
        {
            get
            {
                return m_bDefaultValue;
            }
            set
            {
                m_bDefaultValue = value;
            }
        }
    }

    public class CMvdNodeEnumeration : CMvdNodeBase
    {
        CMvdNodeEnumEntry m_eCurValue = new CMvdNodeEnumEntry();
        CMvdNodeEnumEntry m_eDefaultValue = new CMvdNodeEnumEntry();
        List<CMvdNodeEnumEntry> m_listEnumEntry = new List<CMvdNodeEnumEntry>();

        public CMvdNodeEnumEntry CurValue
        {
            get
            {
                return m_eCurValue;
            }
            set
            {
                m_eCurValue = value;
            }
        }

        public CMvdNodeEnumEntry DefaultValue
        {
            get
            {
                return m_eDefaultValue;
            }
            set
            {
                m_eDefaultValue = value;
            }
        }

        public List<CMvdNodeEnumEntry> EnumRange
        {
            get
            {
                return m_listEnumEntry;
            }
            set
            {
                m_listEnumEntry = value;
            }
        }
    }

    public class CMvdXmlParseTool
    {
        private List<CMvdNodeInteger> m_listIntValue = null;
        private List<CMvdNodeEnumeration> m_listEnumValue = null;
        private List<CMvdNodeFloat> m_listFloatValue = null;
        private List<CMvdNodeBoolean> m_listBooleanValue = null;

        public CMvdXmlParseTool(Byte[] bufXml, uint nXmlLen)
        {
            m_listIntValue = new List<CMvdNodeInteger>();
            m_listEnumValue = new List<CMvdNodeEnumeration>();
            m_listFloatValue = new List<CMvdNodeFloat>();
            m_listBooleanValue = new List<CMvdNodeBoolean>();
            UpdateXmlBuf(bufXml, nXmlLen);
        }

        public List<CMvdNodeInteger> IntValueList
        {
            get
            {
                return m_listIntValue;
            }
        }

        public List<CMvdNodeEnumeration> EnumValueList
        {
            get
            {
                return m_listEnumValue;
            }
        }

        public List<CMvdNodeFloat> FloatValueList
        {
            get
            {
                return m_listFloatValue;
            }
        }

        public List<CMvdNodeBoolean> BooleanValueList
        {
            get
            {
                return m_listBooleanValue;
            }
        }

        public void UpdateXmlBuf(Byte[] bufXml, uint nXmlLen)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;    //忽略文档里面的注释
            XmlReader reader = XmlReader.Create(new MemoryStream(bufXml, 0, (int)nXmlLen), settings);
            xmlDoc.Load(reader);
            reader.Close();
            m_listIntValue.Clear();
            m_listEnumValue.Clear();
            m_listFloatValue.Clear();
            m_listBooleanValue.Clear();
            XmlNode xnCategory = xmlDoc.SelectSingleNode("AlgorithmRoot").SelectSingleNode("Category");
            foreach (XmlNode xn in xnCategory)
            {
                switch (xn.Name)
                {
                    case "Integer":
                        {
                            CMvdNodeInteger NodeInt = new CMvdNodeInteger();
                            NodeInt.Name = ((XmlElement)xn).GetAttribute("Name");
                            NodeInt.Description = xn.SelectSingleNode("Description").InnerText;
                            NodeInt.DisplayName = xn.SelectSingleNode("DisplayName").InnerText;
                            NodeInt.Visibility = xn.SelectSingleNode("Visibility").InnerText;
                            NodeInt.AccessMode = xn.SelectSingleNode("AccessMode").InnerText;
                            NodeInt.AlgorithmIndex = IntStringToInt(xn.SelectSingleNode("AlgorithmIndex").InnerText);
                            NodeInt.CurValue = IntStringToInt(xn.SelectSingleNode("CurValue").InnerText);
                            NodeInt.DefaultValue = IntStringToInt(xn.SelectSingleNode("DefaultValue").InnerText);
                            NodeInt.MinValue = IntStringToInt(xn.SelectSingleNode("MinValue").InnerText);
                            NodeInt.MaxValue = IntStringToInt(xn.SelectSingleNode("MaxValue").InnerText);
                            NodeInt.IncValue = IntStringToInt(xn.SelectSingleNode("IncValue").InnerText);
                            m_listIntValue.Add(NodeInt);
                        }
                        break;
                    case "Enumeration":
                        {
                            CMvdNodeEnumeration NodeEnum = new CMvdNodeEnumeration();
                            NodeEnum.Name = ((XmlElement)xn).GetAttribute("Name");
                            NodeEnum.Description = xn.SelectSingleNode("Description").InnerText;
                            NodeEnum.DisplayName = xn.SelectSingleNode("DisplayName").InnerText;
                            NodeEnum.Visibility = xn.SelectSingleNode("Visibility").InnerText;
                            NodeEnum.AccessMode = xn.SelectSingleNode("AccessMode").InnerText;
                            NodeEnum.AlgorithmIndex = IntStringToInt(xn.SelectSingleNode("AlgorithmIndex").InnerText);
                            int nCurValue = IntStringToInt(xn.SelectSingleNode("CurValue").InnerText);
                            int nDefaultValue = IntStringToInt(xn.SelectSingleNode("DefaultValue").InnerText);
                            XmlNodeList xnlEnumEntry = xn.SelectNodes("EnumEntry");
                            List<CMvdNodeEnumEntry> clistNodeEnumEntry = new List<CMvdNodeEnumEntry>();
                            foreach (XmlNode xnEnumEntry in xnlEnumEntry)
                            {
                                CMvdNodeEnumEntry cNodeEnumEntry = new CMvdNodeEnumEntry();
                                cNodeEnumEntry.Name = ((XmlElement)xnEnumEntry).GetAttribute("Name");
                                cNodeEnumEntry.Description = xnEnumEntry.SelectSingleNode("Description").InnerText;
                                cNodeEnumEntry.DisplayName = xnEnumEntry.SelectSingleNode("DisplayName").InnerText;
                                cNodeEnumEntry.Value = IntStringToInt(xnEnumEntry.SelectSingleNode("Value").InnerText);
                                clistNodeEnumEntry.Add(cNodeEnumEntry);
                                if (nCurValue == cNodeEnumEntry.Value)
                                {
                                    NodeEnum.CurValue = cNodeEnumEntry;
                                }
                                if (nDefaultValue == cNodeEnumEntry.Value)
                                {
                                    NodeEnum.DefaultValue = cNodeEnumEntry;
                                }
                            }
                            NodeEnum.EnumRange = clistNodeEnumEntry;
                            m_listEnumValue.Add(NodeEnum);
                        }
                        break;
                    case "Float":
                        {
                            CMvdNodeFloat NodeFloat = new CMvdNodeFloat();
                            NodeFloat.Name = ((XmlElement)xn).GetAttribute("Name");
                            NodeFloat.Description = xn.SelectSingleNode("Description").InnerText;
                            NodeFloat.DisplayName = xn.SelectSingleNode("DisplayName").InnerText;
                            NodeFloat.Visibility = xn.SelectSingleNode("Visibility").InnerText;
                            NodeFloat.AccessMode = xn.SelectSingleNode("AccessMode").InnerText;
                            NodeFloat.AlgorithmIndex = IntStringToInt(xn.SelectSingleNode("AlgorithmIndex").InnerText);
                            NodeFloat.CurValue = System.Convert.ToSingle(xn.SelectSingleNode("CurValue").InnerText);
                            NodeFloat.DefaultValue = System.Convert.ToSingle(xn.SelectSingleNode("DefaultValue").InnerText);
                            NodeFloat.MinValue = System.Convert.ToSingle(xn.SelectSingleNode("MinValue").InnerText);
                            NodeFloat.MaxValue = System.Convert.ToSingle(xn.SelectSingleNode("MaxValue").InnerText);
                            NodeFloat.IncValue = System.Convert.ToSingle(xn.SelectSingleNode("IncValue").InnerText);
                            m_listFloatValue.Add(NodeFloat);
                        }
                        break;
                    case "Boolean":
                        {
                            CMvdNodeBoolean NodeBoolean = new CMvdNodeBoolean();
                            NodeBoolean.Name = ((XmlElement)xn).GetAttribute("Name");
                            NodeBoolean.Description = xn.SelectSingleNode("Description").InnerText;
                            NodeBoolean.DisplayName = xn.SelectSingleNode("DisplayName").InnerText;
                            NodeBoolean.Visibility = xn.SelectSingleNode("Visibility").InnerText;
                            NodeBoolean.AccessMode = xn.SelectSingleNode("AccessMode").InnerText;
                            NodeBoolean.AlgorithmIndex = IntStringToInt(xn.SelectSingleNode("AlgorithmIndex").InnerText);
                            NodeBoolean.CurValue = xn.SelectSingleNode("CurValue").InnerText.Equals("true", StringComparison.OrdinalIgnoreCase) == true ? true : false;
                            NodeBoolean.DefaultValue = xn.SelectSingleNode("DefaultValue").InnerText.Equals("true", StringComparison.OrdinalIgnoreCase) == true ? true : false;
                            m_listBooleanValue.Add(NodeBoolean);
                        }
                        break;
                    default:
                        {
                            throw new VisionDesigner.MvdException(VisionDesigner.MVD_MODULE_TYPE.MVD_MODUL_APP
                                                                , VisionDesigner.MVD_ERROR_CODE.MVD_E_SUPPORT
                                                                , "Algorithm type not support!");
                        }
                }
            }
        }

        public void ClearXmlBuf()
        {
            m_listIntValue.Clear();
            m_listEnumValue.Clear();
            m_listFloatValue.Clear();
            m_listBooleanValue.Clear();
        }

        private int IntStringToInt(string strIntString)
        {
            if (strIntString.Contains("0x") || strIntString.Contains("0X"))
            {
                return Convert.ToInt32(strIntString, 16);
            }
            else
            {
                return Convert.ToInt32(strIntString, 10);
            }
        }
    }
}

