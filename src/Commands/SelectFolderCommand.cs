using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class SelectFolderCommand : ICommand {
    private readonly ICudotosiApplicationModel _Model;
    private readonly IUserInteraction _UserInteraction;
    private readonly ISimpleTextHandler _FolderTextHandler;

    public SelectFolderCommand(ICudotosiApplicationModel model, IUserInteraction userInteraction, ISimpleTextHandler folderTextHandler) {
        _Model = model;
        _UserInteraction = userInteraction;
        _FolderTextHandler = folderTextHandler;
    }

    public async Task ExecuteAsync() {
        if (!_Model.SelectFolder.Enabled) {
            return;
        }

        await _FolderTextHandler.TextChangedAsync(_UserInteraction.PromptForFolder(_Model.Folder.Text));
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        return await Task.FromResult(true);
    }
}