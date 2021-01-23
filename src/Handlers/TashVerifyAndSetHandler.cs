using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class TashVerifyAndSetHandler : TashVerifyAndSetHandlerBase<ICudotosiApplicationModel> {
        private readonly ICudotosiHandlers vCudotosiHandlers;
        private readonly ITashCommunicator<ICudotosiApplicationModel> vTashCommunicator;
        private readonly Dictionary<string, ISelector> vSelectors;

        public TashVerifyAndSetHandler(ICudotosiHandlers cudotosiHandlers, ISimpleLogger simpleLogger, ITashSelectorHandler<ICudotosiApplicationModel> tashSelectorHandler, ITashCommunicator<ICudotosiApplicationModel> tashCommunicator,
            Dictionary<string, ISelector> selectors) : base(simpleLogger, tashSelectorHandler, tashCommunicator, selectors) {
            vCudotosiHandlers = cudotosiHandlers;
            vTashCommunicator = tashCommunicator;
            vSelectors = selectors;
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
            const string errorMessage = "Not implemented yet";
            SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
            await vTashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
        }

        public override async Task ProcessVerifyNumberOfItemsTaskAsync(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            const string errorMessage = "Not implemented yet";
            SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
            await vTashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
        }
    }
}
