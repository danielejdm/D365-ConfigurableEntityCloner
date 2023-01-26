using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static ConfigurableEntityCloner.Common;

namespace ConfigurableEntityCloner
{
    public class EntityClonerXmlParserService : IEntityClonerXmlParserService
    {
        private IOrganizationService organizationService;
        public EntityClonerXmlParserService(IOrganizationService organizationService)
        {
            this.organizationService = organizationService;
        }
        /// <summary>
        /// Extract the property clone-behaviour from the xml-element
        /// </summary>
        /// <param name="element">Xml element</param>
        /// <returns>CloneBehaviour - default Associate</returns>
        public CloneBehaviour GetCloneBehaviour(XElement element)
        {
            CloneBehaviour cloneBehaviour = CloneBehaviour.Associate;

            if (element.Attributes().Any(x => x.Name == "clone-behaviour"))
            {
                switch (element.Attributes().Where(x => x.Name == "clone-behaviour").First().Value)
                {
                    case "clone":
                        cloneBehaviour = CloneBehaviour.Clone;
                        break;
                }
                element.Attributes().Where(x => x.Name == "clone-behaviour").Remove();
            }

            return cloneBehaviour;
        }

        /// <summary>
        /// Merge more config-xml, injecting each config-xml in the corresponding link-entity tag with attribute "merge-config-id"
        /// </summary>
        /// <param name="configXml"></param>
        /// <returns>Merged Config</returns>
        public XDocument MergeConfigurations(XDocument configXml)
        {
            var copyConfigXml = XDocument.Parse(configXml.ToString());

            var mergeDescendants = copyConfigXml.Descendants().Where(d => d.Attributes().Any(a => a.Name == "merge-config-id"));

            if (mergeDescendants.Count() == 0)
            {
                return copyConfigXml;
            }

            foreach (var c in mergeDescendants)
            {
                var fromField = c.Attribute("from").Value;
                var toField = c.Attribute("to").Value;
                var entityName = c.Attribute("name").Value;

                var configId = Guid.Parse(c.Attribute("merge-config-id").Value);

                var config = this.organizationService.Retrieve("jdm_configuration", configId, new ColumnSet(true));

                var mergeConfigXml = XElement.Parse(config.GetAttributeValue<string>("jdm_configvalue"));


                var subXml = XDocument.Parse(mergeConfigXml.Descendants().Where(d => d.Name == "entity" && d.Attribute("name").Value == entityName).First().ToString());
                subXml.Descendants("condition").Where(x => x.Attribute("attribute")?.Value == toField).Remove();

                copyConfigXml.Descendants("link-entity").Where(e => e.Attribute("merge-config-id").Value == configId.ToString()).First().AddFirst(subXml.Descendants("entity").Elements());
                copyConfigXml.Descendants("link-entity").Where(e => e.Attribute("merge-config-id").Value == configId.ToString()).First().Attribute("merge-config-id").Remove();
            }

            return this.MergeConfigurations(copyConfigXml);
        }

        /// <summary>
        /// Get the link type of a relation
        /// </summary>
        /// <param name="element">fetch-element</param>
        /// <returns>Relation/Association/Connection</returns>
        public EntityLinkType? GetFirstLevelLinkType(XElement element)
        {
            EntityLinkType? linkType = null;

            if (element.Attribute("intersect") == null || element.Attribute("intersect").Value != "true")
            {
                if (element.Attribute("from").Value.Equals(element.Attribute("name").Value + "id"))
                {
                    linkType = EntityLinkType.Relation_N1;
                }
                else
                {
                    linkType = EntityLinkType.Relation_1N;
                }
            }
            else if (element.Attribute("intersect") != null || element.Attribute("intersect").Value == "true")
            {
                linkType = EntityLinkType.Association;
            }
            else if (element.Attributes().Where(x => x.Name == "link-entity").First().Value == "connection")
            {
                linkType = EntityLinkType.Connection;
            }

            return linkType;
        }
        /// <summary>
        /// Get the attribute list from the fetch-xml element
        /// </summary>
        /// <param name="element">fetch-xml element</param>
        /// <returns></returns>
        public IEnumerable<string> GetAttributeList(XElement element)
        {
            var fieldsList = from a in element.Descendants()
                             where a.Name == "attribute"
                             select a.Attribute("name").Value;

            return fieldsList;
        }

        /// Get the attribute list from the connection link-entity element
        /// </summary>
        /// <param name="element">fetch-xml element</param>
        /// <returns></returns>
        public IEnumerable<string> GetAttributeListForConnections(XElement element, string entityName)
        {
            var query = element.Descendants("link-entity").Where(x => x.Attribute("name").Value == entityName).FirstOrDefault();

            return this.GetAttributeList(query);
        }
    }
}
