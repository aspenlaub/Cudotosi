using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class DestinationShapeSquareHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel>, ISimpleToggleButtonHandler {
        public DestinationShapeSquareHandler(ICudotosiApplicationModel model) : base(model, model.DestinationShapeSquare) {
        }

        public Task ToggledAsync(bool isChecked) {
            throw new NotImplementedException();
        }
    }
}
