using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceSizeLgHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>,ISimpleToggleButtonHandler {
        private readonly ISimpleToggleButtonHandler vTargetSizeMdHandler;

        public SourceSizeLgHandler(ICudotosiApplicationModel model, ISimpleToggleButtonHandler targetSizeMdHandler) : base(model, model.SourceSizeLg) {
            vTargetSizeMdHandler = targetSizeMdHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            if (isChecked) {
                Model.TargetSizeLg.Enabled = false;
                Model.TargetSizeMd.Enabled = true;
                Model.TargetSizeSm.Enabled = true;
                Model.TargetSizeXs.Enabled = true;
                if (Model.TargetSizeLg.IsChecked) {
                    await vTargetSizeMdHandler.ToggledAsync(true);
                }
            }
        }
    }
}
