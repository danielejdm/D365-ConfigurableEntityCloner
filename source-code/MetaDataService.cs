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
    public class MetaDataService : IMetaDataService
    {
        private IOrganizationService organizationService;

        public MetaDataService (IOrganizationService organizationService)
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
        /// <summary>
        /// Get the metadata of the attributes of the entity
        /// </summary>
        /// <param name="entityName">Entity name</param>
        /// <returns></returns>
        public IEnumerable<AttributeMetadata> GetAttributeMetadata(string entityName)
        {
            RetrieveEntityRequest retrieveEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entityName
            };
            RetrieveEntityResponse retrieveEntityResponse = (RetrieveEntityResponse)organizationService.Execute(retrieveEntityRequest);
            return retrieveEntityResponse.EntityMetadata.Attributes;
        }
        /// <summary>
        /// Filter the attribute metadata set excluding attributes not valid for being set (in creation and associate)
        /// </summary>
        /// <param name="entityName">Entity name</param>
        /// <returns></returns>
        public IEnumerable<string> GetAttributesBlacklist(string entityName)
        {
            return this.GetAttributeMetadata(entityName)
                .Where(a => a.IsValidForCreate == false || a.IsValidForRead == false ||
                 a.IsPrimaryId == true || a.AttributeType == AttributeTypeCode.Status).Select(a => a.LogicalName);
        }
    }
}
