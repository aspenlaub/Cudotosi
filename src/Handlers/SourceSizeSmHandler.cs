using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceSizeSmHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>,ISimpleToggleButtonHandler {
        private readonly ISimpleToggleButtonHandler vTargetSizeXsHandler;
        private readonly IImageHandler vImageHandler;

        public SourceSizeSmHandler(ICudotosiApplicationModel model, ISimpleToggleButtonHandler targetSizeXsHandler, IImageHandler imageHandler) : base(model, model.SourceSizeSm) {
            vTargetSizeXsHandler = targetSizeXsHandler;
            vImageHandler = imageHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            if (!isChecked) { return; }

            Model.TargetSizeLg.Enabled = false;
            Model.TargetSizeMd.Enabled = false;
            Model.TargetSizeSm.Enabled = false;
            Model.TargetSizeXs.Enabled = true;
            if (Model.TargetSizeLg.IsChecked || Model.TargetSizeMd.IsChecked || Model.TargetSizeSm.IsChecked) {
                await vTargetSizeXsHandler.ToggledAsync(true);
            }

            await vImageHandler.LoadFromFile(vImageHandler.FileName());
        }
    }
}
