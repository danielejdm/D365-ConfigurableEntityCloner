using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
