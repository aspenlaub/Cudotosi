using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceSizeMdHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>,ISimpleToggleButtonHandler {
        private readonly ISimpleToggleButtonHandler vTargetSizeSmHandler;

        public SourceSizeMdHandler(ICudotosiApplicationModel model, ISimpleToggleButtonHandler targetSizeSmHandler) : base(model, model.SourceSizeMd) {
            vTargetSizeSmHandler = targetSizeSmHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            if (isChecked) {
                Model.TargetSizeLg.Enabled = false;
                Model.TargetSizeMd.Enabled = false;
                Model.TargetSizeSm.Enabled = true;
                Model.TargetSizeXs.Enabled = true;
                if (Model.TargetSizeLg.IsChecked || Model.TargetSizeMd.IsChecked) {
                    await vTargetSizeSmHandler.ToggledAsync(true);
                }
            }
        }
    }
}
