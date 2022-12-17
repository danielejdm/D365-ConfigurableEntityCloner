using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using System.Linq;
using System.Xml.Linq;

namespace ConfigurableEntityCloner
{
    /// <summary>
    /// Class for cloning the record and its linked entities from a fetch
    /// </summary>
    public class Cloner
    {
        private const string guidPlaceholder = "@id";
        private IOrganizationService orgService;
        private ITracingService tracingService;
        private Entity configuration;
        private string rootRecordId;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityContext">CodeActivity context.</param>
        public Cloner(CodeActivityContext activityContext)
        {
            IWorkflowContext wfContext = activityContext.GetExtension<IWorkflowContext>();
            this.tracingService = activityContext.GetExtension<ITracingService>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            this.orgService = serviceFactory.CreateOrganizationService(wfContext.UserId);
        }

        /// <summary>
        /// Main method / entry point
        /// </summary>
        /// <param name="configId">Id of the cofiguration record</param>
        /// <param name="rootRecordId">Id of the root record</param>
        /// <returns></returns>
        public string Clone(EntityReference configId, string rootRecordId)
        {
            this.configuration = this.orgService.Retrieve(configId.LogicalName, configId.Id, new ColumnSet(true));
            this.rootRecordId = rootRecordId;
            return CloneRoot();
        }
        /// <summary>
        /// Clone the root entity
        /// </summary>
        /// <param name="rootRecordId">Id of the root entity</param>
        /// <returns>Id of the cloned root entity</returns>
        private string CloneRoot()
        {
            var fetchXml = this.configuration.GetAttributeValue<string>("jdm_configvalue").Replace(guidPlaceholder, this.rootRecordId);

            XElement element = XElement.Parse(fetchXml);

            var queryClone = XElement.Parse(element.ToString());
            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();

            var record = this.orgService.RetrieveMultiple(new FetchExpression(queryClone.ToString())).Entities.FirstOrDefault();
            var fields = from a in queryClone.Descendants() where a.Name == "attribute" select a.Attribute("name");

            tracingService.Trace($"Start cloning root entity '{record.LogicalName}: {record.Id}'");

            var clone = new Entity();
            clone.LogicalName = record.LogicalName;
            foreach (var f in fields.Where(f => f.Value != "statecode" && f.Value != "statuscode"))
            {
                if (record.Contains(f.Value))
                {
                    clone.Attributes.Add(f.Value, record[f.Value]);
                }
            }
            var cloneId = this.orgService.Create(clone);

            tracingService.Trace($"Successfully cloned root entity '{clone.LogicalName} : {clone.Id}'");

            if (configuration.GetAttributeValue<bool>("jdm_clonestatus") == true &&
                record.Contains("statecode") && record.Contains("statuscode"))
            {
                tracingService.Trace($"Start cloning status '{clone.LogicalName}: {clone.Id}'");

                var upClone = new Entity(clone.LogicalName)
                {
                    Id = cloneId,
                    ["statecode"] = record["statecode"],
                    ["statuscode"] = record["statuscode"],
                };
                this.orgService.Update(upClone);

                tracingService.Trace($"Successfully cloned status '{clone.LogicalName}: {clone.Id}'");
            }

            foreach (var el in element.Descendants().Where(e => e.Name == "link-entity" && e.Parent.Name == "entity"))
            {
                CloneLinkedEntity(el, record.ToEntityReference(), new EntityReference(record.LogicalName, cloneId));
            }

            return cloneId.ToString();
        }

        /// <summary>
        /// Recursively clone all the linked entity
        /// </summary>
        /// <param name="element">Current linked entity</param>
        /// <param name="parentid">Id of the parent</param>
        /// <param name="parentclonedid">Id of the cloned parent</param>
        private void CloneLinkedEntity(XElement element, EntityReference parentid = null, EntityReference parentclonedid = null)
        {
            if (element.Name == "link-entity")
            {
                var queryClone = XElement.Parse(element.ToString());
                queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();

                var from = queryClone.Attribute("from").Value;
                var to = queryClone.Attribute("to").Value;
                var name = queryClone.Attribute("name").Value;

                var sfilter = $"<filter><condition attribute='{from}' operator='eq' value='{parentid.Id}'/></filter>";
                var linkentityQuery = XElement.Parse($"<fetch><entity name='{name}'>{sfilter}</entity></fetch>");

                linkentityQuery.Element("entity").AddFirst(queryClone.Elements());

                var fields = from a in linkentityQuery.Descendants() where a.Name == "attribute" select a.Attribute("name");
                var records = this.orgService.RetrieveMultiple(new FetchExpression(linkentityQuery.ToString())).Entities;

                foreach (var record in records)
                {
                    tracingService.Trace($"Start cloning link-entity '{record.LogicalName}: {record.Id}'");
                    var clone = new Entity();
                    clone.LogicalName = record.LogicalName;
                    foreach (var f in fields.Where(f => f.Value != "statecode" && f.Value != "statuscode"))
                    {
                        if (record.Contains(f.Value))
                        {
                            clone.Attributes.Add(f.Value, record[f.Value]);
                        }
                    }
                    clone.Attributes.Add(from, parentclonedid);

                    var cloneId = this.orgService.Create(clone);

                    if (this.configuration.GetAttributeValue<bool>("jdm_clonestatus") == true &&
                        record.Contains("statecode") && record.Contains("statuscode"))
                    {
                        tracingService.Trace($"Start cloning status '{clone.LogicalName}: {clone.Id}'");

                        var upClone = new Entity(clone.LogicalName)
                        {
                            Id = cloneId,
                            ["statecode"] = record["statecode"],
                            ["statuscode"] = record["statuscode"],
                        };
                        this.orgService.Update(upClone);

                        tracingService.Trace($"Successfully cloned status '{clone.LogicalName}: {clone.Id}'");
                    }

                    tracingService.Trace($"Successfully cloned link-entity '{clone.LogicalName}: {clone.Id}'");

                    var linkentities = element.Elements().Where(d => d.Name == "link-entity");

                    foreach (var le in linkentities)
                    {
                        CloneLinkedEntity(le, record.ToEntityReference(), new EntityReference(record.LogicalName, cloneId));
                    }
                }
            }
        }
    }
}
