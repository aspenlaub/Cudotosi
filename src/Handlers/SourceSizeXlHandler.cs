using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceSizeXlHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly IPictureHandler _PictureHandler;
    private readonly IToggleButtonHandler _TargetSizeLgHandler;

    public SourceSizeXlHandler(ICudotosiApplicationModel model, IToggleButtonHandler targetSizeLgHandler, IPictureHandler pictureHandler) : base(model, model.SourceSizeXl) {
        _PictureHandler = pictureHandler;
        _TargetSizeLgHandler = targetSizeLgHandler;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        if (!isChecked) { return; }

        Model.TargetSizeXl.Enabled = false;
        Model.TargetSizeLg.Enabled = true;
        Model.TargetSizeMd.Enabled = true;
        Model.TargetSizeSm.Enabled = true;
        Model.TargetSizeXs.Enabled = true;
        if (Model.TargetSizeXl.IsChecked) {
            await _TargetSizeLgHandler.ToggledAsync(true);
        }

        await _PictureHandler.LoadFromFile(_PictureHandler.FileName());
    }
}