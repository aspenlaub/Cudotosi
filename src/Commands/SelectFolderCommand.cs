using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands {
    public class SelectFolderCommand : ICommand {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IUserInteraction vUserInteraction;
        private readonly ISimpleTextHandler vFolderTextHandler;

        public SelectFolderCommand(ICudotosiApplicationModel model, IUserInteraction userInteraction, ISimpleTextHandler folderTextHandler) {
            vModel = model;
            vUserInteraction = userInteraction;
            vFolderTextHandler = folderTextHandler;
        }

        public async Task ExecuteAsync() {
            if (!vModel.SelectFolder.Enabled) {
                return;
            }

            await vFolderTextHandler.TextChangedAsync(vUserInteraction.PromptForFolder(vModel.Folder.Text));
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            return await Task.FromResult(true);
        }
    }
}