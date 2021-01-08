using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class DestinationShapeAsIsHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>, ISimpleToggleButtonHandler {
        public DestinationShapeAsIsHandler(ICudotosiApplicationModel model) : base(model, model.DestinationShapeAsIs) {
        }

        public Task ToggledAsync(bool isChecked) {
            throw new NotImplementedException();
        }
    }
}
