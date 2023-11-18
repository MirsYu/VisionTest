using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using WpfVisionTest.MvdXmlParse;

namespace WpfVisionTest.BlobFindTool
{
    public class UserControlBlobFindToolViewModel : BindableBase
    {
        private ObservableCollection<ToolRunValue> _beginnerToolRunValues;
        private ObservableCollection<ToolRunValue> _expertToolRunValues;

        public ObservableCollection<ToolRunValue> BeginnerToolRunValues
        {
            get { return _beginnerToolRunValues; }
            set { SetProperty(ref _beginnerToolRunValues, value); }
        }

        public ObservableCollection<ToolRunValue> ExpertToolRunValues
        {
            get { return _expertToolRunValues; }
            set { SetProperty(ref _expertToolRunValues, value); }
        }

        public UserControlBlobFindToolViewModel()
        {
            BeginnerToolRunValues = new ObservableCollection<ToolRunValue>();
            ExpertToolRunValues = new ObservableCollection<ToolRunValue>();
        }

        public void AddToolRunValue(string runParam, object value)
        {
            PropertyInfo m_Name = value.GetType().GetProperty("Name");
            PropertyInfo m_Value = value.GetType().GetProperty("CurValue");
            PropertyInfo m_Description = value.GetType().GetProperty("Description");
            PropertyInfo m_DisplayName = value.GetType().GetProperty("DisplayName");
            PropertyInfo m_Visibility = value.GetType().GetProperty("Visibility");
            PropertyInfo m_nMinValue = value.GetType().GetProperty("MinValue");
            PropertyInfo m_nMaxValue = value.GetType().GetProperty("MaxValue");
            PropertyInfo m_listEnumEntry = value.GetType().GetProperty("EnumRange");
            PropertyInfo m_IncValue = value.GetType().GetProperty("IncValue");

            if (Enum.TryParse<VisibilityType>(m_Visibility.GetValue(value).ToString(), out VisibilityType visibilityType))
            {
                // 当前值
                string strValue = "";
                var varTemp = m_Value.GetValue(value);
                if (varTemp is CMvdNodeEnumEntry)
                {
                    strValue = (varTemp as CMvdNodeEnumEntry).DisplayName;
                }
                else
                {
                    strValue = m_Value.GetValue(value).ToString();
                }
                // 最小值
                int iMinValue = 0;
                int.TryParse(m_nMinValue?.GetValue(value).ToString(), out iMinValue);
                // 最大值
                int iMaxValue = 0;
                int.TryParse(m_nMaxValue?.GetValue(value).ToString(), out iMaxValue);
                // 枚举
                List<CMvdNodeEnumEntry> lNodeEnum = new List<CMvdNodeEnumEntry>();
                lNodeEnum.Clear();
                lNodeEnum = m_listEnumEntry?.GetValue(value) as List<CMvdNodeEnumEntry>;
                // 枚举名称
                List<string> lEnumName = new List<string>();
                lEnumName.Clear();
                for (int iEnumCount = 0; iEnumCount < lNodeEnum?.Count; iEnumCount++)
                {
                    lEnumName.Add(lNodeEnum[iEnumCount].DisplayName.ToString());
                }

                // 枚举参数名称
                List<string> lEnumParamName = new List<string>();
                lEnumParamName.Clear();
                for (int iEnumParamCount = 0; iEnumParamCount < lNodeEnum?.Count; iEnumParamCount++)
                {
                    lEnumParamName.Add(lNodeEnum[iEnumParamCount].Name.ToString());
                }

                // 步长
                float fIncValue = 0;
                float.TryParse(m_IncValue?.GetValue(value).ToString(), out fIncValue);

                // 关联关系
                List<string> lAssValue = new List<string>();
                lAssValue.Clear();
                string strNameSelectBy = m_Name.GetValue(value).ToString();
                if (strNameSelectBy.IndexOf("SelectBy") != -1)
                {
                    string strAssName = strNameSelectBy.Replace("SelectBy", "");
                    lAssValue.Add("Min" + strAssName);
                    lAssValue.Add("Max" + strAssName);
                }

                var toolRunValue = new ToolRunValue
                {
                    _Name = m_Name.GetValue(value).ToString(),
                    _BaseType = value.GetType().Name,
                    _Value = strValue,
                    _RunParam = runParam,
                    _MinValue = iMinValue,
                    _MaxValue = iMaxValue,
                    _IncValue = fIncValue,
                    _EnumRange = lEnumName,
                    _EnumParamRange = lEnumParamName,
                    _Description = m_Description.GetValue(value).ToString(),
                    _DisplayName = m_DisplayName.GetValue(value).ToString(),
                    _Visibility = visibilityType,
                    _AssociatedValue = lAssValue
                };

                if (toolRunValue._Visibility == VisibilityType.Beginner)
                {
                    BeginnerToolRunValues.Add(toolRunValue);
                }
                else if (toolRunValue._Visibility == VisibilityType.Expert)
                {
                    ExpertToolRunValues.Add(toolRunValue);
                }
            }
            else
            {
                // Handle the case when parsing to VisibilityType fails
                // You can set a default VisibilityType value or handle the error as needed
            }
        }

    }
}
