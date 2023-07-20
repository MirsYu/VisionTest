using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfVisionTest.BlobFindTool
{
    public struct ToolRunValue
    {
        public string _RunParam { get; set; }
        public string _Value { get; set; }

        public override string ToString()
        {
            return $"{_RunParam},{_Value}";
        }
    }
}
