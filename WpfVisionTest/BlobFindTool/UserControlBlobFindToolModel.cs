using System.Collections.Generic;
using System.ComponentModel;

namespace WpfVisionTest.BlobFindTool
{
    public enum VisibilityType
    {
        Beginner,
        Expert
    }

    public class ToolRunValue : INotifyPropertyChanged
    {
        private string _name;
        public string _Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("_Name");
                }
            }
        }

        private string _baseType;
        public string _BaseType
        {
            get { return _baseType; }
            set
            {
                if (_baseType != value)
                {
                    _baseType = value;
                    OnPropertyChanged("_BaseType");
                }
            }
        }

        private string _runParam;
        public string _RunParam
        {
            get { return _runParam; }
            set
            {
                if (_runParam != value)
                {
                    _runParam = value;
                    OnPropertyChanged("_RunParam");
                }
            }
        }

        private string _value;
        public string _Value
        {
            get
            {
                if (_BaseType == "CMvdNodeEnumeration" && _EnumRange.Contains(_value))
                {
                    return _EnumParamRange[_EnumRange.IndexOf(_value)];
                }
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("_Value");
                }
            }
        }

        private string _valueEnum;
        public string _ValueEnum
        {
            get
            {
                if (_BaseType == "CMvdNodeEnumeration" && _EnumRange.Contains(_value))
                {
                    return _EnumRange[_EnumRange.IndexOf(_value)];
                }
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("_Value");
                }
            }
        }

        private string _displayName;
        public string _DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged("_DisplayName");
                }
            }
        }

        private VisibilityType _visibility;
        public VisibilityType _Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    OnPropertyChanged("_Visibility");
                }
            }
        }

        private string _description;
        public string _Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("_Description");
                }
            }
        }

        private int _minValue;
        public int _MinValue
        {
            get { return _minValue; }
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    OnPropertyChanged("_MinValue");
                }
            }
        }

        private int _maxValue;
        public int _MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    OnPropertyChanged("_MaxValue");
                }
            }
        }

        private float _incValue;
        public float _IncValue
        {
            get { return _incValue; }
            set
            {
                if (_incValue != value)
                {
                    _incValue = value;
                    OnPropertyChanged("_IncValue");
                }
            }
        }

        private List<string> _enumRange;
        public List<string> _EnumRange
        {
            get { return _enumRange; }
            set
            {
                if (_enumRange != value)
                {
                    _enumRange = value;
                    OnPropertyChanged("_EnumRange");
                }
            }
        }

        private List<string> _enumParamRange;
        public List<string> _EnumParamRange
        {
            get { return _enumParamRange; }
            set
            {
                if (_enumParamRange != value)
                {
                    _enumParamRange = value;
                    OnPropertyChanged("_EnumParamRange");
                }
            }
        }

        private List<string> _associatedValue;
        public List<string> _AssociatedValue
        {
            get { return _associatedValue; }
            set
            {
                if (_associatedValue != value)
                {
                    _associatedValue = value;
                    OnPropertyChanged("_AssociatedValue");
                }
            }
        }

        public string TextToShow => IsSwitchOn ? "True" : "False";

        public bool IsSwitchOn
        {
            get { return _Value == "1"; }
            set
            {
                _Value = value ? "1" : "0";
                OnPropertyChanged(nameof(IsSwitchOn));
                OnPropertyChanged(nameof(TextToShow));
            }
        }

        public override string ToString()
        {
            return $"{_Name},{_BaseType},{_RunParam},{_Value},{_DisplayName},{_Visibility},{_Description},{_MinValue},{_MaxValue},{_IncValue},{IsSwitchOn},{TextToShow}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
