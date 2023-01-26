using Microsoft.Xrm.Sdk;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using Microsoft.Xrm.Sdk.Query;

namespace ConfigurableEntityCloner
{
    public sealed class CloneEntityActivity : CodeActivity
    {
        [Input(nameof(RootRecordInfo))]
        [RequiredArgument]
        public InArgument<string> RootRecordInfo { get; set; }

        [Input(nameof(EntityName))]
        public InArgument<string> EntityName { get; set; }

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
                var entityName = this.EntityName.Get(activityContext);
                string recordInfo = this.RootRecordInfo.Get(activityContext);

                Guid rootRecordId;
                if (!Guid.TryParse(recordInfo, out rootRecordId))
                {
                    rootRecordId = Guid.Parse(DynamicsUrlService.GetRecordIdFromUrl(recordInfo));
                    entityName = DynamicsUrlService.GetEntityNameFromUrl(recordInfo, orgService);
                }
                else if (entityName == null)
                {
                    throw new InvalidPluginExecutionException($"Parameter {nameof(EntityName)} cannot be null if the parameter {nameof(RootRecordInfo)} is not a valid RecordUrl");
                }

                var configurationId = this.ConfigurationId.Get<EntityReference>(activityContext);
                var configuration = orgService.Retrieve(configurationId.LogicalName, configurationId.Id, new ColumnSet(true));
                var entityClonerXmlParserService = new EntityClonerXmlParserService(orgService);

                var entityCloner = new EntityClonerService(orgService, tracingService, metadataService, entityClonerXmlParserService, configuration);
                var cloneId = entityCloner.Clone(rootRecordId.ToString());

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
