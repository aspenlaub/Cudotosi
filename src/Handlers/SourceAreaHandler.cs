using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceAreaHandler  : ISourceAreaHandler {
    private readonly ICudotosiApplicationModel _Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> _GuiAndAppHandler;
    private readonly IMousePositionAdjuster _MousePositionAdjuster;
    private readonly ICutCalculator _CutCalculator;

    public SourceAreaHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, IMousePositionAdjuster mousePositionAdjuster, ICutCalculator cutCalculator) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
        _MousePositionAdjuster = mousePositionAdjuster;
        _CutCalculator = cutCalculator;
    }

    public async Task OnMousePositionChangedAsync() {
        _MousePositionAdjuster.AdjustMousePosition(_Model);

        _CutCalculator.CutOut(_Model);

        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public async Task OnDestinationShapeChangedAsync() {
        await OnMousePositionChangedAsync();
    }

    public async Task OnTransformHowManyPercentChangedAsync() {
        await OnMousePositionChangedAsync();
    }

}