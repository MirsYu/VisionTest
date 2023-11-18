using System.Windows;
using System.Windows.Controls;

namespace WpfVisionTest.BlobFindTool
{
    // 创建一个 DataTemplateSelector，用于根据节点的类型选择适当的 DataTemplate
    public class NodeDataTemplateSelector : DataTemplateSelector
    {
        // 创建 DataTemplate 用于 Integer 类型的节点
        public DataTemplate IntegerTemplate { get; set; }

        // 创建 DataTemplate 用于 Float 类型的节点
        public DataTemplate FloatTemplate { get; set; }

        // 创建 DataTemplate 用于 Boolean 类型的节点
        public DataTemplate BooleanTemplate { get; set; }

        // 创建 DataTemplate 用于 Enumeration 类型的节点
        public DataTemplate EnumerationTemplate { get; set; }

        // 新增属性用于存储 ToolRunValue 对象
        public ToolRunValue CurrentToolRunValue { get; set; }

        // 重写 SelectTemplate 方法来选择适当的 DataTemplate
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ToolRunValue toolRunValue)
            {
                // 将当前 ToolRunValue 赋值给属性
                CurrentToolRunValue = toolRunValue;

                // 根据节点的类型选择对应的 DataTemplate
                if (toolRunValue._BaseType == "CMvdNodeInteger")
                {
                    if (toolRunValue._MinValue == 0 && toolRunValue._MaxValue == 1)
                    {
                        return BooleanTemplate;
                    }
                    return IntegerTemplate;
                }

                else if (toolRunValue._BaseType == "CMvdNodeFloat")
                    return FloatTemplate;
                else if (toolRunValue._BaseType == "CMvdNodeBoolean")
                    return BooleanTemplate;
                else if (toolRunValue._BaseType == "CMvdNodeEnumeration")
                    return EnumerationTemplate;
            }
            // 默认的 DataTemplate（可以是空模板或任何其他备用模板）
            return base.SelectTemplate(item, container);
        }
    }
}
