using ConfigurableEntityCloner.Test.ProxyClasses;
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConfigurableEntityCloner.Test
{
    [TestClass]
    public class CloneEntityActivityTest
    {
        private XrmFakedContext xrmFakedContext;
        private IOrganizationService orgService;
        private List<AttributeMetadata> attributesMetadata;

        [TestInitialize]
        public void Initialize()
        {
            Mock<IMetaDataService> chk = new Mock<IMetaDataService>();

            chk.Setup(x => x.GetAttributeMetadata("account")).Returns(
                attributesMetadata
            );

            this.xrmFakedContext = new XrmFakedContext();
            this.xrmFakedContext.EnableProxyTypes(Assembly.GetExecutingAssembly());

            this.orgService = xrmFakedContext.GetOrganizationService();
            this.InitializeEntityMetadata();
        }

        [TestMethod]
        public void Should_Clone_Account_PrimaryContact_Contacts_Notes_Phonecalls()
        {
            var pcontact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "Primary",
                LastName = "Contact"
            };

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Account",
                AccountNumber = "Acc-1234",
                ["statecode"] = new OptionSetValue(0),
                ["statuscode"] = new OptionSetValue(1),
                OwnerId = new EntityReference("systemuser", Guid.NewGuid()),
                ["createdby"] = new EntityReference("systemuser", Guid.NewGuid()),
                ["createdon"] = DateTime.Now,
                Address1_City = "abc",
                Address1_Country = "abc",
                Address1_County = "abc",
                Address1_Fax = "abc",
                Address1_Line1 = "abc",
                Address1_Line2 = "abc",
                Address1_Line3 = "abc",
                PrimaryContactId = pcontact.ToEntityReference()
            };

            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "Mario",
                 LastName = "Rossi",
                ParentCustomerId = account.ToEntityReference()
            };

            var contact2 = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "Max",
                LastName = "Mustermann",
                ParentCustomerId = account.ToEntityReference()
            };

            var note = new Annotation
            {
                Id = Guid.NewGuid(),
                FileName = "Test.txt",
                MimeType = "text /plain",
                DocumentBody = "just a test",
                NoteText = "some text",
                IsDocument = true,
                ObjectId = contact.ToEntityReference(),
            };
            var phonecall = new PhoneCall
            {
                Id = Guid.NewGuid(),
                RegardingObjectId = account.ToEntityReference(),
                Subject = "Test Phonecall"
            };

            var config = new jdm_configuration
            {
                Id = Guid.NewGuid(),
                jdm_configvalue = "<fetch>" +
                                      "<entity name='account' >" +
                                        "<attribute name='accountnumber' />" +
                                        "<attribute name='name' />" +
                                        "<attribute name='statecode' />" +
                                        "<filter>" +
                                          "<condition attribute='statecode' operator='eq' value='0' />" +
                                        "</filter>" +
                                        "<link-entity name='contact' from='contactid' to='primarycontactid'>" +
                                          "<attribute name='firstname' />" +
                                          "<attribute name='lastname' />" +
                                        "</link-entity>" +
                                        "<link-entity name='contact' from='parentcustomerid' to='accountid' >" +
                                          "<attribute name='address1_composite' />" +
                                          "<attribute name='fullname' />" +
                                          "<attribute name='firstname' />" +
                                          "<attribute name='lastname' />" +
                                          "<attribute name='statecode' />" +
                                          "<attribute name='statuscode' />" +
                                          "<link-entity name='annotation' from='objectid' to='contactid' >" +
                                            "<attribute name='filename' />" +
                                            "<attribute name='filesize' />" +
                                            "<attribute name='mimetype' />" +
                                            "<attribute name='documentbody' />" +
                                            "<attribute name='notetext' />" +
                                            "<attribute name='isdocument' />" +
                                          "</link-entity>" +
                                        "</link-entity>" +
                                         "<link-entity name='phonecall' from='regardingobjectid' to='accountid' >" +
                                          "<attribute name='subject' />" +
                                        "</link-entity>" +
                                      "</entity>" +
                                    "</fetch>"
            };

            this.xrmFakedContext.Initialize(new List<Entity>() { account, pcontact, contact, contact2, note, phonecall, config });

            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "RootRecordInfo", "https://myorg.crm.dynamics.com/main.aspx?appid=f8a69fd7-e37a-ed11-81ad-0022486f4310&pagetype=entityrecord&etn=account&id=" + account.Id.ToString() },
                { "ConfigurationId", config.ToEntityReference() }
            };

            var result = this.xrmFakedContext.ExecuteCodeActivity<CloneEntityActivity>(inputs);

            var clonedAccount = this.xrmFakedContext.CreateQuery("account").Where(e => e.Id == new Guid((string)result["RootCloneId"])).First();
            Assert.IsTrue(clonedAccount["name"].Equals(account["name"]) && clonedAccount["accountnumber"].Equals(account["accountnumber"]));

            var clonedPrimaryContact = this.xrmFakedContext.CreateQuery("contact").Where(e => e.Id.Equals(contact.Id) == false && e.Id.Equals(contact2.Id) == false && e.Id.Equals(pcontact.Id) == false).First();
            Assert.IsTrue(clonedAccount.GetAttributeValue<EntityReference>("primarycontactid").Id == clonedPrimaryContact.Id);

            Assert.IsTrue(clonedPrimaryContact["firstname"].Equals(pcontact["firstname"]) &&
                    clonedPrimaryContact["lastname"].Equals(pcontact["lastname"]));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("contact").Any(e => e.Id != contact.Id &&
                    e["firstname"].Equals(contact["firstname"]) &&
                    e["lastname"].Equals(contact["lastname"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("contact").Any(e => e.Id != pcontact.Id &&
                    e["firstname"].Equals(contact["firstname"]) &&
                    e["lastname"].Equals(contact["lastname"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("contact").Any(e => e.Id != contact2.Id &&
                    e["firstname"].Equals(contact2["firstname"]) &&
                    e["lastname"].Equals(contact2["lastname"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("annotation").Any(e => e.Id != note.Id &&
                    e["filename"].Equals(note["filename"]) &&
                    e["documentbody"].Equals(note["documentbody"]) &&
                    e["isdocument"].Equals(note["isdocument"]) &&
                    e["mimetype"].Equals(note["mimetype"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("phonecall").Any(e => e.Id != phonecall.Id &&
                    e["subject"].Equals(phonecall["subject"])));
        }

        [TestMethod]
        public void Should_Clone_Connections_And_Connected_Entities()
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

            var config = new jdm_configuration
            {
                Id = Guid.NewGuid(),
                jdm_configvalue = "<fetch>" +
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

            this.xrmFakedContext.Initialize(new List<Entity>() { account, contact, connection1, connectionrole1, config });

            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "RootRecordInfo", "https://myorg.crm.dynamics.com/main.aspx?appid=f8a69fd7-e37a-ed11-81ad-0022486f4310&pagetype=entityrecord&etn=account&id=" + account.Id.ToString() },
                { "ConfigurationId", config.ToEntityReference() }
            };

            //'Exception: The organization request type 'Microsoft.Xrm.Sdk.Messages.RetrieveMetadataChangesRequest' is not yet supported... but we DO love pull requests so please feel free to submit one! :). This functionality is not available yet
            //var result = this.xrmFakedContext.ExecuteCodeActivity<CloneEntityActivity>(inputs);
        }

        [TestMethod]
        public void Should_Merge_Two_Config_And_Clone_Account_Contact_Note()
        {
            var config2 = new Entity("jdm_configuration")
            {
                Id = Guid.NewGuid(),
                ["jdm_configvalue"] = "<fetch>" +
                                          "<entity name='contact' >" +
                                            "<attribute name='firstname' />" +
                                            "<attribute name='lastname' />" +
                                            "<attribute name='fullname' />" +
                                            "<attribute name='parentcustomerid' />" +
                                            "<filter>" +
                                              "<condition attribute='statecode' operator='eq' value='0' />" +
                                            "</filter>" +
                                            "<link-entity name='annotation' from='objectid' to='contactid' >" +
                                              "<attribute name='subject' />" +
                                            "</link-entity>" +
                                          "</entity>" +
                                        "</fetch>"
            };

            var config1 = new Entity("jdm_configuration")
            {
                Id = Guid.NewGuid(),
                ["jdm_configvalue"] = "<fetch>" +
                                          "<entity name='account' >" +
                                            "<attribute name='accountnumber' />" +
                                            "<attribute name='name' />" +
                                            $"<link-entity name='contact' from='parentcustomerid' to='accountid' merge-config-id='{config2.Id}' />" +
                                          "</entity>" +
                                        "</fetch>"
            };

            var account = new Entity("account")
            {
                Id = Guid.NewGuid(),
                ["name"] = "Account",
                ["accountnumber"] = "Acc-1234",
                ["statecode"] = new OptionSetValue(0),
                ["statuscode"] = new OptionSetValue(1),
                ["ownerid"] = new EntityReference("systemuser", Guid.NewGuid()),
                ["createdby"] = new EntityReference("systemuser", Guid.NewGuid()),
                ["createdon"] = DateTime.Now,
                ["address1_city"] = "abc",
                ["address1_country"] = "abc",
                ["address1_county"] = "abc",
                ["address1_fax"] = "abc",
                ["address1_line1"] = "abc",
                ["address1_line2"] = "abc",
                ["address1_line3"] = "abc",
            };

            var contact = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["statecode"] = new OptionSetValue(0),
                ["firstname"] = "Mario",
                ["lastname"] = "Rossi",
                ["parentcustomerid"] = account.ToEntityReference(),
                ["fullname"] = "Mario Rossi",
                ["address1_composite"] = "Musterstr. 1, 11111 Musterstadt"
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
                ["subject"] = "Test annotation"
            };

            this.xrmFakedContext.Initialize(new List<Entity>()
            {
                config1, config2, account, contact, note
            });


            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "RootRecordInfo", "https://myorg.crm.dynamics.com/main.aspx?appid=f8a69fd7-e37a-ed11-81ad-0022486f4310&pagetype=entityrecord&etn=account&id=" + account.Id.ToString() },
                { "ConfigurationId", config1.ToEntityReference() }
            };

            var result = this.xrmFakedContext.ExecuteCodeActivity<CloneEntityActivity>(inputs);

            Assert.IsNotNull(result);

            var cloneId = new Guid((string)result["RootCloneId"]);

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("account")
                .Any(e => e.Id == cloneId &&
                e["accountnumber"].Equals(account["accountnumber"]) &&
                e["name"].Equals(account["name"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("contact").Any(e => e.Id != contact.Id &&
                    e["firstname"].Equals(contact["firstname"]) &&
                    e["lastname"].Equals(contact["lastname"]) &&
                    e["fullname"].Equals(contact["fullname"]) &&
                    e.GetAttributeValue<EntityReference>("parentcustomerid").Id.Equals(cloneId)));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("annotation").Any(e => e.Id != note.Id &&
                    e["subject"].Equals(note["subject"])));
        }

        [TestMethod]
        public void Should_Clone_Account_And_MyEntity_Associations()
        {
            var myentity = new new_ddentity
            {
                Id = Guid.NewGuid(),
                new_name = "Active MyEntity",
                statecode = new_ddentityState.Active
            };

            var myentity2 = new new_ddentity
            {
                Id = Guid.NewGuid(),
                new_name = "Inactive MyEntity",
                statecode = new_ddentityState.Inactive
            };

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Account",
                AccountNumber = "Acc-1234",
                StateCode = AccountState.Active,
                StatusCode = account_statuscode.Active,
                OwnerId = new EntityReference("systemuser", Guid.NewGuid()),
                ["createdby"] = new EntityReference("systemuser", Guid.NewGuid()),
                ["createdon"] = DateTime.Now,
                Address1_City = "My City",
                Address1_Country = "Germany",
                Address1_County = "County",
                Address1_Fax = "abc",
                Address1_Line1 = "abc",
                Address1_Line2 = "abc",
                Address1_Line3 = "abc",
            };

            var jdm_myentity_account = new new_new_ddentity_account
            {
                Id = Guid.NewGuid(),
                ["accountid"] = account.Id,
                ["new_ddentityid"] = myentity.Id,
            };

            var jdm_jdm_myentity_account2 = new new_new_ddentity_account
            {
                Id = Guid.NewGuid(),
                ["accountid"] = account.Id,
                ["new_ddentityid"] = myentity2.Id,
            };

            var config = new jdm_configuration
            {
                Id = Guid.NewGuid(),
                jdm_configvalue = "<fetch>" +
                                      "<entity name='account' >" +
                                        "<attribute name='name' />" +
                                        "<attribute name='accountnumber' />" +
                                        "<link-entity name='new_new_ddentity_account' from='accountid' to='accountid' intersect='true' clone-behaviour='clone'>" +
                                          "<link-entity name='new_ddentity' from='new_ddentityid' to='new_ddentityid' intersect='true' >" +
                                          "<attribute name='new_name' />" +
                                          "<filter>" +
                                                "<condition attribute='statecode' operator='eq' value='0' />" +
                                            "</filter>" +
                                           "</link-entity>" +
                                        "</link-entity>" +
                                      "</entity>" +
                                    "</fetch>"
            };

            this.xrmFakedContext.Initialize(new List<Entity>() { account, myentity, myentity2, jdm_myentity_account, jdm_jdm_myentity_account2, config });

            this.xrmFakedContext.AddRelationship("new_new_ddentity_account", new XrmFakedRelationship
            {
                IntersectEntity = new_new_ddentity_account.EntityLogicalName,
                Entity1LogicalName = Account.EntityLogicalName,
                Entity1Attribute = "accountid",
                Entity2LogicalName = new_ddentity.EntityLogicalName,
                Entity2Attribute = "new_ddentityid"
            });

            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "RootRecordInfo", "https://myorg.crm.dynamics.com/main.aspx?appid=f8a69fd7-e37a-ed11-81ad-0022486f4310&pagetype=entityrecord&etn=account&id=" + account.Id.ToString() },
                { "ConfigurationId", config.ToEntityReference() }
            };

            var result = this.xrmFakedContext.ExecuteCodeActivity<CloneEntityActivity>(inputs);

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("account").Any(e => e.Id == new Guid((string)result["RootCloneId"]) && e["name"].Equals(account["name"])));

            Assert.IsTrue(this.xrmFakedContext.CreateQuery("new_ddentity").Any(e => e.Id != myentity.Id &&
                    e["new_name"].Equals(myentity["new_name"])));

            Assert.IsFalse(this.xrmFakedContext.CreateQuery("new_ddentity").Any(e => e.Id != myentity2.Id &&
                e["new_name"].Equals(myentity2["new_name"])));

            // Assert.IsTrue(this.xrmFakedContext.CreateQuery("deap_deap_produktvariante_deap_auflagen").Any(e => e.Id != deap_deap_produktvariante_deap_auflagen.Id && !e["deap_auflagenId"].Equals(aufl.Id)));
        }

        private void InitializeEntityMetadata()
        {
            this.attributesMetadata = new List<AttributeMetadata>()
                {
                    new AttributeMetadata()
                    {
                        LogicalName = "ownerid",
                        IsValidForCreate= false,
                    },
                    new AttributeMetadata()
                    {
                        LogicalName = "createdon",
                        IsValidForCreate= false,
                    },
                    new AttributeMetadata()
                    {
                        LogicalName = "createdby",
                        IsValidForCreate= false,
                    },
                    new AttributeMetadata()
                    {
                        LogicalName = "modifiedby",
                        IsValidForCreate= false,
                    },
                    new AttributeMetadata()
                    {
                        LogicalName = "modifiedon",
                        IsValidForCreate= false,
                    },
                    new AttributeMetadata()
                    {
                        LogicalName = "statecode",
                        IsValidForCreate= false,
                    },
                    new AttributeMetadata()
                    {
                        LogicalName = "statuscode",
                        IsValidForCreate= false,
                    }
                };


            EntityMetadata accountMetadata = new EntityMetadata()
            {
                LogicalName = Account.EntityLogicalName
            };
            accountMetadata.SetAttributeCollection(attributesMetadata);
            accountMetadata.SetSealedPropertyValue("ObjectTypeCode", 1);
            this.xrmFakedContext.InitializeMetadata(accountMetadata);

            EntityMetadata contactMetadata = new EntityMetadata()
            {
                LogicalName = Contact.EntityLogicalName
            };
            contactMetadata.SetAttributeCollection(attributesMetadata);
            contactMetadata.SetSealedPropertyValue("ObjectTypeCode", 2);
            this.xrmFakedContext.InitializeMetadata(contactMetadata);

            EntityMetadata noteMetadata = new EntityMetadata()
            {
                LogicalName = Annotation.EntityLogicalName
            };
            noteMetadata.SetAttributeCollection(attributesMetadata);
            this.xrmFakedContext.InitializeMetadata(noteMetadata);

            EntityMetadata phonecallMetadata = new EntityMetadata()
            {
                LogicalName = PhoneCall.EntityLogicalName
            };
            phonecallMetadata.SetAttributeCollection(attributesMetadata);
            this.xrmFakedContext.InitializeMetadata(phonecallMetadata);

            EntityMetadata new_ddentityMetadata = new EntityMetadata()
            {
                LogicalName = new_ddentity.EntityLogicalName
            };
            new_ddentityMetadata.SetAttributeCollection(attributesMetadata);
            this.xrmFakedContext.InitializeMetadata(new_ddentityMetadata);

            EntityMetadata new_ddentity_accountMetadata = new EntityMetadata()
            {
                LogicalName = new_new_ddentity_account.EntityLogicalName
            };
            new_ddentity_accountMetadata.SetAttributeCollection(attributesMetadata);
            this.xrmFakedContext.InitializeMetadata(new_ddentity_accountMetadata);

            EntityMetadata jdm_configurationMetadata = new EntityMetadata()
            {
                LogicalName = jdm_configuration.EntityLogicalName
            };
            jdm_configurationMetadata.SetAttributeCollection(attributesMetadata);
            this.xrmFakedContext.InitializeMetadata(jdm_configurationMetadata);

            EntityMetadata connectionMetadata = new EntityMetadata()
            {
                LogicalName = Connection.EntityLogicalName
            };
            connectionMetadata.SetAttributeCollection(attributesMetadata);
            this.xrmFakedContext.InitializeMetadata(connectionMetadata);

            EntityMetadata connectionRoleMetadata = new EntityMetadata()
            {
                LogicalName = ConnectionRole.EntityLogicalName
            };
            connectionRoleMetadata.SetAttributeCollection(attributesMetadata);
            this.xrmFakedContext.InitializeMetadata(connectionRoleMetadata);
        }
    }
}
