using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace WpfVisionTest.BlobFindTool
{
    public class UserControlBlobFindToolViewModel : BindableBase
    {
        private ObservableCollection<ToolRunValue> _toolRunValues;
        public ObservableCollection<ToolRunValue> ToolRunValues
        {
            get { return _toolRunValues; }
            set { SetProperty(ref _toolRunValues, value); }
        }

        public UserControlBlobFindToolViewModel() 
        {
            ToolRunValues = new ObservableCollection<ToolRunValue>
            {
                new ToolRunValue{ _RunParam = "111",_Value = "2222"}
            };
        }
    }
}
