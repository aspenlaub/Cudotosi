using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class JpgFileSelectorHandler : ISimpleSelectorHandler {
    private readonly ICudotosiApplicationModel Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> GuiAndAppHandler;
    private readonly IPictureHandler PictureHandler;
    private readonly IToggleButtonHandler SourceSizeXlHandler;
    private readonly IJpgFileNameChanger JpgFileNameChanger;

    public JpgFileSelectorHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, IPictureHandler pictureHandler, IToggleButtonHandler sourceSizeXlHandler, IJpgFileNameChanger jpgFileNameChanger) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
        PictureHandler = pictureHandler;
        SourceSizeXlHandler = sourceSizeXlHandler;
        JpgFileNameChanger = jpgFileNameChanger;
    }

    public async Task UpdateSelectableValuesAsync() {
        var shortFileNames = Model.Folder.Type == StatusType.None
            ? Directory.GetFiles(Model.Folder.Text, "*_XL.jpg", SearchOption.TopDirectoryOnly).OrderBy(x => x).ToList()
            : new List<string>();
        var selectables = shortFileNames.Select(f => new Selectable { Guid = f, Name = f.Substring(f.LastIndexOf('\\') + 1) }).ToList();
        if (Model.JpgFile.AreSelectablesIdentical(selectables)) { return; }

        Model.JpgFile.UpdateSelectables(selectables);
        await SelectedIndexChangedAsync(-1);
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public async Task SelectedIndexChangedAsync(int selectedIndex) {
        var haveSelectedIndex = selectedIndex >= 0;
        if (haveSelectedIndex && Model.JpgFile.SelectedIndex == selectedIndex) { return; }

        Model.JpgFile.SelectedIndex = selectedIndex;
        Model.DestinationShapeAsIs.Enabled = haveSelectedIndex;
        Model.DestinationShapeSquare.Enabled = haveSelectedIndex;
        Model.DestinationShapePreview.Enabled = haveSelectedIndex;
        Model.TransformHowManyPercent100.Enabled = haveSelectedIndex;
        Model.TransformHowManyPercent50.Enabled = haveSelectedIndex;
        var fileName = PictureHandler.FileName();
        await PictureHandler.LoadFromFile(fileName);
        Model.SourceSizeLg.Enabled = File.Exists(JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Lg, false));
        Model.SourceSizeMd.Enabled = File.Exists(JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Md, false));
        Model.SourceSizeSm.Enabled = File.Exists(JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Sm, false));
        SourceSizeXlHandler.SetChecked(false);
        await SourceSizeXlHandler.ToggledAsync(true);
        Model.Status.Type = StatusType.None;
        Model.Status.Text = string.IsNullOrEmpty(fileName) ? "" : Properties.Resources.PleaseClickTopLeft;
    }
}