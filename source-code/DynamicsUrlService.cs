using Microsoft.Xrm.Sdk;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Web;

namespace ConfigurableEntityCloner
{
    public static class DynamicsUrlService
    {
        /// <summary>
        /// Extract the record id from the querystring
        /// </summary>
        /// <param name="url">record url</param>
        /// <returns></returns>
        public static string GetRecordIdFromUrl(string url)
        {
            string[] urlParts = url.Split("?".ToArray());
            string[] urlParams = urlParts[1].Split("&".ToCharArray());
            string id = urlParams.Where(p => p.StartsWith("id")).FirstOrDefault().Replace("id=", "");
            return id;
        }
        /// <summary>
        /// Extract the entity name from the querystring
        /// </summary>
        /// <param name="url">record url</param>
        /// <param name="organizationService">organization service</param>
        /// <returns>entity name</returns>
        /// <exception cref="Exception"></exception>
        public static string GetEntityNameFromUrl(string url, IOrganizationService organizationService)
        {
            string entityName;

            string[] urlParts = url.Split("?".ToArray());
            string[] urlParams = urlParts[1].Split("&".ToCharArray());
            string etn = urlParams.Where(p => p.StartsWith("etn"))?.FirstOrDefault()?.Replace("etn=", "");
            string etc = urlParams.Where(p => p.StartsWith("etc"))?.FirstOrDefault()?.Replace("etc=", "");
            
            entityName = etn != null ? etn : etc != null ? 
                new MetaDataService(organizationService).GetEntityLogicalName(int.Parse(etc)) : 
                throw new Exception("RecordUrl not in a valid format!");

            return entityName;
        }
    }
}
