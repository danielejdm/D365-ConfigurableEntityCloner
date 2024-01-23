using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static ConfigurableEntityCloner.Common;

namespace ConfigurableEntityCloner
{
    /// <summary>
    /// Class for cloning the record and its linked entities from a fetch-like Xml
    /// </summary>
    public class EntityClonerService
    {
        private string rootRecordId;
        private IOrganizationService orgService;
        private ITracingService tracingService;
        private Entity configuration;
        private IMetaDataService metaDataService;
        private IEntityClonerXmlParserService entityClonerXmlParserService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orgService">OrganizationService</param>
        /// <param name="tracingService">TracingService</param>
        /// <param name="metaDataService">MetaDataService</param>
        /// <param name="configuration">Configuration</param>
        public EntityClonerService(IOrganizationService orgService, ITracingService tracingService, IMetaDataService metaDataService, IEntityClonerXmlParserService entityClonerXmlParserService, Entity configuration)
        {
            this.orgService = orgService;
            this.tracingService = tracingService;
            this.metaDataService = metaDataService;
            this.entityClonerXmlParserService = entityClonerXmlParserService;
            this.configuration = configuration;
        }

        /// <summary>
        /// Main method / entry point
        /// </summary>
        /// <param name="rootRecordId">root record url</param>
        /// <returns>Id of the cloned root record</returns>
        public string Clone(string rootRecordId)
        {
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
            var fetchXml = this.configuration.GetAttributeValue<string>("jdm_configvalue");

            XDocument configDoc = this.entityClonerXmlParserService.MergeConfigurations(XDocument.Parse(fetchXml));

            var configEl = XElement.Parse(configDoc.ToString());

            configEl.Descendants().Where(x => x.Name == "link-entity").Remove();

            var entityIdField = configEl.Descendants("entity").FirstOrDefault().Attribute("name").Value + "id";

            if (configEl.Descendants().Any(d => d.Name == "filter"))
            {
                configEl.Descendants().Where(d => d.Name == "filter").First().Add(XElement.Parse($"<condition attribute = '{entityIdField}' operator= 'eq' value = '{rootRecordId}'/>"));
            }
            else
            {
                configEl.Descendants().Where(d => d.Name == "entity").First().Add(XElement.Parse($"<filter><condition attribute = '{entityIdField}' operator= 'eq' value = '{rootRecordId}'/></filter>"));
            }

            var record = this.orgService.RetrieveMultiple(new FetchExpression(configEl.ToString())).Entities.FirstOrDefault();
            
            tracingService.Trace($"Start cloning root entity '{record.LogicalName}: {record.Id}'");

            var clone = new Entity();
            clone.LogicalName = record.LogicalName;

            var copyEntityInfo = new CopyEntityInfo
            {
                EntityName = record.LogicalName,
                FieldsToCopy = this.entityClonerXmlParserService.GetAttributeList(configEl),
                BlacklistFields = this.metaDataService.GetAttributesBlacklist(record.LogicalName)
            };

            foreach (var f in record.Attributes.Where(a => a.Value != null))
            {
                if (this.CanCopyAttribute(f.Key, copyEntityInfo))
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
            EntityLinkType? linkType = null;

            if (element.Attribute("intersect") == null || element.Attribute("intersect").Value != "true")
            {
                if (element.Attribute("from").Value.Equals(element.Attribute("name").Value + "id"))
                {
                    linkType = EntityLinkType.Relation_N1;
                }
                else
                {
                    linkType = EntityLinkType.Relation_1N;
                }
            } else if(element.Attribute("intersect") != null || element.Attribute("intersect").Value == "true")
            {
                linkType = EntityLinkType.Association;
            } 
            
            return linkType;
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

            queryClone.Attribute("to").Remove();
            queryClone.Attribute("from").Remove();

            if (queryClone.Elements().Any(d => d.Name == "filter"))
            {
                queryClone.Elements().Where(d => d.Name == "filter").First().Add(XElement.Parse($"<condition attribute = '{from}' operator= 'eq' value = '{parentid.Id}'/>"));
            }
            else
            {
                queryClone.Add(XElement.Parse($"<filter><condition attribute = '{from}' operator= 'eq' value = '{parentid.Id}'/></filter>"));
            }

            var linkentityQuery = "<fetch>" + queryClone.ToString().Replace("link-entity", "entity") + "</fetch>";
            var records = this.orgService.RetrieveMultiple(new FetchExpression(linkentityQuery.ToString())).Entities;

            foreach (var record in records)
            {
                tracingService.Trace($"Start cloning link-entity '{record.LogicalName}: {record.Id}'");
                var clone = new Entity();
                clone.LogicalName = record.LogicalName;

                var copyEntityInfo = new CopyEntityInfo
                {
                    EntityName = record.LogicalName,
                    FieldsToCopy = this.entityClonerXmlParserService.GetAttributeList(queryClone),
                    BlacklistFields = this.metaDataService.GetAttributesBlacklist(record.LogicalName)
                };

                foreach (var f in record.Attributes.Where(a => a.Value != null && a.Key != from))
                {
                    if (this.CanCopyAttribute(f.Key, copyEntityInfo))
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
        /// Clone a lookup
        /// </summary>
        /// <param name="element">Current lookup</param>
        /// <param name="parentid">Id of the parent</param>
        /// <param name="parentclonedid">Id of the cloned parent</param>
        private void CloneLookup(XElement element, EntityReference parentid, EntityReference parentclonedid)
        {
            var queryClone = XElement.Parse(element.ToString());
            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();

            var from = queryClone.Attribute("from").Value;
            var to = queryClone.Attribute("to").Value;
            var lookupEntityType = queryClone.Attribute("name").Value;

            queryClone.Attribute("to").Remove();
            queryClone.Attribute("from").Remove();

            var parentRecord = this.orgService.Retrieve(parentid.LogicalName, parentid.Id, new ColumnSet(new string[] { to }));
            if (!parentRecord.Contains(to) || parentRecord[to] == null)
            {
                return;
            }

            var lookupRecord = this.orgService.Retrieve(lookupEntityType, parentRecord.GetAttributeValue<EntityReference>(to).Id, new ColumnSet(true));

            tracingService.Trace($"Start cloning lookup '{to}'");
            var clone = new Entity();
            clone.LogicalName = lookupEntityType;

            var copyEntityInfo = new CopyEntityInfo
            {
                EntityName = lookupEntityType,
                FieldsToCopy = this.entityClonerXmlParserService.GetAttributeList(queryClone),
                BlacklistFields = this.metaDataService.GetAttributesBlacklist(lookupEntityType)
            };
            foreach (var f in lookupRecord.Attributes.Where(a => a.Value != null && a.Key != from))
            {
                if (this.CanCopyAttribute(f.Key, copyEntityInfo))
                {
                    clone.Attributes.Add(f.Key, lookupRecord[f.Key]);
                }
            }

            var cloneId = this.orgService.Create(clone);

            this.orgService.Update(new Entity(parentid.LogicalName)
            {
                Id = parentclonedid.Id,
                [to] = new EntityReference(lookupEntityType, cloneId)
            });

            tracingService.Trace($"Successfully cloned lookup '{to}'");

            var linkentities = element.Elements().Where(d => d.Name == "link-entity");

            foreach (var le in linkentities)
            {
                CloneChildren(le, parentRecord.ToEntityReference(), new EntityReference(parentRecord.LogicalName, cloneId));
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
            var innerQuery = queryClone.Descendants().Where(x => x.Name == "link-entity").First();
            var innerQueryClone = XElement.Parse(innerQuery.ToString());

            var cloneBehaviour = this.entityClonerXmlParserService.GetCloneBehaviour(queryClone);

            queryClone.Descendants().Where(x => x.Name == "link-entity").Remove();
            innerQueryClone.Descendants().Where(x => x.Name == "link-entity").Remove();

            var fromField = queryClone.Attribute("from").Value;
            var relationName = queryClone.Attribute("name").Value;
            var toEntity = innerQueryClone.Attribute("name").Value;
            var toField = innerQueryClone.Attribute("from").Value;

            var associationsQuery = XElement.Parse($"<fetch><entity name='{relationName}'><all-attributes/><filter><condition attribute='{fromField}' operator='eq' value='{parentid.Id}' /></filter></entity></fetch>");
            associationsQuery.Element("entity").AddFirst(queryClone.Elements());

            var copyEntityInfo = new CopyEntityInfo
            {
                FieldsToCopy = this.entityClonerXmlParserService.GetAttributeList(innerQueryClone)
            };

            var queryAssociated = XElement.Parse(innerQueryClone.ToString().Replace("link-entity", "entity"));

            queryAssociated.Attributes().Where(x => x.Name == "intersect").Remove();
            queryAssociated.Attributes().Where(x => x.Name == "from").Remove();
            queryAssociated.Attributes().Where(x => x.Name == "to").Remove();

            if (queryAssociated.Descendants().Any(d => d.Name == "filter"))
            {
                queryAssociated.Descendants().Where(d => d.Name == "filter").First().Add(XElement.Parse($"<condition attribute = '{toField}' operator= 'eq' value = '@id'/>"));
            }
            else
            {
                queryAssociated.Add(XElement.Parse($"<filter><condition attribute = '{toField}' operator= 'eq' value = '@id'/></filter>"));
            }

            var associations = this.orgService.RetrieveMultiple(new FetchExpression(associationsQuery.ToString())).Entities;

            foreach (var association in associations)
            {
                tracingService.Trace($"Start cloning association '{association.LogicalName}'");

                Guid associateId = association.GetAttributeValue<Guid>(toField);

                if (cloneBehaviour == CloneBehaviour.Clone)
                {
                    var query = XElement.Parse("<fetch>" + queryAssociated.ToString() + "</fetch>")
                        .ToString().Replace("@id", associateId.ToString());

                    var result = this.orgService.RetrieveMultiple(new FetchExpression(query)).Entities;

                    if (result.Count == 0)
                    {
                        continue;
                    }

                    var record = result.First();

                    var clone = new Entity();
                    clone.LogicalName = toEntity;

                    copyEntityInfo.EntityName = record.LogicalName;
                    copyEntityInfo.BlacklistFields = this.metaDataService.GetAttributesBlacklist(record.LogicalName);

                    foreach (var f in record.Attributes.Where(a => a.Value != null))
                    {
                        if (this.CanCopyAttribute(f.Key, copyEntityInfo))
                        {
                            clone.Attributes.Add(f.Key, record[f.Key]);
                        }
                    }
                    associateId = this.orgService.Create(clone);
                }

                var entityReferenceCollection = new EntityReferenceCollection { parentclonedid };
                this.orgService.Associate(toEntity, associateId, new Relationship(relationName), entityReferenceCollection);

                tracingService.Trace($"Successfully clone association '{association.LogicalName}'");

                var linkentities = innerQuery.Elements().Where(d => d.Name == "link-entity");

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
            var record1entityname = metaDataService.GetEntityLogicalName(record1objecttypecode);
            var record2entityname = metaDataService.GetEntityLogicalName(record2objecttypecode);

            var copyEntityFromInfo = new CopyEntityInfo
            {
                EntityName = record1entityname,
                FieldsToCopy = this.entityClonerXmlParserService.GetAttributeListForConnections(element, record1entityname),
                BlacklistFields = this.metaDataService.GetAttributesBlacklist(record1entityname)
            };

            var copyEntityToInfo = new CopyEntityInfo
            {
                EntityName = record2entityname,
                FieldsToCopy = this.entityClonerXmlParserService.GetAttributeListForConnections(element, record2entityname),
                BlacklistFields = this.metaDataService.GetAttributesBlacklist(record2entityname)
            };

            foreach (var c in connections)
            {
                var entityFrom = this.orgService.Retrieve(record1entityname, record1id, new ColumnSet(true));
                var entityTo = this.orgService.Retrieve(record2entityname, record2id, new ColumnSet(true));

                var cloneEntityFrom = new Entity();
                cloneEntityFrom.LogicalName = record1entityname;
                
                foreach (var f in entityFrom.Attributes.Where(a => a.Value != null))
                {
                    if (this.CanCopyAttribute(f.Key, copyEntityFromInfo))
                    {
                        cloneEntityFrom.Attributes.Add(f.Key, entityFrom[f.Key]);
                    }
                }

                var cloneEntityFromId = this.orgService.Create(cloneEntityFrom);

                var cloneEntityTo = new Entity();
                cloneEntityTo.LogicalName = record2entityname;

                foreach (var f in entityTo.Attributes.Where(a => a.Value != null))
                {
                    if (this.CanCopyAttribute(f.Key,copyEntityToInfo))
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
        /// <param name="attributename">The original record attribute</param>
        /// <returns>true/false</returns>
        private bool CanCopyAttribute(string attributename, CopyEntityInfo copyEntityInfo)
        {
            if (copyEntityInfo.BlacklistFields.Any(a => a == attributename) ||
                !copyEntityInfo.FieldsToCopy.Any(a => a == attributename))
            {
                return false;
            }

            return true;
        }
    }
}
