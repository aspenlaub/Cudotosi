using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceSizeXlHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>, ISimpleToggleButtonHandler {
        private readonly IImageHandler vImageHandler;

        public SourceSizeXlHandler(ICudotosiApplicationModel model, IImageHandler imageHandler) : base(model, model.SourceSizeXl) {
            vImageHandler = imageHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            if (!isChecked) { return; }

            Model.TargetSizeLg.Enabled = true;
            Model.TargetSizeMd.Enabled = true;
            Model.TargetSizeSm.Enabled = true;
            Model.TargetSizeXs.Enabled = true;

            await vImageHandler.LoadFromFile(vImageHandler.FileName());
        }
    }
}
