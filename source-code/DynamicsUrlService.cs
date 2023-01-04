using Microsoft.Xrm.Sdk;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Web;

namespace ConfigurableEntityCloner
{
    public static class DynamicsUrlService
    {
        public static string GetRecordIdFromUrl(string url)
        {
            string[] urlParts = url.Split("?".ToArray());
            var querystringParams = HttpUtility.ParseQueryString(urlParts[1]);

            var id = querystringParams["id"];
            return id;
        }
    }
}
