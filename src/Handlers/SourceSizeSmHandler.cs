﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceSizeSmHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly IToggleButtonHandler _TargetSizeXsHandler;
    private readonly IPictureHandler _PictureHandler;

    public SourceSizeSmHandler(ICudotosiApplicationModel model, IToggleButtonHandler targetSizeXsHandler, IPictureHandler pictureHandler) : base(model, model.SourceSizeSm) {
        _TargetSizeXsHandler = targetSizeXsHandler;
        _PictureHandler = pictureHandler;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        if (!isChecked) { return; }

        Model.TargetSizeXl.Enabled = false;
        Model.TargetSizeLg.Enabled = false;
        Model.TargetSizeMd.Enabled = false;
        Model.TargetSizeSm.Enabled = false;
        Model.TargetSizeXs.Enabled = true;
        if (Model.TargetSizeXl.IsChecked || Model.TargetSizeLg.IsChecked || Model.TargetSizeMd.IsChecked || Model.TargetSizeSm.IsChecked) {
            await _TargetSizeXsHandler.ToggledAsync(true);
        }

        await _PictureHandler.LoadFromFile(_PictureHandler.FileName());
    }
}