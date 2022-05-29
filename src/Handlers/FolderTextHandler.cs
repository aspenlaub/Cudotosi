using System.IO;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class FolderTextHandler : ISimpleTextHandler {
    private readonly ICudotosiApplicationModel Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> GuiAndAppHandler;
    private readonly ISimpleSelectorHandler JpgFileSelectorHandler;

    public FolderTextHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, ISimpleSelectorHandler jpgFileSelectorHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
        JpgFileSelectorHandler = jpgFileSelectorHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (Model.Folder.Text == text) { return; }

        Model.Folder.Text = text;
        Model.Folder.Type = Directory.Exists(text) ? StatusType.None : StatusType.Error;
        await JpgFileSelectorHandler.UpdateSelectableValuesAsync();
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}