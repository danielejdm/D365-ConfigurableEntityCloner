﻿
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurableEntityCloner.Test
{
    [TestClass]
    public class CloneEntityActivityTest
    {
        private XrmFakedContext xrmFakedContext;
        private IOrganizationService orgService;
        private Entity config = new Entity("jdm_configuration");

        [TestInitialize]
        public void Initialize()
        {
            this.xrmFakedContext = new XrmFakedContext();
            this.orgService = xrmFakedContext.GetOrganizationService();
        }

        [TestMethod]
        public void Should_Clone_Account_Contact_Notes()
        {
            var account = new Entity("account")
            {
                Id = Guid.NewGuid(),
                ["name"] = "Account",
                ["accountnumber"] = "Acc-1234",
                ["statecode"] = new OptionSetValue(0),
                ["statuscode"] = new OptionSetValue(1)
            };

            var contact = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["firstname"] = "Mario",
                ["lastname"] = "Rossi",
                ["parentcustomerid"] = account.ToEntityReference(),
                ["fullname"] = "Mario Rossi",
                ["address1_composite"] = "Musterstr. 1, 11111 Musterstadt"
            };

            var contact2 = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["firstname"] = "Max",
                ["lastname"] = "Mustermann",
                ["parentcustomerid"] = account.ToEntityReference(),
                ["fullname"] = "Max Mustermann",
                ["address1_composite"] = "Musterstr. 1, 54546 Somestadt",
            };

            var note = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filename"] = "Test.txt",
                ["mimetype"] = "text /plain",
                ["documentbody"] = "just a test",
                ["notetext"] = "some text",
                ["isdocument"] = true,
                ["objectid"] = contact.ToEntityReference(),
            };
            var phonecall = new Entity("phonecall")
            {
                Id = Guid.NewGuid(),
                ["regardingobjectid"] = account.ToEntityReference(),
                ["subject"] = "Test Phonecall"
            };

            var jdm_myentity = new Entity("jdm_myentity")
            {
                Id = Guid.NewGuid(),
                ["jdm_name"] = "Test DD"
            };

            var jdm_jdm_myentity_account = new Entity("jdm_jdm_myentity_account")
            {
                Id = Guid.NewGuid(),
                ["accountid"] = account.Id,
                ["jdm_myentityid"] = jdm_myentity.Id,
            };

            var config = new Entity("jdm_configuration")
            {
                Id = Guid.NewGuid(),
                ["jdm_configvalue"] = "<fetch>" +
                                      "<entity name='account' exclude-attributes='false' >" +
                                        "<attribute name='name' />" +
                                        "<attribute name='accountnumber' />" +
                                        "<attribute name='statecode' original-new-value='1' />" +
                                        "<attribute name='statuscode' original-new-value='2' />" +
                                        "<filter>" +
                                          "<condition attribute='accountid' operator='eq' value='@id' />" +
                                        "</filter>" +
                                        "<link-entity name='jdm_jdm_myentity_account' from='accountid' to='accountid' intersect='true'>" +
                                          "<attribute name='accountid' />" +
                                          "<attribute name='jdm_myentityid' />" +
                                          "<associate-entity exclude-attributes='false' name='jdm_myentity'>" +
                                            "<attribute name='jdm_name'/>" +
                                          "</associate-entity>" +
                                        "</link-entity>" +
                                        "<link-entity exclude-attributes='false' name='contact' from='parentcustomerid' to='accountid' link-type='outer' >" +
                                          "<attribute name='address1_composite' />" +
                                          "<attribute name='fullname' />" +
                                          "<attribute name='firstname' />" +
                                          "<attribute name='lastname' />" +
                                          "<attribute name='statecode' />" +
                                          "<attribute name='statuscode' />" +
                                          "<link-entity exclude-attributes='false' name='annotation' from='objectid' to='contactid' link-type='outer' >" +
                                            "<attribute name='filename' />" +
                                            "<attribute name='filesize' />" +
                                            "<attribute name='mimetype' />" +
                                            "<attribute name='documentbody' />" +
                                            "<attribute name='notetext' />" +
                                            "<attribute name='isdocument' />" +
                                          "</link-entity>" +
                                        "</link-entity>" +
                                         "<link-entity exclude-attributes='false' name='phonecall' from='regardingobjectid' to='accountid' link-type='outer' >" +
                                          "<attribute name='subject' />" +
                                        "</link-entity>" +
                                      "</entity>" +
                                    "</fetch>"
            };

#pragma warning disable CS0618 // Type or member is obsolete
            this.xrmFakedContext.Initialize(new List<Entity>() { account, contact, contact2, note, phonecall, config, jdm_myentity, jdm_jdm_myentity_account });
