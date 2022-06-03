using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceSizeLgHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly IToggleButtonHandler _TargetSizeMdHandler;
    private readonly IPictureHandler _PictureHandler;

    public SourceSizeLgHandler(ICudotosiApplicationModel model, IToggleButtonHandler targetSizeMdHandler, IPictureHandler pictureHandler) : base(model, model.SourceSizeLg) {
        _TargetSizeMdHandler = targetSizeMdHandler;
        _PictureHandler = pictureHandler;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        if (!isChecked) { return; }

        Model.TargetSizeLg.Enabled = false;
        Model.TargetSizeMd.Enabled = true;
        Model.TargetSizeSm.Enabled = true;
        Model.TargetSizeXs.Enabled = true;
        if (Model.TargetSizeLg.IsChecked) {
            await _TargetSizeMdHandler.ToggledAsync(true);
        }

        await _PictureHandler.LoadFromFile(_PictureHandler.FileName());
    }
}