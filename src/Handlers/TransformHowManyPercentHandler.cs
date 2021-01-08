using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class TransformHowManyPercentHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>, ISimpleToggleButtonHandler {
        protected int Percentage;

        public TransformHowManyPercentHandler(ICudotosiApplicationModel model, ToggleButton toggleButton, int percentage) : base(model, toggleButton) {
            Percentage = percentage;
        }

        public Task ToggledAsync(bool isChecked) {
            throw new NotImplementedException();
        }
    }
}
