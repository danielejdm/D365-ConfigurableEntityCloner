using Microsoft.Xrm.Sdk;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using Microsoft.Xrm.Sdk.Query;

namespace ConfigurableEntityCloner
{
    public sealed class CloneEntityActivity : CodeActivity
    {
        [Input(nameof(RootRecordUrl))]
        [RequiredArgument]
        public InArgument<string> RootRecordUrl { get; set; }

        [Input(nameof(ConfigurationId))]
        [RequiredArgument]
        [ReferenceTarget("jdm_configuration")]
        public InArgument<EntityReference> ConfigurationId { get; set; }

        [Output(nameof(RootCloneId))]
        public OutArgument<string> RootCloneId { get; set; }

        protected override void Execute(CodeActivityContext activityContext)
        {
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();
            IWorkflowContext wfContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            var orgService = serviceFactory.CreateOrganizationService(wfContext.UserId);
            var metadataService = new MetaDataService(orgService);

            try
            {
                var rootRecordId = DynamicsUrlService.GetRecordIdFromUrl(this.RootRecordUrl.Get(activityContext));
                var configurationId = this.ConfigurationId.Get<EntityReference>(activityContext);
                var configuration = orgService.Retrieve(configurationId.LogicalName, configurationId.Id, new ColumnSet(true));
                var entityClonerXmlParserService = new EntityClonerXmlParserService(orgService);

                var entityCloner = new EntityClonerService(orgService, tracingService, metadataService, entityClonerXmlParserService, configuration);
                var cloneId = entityCloner.Clone(rootRecordId);

                this.RootCloneId.Set(activityContext, cloneId);

            }
            catch (Exception ex)
            {
                tracingService.Trace($"{nameof(CloneEntityActivity)}: {ex.Message} StackTrace: {ex.StackTrace}");
                throw ex;
            }
        }
    }
}
