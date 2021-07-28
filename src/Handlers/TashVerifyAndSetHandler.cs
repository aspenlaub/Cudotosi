using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class TashVerifyAndSetHandler : TashVerifyAndSetHandlerBase<ICudotosiApplicationModel> {
        private readonly ICudotosiHandlers vCudotosiHandlers;

        public TashVerifyAndSetHandler(ICudotosiHandlers cudotosiHandlers, ISimpleLogger simpleLogger, ITashSelectorHandler<ICudotosiApplicationModel> tashSelectorHandler, ITashCommunicator<ICudotosiApplicationModel> tashCommunicator,
            Dictionary<string, ISelector> selectors) : base(simpleLogger, tashSelectorHandler, tashCommunicator, selectors) {
            vCudotosiHandlers = cudotosiHandlers;
        }

        protected override void OnValueTaskProcessed(ITashTaskHandlingStatus<ICudotosiApplicationModel> status, bool verify, bool set, string actualValue) { }

        protected override Dictionary<string, ITextBox> TextBoxNamesToTextBoxDictionary(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            return new() {
                { nameof(status.Model.Folder), status.Model.Folder },
                { nameof(status.Model.Status), status.Model.Status }
            };
        }

        protected override Dictionary<string, ISimpleTextHandler> TextBoxNamesToTextHandlerDictionary(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            return new() {
                { nameof(status.Model.Folder), vCudotosiHandlers.FolderTextHandler }
            };
        }

        protected override Dictionary<string, ISimpleCollectionViewSourceHandler> CollectionViewSourceNamesToCollectionViewSourceHandlerDictionary(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            return new();
        }

        protected override Dictionary<string, ICollectionViewSource> CollectionViewSourceNamesToCollectionViewSourceDictionary(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
            return new();
        }
    }
}
