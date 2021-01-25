using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceAreaHandler  : ISourceAreaHandler {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly IMousePositionAdjuster vMousePositionAdjuster;
        private readonly ICutCalculator vCutCalculator;

        public SourceAreaHandler(ICudotosiApplicationModel model, IGuiAndAppHandler guiAndAppHandler, IMousePositionAdjuster mousePositionAdjuster, ICutCalculator cutCalculator) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vMousePositionAdjuster = mousePositionAdjuster;
            vCutCalculator = cutCalculator;
        }

        public async Task OnMousePositionChangedAsync() {
            vMousePositionAdjuster.AdjustMousePosition(vModel);

            vCutCalculator.CutOut(vModel);

            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task OnDestinationShapeChangedAsync() {
            await OnMousePositionChangedAsync();
        }

        public async Task OnTransformHowManyPercentChangedAsync() {
            await OnMousePositionChangedAsync();
        }

    }
}
