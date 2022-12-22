using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Linq;
using System.Text.RegularExpressions;
using System;

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

        /// <summary>
        /// Find the Logical Name from the entity type code
        /// </summary>
        /// <param name="service">Organization service</param>
        /// <param name="entityTypeCode">Entity type code</param>
        /// <returns></returns>
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

        public static bool CheckIfAttributeIsCustom(string attributename)
        {
            var pattern = "^.*_";
            Regex rg = new Regex(pattern);

            return rg.IsMatch(attributename);
        }
        /// <summary>
        /// Check if the attribute has to be copied
        /// </summary>
        /// <param name="exclude_attributes">Attribute in the fetch to indicate weather the list of attribute are to copy or to ignore (black/white list)</param>
        /// <param name="record">The original record</param>
        /// <param name="attributename">The original record attribute</param>
        /// <returns></returns>
        public static bool CopyAttribute(bool exclude_attributes, Entity record, string attributename)
        {
            var blacklist = new string[] { record.LogicalName + "id", "statecode", "statuscode" };
            return exclude_attributes != true && record.Contains(attributename)
                && !blacklist.Contains(attributename);
        }

        public static void SetAttributeValue(ref Entity recordToUpdate, Entity record, string fieldName, string newValue)
        {
            if(!record.Contains(fieldName))
            {
                return;
            }
            var attr = record[fieldName];

            if (attr is string)
            {
                recordToUpdate.Attributes.Add(fieldName, newValue);
            }
            else if (attr is int)
            {
                recordToUpdate[fieldName] = int.Parse(newValue);
            } else if(attr is OptionSetValue)
            {
                recordToUpdate[fieldName] = new OptionSetValue(int.Parse(newValue));
            } else if (attr is decimal)
            {
                recordToUpdate[fieldName] = decimal.Parse(newValue);
            } else if (attr is bool)
            {
                recordToUpdate[fieldName] = bool.Parse(newValue);
            }
        }
    }
}
