using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceSizeXlHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>,ISimpleToggleButtonHandler {
        public SourceSizeXlHandler(ICudotosiApplicationModel model) : base(model, model.SourceSizeXl) {
        }

        public async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            if (isChecked) {
                Model.TargetSizeLg.Enabled = true;
                Model.TargetSizeMd.Enabled = true;
                Model.TargetSizeSm.Enabled = true;
                Model.TargetSizeXs.Enabled = true;
            }

            await Task.Run(() => { });
        }
    }
}
