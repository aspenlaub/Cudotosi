using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class SourceSizeLgHandler : ToggleButtonHandlerBase<ICudotosiApplicationModel> {
    private readonly IToggleButtonHandler TargetSizeMdHandler;
    private readonly IPictureHandler PictureHandler;

    public SourceSizeLgHandler(ICudotosiApplicationModel model, IToggleButtonHandler targetSizeMdHandler, IPictureHandler pictureHandler) : base(model, model.SourceSizeLg) {
        TargetSizeMdHandler = targetSizeMdHandler;
        PictureHandler = pictureHandler;
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
            await TargetSizeMdHandler.ToggledAsync(true);
        }

        await PictureHandler.LoadFromFile(PictureHandler.FileName());
    }
}