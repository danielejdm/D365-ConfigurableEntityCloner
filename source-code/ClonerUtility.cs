using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurableEntityCloner
{
    public static class ClonerUtility
    {
        /// <summary>
        /// Check if the attribute can be copied
        /// </summary>
        /// <param name="exclude_attributes">Attribute in the config-xml to indicate weather the list of attributes are to copy or to ignore (black/white list)</param>
        /// <param name="record">The original record</param>
        /// <param name="attributename">The original record attribute</param>
        /// <returns>true/false</returns>
        public static bool CanCopyAttribute(bool exclude_attributes, Entity record, string attributename, IEnumerable<string> attributeBlackList, IEnumerable<string> fetchFields)
        {
            var canCopy = false;
            if (attributeBlackList.Any(a => a == attributename) || attributename == record.LogicalName + "id")
            {
                return false;
            }
            if (!exclude_attributes)
            {
                canCopy = record.Contains(attributename);
            }
            else
            {
                canCopy = !fetchFields.Any(a => a == attributename);
            }

            return canCopy;
        }

        public static void SetAttributeValue(ref Entity recordToUpdate, Entity record, string fieldName, string newValue)
        {
            if (!record.Contains(fieldName))
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
            }
            else if (attr is OptionSetValue)
            {
                recordToUpdate[fieldName] = new OptionSetValue(int.Parse(newValue));
            }
            else if (attr is decimal)
            {
                recordToUpdate[fieldName] = decimal.Parse(newValue);
            }
            else if (attr is bool)
            {
                recordToUpdate[fieldName] = bool.Parse(newValue);
            }
            else if (attr is EntityReference)
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
