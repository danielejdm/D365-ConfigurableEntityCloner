# D365 Configurable Entity/Record Cloner

This solution allows for configurable cloning of entities/records, link-entities (child), relationships, associations, connections and attributes, in Dynamics 365, at any depth level.


## Description

There are sometimes use cases where cloning of record(s), relationship(s) and associaton(s) is required.
The idea behind this solution came as a result of some requirements needed for a project, where various record cloning functionalities with specific rules were required, such as cloning an account with some fields, associated contacts, annotations, etc.
<br/> The goal was to have a single component with the ability to configure cloning information and thus provide a No-Code/Low-Code solution.


### Features

- Configurable cloning of entities and sub-entities (child entities).
- Configurable cloning of associations (n:m relationships).
- Configurable cloning of connections and connected records.
- Configurable set of fields to be cloned.
- Automatically skipping of fields not valid for being set in creation.
- Modularization of configurations (splitting).

## Getting started

### Prerequisites

Dynamics 365 v9.2+

### Install

Download the managed or unmanaged solution and import it in your environment.

### Configure

- Create a configuration record for each needed cloning functionality.
![image](https://user-images.githubusercontent.com/34159960/210876705-5f2f16e3-e34b-4606-8cb7-358acafd0484.png)


  
- Provide a meaningful name.
- Provide the xml representing entities, filters, relationships, associations, attributes, etc. for cloning a specific entity.
  - I recommend to use [FetchXml Builder](https://www.xrmtoolbox.com/plugins/Cinteros.Xrm.FetchXmlBuilder) to build the FetchXml.
- The root entity <b>cannot be</b> an intersect-entity (entity for the <i>n:m relations</i>).
- The attribute <i>link-type</i> in the FetchXml has no effect:
  - The action always applies an <i>outer join</i>.
- The <i>cloner module</i> skips all the system fields not valid for creation (i.e.: <i>createdon</i>, <i>createdby</i>, <i>statecode</i>, etc.).
 
### Sample Config Xml
~~~ xml 
<fetch>
  <entity name="account" >
    <attribute name="name" />
    <attribute name="accountnumber" />
    <filter>
      <condition attribute="statecode" operator="eq" value="0" />
    </filter>
    <link-entity name="new_new_ddentity_account" from="accountid" to="accountid" intersect="true">
      <attribute name="new_ddentityid" />
      <attribute name="accountid" />
      <link-entity name="new_ddentity" from="new_ddentityid" to="new_ddentityid" intersect="true" clone-behaviour="clone">
        <link-entity name="new_ddentity" from="new_ddentityid" to="new_ddentityid" intersect="true">
          <attribute name="new_name" />
          <filter> 
            <condition attribute='statecode' operator='eq' value='0' /> 
          </filter> 
        </link-entity>
      </link-entity>
    </link-entity>
    <link-entity name="contact" from="parentcustomerid" to="accountid" >
      <attribute name="address1_city" />
      <attribute name="address1_country" />
      <attribute name="address1_line1" />
      <link-entity name="annotation" from="objectid" to="contactid" >
        <attribute name="filename" />
      </link-entity>
    </link-entity>
    <link-entity name="phonecall" from="regardingobjectid" to="accountid" >
      <attribute name="subject" />
      <filter> 
        <condition attribute='statecode' operator='eq' value='0' /> 
      </filter> 
    </link-entity>
  </entity>
</fetch>
~~~

#### Config Xml - Properties
- <i>clone-behaviour</i> (required only for <i>intersect-entity</i>): associate/clone
  - clone -> the associated entity is cloned and the clone is associated to the new parent
  - associate (default) -> the associated entity is not cloned: it is associated to the new parent

#### Config Xml - Connections
The connections (entity <i>connection</i>) are represented with the following xml-block:
~~~ xml
    <fetch>
      <entity name="connection">
        <attribute name="record1roleid" />
        <attribute name="record2roleid" />
        <attribute name="record2id" />
        <attribute name="record1id" />
        <attribute name="record2objecttypecode" />
        <attribute name="record1objecttypecode" />
        <filter>
          [<condition attribute="record1roleid" operator="eq" value="<Id of the Role1>" />]
          [<condition attribute="record2roleid" operator="eq" value="<Id of the Role2>" />]
        </filter>
        <link-entity name="contact" from="contactid" to="record2id">
          <attribute name="address1_composite" />
          <attribute name="firstname" />
          <attribute name="fullname" />
        </link-entity>
        <link-entity name="account" from="accountid" to="record1id">
          <attribute name="address1_composite" />
          <attribute name="name" />
        </link-entity>
      </entity>
    </fetch>
~~~
  - the following attributes for the <i>connection</i> entity are all required: 
    - <i>record1roleid</i>
    - <i>record2roleid</i>
    - <i>record2id</i>
    - <i>record1id</i>
    - <i>record2objecttypecode</i>
    - <i>record1objecttypecode</i>
  - the two linked entities are necessary for providing information of the entities that are connected and will be cloned
  - in the filter for the <i>connection</i> entity we can insert the id of the role1 and role2 for which we want to look
  - the associated entities (xml-blocks for <i>record1</i> and <i>record2</i>) do not support filters (for the moment) 
  - connections do not support link-entity (for the moment): a connection is always the leaf of a branch in the xml

#### Config Xml - Modularization

It is possible to modularize Config-Xml to allow splittig of configurations and reuse the single configuration.
Below an example of config modularization:
 - Config1
 ~~~ xml
     <fetch>
      <entity name='contact' >
        <attribute name='firstname' />
        <attribute name='lastname' />
        <filter>
          <condition attribute='statuscode' operator='eq' value='1' />
          <condition attribute='contactid' operator='eq' value='@id' />
        </filter>
        <link-entity name='annotation' from='objectid' to='contactid' >
          <attribute name='subject' />
        </link-entity>
      </entity>
    </fetch>
 ~~~

 - Config2
  ~~~ xml
     <fetch>
      <entity name='account'>
        <attribute name='address1_composite' />
        <attribute name='name' />
        <filter>
          <condition attribute='accountid' operator='eq' value='@id' />
        </filter>
        <link-entity name='contact' from='parentcustomerid' to='accountid' merge-config-id='{config1.Id}' />
      </entity>
    </fetch>
 ~~~

   - The <i>Config1</i> can be used <i>stand-alone</i> or can be integrated into another Config-Xml (in the example, <i>Config2</i>).
   - The attribute <i>merge-config-id</i> contains the <i>Guid</i> of the Config-Xml that has to be integrated in the <i>Config2</i> (in the example, <i>Config1</i>).
   - After merging, the config-xml looks like following:
  ~~~ xml
    <fetch>
      <entity name="account" >
        <attribute name="address1_composite" />
        <attribute name="name" />
        <filter>
          <condition attribute="accountid" operator="eq" value="@id" />
        </filter>
        <link-entity name="contact" from="parentcustomerid" to="accountid" >
          <attribute name="firstname" />
          <attribute name="lastname" />
          <filter>
            <condition attribute="statuscode" operator="eq" value="1" />
          </filter>
          <link-entity name="annotation" from="objectid" to="contactid" >
            <attribute name="subject" />
          </link-entity>
        </link-entity>
      </entity>
    </fetch>
 ~~~


### Usage

- Integrate the action <i>CloneEntityFromFetch</i> where you want to trigger it (Workflow, Javascript function, etc.).
- Configure the parameters:
    - RootRecordInfo: 
      - Url of the root entity (parameter <i>Record Url (Dynamic)</i> in Workflow configuration) or
      - Guid of the root entity as string
    - EntityName: schema mane of the entity. This parameter is <b>required</b> if the parameter <i>RootRecordInfo</i> is a simple guid (not an url). 
    - Configuration: EntityReference to the configuration record.


### Note

- This solution is <i>work in progress</i>:
  - it will be update frequently
  - it is not fully tested: if you find any bug, please open an issue or push a fix on a new branch
- Technically the solution should allow an unlimited number of levels for link-entities, however always consider the 2-minute limit for running Plugins/CWAs. Analyze the amount of data involved in fetching.


## Back matter

### Acknowledgements

Thanks to all who helped inspire this solution.

### License

This project is licensed under the [GPLv3](https://www.gnu.org/licenses/gpl-3.0.html).
