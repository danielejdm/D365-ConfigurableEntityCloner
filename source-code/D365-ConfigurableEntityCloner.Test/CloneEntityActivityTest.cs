using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Xrm.Sdk;
using ConfigurableEntityCloner;
using FakeXrmEasy;

namespace ConfigurableEntityEntityCloner.Test
{
    [TestClass]
    public class CloneEntityActivityTest
    {
        [TestMethod]
        public void Should_Clone_Account_Contact_Notes()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var fakedContext = new XrmFakedContext();
#pragma warning restore CS0618 // Type or member is obsolete

            var service = fakedContext.GetOrganizationService();

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

            var contact2 = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["firstname"] = "Max",
                ["lastname"] = "Mustermann",
                ["parentcustomerid"] = account.ToEntityReference(),
                ["fullname"] = "Max Mustermann",
                ["address1_composite"] = "Musterstr. 1, 54546 Somestadt",
                ["statecode"] = new OptionSetValue(1),
                ["statuscode"] = new OptionSetValue(2)
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

            var config = new Entity("jdm_configuration")
            {
                Id = Guid.NewGuid(),
                ["jdm_clonestatus"] = true,
                ["jdm_configvalue"] = "<fetch>" +
                                      "<entity name='account' >" +
                                        "<attribute name='name' />" +
                                        "<attribute name='accountnumber' />" +
                                        "<filter>" +
                                          "<condition attribute='accountid' operator='eq' value='@id' />" +
                                        "</filter>" +
                                        "<link-entity name='contact' from='parentcustomerid' to='accountid' link-type='outer' >" +
                                          "<attribute name='address1_composite' />" +
                                          "<attribute name='fullname' />" +
                                          "<attribute name='firstname' />" +
                                          "<attribute name='lastname' />" +
                                          "<attribute name='statecode' />" +
                                          "<attribute name='statuscode' />" +
                                          "<link-entity name='annotation' from='objectid' to='contactid' link-type='outer' >" +
                                            "<attribute name='filename' />" +
                                            "<attribute name='filesize' />" +
                                            "<attribute name='mimetype' />" +
                                            "<attribute name='documentbody' />" +
                                            "<attribute name='notetext' />" +
                                            "<attribute name='isdocument' />" +
                                          "</link-entity>" +
                                        "</link-entity>" +
                                         "<link-entity name='phonecall' from='regardingobjectid' to='accountid' link-type='outer' >" +
                                          "<attribute name='subject' />" +
                                        "</link-entity>" +
                                      "</entity>" +
                                    "</fetch>"
            };

#pragma warning disable CS0618 // Type or member is obsolete
            fakedContext.Initialize(new List<Entity>() { account, contact, contact2, note, phonecall, config });
#pragma warning restore CS0618 // Type or member is obsolete

            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "RootRecordUrl", "https://myorg.crm.dynamics.com/main.aspx?appid=f8a69fd7-e37a-ed11-81ad-0022486f4310&pagetype=entityrecord&etn=account&id=" + account.Id.ToString() },
                { "ConfigurationId", config.ToEntityReference() }
            };

            var result = fakedContext.ExecuteCodeActivity<CloneEntityActivity>(inputs);

            Assert.IsTrue(fakedContext.CreateQuery("account").Any(e => e.Id == new Guid((string)result["RootCloneId"])));

            Assert.IsTrue(fakedContext.CreateQuery("contact").Any(e => e.Id != contact.Id &&
                    e["firstname"].Equals(contact["firstname"]) &&
                    e["lastname"].Equals(contact["lastname"]) &&
                    e["fullname"].Equals(contact["fullname"]) &&
                    e["address1_composite"].Equals(contact["address1_composite"])));

            Assert.IsTrue(fakedContext.CreateQuery("contact").Any(e => e.Id != contact.Id &&
                    e["firstname"].Equals(contact["firstname"]) &&
                    e["lastname"].Equals(contact["lastname"]) &&
                    e["fullname"].Equals(contact["fullname"]) &&
                    e["address1_composite"].Equals(contact["address1_composite"])));

            Assert.IsTrue(fakedContext.CreateQuery("contact").Any(e => e.Id != contact2.Id &&
                    e["firstname"].Equals(contact2["firstname"]) &&
                    e["lastname"].Equals(contact2["lastname"]) &&
                    e["fullname"].Equals(contact2["fullname"]) &&
                    e["statecode"].Equals(contact2["statecode"]) &&
                    e["statuscode"].Equals(contact2["statuscode"]) &&
                    e["address1_composite"].Equals(contact2["address1_composite"])));

            Assert.IsTrue(fakedContext.CreateQuery("annotation").Any(e => e.Id != note.Id &&
                    e["filename"].Equals(note["filename"]) &&
                    e["documentbody"].Equals(note["documentbody"]) &&
                    e["isdocument"].Equals(note["isdocument"]) &&
                    e["mimetype"].Equals(note["mimetype"])));

            Assert.IsTrue(fakedContext.CreateQuery("phonecall").Any(e => e.Id != phonecall.Id &&
                    e["subject"].Equals(phonecall["subject"])));
        }
    }
}
