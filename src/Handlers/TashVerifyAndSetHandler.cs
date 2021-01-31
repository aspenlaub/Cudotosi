using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class TashVerifyAndSetHandler : TashVerifyAndSetHandlerBase<ICudotosiApplicationModel> {
        private readonly ICudotosiHandlers vCudotosiHandlers;
        private readonly ITashCommunicator<ICudotosiApplicationModel> vTashCommunicator;

        public TashVerifyAndSetHandler(ICudotosiHandlers cudotosiHandlers, ISimpleLogger simpleLogger, ITashSelectorHandler<ICudotosiApplicationModel> tashSelectorHandler, ITashCommunicator<ICudotosiApplicationModel> tashCommunicator,
            Dictionary<string, ISelector> selectors) : base(simpleLogger, tashSelectorHandler, tashCommunicator, selectors) {
            vCudotosiHandlers = cudotosiHandlers;
            vTashCommunicator = tashCommunicator;
        }

        protected override void OnValueTaskProcessed(ITashTaskHandlingStatus<ICudotosiApplicationModel> status, bool verify, bool set, string actualValue) { }

        protected override Dictionary<string, ITextBox> TextBoxNamesToTextBoxDictionary(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            return new Dictionary<string, ITextBox> {
                { nameof(status.Model.Folder), status.Model.Folder },
                { nameof(status.Model.Status), status.Model.Status }
            };
        }

        protected override Dictionary<string, ISimpleTextHandler> TextBoxNamesToTextHandlerDictionary(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            return new Dictionary<string, ISimpleTextHandler> {
                { nameof(status.Model.Folder), vCudotosiHandlers.FolderTextHandler }
            };
        }

        public override async Task ProcessVerifyWhetherEnabledTaskAsync(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            bool actualEnabled;
            switch (status.TaskBeingProcessed.ControlName) {
                case nameof(status.Model.Default):
                    actualEnabled = status.Model.Default.Enabled;
                    break;
                case nameof(status.Model.Save):
                    actualEnabled = status.Model.Save.Enabled;
                    break;
                default:
                    var errorMessage = $"Unknown enabled/disabled control {status.TaskBeingProcessed.ControlName}";
                    SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                    return;
            }

            status.Model.Status.Type = StatusType.Success;
            if (status.TaskBeingProcessed.Text == "true") {
                if (!actualEnabled) {
                    status.Model.Status.Type = StatusType.Error;
                    status.Model.Status.Text = $"Expected {status.TaskBeingProcessed.ControlName} to be enabled";
                }
            } else if (actualEnabled) {
                status.Model.Status.Type = StatusType.Error;
                status.Model.Status.Text = $"Expected {status.TaskBeingProcessed.ControlName} to be disabled";
            }

            await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
        }

        public override async Task ProcessVerifyNumberOfItemsTaskAsync(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            const string errorMessage = "Not implemented yet";
            SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
            await vTashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
        }
    }
}
