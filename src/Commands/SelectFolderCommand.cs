using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands {
    public class SelectFolderCommand : ICommand {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IFolderDialog vFolderDialog;
        private readonly IFolderUpdater vFolderUpdater;

        public SelectFolderCommand(ICudotosiApplicationModel model, IFolderDialog folderDialog, IFolderUpdater folderUpdater) {
            vModel = model;
            vFolderDialog = folderDialog;
            vFolderUpdater = folderUpdater;
        }

        public async Task ExecuteAsync() {
            if (!vModel.SelectFolder.Enabled) {
                return;
            }

            await vFolderUpdater.FolderTextChangedAsync(vFolderDialog.PromptForFolder(vModel.Folder.Text));
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            return await Task.FromResult(true);
        }
    }
}