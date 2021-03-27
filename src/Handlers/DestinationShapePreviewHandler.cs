using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class DestinationShapePreviewHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
        private readonly ISourceAreaHandler vSourceAreaHandler;

        public DestinationShapePreviewHandler(ICudotosiApplicationModel model, ISourceAreaHandler sourceAreaHandler) : base(model, model.DestinationShapePreview) {
            vSourceAreaHandler = sourceAreaHandler;
        }

        public override async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);

            await vSourceAreaHandler.OnDestinationShapeChangedAsync();
        }
    }
}
