using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace ConfigurableEntityCloner
{
    public static class Helper
    {
        public static string GetRecordIdFromUrl(string url)
        {

            string[] urlParts = url.Split("?".ToArray());
            string[] urlParams = urlParts[1].Split("&".ToCharArray());
            var id = urlParams.Where(e => e.StartsWith("id=")).FirstOrDefault().Replace("id=", "");

            return id;
        }

        //Find the Logical Name from the entity type code - this needs a reference to the Organization Service to look up metadata
        public static string GetEntityLogicalName(IOrganizationService service, int entityTypeCode)
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

            var response = (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);

            if (response.EntityMetadata.Count == 1)
            {
                return response.EntityMetadata[0].LogicalName;
            }
            return null;
        }
    }
}
