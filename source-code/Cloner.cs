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

            if(queryClone.Descendants().FirstOrDefault().Attributes().Where(x => x.Name == "name").First().Value == "connection")
            {
                CloneEntityConnections(queryClone);
            }


            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();
            var queryCloneAllAttributes = XElement.Parse(queryClone.ToString());
            var exclude_attributes = queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";
            queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "exclude-attributes").Remove();

            //var ignore_system_attributes = queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "ignore-system-attributes").First().Value == "true";
            //queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "ignore-system-attributes").Remove(); 

            queryCloneAllAttributes.Descendants().Where(x => x.Name == "attribute").Remove();
            queryCloneAllAttributes.Descendants("entity").FirstOrDefault().AddFirst("<all-attribute/>");

            var record = this.orgService.RetrieveMultiple(new FetchExpression(queryClone.ToString())).Entities.FirstOrDefault();
            var fields = from a in queryClone.Descendants() where a.Name == "attribute"
                         select new AttributeInfo(a);

            tracingService.Trace($"Start cloning root entity '{record.LogicalName}: {record.Id}'");

            var clone = new Entity();
            clone.LogicalName = record.LogicalName;

            var originalRecord = new Entity(record.LogicalName) { Id= record.Id };
            var updateRecord = false;

            foreach (var f in fields)
            {
                if (Helper.CopyAttribute(exclude_attributes, record, f.Name))
                {
                    clone.Attributes.Add(f.Name, record[f.Name]);
                }

                if(f.OriginalNewValue != null)
                {
                    updateRecord= true;
                    originalRecord.Attributes.Add(f.Name, f.OriginalNewValue);
                }
            }
            var cloneId = this.orgService.Create(clone);

            if (updateRecord)
            {
                this.orgService.Update(originalRecord);
            }

            tracingService.Trace($"Successfully cloned root entity '{clone.LogicalName} : {clone.Id}'");

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
        /// Recursively clone normal link-entity (1:N || N:1 relationships)
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

            var queryCloneAllAttributes = XElement.Parse(queryClone.ToString());
            var exclude_attributes = queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";
            queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "exclude-attributes").Remove();

            //var ignore_system_attributes = queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "ignore-system-attributes").First().Value == "true";
            //queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "ignore-system-attributes").Remove(); 

            queryCloneAllAttributes.Descendants().Where(x => x.Name == "attribute").Remove();
            queryCloneAllAttributes.Descendants("entity").FirstOrDefault().AddFirst("<all-attribute/>");

            var fields = from a in queryClone.Descendants()
                         where a.Name == "attribute"
                         select new AttributeInfo(a);

            var records = this.orgService.RetrieveMultiple(new FetchExpression(linkentityQuery.ToString())).Entities;

            foreach (var record in records)
            {
                tracingService.Trace($"Start cloning link-entity '{record.LogicalName}: {record.Id}'");
                var clone = new Entity();
                clone.LogicalName = record.LogicalName;
                var originalRecord = new Entity(record.LogicalName) { Id = record.Id };
                var updateRecord = false;

                foreach (var f in fields)
                {
                    if (Helper.CopyAttribute(exclude_attributes, record, f.Name))
                    {
                        clone.Attributes.Add(f.Name, record[f.Name]);
                    }

                    if (f.OriginalNewValue != null)
                    {
                        updateRecord = true;
                        originalRecord.Attributes.Add(f.Name, f.OriginalNewValue);
                    }
                }

                clone.Attributes.Add(from, parentclonedid);

                var cloneId = this.orgService.Create(clone);

                if (updateRecord)
                {
                    this.orgService.Update(originalRecord);
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

            var queryAssociated = XElement.Parse(queryClone.ToString().Replace("associate-entity", "entity")).Descendants("entity").First();


            var queryCloneAllAttributes = XElement.Parse(queryAssociated.ToString());
            var exclude_attributes = queryCloneAllAttributes.Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";
           
            queryCloneAllAttributes.Attributes().Where(x => x.Name == "exclude-attributes").Remove();

            //var ignore_system_attributes = queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "ignore-system-attributes").First().Value == "true";
            //queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "ignore-system-attributes").Remove(); 

            var columnsList = from a in queryCloneAllAttributes.Descendants()
                              where a.Name == "attribute"
                              select new AttributeInfo(a);

            queryCloneAllAttributes.Descendants("entity").Where(x => x.Name == "attribute").Remove();
            queryCloneAllAttributes.Descendants().FirstOrDefault().AddFirst("<all-attribute/>");

            //var columnsList = from a in queryAssociated.Descendants() where a.Name == "attribute" select a.Attribute("name").Value;

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

                var record = this.orgService.Retrieve(toEntity, association.GetAttributeValue<Guid>(toEntityIdField), new ColumnSet(true));

                var clone = new Entity();
                clone.LogicalName = toEntity;
                var originalRecord = new Entity(record.LogicalName) { Id = record.Id };
                var updateRecord = false;

                foreach (var f in columnsList)
                {
                    if (Helper.CopyAttribute(exclude_attributes, record, f.Name))
                    {
                        clone.Attributes.Add(f.Name, record[f.Name]);
                    }

                    if (f.OriginalNewValue != null)
                    {
                        updateRecord = true;
                        originalRecord.Attributes.Add(f.Name, f.OriginalNewValue);
                    }
                }
                var cloneId = this.orgService.Create(clone);

                if (updateRecord)
                {
                    this.orgService.Update(originalRecord);
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
        /// <summary>
        /// Clone the connections (entity 'connection')
        /// </summary>
        /// <param name="element">The element with the intersect-entity</param>
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

            var fieldsEntity1 = from a in entity1Query.Descendants()
                              where a.Name == "attribute"
                              select new AttributeInfo(a);

            var fieldsEntity2 = from a in entity2Query.Descendants()
                                where a.Name == "attribute"
                                select new AttributeInfo(a);

            foreach(var c in connections)
            {
                var entity1 = this.orgService.Retrieve(record1entityname, record1id, new ColumnSet(true));
                var entity2 = this.orgService.Retrieve(record2entityname, record2id, new ColumnSet(true));

                var clone1 = new Entity();
                clone1.LogicalName = record1entityname;

                var exclude_attributes_e1 = entity1Query.Elements().Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";

                var originalRecord = new Entity(entity1.LogicalName) { Id = entity1.Id };
                var updateRecord = false;

                foreach (var f in fieldsEntity1)
                {
                    if (Helper.CopyAttribute(exclude_attributes_e1, entity1, f.Name))
                    {
                        clone1.Attributes.Add(f.Name, entity1[f.Name]);
                    }

                    if (f.OriginalNewValue != null)
                    {
                        updateRecord = true;
                        originalRecord.Attributes.Add(f.Name, f.OriginalNewValue);
                    }
                }

                var clone1Id = this.orgService.Create(clone1);

                if (updateRecord)
                {
                    this.orgService.Update(originalRecord);
                }

                var clone2 = new Entity();
                clone2.LogicalName = record2entityname;
                var exclude_attributes_e2 = entity1Query.Elements().Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";

                originalRecord = new Entity(entity1.LogicalName) { Id = entity1.Id };
                updateRecord = false;

                foreach (var f in fieldsEntity2)
                {
                    if (Helper.CopyAttribute(exclude_attributes_e1, entity2, f.Name))
                    {
                        clone2.Attributes.Add(f.Name, entity1[f.Name]);
                    }

                    if (f.OriginalNewValue != null)
                    {
                        updateRecord = true;
                        originalRecord.Attributes.Add(f.Name, f.OriginalNewValue);
                    }
                }

                var clone2Id = this.orgService.Create(clone2);

                var cloneConnection = new Entity("connection")
                {
                    ["record1id"] = clone1Id,
                    ["record2id"] = clone2Id,
                    ["record1roleid"] = record1roleid,
                    ["record2roleid"] = record2roleid,
                };

                this.orgService.Create(cloneConnection);
            }
        }
    }
}
