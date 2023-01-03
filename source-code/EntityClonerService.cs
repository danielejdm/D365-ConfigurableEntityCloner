using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConfigurableEntityCloner
{
    /// <summary>
    /// Class for cloning the record and its linked entities from a fetch-like Xml
    /// </summary>
    public class EntityClonerService
    {
        private const string guidPlaceholder = "@id";
        private IOrganizationService orgService;
        private ITracingService tracingService;
        private Entity configuration;
        private string rootRecordId;
        private MetaDataService metadataService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityContext">CodeActivity context.</param>
        public EntityClonerService(CodeActivityContext activityContext)
        {
            IWorkflowContext wfContext = activityContext.GetExtension<IWorkflowContext>();
            this.tracingService = activityContext.GetExtension<ITracingService>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            this.orgService = serviceFactory.CreateOrganizationService(wfContext.UserId);
            this.metadataService = new MetaDataService(orgService);
        }

        /// <summary>
        /// Main method / entry point
        /// </summary>
        /// <param name="configId">Id of the cofiguration record</param>
        /// <param name="rootRecordId">Id of the root record</param>
        /// <returns></returns>
        public string Clone(EntityReference configId, string rootRecordId)
        {
            this.rootRecordId = rootRecordId;
            this.configuration = this.orgService.Retrieve(configId.LogicalName, configId.Id, new ColumnSet(true));

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

            XDocument configDoc = this.MergeConfigurations(XDocument.Parse(fetchXml));

            var queryClone = XElement.Parse(configDoc.ToString());

            if (queryClone.Descendants().FirstOrDefault().Attributes().Where(x => x.Name == "name").First().Value == "connection")
            {
                CloneEntityConnections(queryClone);
            }

            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();
            var queryCloneAllAttributes = XElement.Parse(queryClone.ToString());
            var exclude_attributes = queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";
            queryCloneAllAttributes.Elements().Attributes().Where(x => x.Name == "exclude-attributes").Remove();

            queryCloneAllAttributes.Descendants().Where(x => x.Name == "attribute").Remove();

            queryCloneAllAttributes.Descendants("entity").FirstOrDefault().AddFirst(new XElement("all-attributes"));

            var record = this.orgService.RetrieveMultiple(new FetchExpression(queryCloneAllAttributes.ToString())).Entities.FirstOrDefault();
            var fetchFields = from a in queryClone.Descendants()
                              where a.Name == "attribute"
                              select a.Attribute("name").Value;

            tracingService.Trace($"Start cloning root entity '{record.LogicalName}: {record.Id}'");

            var clone = new Entity();
            clone.LogicalName = record.LogicalName;

            var attributeBlackList = metadataService.GetAttributeMetadata(record.LogicalName).Where(a => a.IsValidForCreate == false).Select(a => a.LogicalName);


            foreach (var f in record.Attributes.Where(a => a.Value != null))
            {
                if (this.CanCopyAttribute(exclude_attributes, record, f.Key, attributeBlackList, fetchFields))
                {
                    clone.Attributes.Add(f.Key, record[f.Key]);
                }
            }
            var cloneId = this.orgService.Create(clone);

            tracingService.Trace($"Successfully cloned root entity '{clone.LogicalName} : {clone.Id}'");

            var linkEntities = configDoc.Descendants().Where(e => e.Name == "link-entity" &&
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
            if (element.Name != "link-entity")
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
            var linkentityQuery = XElement.Parse($"<fetch><entity name='{name}'><all-attributes/>{sfilter}</entity></fetch>");

            var exclude_attributes = queryClone.Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";

            var fetchFields = from a in queryClone.Descendants()
                              where a.Name == "attribute"
                              select a.Attribute("name").Value;

            var records = this.orgService.RetrieveMultiple(new FetchExpression(linkentityQuery.ToString())).Entities;

            foreach (var record in records)
            {
                tracingService.Trace($"Start cloning link-entity '{record.LogicalName}: {record.Id}'");
                var clone = new Entity();
                clone.LogicalName = record.LogicalName;

                var attributeBlackList = metadataService.GetAttributeMetadata(record.LogicalName).Where(a => a.IsValidForCreate == false).Select(a => a.LogicalName);
                foreach (var f in record.Attributes.Where(a => a.Value != null && a.Key != from))
                {
                    if (this.CanCopyAttribute(exclude_attributes, record, f.Key, attributeBlackList, fetchFields))
                    {
                        clone.Attributes.Add(f.Key, record[f.Key]);
                    }
                }

                clone.Attributes.Add(from, parentclonedid);

                var cloneId = this.orgService.Create(clone);

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

            var reassociate = queryCloneAllAttributes.Attributes().Where(x => x.Name == "reassociate").First().Value == "true";
            queryCloneAllAttributes.Attributes().Where(x => x.Name == "reassociate").Remove();

            var columnsList = from a in queryCloneAllAttributes.Descendants()
                              where a.Name == "attribute"
                              select a.Attribute("name").Value;

            queryCloneAllAttributes.Elements().Where(x => x.Name == "attribute").Remove();
            queryCloneAllAttributes.AddFirst(new XElement("all-attributes"));

            queryClone.Descendants().Where(x => x.Name == "associate-entity").Remove();

            var from = queryClone.Attribute("from").Value;
            var relationName = queryClone.Attribute("name").Value;
            var toEntity = queryAssociated.Attribute("name").Value;
            var toEntityIdField = toEntity + "id";

            if(queryCloneAllAttributes.Descendants().Any(d => d.Name == "filter"))
            {
                queryCloneAllAttributes.Descendants().Where(d => d.Name == "filter").First().Add(XElement.Parse($"<condition attribute = '{toEntityIdField}' operator= 'eq' value = '@id'/>"));
            } else
            {
                queryCloneAllAttributes.Add(XElement.Parse($"<filter><condition attribute = '{toEntityIdField}' operator= 'eq' value = '@id'/></filter>"));
            }
            
            var associatedEntityQuery = XElement.Parse($"<fetch><entity name='{relationName}'></entity></fetch>");

            associatedEntityQuery.Element("entity").AddFirst(queryClone.Elements());

            var associations = this.orgService.RetrieveMultiple(new FetchExpression(associatedEntityQuery.ToString())).Entities;

            foreach (var association in associations)
            {
                tracingService.Trace($"Start cloning association '{association.LogicalName}'");

                Guid associateId = association.GetAttributeValue<Guid>(toEntityIdField);

                if (reassociate == false)
                {
                    var query = XElement.Parse("<fetch>" + queryCloneAllAttributes.ToString() + "</fetch>")
                        .ToString().Replace("@id", associateId.ToString());

                    var result = this.orgService.RetrieveMultiple(new FetchExpression(query)).Entities;

                    if (result.Count == 0)
                    {
                        continue;
                    }

                    var record = result.First();

                    var clone = new Entity();
                    clone.LogicalName = toEntity;

                    var attributeBlackList = metadataService.GetAttributeMetadata(record.LogicalName).Where(a => a.IsValidForCreate == false).Select(a => a.LogicalName);
                    foreach (var f in record.Attributes.Where(a => a.Value != null))
                    {
                        if (this.CanCopyAttribute(exclude_attributes, record, f.Key, attributeBlackList, columnsList))
                        {
                            clone.Attributes.Add(f.Key, record[f.Key]);
                        }
                    }
                    associateId = this.orgService.Create(clone);
                }

                var entityReferenceCollection = new EntityReferenceCollection
                    {
                        parentclonedid
                    };
                this.orgService.Associate(toEntity, associateId, new Relationship(relationName), entityReferenceCollection);

                tracingService.Trace($"Successfully clone association '{association.LogicalName}'");

                var linkentities = element.Elements().Where(d => d.Name == "link-entity");

                foreach (var le in linkentities)
                {
                    CloneChildren(le, association.ToEntityReference(), new EntityReference(association.LogicalName, associateId));
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
            var record1entityname = metadataService.GetEntityLogicalName(record1objecttypecode);
            var record2entityname = metadataService.GetEntityLogicalName(record2objecttypecode);

            var entityFromQuery = element.Descendants("link-entity").Where(x => x.Attribute("name").Value == record1entityname).FirstOrDefault();
            var entityToQuery = element.Descendants("link-entity").Where(x => x.Attribute("name").Value == record2entityname).FirstOrDefault();

            var fieldsEntityFrom = from a in entityFromQuery.Descendants()
                                   where a.Name == "attribute"
                                   select a.Attribute("name").Value;

            var fieldsEntityTo = from a in entityToQuery.Descendants()
                                 where a.Name == "attribute"
                                 select a.Attribute("name").Value;

            foreach (var c in connections)
            {
                var entityFrom = this.orgService.Retrieve(record1entityname, record1id, new ColumnSet(true));
                var entityTo = this.orgService.Retrieve(record2entityname, record2id, new ColumnSet(true));

                var cloneEntityFrom = new Entity();
                cloneEntityFrom.LogicalName = record1entityname;

                var exclude_attributes_efrom = entityFromQuery.Elements().Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";

                var attributeBlackListEfrom = metadataService.GetAttributeMetadata(entityFrom.LogicalName).Where(a => a.IsValidForCreate == false).Select(a => a.LogicalName);
                foreach (var f in entityFrom.Attributes.Where(a => a.Value != null))
                {
                    if (this.CanCopyAttribute(exclude_attributes_efrom, entityFrom, f.Key, attributeBlackListEfrom, fieldsEntityFrom))
                    {
                        cloneEntityFrom.Attributes.Add(f.Key, entityFrom[f.Key]);
                    }
                }

                var cloneEntityFromId = this.orgService.Create(cloneEntityFrom);

                var cloneEntityTo = new Entity();
                cloneEntityTo.LogicalName = record2entityname;
                var exclude_attributes_eto = entityFromQuery.Elements().Attributes().Where(x => x.Name == "exclude-attributes").First().Value == "true";

                var attributeBlackListEto = metadataService.GetAttributeMetadata(entityTo.LogicalName).Where(a => a.IsValidForCreate == false).Select(a => a.LogicalName);
                foreach (var f in entityTo.Attributes.Where(a => a.Value != null))
                {
                    if (this.CanCopyAttribute(exclude_attributes_eto, entityTo, f.Key, attributeBlackListEto, fieldsEntityTo))
                    {
                        cloneEntityTo.Attributes.Add(f.Key, entityFrom[f.Key]);
                    }
                }

                var cloneEntityToId = this.orgService.Create(cloneEntityTo);

                var cloneConnection = new Entity("connection")
                {
                    ["record1id"] = cloneEntityFromId,
                    ["record2id"] = cloneEntityToId,
                    ["record1roleid"] = record1roleid,
                    ["record2roleid"] = record2roleid,
                };

                this.orgService.Create(cloneConnection);
            }
        }
        /// <summary>
        /// Check if the attribute can be copied
        /// </summary>
        /// <param name="exclude_attributes">Attribute in the fetch to indicate weather the list of attribute are to copy or to ignore (black/white list)</param>
        /// <param name="record">The original record</param>
        /// <param name="attributename">The original record attribute</param>
        /// <returns>true/false</returns>
        private bool CanCopyAttribute(bool exclude_attributes, Entity record, string attributename, IEnumerable<string> attributeBlackList, IEnumerable<string> fetchFields)
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

        /// <summary>
        /// Merge more config-xml, injecting each config-xml in the corresponding link-entity tag with attribute "merge-config-id"
        /// </summary>
        /// <param name="configXml"></param>
        /// <returns></returns>
        private XDocument MergeConfigurations(XDocument configXml)
        {
            var copyConfigXml = XDocument.Parse(configXml.ToString());

            var mergeDescendants = copyConfigXml.Descendants().Where(d => d.Attributes().Any(a => a.Name == "merge-config-id"));

            if (mergeDescendants.Count() == 0)
            {
                return copyConfigXml;
            }

            foreach (var c in mergeDescendants)
            {
                var fromField = c.Attribute("from").Value;
                var toField = c.Attribute("to").Value;
                var entityName = c.Attribute("name").Value;

                var configId = Guid.Parse(c.Attribute("merge-config-id").Value);

                var config = this.orgService.Retrieve(this.configuration.LogicalName, configId, new ColumnSet(true));

                var mergeConfigXml = XElement.Parse(config.GetAttributeValue<string>("jdm_configvalue"));


                var subXml = XDocument.Parse(mergeConfigXml.Descendants().Where(d => d.Name == "entity" && d.Attribute("name").Value == entityName).First().ToString());
                subXml.Descendants("condition").Where(x => x.Attribute("attribute")?.Value == toField).Remove();

                copyConfigXml.Descendants("link-entity").Where(e => e.Attribute("merge-config-id").Value == configId.ToString()).First().AddFirst(subXml.Descendants("entity").Elements());
                copyConfigXml.Descendants("link-entity").Where(e => e.Attribute("merge-config-id").Value == configId.ToString()).First().Attribute("merge-config-id").Remove();
            }

            return this.MergeConfigurations(copyConfigXml);
        }
    }
}
