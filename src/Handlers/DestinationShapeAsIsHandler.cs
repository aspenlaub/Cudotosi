using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class DestinationShapeAsIsHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>, ISimpleToggleButtonHandler {
        private readonly ISourceAreaHandler vSourceAreaHandler;

        public DestinationShapeAsIsHandler(ICudotosiApplicationModel model, ISourceAreaHandler sourceAreaHandler) : base(model, model.DestinationShapeAsIs) {
            vSourceAreaHandler = sourceAreaHandler;
        }

        public async Task ToggledAsync(bool isChecked) {
            await vSourceAreaHandler.OnDestinationShapeChangedAsync();
        }
    }
}
