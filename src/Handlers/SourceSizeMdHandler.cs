using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceSizeMdHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly IToggleButtonHandler _TargetSizeSmHandler;
    private readonly IPictureHandler _PictureHandler;

    public SourceSizeMdHandler(ICudotosiApplicationModel model, IToggleButtonHandler targetSizeSmHandler, IPictureHandler pictureHandler) : base(model, model.SourceSizeMd) {
        _TargetSizeSmHandler = targetSizeSmHandler;
        _PictureHandler = pictureHandler;
    }

    public override async Task ToggledAsync(bool isChecked) {
        if (Unchanged(isChecked)) { return; }

        SetChecked(isChecked);
        if (!isChecked) { return; }

        Model.TargetSizeLg.Enabled = false;
        Model.TargetSizeMd.Enabled = false;
        Model.TargetSizeSm.Enabled = true;
        Model.TargetSizeXs.Enabled = true;
        if (Model.TargetSizeLg.IsChecked || Model.TargetSizeMd.IsChecked) {
            await _TargetSizeSmHandler.ToggledAsync(true);
        }

        await _PictureHandler.LoadFromFile(_PictureHandler.FileName());
    }
}