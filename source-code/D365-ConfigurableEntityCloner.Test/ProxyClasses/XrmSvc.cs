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
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.Annotation"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.Annotation> AnnotationSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.Annotation>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.Connection"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.Connection> ConnectionSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.Connection>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole> ConnectionRoleSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.ConnectionRole>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.Contact"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.Contact> ContactSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.Contact>();
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
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall"/> entities.
		/// </summary>
		public System.Linq.IQueryable<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall> PhoneCallSet
		{
			get
			{
				return this.CreateQuery<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall>();
			}
		}
	}
}
