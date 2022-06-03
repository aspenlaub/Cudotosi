using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceSizeXlHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly IPictureHandler _PictureHandler;

    public SourceSizeXlHandler(ICudotosiApplicationModel model, IPictureHandler pictureHandler) : base(model, model.SourceSizeXl) {
        _PictureHandler = pictureHandler;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        if (!isChecked) { return; }

        Model.TargetSizeLg.Enabled = true;
        Model.TargetSizeMd.Enabled = true;
        Model.TargetSizeSm.Enabled = true;
        Model.TargetSizeXs.Enabled = true;

        await _PictureHandler.LoadFromFile(_PictureHandler.FileName());
    }
}