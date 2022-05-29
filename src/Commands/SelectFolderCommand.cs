using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class SelectFolderCommand : ICommand {
    private readonly ICudotosiApplicationModel Model;
    private readonly IUserInteraction UserInteraction;
    private readonly ISimpleTextHandler FolderTextHandler;

    public SelectFolderCommand(ICudotosiApplicationModel model, IUserInteraction userInteraction, ISimpleTextHandler folderTextHandler) {
        Model = model;
        UserInteraction = userInteraction;
        FolderTextHandler = folderTextHandler;
    }

    public async Task ExecuteAsync() {
        if (!Model.SelectFolder.Enabled) {
            return;
        }

        await FolderTextHandler.TextChangedAsync(UserInteraction.PromptForFolder(Model.Folder.Text));
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        return await Task.FromResult(true);
    }
}