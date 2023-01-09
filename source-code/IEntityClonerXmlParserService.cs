using System.Collections.Generic;
using System.Xml.Linq;
using static ConfigurableEntityCloner.Common;

namespace ConfigurableEntityCloner
{
    public interface IEntityClonerXmlParserService
    {
        CloneBehaviour GetCloneBehaviour(XElement element);
        XDocument MergeConfigurations(XDocument configXml);
        EntityLinkType? GetFirstLevelLinkType(XElement element);
        IEnumerable<string> GetAttributeList(XElement element);
        IEnumerable<string> GetAttributeListForConnections(XElement element, string entityName);

    }
}
