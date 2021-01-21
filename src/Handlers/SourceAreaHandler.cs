using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class SourceAreaHandler  : ISourceAreaHandler {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly IMousePositionAdjuster vMousePositionAdjuster;

        public SourceAreaHandler(ICudotosiApplicationModel model, IGuiAndAppHandler guiAndAppHandler, IMousePositionAdjuster mousePositionAdjuster) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vMousePositionAdjuster = mousePositionAdjuster;
        }

        public async Task OnMousePositionChangedAsync() {
            vMousePositionAdjuster.AdjustMousePosition(vModel);
            var xPercent = (int)(100.0 * vModel.MousePosX / vModel.PictureWidth);
            var yPercent = (int)(100.0 * vModel.MousePosY / vModel.PictureHeight);
            vModel.Status.Text = $"X: {xPercent}%, Y: {yPercent}%";
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
