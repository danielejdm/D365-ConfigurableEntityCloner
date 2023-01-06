namespace ConfigurableEntityCloner.Test.ProxyClasses
{

	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("connection")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.82")]
	public partial class Connection : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Connection() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "connection";
		
		public const string EntityLogicalCollectionName = "connections";
		
		public const string EntitySetName = "connections";
		
		public const int EntityTypeCode = 3234;
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		private void OnPropertyChanged(string propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void OnPropertyChanging(string propertyName)
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
			}
		}
		
		/// <summary>
		/// Unique identifier of the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionid")]
		public System.Nullable<System.Guid> ConnectionId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("connectionid");
			}
			set
			{
				this.OnPropertyChanging("ConnectionId");
				this.SetAttributeValue("connectionid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ConnectionId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.ConnectionId = value;
			}
		}
		
		/// <summary>
		/// Shows who created the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
			}
		}
		
		/// <summary>
		/// Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
		public System.Nullable<System.DateTime> CreatedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
			}
		}
		
		/// <summary>
		/// Shows who created the record on behalf of another user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
			}
		}
		
		/// <summary>
		/// Type additional information to describe the connection, such as the length or quality of the relationship.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
		public string Description
		{
			get
			{
				return this.GetAttributeValue<string>("description");
			}
			set
			{
				this.OnPropertyChanging("Description");
				this.SetAttributeValue("description", value);
				this.OnPropertyChanged("Description");
			}
		}
		
		/// <summary>
		/// Enter the end date of the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("effectiveend")]
		public System.Nullable<System.DateTime> EffectiveEnd
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("effectiveend");
			}
			set
			{
				this.OnPropertyChanging("EffectiveEnd");
				this.SetAttributeValue("effectiveend", value);
				this.OnPropertyChanged("EffectiveEnd");
			}
		}
		
		/// <summary>
		/// Enter the start date of the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("effectivestart")]
		public System.Nullable<System.DateTime> EffectiveStart
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("effectivestart");
			}
			set
			{
				this.OnPropertyChanging("EffectiveStart");
				this.SetAttributeValue("effectivestart", value);
				this.OnPropertyChanged("EffectiveStart");
			}
		}
		
		/// <summary>
		/// The default image for the entity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entityimage")]
		public byte[] EntityImage
		{
			get
			{
				return this.GetAttributeValue<byte[]>("entityimage");
			}
			set
			{
				this.OnPropertyChanging("EntityImage");
				this.SetAttributeValue("entityimage", value);
				this.OnPropertyChanged("EntityImage");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entityimage_timestamp")]
		public System.Nullable<long> EntityImage_Timestamp
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<long>>("entityimage_timestamp");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entityimage_url")]
		public string EntityImage_URL
		{
			get
			{
				return this.GetAttributeValue<string>("entityimage_url");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entityimageid")]
		public System.Nullable<System.Guid> EntityImageId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("entityimageid");
			}
		}
		
		/// <summary>
		/// Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("exchangerate")]
		public System.Nullable<decimal> ExchangeRate
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("exchangerate");
			}
		}
		
		/// <summary>
		/// Unique identifier of the data import or data migration that created this record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("importsequencenumber")]
		public System.Nullable<int> ImportSequenceNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("importsequencenumber");
			}
			set
			{
				this.OnPropertyChanging("ImportSequenceNumber");
				this.SetAttributeValue("importsequencenumber", value);
				this.OnPropertyChanged("ImportSequenceNumber");
			}
		}
		
		/// <summary>
		/// Indicates that this is the master record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismaster")]
		public System.Nullable<bool> IsMaster
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("ismaster");
			}
		}
		
		/// <summary>
		/// Shows who last updated the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
			}
		}
		
		/// <summary>
		/// Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
		public System.Nullable<System.DateTime> ModifiedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
			}
		}
		
		/// <summary>
		/// Shows who last updated the record on behalf of another user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// Name of the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
		public string Name
		{
			get
			{
				return this.GetAttributeValue<string>("name");
			}
		}
		
		/// <summary>
		/// Date and time that the record was migrated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overriddencreatedon")]
		public System.Nullable<System.DateTime> OverriddenCreatedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overriddencreatedon");
			}
			set
			{
				this.OnPropertyChanging("OverriddenCreatedOn");
				this.SetAttributeValue("overriddencreatedon", value);
				this.OnPropertyChanged("OverriddenCreatedOn");
			}
		}
		
		/// <summary>
		/// Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ownerid")]
		public Microsoft.Xrm.Sdk.EntityReference OwnerId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("ownerid");
			}
			set
			{
				this.OnPropertyChanging("OwnerId");
				this.SetAttributeValue("ownerid", value);
				this.OnPropertyChanged("OwnerId");
			}
		}
		
		/// <summary>
		/// Shows the business unit that the record owner belongs to.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		public Microsoft.Xrm.Sdk.EntityReference OwningBusinessUnit
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningbusinessunit");
			}
		}
		
		/// <summary>
		/// Unique identifier of the team who owns the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		public Microsoft.Xrm.Sdk.EntityReference OwningTeam
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningteam");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who owns the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		public Microsoft.Xrm.Sdk.EntityReference OwningUser
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owninguser");
			}
		}
		
		/// <summary>
		/// Choose the primary account, contact, or other record involved in the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record1id")]
		public Microsoft.Xrm.Sdk.EntityReference Record1Id
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("record1id");
			}
			set
			{
				this.OnPropertyChanging("Record1Id");
				this.SetAttributeValue("record1id", value);
				this.OnPropertyChanged("Record1Id");
			}
		}
		
		/// <summary>
		/// Shows the record type of the source record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record1objecttypecode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.connection_record1objecttypecode> Record1ObjectTypeCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("record1objecttypecode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.connection_record1objecttypecode)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.connection_record1objecttypecode), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Choose the primary party's role or relationship with the second party.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record1roleid")]
		public Microsoft.Xrm.Sdk.EntityReference Record1RoleId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("record1roleid");
			}
			set
			{
				this.OnPropertyChanging("Record1RoleId");
				this.SetAttributeValue("record1roleid", value);
				this.OnPropertyChanged("Record1RoleId");
			}
		}
		
		/// <summary>
		/// Select the secondary account, contact, or other record involved in the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record2id")]
		public Microsoft.Xrm.Sdk.EntityReference Record2Id
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("record2id");
			}
			set
			{
				this.OnPropertyChanging("Record2Id");
				this.SetAttributeValue("record2id", value);
				this.OnPropertyChanged("Record2Id");
			}
		}
		
		/// <summary>
		/// Shows the record type of the target record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record2objecttypecode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.connection_record2objecttypecode> Record2ObjectTypeCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("record2objecttypecode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.connection_record2objecttypecode)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.connection_record2objecttypecode), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Choose the secondary party's role or relationship with the primary party.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record2roleid")]
		public Microsoft.Xrm.Sdk.EntityReference Record2RoleId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("record2roleid");
			}
			set
			{
				this.OnPropertyChanging("Record2RoleId");
				this.SetAttributeValue("record2roleid", value);
				this.OnPropertyChanged("Record2RoleId");
			}
		}
		
		/// <summary>
		/// Unique identifier for the reciprocal connection record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relatedconnectionid")]
		public Microsoft.Xrm.Sdk.EntityReference RelatedConnectionId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("relatedconnectionid");
			}
		}
		
		/// <summary>
		/// Shows whether the connection is active or inactive. Inactive connections are read-only and can't be edited unless they are reactivated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionState> StateCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.ConnectionState)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.ConnectionState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("StateCode");
				if ((value == null))
				{
					this.SetAttributeValue("statecode", null);
				}
				else
				{
					this.SetAttributeValue("statecode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
				}
				this.OnPropertyChanged("StateCode");
			}
		}
		
		/// <summary>
		/// Reason for the status of the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.connection_statuscode> StatusCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.connection_statuscode)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.connection_statuscode), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("StatusCode");
				if ((value == null))
				{
					this.SetAttributeValue("statuscode", null);
				}
				else
				{
					this.SetAttributeValue("statuscode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
				}
				this.OnPropertyChanged("StatusCode");
			}
		}
		
		/// <summary>
		/// Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("transactioncurrencyid")]
		public Microsoft.Xrm.Sdk.EntityReference TransactionCurrencyId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("transactioncurrencyid");
			}
			set
			{
				this.OnPropertyChanging("TransactionCurrencyId");
				this.SetAttributeValue("transactioncurrencyid", value);
				this.OnPropertyChanged("TransactionCurrencyId");
			}
		}
		
		/// <summary>
		/// Version number of the connection.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
		public System.Nullable<long> VersionNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
			}
		}
		
		/// <summary>
		/// 1:N connection_related_connection
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connection_related_connection", Microsoft.Xrm.Sdk.EntityRole.Referenced)]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.Connection> Referencedconnection_related_connection
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("connection_related_connection", Microsoft.Xrm.Sdk.EntityRole.Referenced);
			}
			set
			{
				this.OnPropertyChanging("Referencedconnection_related_connection");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("connection_related_connection", Microsoft.Xrm.Sdk.EntityRole.Referenced, value);
				this.OnPropertyChanged("Referencedconnection_related_connection");
			}
		}
		
		/// <summary>
		/// N:1 account_connections1
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record1id")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("account_connections1")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Account account_connections1
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("account_connections1", null);
			}
			set
			{
				this.OnPropertyChanging("account_connections1");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("account_connections1", null, value);
				this.OnPropertyChanged("account_connections1");
			}
		}
		
		/// <summary>
		/// N:1 account_connections2
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record2id")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("account_connections2")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Account account_connections2
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("account_connections2", null);
			}
			set
			{
				this.OnPropertyChanging("account_connections2");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("account_connections2", null, value);
				this.OnPropertyChanged("account_connections2");
			}
		}
		
		/// <summary>
		/// N:1 connection_related_connection
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relatedconnectionid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connection_related_connection", Microsoft.Xrm.Sdk.EntityRole.Referencing)]
		public ConfigurableEntityCloner.Test.ProxyClasses.Connection Referencingconnection_related_connection
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("connection_related_connection", Microsoft.Xrm.Sdk.EntityRole.Referencing);
			}
		}
		
		/// <summary>
		/// N:1 connection_role_connections1
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record1roleid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connection_role_connections1")]
		public ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole connection_role_connections1
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connection_role_connections1", null);
			}
			set
			{
				this.OnPropertyChanging("connection_role_connections1");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connection_role_connections1", null, value);
				this.OnPropertyChanged("connection_role_connections1");
			}
		}
		
		/// <summary>
		/// N:1 connection_role_connections2
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record2roleid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connection_role_connections2")]
		public ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole connection_role_connections2
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connection_role_connections2", null);
			}
			set
			{
				this.OnPropertyChanging("connection_role_connections2");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connection_role_connections2", null, value);
				this.OnPropertyChanged("connection_role_connections2");
			}
		}
		
		/// <summary>
		/// N:1 contact_connections1
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record1id")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("contact_connections1")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Contact contact_connections1
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("contact_connections1", null);
			}
			set
			{
				this.OnPropertyChanging("contact_connections1");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("contact_connections1", null, value);
				this.OnPropertyChanged("contact_connections1");
			}
		}
		
		/// <summary>
		/// N:1 contact_connections2
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record2id")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("contact_connections2")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Contact contact_connections2
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("contact_connections2", null);
			}
			set
			{
				this.OnPropertyChanging("contact_connections2");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("contact_connections2", null, value);
				this.OnPropertyChanged("contact_connections2");
			}
		}
		
		/// <summary>
		/// N:1 phonecall_connections1
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record1id")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("phonecall_connections1")]
		public ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall phonecall_connections1
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall>("phonecall_connections1", null);
			}
			set
			{
				this.OnPropertyChanging("phonecall_connections1");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall>("phonecall_connections1", null, value);
				this.OnPropertyChanged("phonecall_connections1");
			}
		}
		
		/// <summary>
		/// N:1 phonecall_connections2
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("record2id")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("phonecall_connections2")]
		public ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall phonecall_connections2
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall>("phonecall_connections2", null);
			}
			set
			{
				this.OnPropertyChanging("phonecall_connections2");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall>("phonecall_connections2", null, value);
				this.OnPropertyChanged("phonecall_connections2");
			}
		}
	}
}
