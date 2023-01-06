namespace ConfigurableEntityCloner.Test.ProxyClasses
{

	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("new_new_ddentity_account")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.82")]
	public partial class new_new_ddentity_account : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public new_new_ddentity_account() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "new_new_ddentity_account";
		
		public const string EntityLogicalCollectionName = null;
		
		public const string EntitySetName = "new_new_ddentity_accountset";
		
		public const int EntityTypeCode = 10320;
		
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
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("accountid")]
		public System.Nullable<System.Guid> accountid
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("accountid");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_ddentityid")]
		public System.Nullable<System.Guid> new_ddentityid
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("new_ddentityid");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_new_ddentity_accountid")]
		public System.Nullable<System.Guid> new_new_ddentity_accountId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("new_new_ddentity_accountid");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("new_new_ddentity_accountid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				base.Id = value;
			}
		}
		
		/// <summary>
		/// 
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
		/// N:N new_new_ddentity_account
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("new_new_ddentity_account")]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.new_ddentity> new_new_ddentity_account1
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.new_ddentity>("new_new_ddentity_account", null);
			}
			set
			{
				this.OnPropertyChanging("new_new_ddentity_account1");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.new_ddentity>("new_new_ddentity_account", null, value);
				this.OnPropertyChanged("new_new_ddentity_account1");
			}
		}
	}
}
