# D365 Configurable Entity/Record Cloner

This solution allows for configurable cloning of entities/records and link-entities/records (child) at any depth level, in Dynamics 365.


## Description

There are sometimes use cases where cloning of record(s) and associated records is required.
The idea behind this solution came about as a result of some requirements that came to me, where various record cloning functionalities with specific rules were required, such as cloning an account with some fields, associated contacts, annotations of contacts, etc.
<br/> The goal was to have a single component with the ability to configure cloning information and thus provide a No-Code/Low-Code solution.


### Features

- Configurable cloning of entities and sub-entities (child entities) in Dynamics 365.

## Getting started

### Prerequisites

Dynamics 365 v9.2+

### Install

Download the managed or unmanaged solution and import it in your environment.

### Configure

- Create a configuration record for each needed cloning functionality.
![image](https://user-images.githubusercontent.com/34159960/208450207-1b69370c-6243-4deb-b04f-d8dfd26b4524.png)

  
- Provide a meaningful name.
- Provide the FetchXml which represents the entity(/entities) and fields that need to be cloned. I recommend to use [FetchXml Builder](https://www.xrmtoolbox.com/plugins/Cinteros.Xrm.FetchXmlBuilder) to build the fetch query.
- The value for the Guid of the root entity in the fetch query must be '<b>@id</b>' (placeholder).
- Set the flag <i>Clone Status?</i> to yes if the cloned entity(/entities) need to have the same status of the record to clone from.
    - The action sets the status for the clones of the entity(/entities) which have both <i>statecode</i> and <i>statuscode</i> in the list of attributes on the FetchXml.
- The FetchXml <b>cannot</b> contains readonly attributes (i.e.: <i>createdon</i>, <i>createdby</i>, etc.).
- The attribute <i>link-type</i> in the FetchXml has no effect:
  - The action always applies an <i>outer join</i>.
- The <i>N:N relations</i> in the Fetch must be modified as per following template:
    ~~~ xml
    <fetch>
        ...
        <link-entity name='<intersect-entity-name>' from='<recordid>' to='<recordid>' intersect='true'>
          <attribute name='<record1id>' />
          <attribute name='<record2id>' />
          <to-entity name='<record2.LogicalName>' entityid-field='<record2idFieldName>'>
            <attribute name='<attribute1>' />
            ...
          </to-entity>
        </link-entity>
        ...
    </fetch>
    ~~~
 
#### Sample FetchXml
~~~ xml
<fetch>
  <entity name='account' >
    <attribute name='name' />
    <attribute name='accountnumber' />
    <filter>
      <condition attribute='accountid' operator='eq' value='@id' />
    </filter>
    <link-entity name='new_new_ddentity_account' from='accountid' to='accountid' intersect='true'>
      <attribute name='accountid' />
      <attribute name='new_ddentityid' />
      <to-entity name='new_ddentity' entityid-field='new_ddentityid'>
        <attribute name='new_name' />
      </to-entity>
    </link-entity>
    <link-entity name='contact' from='parentcustomerid' to='accountid' link-type='outer' >
      <attribute name='address1_composite' />
      <attribute name='fullname' />
      <attribute name='firstname' />
      <attribute name='lastname' />
      <attribute name='statecode' />
      <attribute name='statuscode' />
      <link-entity name='annotation' from='objectid' to='contactid' link-type='outer' >
        <attribute name='filename' />
        <attribute name='filesize' />
        <attribute name='mimetype' />
        <attribute name='documentbody' />
        <attribute name='notetext' />
        <attribute name='isdocument' />
      </link-entity>
    </link-entity>
    <link-entity name='phonecall' from='regardingobjectid' to='accountid' link-type='outer' >
      <attribute name='subject' />
    </link-entity>
  </entity>
</fetch>
~~~

### Usage

- Integrate the action <i>CloneEntityFromFetch</i> where you want to trigger it (Workflow, Javascript function, etc.).
- Configure the parameters:
    - RootRecordUrl: url of the root entity (in Workflows is the parameter <i>Record Url (Dynamic)</i>).
    - Configuration: EntityReference to the configuration record.

![image](https://user-images.githubusercontent.com/34159960/207928348-7b96b22c-001c-4874-b995-5bae46bff558.png)


### Note

Technically the solution should allow an unlimited number of levels for link-entities, however always consider the 2-minute limit for running Plugins/CWAs. Analyze the amount of data involved in fetching.


## Back matter

### Acknowledgements

Thanks to all who helped inspire this solution.

### License

This project is licensed under the [GPLv3](https://www.gnu.org/licenses/gpl-3.0.html).
