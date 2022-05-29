using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceAreaHandler  : ISourceAreaHandler {
    private readonly ICudotosiApplicationModel Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> GuiAndAppHandler;
    private readonly IMousePositionAdjuster MousePositionAdjuster;
    private readonly ICutCalculator CutCalculator;

    public SourceAreaHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, IMousePositionAdjuster mousePositionAdjuster, ICutCalculator cutCalculator) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
        MousePositionAdjuster = mousePositionAdjuster;
        CutCalculator = cutCalculator;
    }

    public async Task OnMousePositionChangedAsync() {
        MousePositionAdjuster.AdjustMousePosition(Model);

        CutCalculator.CutOut(Model);

        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public async Task OnDestinationShapeChangedAsync() {
        await OnMousePositionChangedAsync();
    }

    public async Task OnTransformHowManyPercentChangedAsync() {
        await OnMousePositionChangedAsync();
    }

}