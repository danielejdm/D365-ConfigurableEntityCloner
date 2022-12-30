using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConfigurableEntityCloner
{
    public class ClonerService : IClonerService
    {
        private IOrganizationService organizationService;

        public ClonerService (IOrganizationService organizationService)
        {
            this.organizationService = organizationService; ;
        }
        /// <summary>
        /// Find the Logical Name from the entity type code
        /// </summary>
        /// <param name="service">Organization service</param>
        /// <param name="entityTypeCode">Entity type code</param>
        /// <returns></returns>
        public string GetEntityLogicalName(int entityTypeCode)
        {
            var entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode ", MetadataConditionOperator.Equals, entityTypeCode));
            var propertyExpression = new MetadataPropertiesExpression { AllProperties = false };
            propertyExpression.PropertyNames.Add("LogicalName");
            var entityQueryExpression = new EntityQueryExpression()
            {
                Criteria = entityFilter,
                Properties = propertyExpression
            };

            var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression
            };

            var response = (RetrieveMetadataChangesResponse)organizationService.Execute(retrieveMetadataChangesRequest);

            if (response.EntityMetadata.Count == 1)
            {
                return response.EntityMetadata[0].LogicalName;
            }
            return null;
        }

        public IEnumerable<AttributeMetadata> GetEntityAttributesMetadata(Entity entity)
        {
            RetrieveEntityRequest retrieveEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entity.LogicalName
            };
            RetrieveEntityResponse retrieveEntityResponse = (RetrieveEntityResponse)organizationService.Execute(retrieveEntityRequest);
            return retrieveEntityResponse.EntityMetadata.Attributes;
        }

        /// <summary>
        /// Merge more config-xml, injecting each config-xml in the corresponding link-entity tag with attribute "merge-config-id"
        /// </summary>
        /// <param name="configXml"></param>
        /// <returns></returns>
        public XDocument ExpandInnerConfigurations(XDocument configXml)
        {
            var copyConfigXml = XDocument.Parse(configXml.ToString());

            var mergeDescendants = copyConfigXml.Descendants().Where(d => d.Attributes().Any(a => a.Name == "merge-config-id"));

            if (mergeDescendants.Count() == 0)
            {
                return copyConfigXml;
            }

            foreach(var c in mergeDescendants)
            {
                var from = c.Attribute("from").Value;
                var to = c.Attribute("to").Value;
                var name = c.Attribute("name").Value;

                var configId = Guid.Parse(c.Attribute("merge-config-id").Value);

                var config = this.organizationService.Retrieve("jdm_configuration", configId, new ColumnSet(true));

                var mergeConfigXml = XElement.Parse(config.GetAttributeValue<string>("jdm_configvalue"));


                var subXml = XDocument.Parse(mergeConfigXml.Descendants().Where(d => d.Name == "entity" && d.Attribute("name").Value == name).First().ToString());
                subXml.Descendants("condition").Where(x => x.Attribute("attribute")?.Value == "contactid").Remove();

                copyConfigXml.Descendants("link-entity").Where(e => e.Attribute("merge-config-id").Value == configId.ToString()).First().AddFirst(subXml.Descendants("entity").Elements());
                copyConfigXml.Descendants("link-entity").Where(e => e.Attribute("merge-config-id").Value == configId.ToString()).First().Attribute("merge-config-id").Remove();
            }

            return this.ExpandInnerConfigurations(copyConfigXml);
        }
    }
}
