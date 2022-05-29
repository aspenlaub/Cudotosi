using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class TargetSizeLgHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    public TargetSizeLgHandler(ICudotosiApplicationModel model) : base(model, model.TargetSizeLg) {
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        await Task.CompletedTask;
    }
}