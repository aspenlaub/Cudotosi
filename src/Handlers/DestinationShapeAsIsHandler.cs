﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class DestinationShapeAsIsHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
        private readonly ISourceAreaHandler SourceAreaHandler;

        public DestinationShapeAsIsHandler(ICudotosiApplicationModel model, ISourceAreaHandler sourceAreaHandler) : base(model, model.DestinationShapeAsIs) {
            SourceAreaHandler = sourceAreaHandler;
        }

        public override async Task ToggledAsync(bool isChecked) {
            if (Unchanged(isChecked)) { return; }

            SetChecked(isChecked);

            await SourceAreaHandler.OnDestinationShapeChangedAsync();
        }
    }
}
