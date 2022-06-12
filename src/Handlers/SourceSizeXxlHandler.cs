using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceSizeXxlHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly IPictureHandler _PictureHandler;

    public SourceSizeXxlHandler(ICudotosiApplicationModel model, IPictureHandler pictureHandler) : base(model, model.SourceSizeXxl) {
        _PictureHandler = pictureHandler;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        if (!isChecked) { return; }

        Model.TargetSizeXl.Enabled = true;
        Model.TargetSizeLg.Enabled = true;
        Model.TargetSizeMd.Enabled = true;
        Model.TargetSizeSm.Enabled = true;
        Model.TargetSizeXs.Enabled = true;

        await _PictureHandler.LoadFromFile(_PictureHandler.FileName());
    }
}