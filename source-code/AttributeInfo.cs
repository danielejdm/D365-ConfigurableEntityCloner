using System;
using System.Xml.Linq;

namespace ConfigurableEntityCloner
{
    public class AttributeInfo
    {
        public string Name { get; }
        public string OriginalNewValue { get; }

        public AttributeInfo(XElement element)
        {
            if(element.Name != "attribute")
            {
                throw new Exception("XEelement name must be 'attribute'");
            }
            Name = element.Attribute("name").Value;
            OriginalNewValue = element.Attribute("original-new-value")?.Value;
        }
    }
}
