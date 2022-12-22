using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
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

            if(queryClone.Attributes().Where(x => x.Name == "entity").First().Value == "connection")
            {
                CloneEntityConnections(queryClone);
            }


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

            var linkEntities = element.Descendants().Where(e => e.Name == "link-entity" &&
                        e.Parent.Name == "entity");
            foreach (var el in linkEntities)
            {
                CloneChildren(el, record.ToEntityReference(), new EntityReference(record.LogicalName, cloneId));
            }

            return cloneId.ToString();
        }

        /// <summary>
        /// Recursively clone all the linked entity
        /// </summary>
        /// <param name="element">Current linked entity</param>
        /// <param name="parentid">Id of the parent</param>
        /// <param name="parentclonedid">Id of the cloned parent</param>
        private void CloneChildren(XElement element, EntityReference parentid = null, EntityReference parentclonedid = null)
        {
            if(element.Name != "link-entity")
            {
                return;
            }

            if (element.Attribute("intersect") == null || element.Attribute("intersect").Value != "true")
            {
                CloneLinkEntity(element, parentid, parentclonedid);
            }
            else if (element.Attribute("intersect") != null || element.Attribute("intersect").Value == "true")
            {
                CloneAssociateEntity(element, parentid, parentclonedid);
            }
            else if (element.Attributes().Where(x => x.Name == "link-entity").First().Value == "connection")
            {
                CloneEntityConnections(element);
            }
        }
        /// <summary>
        /// Clone normal link-entity (1:N || N:1 relationships)
        /// </summary>
        /// <param name="element">Current linked entity</param>
        /// <param name="parentid">Id of the parent</param>
        /// <param name="parentclonedid">Id of the cloned parent</param>
        private void CloneLinkEntity(XElement element, EntityReference parentid, EntityReference parentclonedid)
        {
            var queryClone = XElement.Parse(element.ToString());
            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();

            var from = queryClone.Attribute("from").Value;
            var to = queryClone.Attribute("to").Value;
            var name = queryClone.Attribute("name").Value;

            var sfilter = $"<filter><condition attribute='{from}' operator='eq' value='{parentid.Id}'/></filter>";
            var linkentityQuery = XElement.Parse($"<fetch><entity name='{name}'>{sfilter}</entity></fetch>");

            linkentityQuery.Element("entity").AddFirst(queryClone.Elements());

            var fields = from a in queryClone.Descendants() where a.Name == "attribute" select a.Attribute("name");
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
                    CloneChildren(le, record.ToEntityReference(), new EntityReference(record.LogicalName, cloneId));
                }
            }
        }

        /// <summary>
        /// Clone link-entity with intersect-entity (N:N relationships)
        /// </summary>
        /// <param name="element">Current linked entity</param>
        /// <param name="parentid">Id of the parent</param>
        /// <param name="parentclonedid">Id of the cloned parent</param>
        private void CloneAssociateEntity(XElement element, EntityReference parentid, EntityReference parentclonedid)
        {
            var queryClone = XElement.Parse(element.ToString());
            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();
            var queryAssociated = XElement.Parse(queryClone.ToString()).Descendants("associate-entity").First();

            var columnsList = from a in queryAssociated.Descendants() where a.Name == "attribute" select a.Attribute("name").Value;

            var columns = new ColumnSet(columnsList.ToArray());

            queryClone.Descendants().Where(x => x.Name == "associate-entity").Remove();

            var from = queryClone.Attribute("from").Value;
            var relationName = queryClone.Attribute("name").Value;
            var toEntity = queryAssociated.Attribute("name").Value;
            var toEntityIdField = toEntity + "id";

            var associatedEntityQuery = XElement.Parse($"<fetch><entity name='{relationName}'></entity></fetch>");

            associatedEntityQuery.Element("entity").AddFirst(queryClone.Elements());

            var associations = this.orgService.RetrieveMultiple(new FetchExpression(associatedEntityQuery.ToString())).Entities;

            foreach (var association in associations)
            {
                tracingService.Trace($"Start cloning association '{association.LogicalName}'");

                var record = this.orgService.Retrieve(toEntity, association.GetAttributeValue<Guid>(toEntityIdField), columns);

                var clone = new Entity();
                clone.LogicalName = toEntity;
                foreach (var f in columns.Columns.Where(f => f != "statecode" && f != "statuscode"))
                {
                    if (record.Contains(f))
                    {
                        clone.Attributes.Add(f, record[f]);
                    }
                }
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

                var entityReferenceCollection = new EntityReferenceCollection
                {
                    parentclonedid
                };

                this.orgService.Associate(toEntity, cloneId, new Relationship(relationName), entityReferenceCollection);

                tracingService.Trace($"Successfully clone association '{association.LogicalName}'");

                var linkentities = element.Elements().Where(d => d.Name == "link-entity");

                foreach (var le in linkentities)
                {
                    CloneChildren(le, association.ToEntityReference(), new EntityReference(association.LogicalName, cloneId));
                }
            }
        }
        private void CloneEntityConnections(XElement element)
        {
            var queryClone = XElement.Parse(element.ToString());

            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();

            var connections = this.orgService.RetrieveMultiple(new FetchExpression(queryClone.ToString())).Entities;

            var record1id = connections.FirstOrDefault().GetAttributeValue<Guid>("record1id");
            var record2id = connections.FirstOrDefault().GetAttributeValue<Guid>("record2id");
            var record1roleid = connections.FirstOrDefault().GetAttributeValue<Guid>("record1roleid");
            var record2roleid = connections.FirstOrDefault().GetAttributeValue<Guid>("record2roleid");
            var record1objecttypecode = connections.FirstOrDefault().GetAttributeValue<int>("record1objecttypecode");
            var record2objecttypecode = connections.FirstOrDefault().GetAttributeValue<int>("record2objecttypecode");
            var record1entityname = Helper.GetEntityLogicalName(this.orgService, record1objecttypecode);
            var record2entityname = Helper.GetEntityLogicalName(this.orgService, record2objecttypecode);

            var entity1Query = element.Descendants("link-entity").Where(x => x.Attribute("name").Value == record1entityname).FirstOrDefault();
            var entity2Query = element.Descendants("link-entity").Where(x => x.Attribute("name").Value == record2entityname).FirstOrDefault();

            var fieldsEntity1 = from a in entity1Query.Descendants() where a.Name == "attribute" select a.Attribute("name").Value;
            var fieldsEntity2 = from a in entity2Query.Descendants() where a.Name == "attribute" select a.Attribute("name").Value;

            foreach(var c in connections)
            {
                var entity1 = this.orgService.Retrieve(record1entityname, record1id, new ColumnSet(fieldsEntity1.ToArray()));
                var entity2 = this.orgService.Retrieve(record2entityname, record2id, new ColumnSet(fieldsEntity2.ToArray()));

                var clone1 = new Entity();
                clone1.LogicalName = record1entityname;
                foreach (var f in fieldsEntity1.Where(f => f != "statecode" && f != "statuscode"))
                {
                    if (entity1.Contains(f))
                    {
                        clone1.Attributes.Add(f, entity1[f]);
                    }
                }
                var clone1Id = this.orgService.Create(clone1);

                var clone2 = new Entity();
                clone1.LogicalName = record2entityname;
                foreach (var f in fieldsEntity2.Where(f => f != "statecode" && f != "statuscode"))
                {
                    if (entity1.Contains(f))
                    {
                        clone1.Attributes.Add(f, entity2[f]);
                    }
                }
                var clone2Id = this.orgService.Create(clone1);

                var cloneConnection = new Entity("connection")
                {
                    ["record1id"] = clone1Id,
                    ["record2id"] = clone2Id,
                    ["record1roleid"] = record1roleid,
                    ["record2roleid"] = record2roleid,
                };

                this.orgService.Create(cloneConnection);
            }

            //tracingService.Trace($"Start cloning root entity '{record.LogicalName}: {record.Id}'");
        }
    }
}
