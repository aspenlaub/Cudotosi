using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class TransformHowManyPercentHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly ISourceAreaHandler _SourceAreaHandler;

    protected int Percentage;

    public TransformHowManyPercentHandler(ICudotosiApplicationModel model, ISourceAreaHandler sourceAreaHandler, ToggleButton toggleButton, int percentage) : base(model, toggleButton) {
        _SourceAreaHandler = sourceAreaHandler;
        Percentage = percentage;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);

        await _SourceAreaHandler.OnTransformHowManyPercentChangedAsync();
    }
}