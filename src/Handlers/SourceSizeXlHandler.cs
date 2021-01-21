using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceSizeXlHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>, ISimpleToggleButtonHandler {
        private readonly IPictureHandler vPictureHandler;

        public SourceSizeXlHandler(ICudotosiApplicationModel model, IPictureHandler pictureHandler) : base(model, model.SourceSizeXl) {
            vPictureHandler = pictureHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            if (!isChecked) { return; }

            Model.TargetSizeLg.Enabled = true;
            Model.TargetSizeMd.Enabled = true;
            Model.TargetSizeSm.Enabled = true;
            Model.TargetSizeXs.Enabled = true;

            await vPictureHandler.LoadFromFile(vPictureHandler.FileName());
        }
    }
}
