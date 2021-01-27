using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class TargetSizeSmHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
        public TargetSizeSmHandler(ICudotosiApplicationModel model) : base(model, model.TargetSizeSm) {
        }

        public override async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);
            await Task.CompletedTask;
        }
    }
}
