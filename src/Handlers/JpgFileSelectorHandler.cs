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
    private readonly ICudotosiApplicationModel _Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> _GuiAndAppHandler;
    private readonly IPictureHandler _PictureHandler;
    private readonly IToggleButtonHandler _SourceSizeXlHandler;
    private readonly IJpgFileNameChanger _JpgFileNameChanger;

    public JpgFileSelectorHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, IPictureHandler pictureHandler, IToggleButtonHandler sourceSizeXlHandler, IJpgFileNameChanger jpgFileNameChanger) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
        _PictureHandler = pictureHandler;
        _SourceSizeXlHandler = sourceSizeXlHandler;
        _JpgFileNameChanger = jpgFileNameChanger;
    }

    public async Task UpdateSelectableValuesAsync() {
        var shortFileNames = _Model.Folder.Type == StatusType.None
            ? Directory.GetFiles(_Model.Folder.Text, "*_XL.jpg", SearchOption.TopDirectoryOnly).OrderBy(x => x).ToList()
            : new List<string>();
        var selectables = shortFileNames.Select(f => new Selectable { Guid = f, Name = f.Substring(f.LastIndexOf('\\') + 1) }).ToList();
        if (_Model.JpgFile.AreSelectablesIdentical(selectables)) { return; }

        _Model.JpgFile.UpdateSelectables(selectables);
        await SelectedIndexChangedAsync(-1);
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public async Task SelectedIndexChangedAsync(int selectedIndex) {
        var haveSelectedIndex = selectedIndex >= 0;
        if (haveSelectedIndex && _Model.JpgFile.SelectedIndex == selectedIndex) { return; }

        _Model.JpgFile.SelectedIndex = selectedIndex;
        _Model.DestinationShapeAsIs.Enabled = haveSelectedIndex;
        _Model.DestinationShapeSquare.Enabled = haveSelectedIndex;
        _Model.DestinationShapePreview.Enabled = haveSelectedIndex;
        _Model.TransformHowManyPercent100.Enabled = haveSelectedIndex;
        _Model.TransformHowManyPercent50.Enabled = haveSelectedIndex;
        var fileName = _PictureHandler.FileName();
        await _PictureHandler.LoadFromFile(fileName);
        _Model.SourceSizeLg.Enabled = File.Exists(_JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Lg, false));
        _Model.SourceSizeMd.Enabled = File.Exists(_JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Md, false));
        _Model.SourceSizeSm.Enabled = File.Exists(_JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Sm, false));
        _SourceSizeXlHandler.SetChecked(false);
        await _SourceSizeXlHandler.ToggledAsync(true);
        _Model.Status.Type = StatusType.None;
        _Model.Status.Text = string.IsNullOrEmpty(fileName) ? "" : Properties.Resources.PleaseClickTopLeft;
    }
}