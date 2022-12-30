using Microsoft.Xrm.Sdk;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using System;

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
            try
            {
                var rootRecordId = Helper.GetRecordIdFromUrl(this.RootRecordUrl.Get(activityContext));
                var configurationId = this.ConfigurationId.Get(activityContext);

                var entityCloner = new Cloner(activityContext);
                var cloneId = entityCloner.Clone(configurationId, rootRecordId);

                this.RootCloneId.Set(activityContext, cloneId);

            }catch (Exception ex)
            {
                tracingService.Trace($"{ nameof(CloneEntityActivity) }: { ex.Message } StackTrace: { ex.StackTrace }");
                throw ex;
            }
        }
    }
}
