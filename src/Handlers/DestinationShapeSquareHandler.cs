using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class DestinationShapeSquareHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly ISourceAreaHandler _SourceAreaHandler;

    public DestinationShapeSquareHandler(ICudotosiApplicationModel model, ISourceAreaHandler sourceAreaHandler) : base(model, model.DestinationShapeSquare) {
        _SourceAreaHandler = sourceAreaHandler;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);

        await _SourceAreaHandler.OnDestinationShapeChangedAsync();
    }
}