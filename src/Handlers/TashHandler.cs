using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class TashHandler : TashHandlerBase<ICudotosiApplicationModel> {
        public TashHandler(ITashAccessor tashAccessor, IApplicationLogger applicationLogger,
            IButtonNameToCommandMapper buttonNameToCommandMapper,
            ITashVerifyAndSetHandler<ICudotosiApplicationModel> tashVerifyAndSetHandler, ITashSelectorHandler<ICudotosiApplicationModel> tashSelectorHandler, ITashCommunicator<ICudotosiApplicationModel> tashCommunicator)
            : base(tashAccessor, applicationLogger, buttonNameToCommandMapper, tashVerifyAndSetHandler, tashSelectorHandler, tashCommunicator) {
        }

        protected override async Task ProcessSingleTaskAsync(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            ApplicationLogger.LogMessage($"Processing a task of type {status.TaskBeingProcessed.Type} in {nameof(TashHandler)}");

            switch (status.TaskBeingProcessed.Type) {
                case ControllableProcessTaskType.Reset:
                    await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
                    break;
                default:
                    await base.ProcessSingleTaskAsync(status);
                    break;
            }
        }
    }
}
