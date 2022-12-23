using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using Microsoft.Xrm.Sdk.Metadata;

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
            } else if (attr is EntityReference)
            {
                recordToUpdate[fieldName] = new EntityReference((attr as EntityReference).LogicalName, new Guid(newValue));
            }
            else if (attr is Guid)
            {
                recordToUpdate[fieldName] = new Guid(newValue);
            } 
            else if (attr is Money)
            {
                recordToUpdate[fieldName] = new Money(decimal.Parse(newValue));
            }
            else if (attr is DateTime)
            {
                recordToUpdate[fieldName] = DateTime.Parse(newValue);
            }
        }
    }
}
