using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class TargetSizeXlHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    public TargetSizeXlHandler(ICudotosiApplicationModel model) : base(model, model.TargetSizeXl) {
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        await Task.CompletedTask;
    }
}