#pragma warning restore CS0618 // Type or member is obsolete
            this.xrmFakedContext.AddRelationship("jdm_jdm_myentity_account", new XrmFakedRelationship
            {
                IntersectEntity = "jdm_jdm_myentity_account",
                Entity1LogicalName = account.LogicalName,
                Entity1Attribute = "accountid",
                Entity2LogicalName = jdm_myentity.LogicalName,
                Entity2Attribute = "jdm_myentityid"
            });

            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "RootRecordUrl", "https://myorg.crm.dynamics.com/main.aspx?appid=f8a69fd7-e37a-ed11-81ad-0022486f4310&pagetype=entityrecord&etn=account&id=" + account.Id.ToString() },
                { "ConfigurationId", config.ToEntityReference() }
            };

            var result = this.xrmFakedContext.ExecuteCodeActivity<CloneEntityActivity>(inputs);

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("account").Any(e => e.Id == new Guid((string)result["RootCloneId"])));
            Assert.IsTrue(this.xrmFakedContext.CreateQuery("account").Any(e => e.Id == account.Id && 
                    e.GetAttributeValue<OptionSetValue>("statecode").Value == 1 && e.GetAttributeValue<OptionSetValue>("statuscode").Value == 2));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("contact").Any(e => e.Id != contact.Id &&
                    e["firstname"].Equals(contact["firstname"]) &&
                    e["lastname"].Equals(contact["lastname"]) &&
                    e["fullname"].Equals(contact["fullname"]) &&
                    e["address1_composite"].Equals(contact["address1_composite"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("contact").Any(e => e.Id != contact.Id &&
                    e["firstname"].Equals(contact["firstname"]) &&
                    e["lastname"].Equals(contact["lastname"]) &&
                    e["fullname"].Equals(contact["fullname"]) &&
                    e["address1_composite"].Equals(contact["address1_composite"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("contact").Any(e => e.Id != contact2.Id &&
                    e["firstname"].Equals(contact2["firstname"]) &&
                    e["lastname"].Equals(contact2["lastname"]) &&
                    e["fullname"].Equals(contact2["fullname"]) &&
                    e["address1_composite"].Equals(contact2["address1_composite"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("annotation").Any(e => e.Id != note.Id &&
                    e["filename"].Equals(note["filename"]) &&
                    e["documentbody"].Equals(note["documentbody"]) &&
                    e["isdocument"].Equals(note["isdocument"]) &&
                    e["mimetype"].Equals(note["mimetype"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("phonecall").Any(e => e.Id != phonecall.Id &&
                    e["subject"].Equals(phonecall["subject"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("jdm_jdm_myentity_account").Any(e => e.Id != jdm_jdm_myentity_account.Id));
        }

        [TestMethod]
        public void Should_Clone_Connections()
        {
            var account = new Entity("account")
            {
                Id = Guid.NewGuid(),
                ["name"] = "Account",
                ["accountnumber"] = "Acc-1234"
            };

            var contact = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["firstname"] = "Mario",
                ["lastname"] = "Rossi",
                ["parentcustomerid"] = account.ToEntityReference(),
                ["fullname"] = "Mario Rossi",
                ["address1_composite"] = "Musterstr. 1, 11111 Musterstadt"
            };

            var connectionrole1 = new Entity("connectionrole")
            {
                Id = Guid.NewGuid(),
                ["name"] = "Connection 1"
            };

            var connection1 = new Entity("connection")
            {
                Id = Guid.NewGuid(),
                ["record1id"] = account.Id,
                ["record2id"] = contact.Id,
                ["record1roleid"] = connectionrole1.Id,
                ["record1objecttypecode"] = 1,
                ["record2objecttypecode"] = 2
            };

            var config = new Entity("jdm_configuration")
            {
                Id = Guid.NewGuid(),
                ["jdm_configvalue"] = "<fetch>" +
                                      "<entity name='connection'>" +
                                        "<attribute name='record1roleid' />" +
                                        "<attribute name='record2roleid' />" +
                                        "<attribute name='record2id' />" +
                                        "<attribute name='record1id' />" +
                                        "<attribute name='record1objecttypecode' />" +
                                        "<attribute name='record2objecttypecode' />" +
                                        "<filter>" +
                                          $"<condition attribute='record1id' operator='eq' value='{account.Id}'/>" +
                                          $"<condition attribute='record2id' operator='eq' value='{contact.Id}'/>" +
                                          $"<condition attribute='record1roleid' operator='eq' value='{connectionrole1.Id}'/>" +
                                        "</filter>" +
                                      "</entity>" +
                                    "</fetch>"
            };

            this.xrmFakedContext.Initialize(new List<Entity>() { account, contact, connectionrole1, connection1, config });

            var accountMetadata = new EntityMetadata()
            {
                LogicalName = "account",
            };
            accountMetadata.SetSealedPropertyValue("ObjectTypeCode", 1);

            var contactMetadata = new EntityMetadata()
            {
                LogicalName = "contact",
            };
            contactMetadata.SetSealedPropertyValue("ObjectTypeCode", 2);

            this.xrmFakedContext.InitializeMetadata(new List<EntityMetadata>() { accountMetadata, contactMetadata });

            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "RootRecordUrl", "https://myorg.crm.dynamics.com/main.aspx?appid=f8a69fd7-e37a-ed11-81ad-0022486f4310&pagetype=entityrecord&etn=account&id=" + account.Id.ToString() },
                { "ConfigurationId", config.ToEntityReference() }
            };

            //'Exception: The organization request type 'Microsoft.Xrm.Sdk.Messages.RetrieveMetadataChangesRequest' is not yet supported... but we DO love pull requests so please feel free to submit one! :). This functionality is not available yet
            //var result = this.xrmFakedContext.ExecuteCodeActivity<CloneEntityActivity>(inputs);
        }
    }
}
