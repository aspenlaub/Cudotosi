using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class TransformHowManyPercentHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>, ISimpleToggleButtonHandler {
        private readonly ISourceAreaHandler vSourceAreaHandler;

        protected int Percentage;

        public TransformHowManyPercentHandler(ICudotosiApplicationModel model, ISourceAreaHandler sourceAreaHandler, ToggleButton toggleButton, int percentage) : base(model, toggleButton) {
            vSourceAreaHandler = sourceAreaHandler;
            Percentage = percentage;
        }

        public async Task ToggledAsync(bool isChecked) {
            await vSourceAreaHandler.OnTransformHowManyPercentChangedAsync();
        }
    }
}
