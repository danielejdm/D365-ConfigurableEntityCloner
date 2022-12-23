using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace ConfigurableEntityCloner
{
    public interface IClonerService
    {
        string GetEntityLogicalName(int entityTypeCode);
        IEnumerable<AttributeMetadata> GetEntityAttributesMetadata(Entity entity);
    }
}
