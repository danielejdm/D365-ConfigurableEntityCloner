using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace ConfigurableEntityCloner
{
    public interface IMetaDataService
    {
        string GetEntityLogicalName(int entityTypeCode);
        IEnumerable<AttributeMetadata> GetAttributeMetadata(string entityName);
    }
}
