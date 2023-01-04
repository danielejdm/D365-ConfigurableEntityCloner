namespace ConfigurableEntityCloner.Test.ProxyClasses
{

	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("connectionrole")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.82")]
	public partial class ConnectionRole : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public ConnectionRole() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "connectionrole";
		
		public const string EntityLogicalCollectionName = "connectionroles";
		
		public const string EntitySetName = "connectionroles";
		
		public const int EntityTypeCode = 3231;
		
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
		/// Categories for connection roles.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("category")]
		public Microsoft.Xrm.Sdk.OptionSetValue Category
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("category");
			}
			set
			{
				this.OnPropertyChanging("Category");
				this.SetAttributeValue("category", value);
				this.OnPropertyChanged("Category");
			}
		}
		
		/// <summary>
		/// State of the component.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
		public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
			}
		}
		
		/// <summary>
		/// Unique identifier of the connection role.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionroleid")]
		public System.Nullable<System.Guid> ConnectionRoleId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("connectionroleid");
			}
			set
			{
				this.OnPropertyChanging("ConnectionRoleId");
				this.SetAttributeValue("connectionroleid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ConnectionRoleId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionroleid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.ConnectionRoleId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier of the published or unpublished connection role record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("connectionroleidunique")]
		public System.Nullable<System.Guid> ConnectionRoleIdUnique
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("connectionroleidunique");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the relationship role.
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
		/// Date and time when the connection role was created.
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
		/// Unique identifier of the delegate user who created the relationship role.
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
		/// Description of the connection role.
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
		/// Version in which the form is introduced.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
		public string IntroducedVersion
		{
			get
			{
				return this.GetAttributeValue<string>("introducedversion");
			}
			set
			{
				this.OnPropertyChanging("IntroducedVersion");
				this.SetAttributeValue("introducedversion", value);
				this.OnPropertyChanged("IntroducedVersion");
			}
		}
		
		/// <summary>
		/// Information that specifies whether this component can be customized.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
		public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
			}
			set
			{
				this.OnPropertyChanging("IsCustomizable");
				this.SetAttributeValue("iscustomizable", value);
				this.OnPropertyChanged("IsCustomizable");
			}
		}
		
		/// <summary>
		/// Indicates whether the solution component is part of a managed solution.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
		public System.Nullable<bool> IsManaged
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the connection role.
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
		/// Date and time when the connection role was last modified.
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
		/// Unique identifier of the delegate user who modified the relationship role.
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
		/// Name of the connection role.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
		public string Name
		{
			get
			{
				return this.GetAttributeValue<string>("name");
			}
			set
			{
				this.OnPropertyChanging("Name");
				this.SetAttributeValue("name", value);
				this.OnPropertyChanged("Name");
			}
		}
		
		/// <summary>
		/// Unique identifier of the organization that this connection role belongs to.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		public Microsoft.Xrm.Sdk.EntityReference OrganizationId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
			}
		}
		
		/// <summary>
		/// Date and time when the record was last overwritten.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
		public System.Nullable<System.DateTime> OverwriteTime
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
			}
		}
		
		/// <summary>
		/// Unique identifier of the associated solution.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
		public System.Nullable<System.Guid> SolutionId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
			}
		}
		
		/// <summary>
		/// Status of the connection role.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRoleState> StateCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRoleState)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRoleState), optionSet.Value)));
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
		/// Reason for the status of the connection role.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.connectionrole_statuscode> StatusCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.connectionrole_statuscode)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.connectionrole_statuscode), optionSet.Value)));
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
		/// Version number of the connection role.
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
		/// 1:N connection_role_connections1
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connection_role_connections1")]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.Connection> connection_role_connections1
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("connection_role_connections1", null);
			}
			set
			{
				this.OnPropertyChanging("connection_role_connections1");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("connection_role_connections1", null, value);
				this.OnPropertyChanged("connection_role_connections1");
			}
		}
		
		/// <summary>
		/// 1:N connection_role_connections2
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connection_role_connections2")]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.Connection> connection_role_connections2
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("connection_role_connections2", null);
			}
			set
			{
				this.OnPropertyChanging("connection_role_connections2");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("connection_role_connections2", null, value);
				this.OnPropertyChanged("connection_role_connections2");
			}
		}
		
		/// <summary>
		/// N:N connectionroleassociation_association
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connectionroleassociation_association", Microsoft.Xrm.Sdk.EntityRole.Referencing)]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole> Referencingconnectionroleassociation_association
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connectionroleassociation_association", Microsoft.Xrm.Sdk.EntityRole.Referencing);
			}
			set
			{
				this.OnPropertyChanging("Referencingconnectionroleassociation_association");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connectionroleassociation_association", Microsoft.Xrm.Sdk.EntityRole.Referencing, value);
				this.OnPropertyChanged("Referencingconnectionroleassociation_association");
			}
		}
		
		/// <summary>
		/// N:N connectionroleassociation_association
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("connectionroleassociation_association", Microsoft.Xrm.Sdk.EntityRole.Referenced)]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole> Referencedconnectionroleassociation_association
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connectionroleassociation_association", Microsoft.Xrm.Sdk.EntityRole.Referenced);
			}
			set
			{
				this.OnPropertyChanging("Referencedconnectionroleassociation_association");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>("connectionroleassociation_association", Microsoft.Xrm.Sdk.EntityRole.Referenced, value);
				this.OnPropertyChanged("Referencedconnectionroleassociation_association");
			}
		}
	}
}
