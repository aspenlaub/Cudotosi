using System.IO;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class FolderTextHandler : ISimpleTextHandler {
    private readonly ICudotosiApplicationModel _Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> _GuiAndAppHandler;
    private readonly ISimpleSelectorHandler _JpgFileSelectorHandler;

    public FolderTextHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, ISimpleSelectorHandler jpgFileSelectorHandler) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
        _JpgFileSelectorHandler = jpgFileSelectorHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (_Model.Folder.Text == text) { return; }

        _Model.Folder.Text = text;
        _Model.Folder.Type = Directory.Exists(text) ? StatusType.None : StatusType.Error;
        await _JpgFileSelectorHandler.UpdateSelectableValuesAsync();
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}