namespace ConfigurableEntityCloner.Test.ProxyClasses
{

	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.82")]
	public partial class XrmSvc : Microsoft.Xrm.Sdk.Client.OrganizationServiceContext
	{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public XrmSvc(Microsoft.Xrm.Sdk.IOrganizationService service) : 
				base(service)
		{
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.Account"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.Account> AccountSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.Account>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.jdm_configuration"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.jdm_configuration> jdm_configurationSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.jdm_configuration>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.new_ddentity"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.new_ddentity> new_ddentitySet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.new_ddentity>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.new_new_ddentity_account"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.new_new_ddentity_account> new_new_ddentity_accountSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.new_new_ddentity_account>();
			}
		}
	}
}
