using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application {
    public class CudotosiApplication : ApplicationBase<IGuiAndApplicationSynchronizer<ICudotosiApplicationModel>, ICudotosiApplicationModel>, ICudotosiApplication {
        public ICudotosiHandlers Handlers { get; private set; }
        public ICudotosiCommands Commands { get; private set; }
        private readonly IFolderDialog vFolderDialog;

        public CudotosiApplication(IButtonNameToCommandMapper buttonNameToCommandMapper,
                IGuiAndApplicationSynchronizer<ICudotosiApplicationModel> guiAndApplicationSynchronizer,
                ICudotosiApplicationModel model,
                IFolderDialog folderDialog)
            : base(buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) {
            vFolderDialog = folderDialog;
        }

        protected override async Task EnableOrDisableButtonsAsync() {
            Model.SelectFolder.Enabled = await Commands.SelectFolderCommand.ShouldBeEnabledAsync();
            Model.Save.Enabled = await Commands.SaveCommand.ShouldBeEnabledAsync();
        }

        public override void RegisterTypes() {
            Handlers = new CudotosiHandlers();
            Commands = new CudotosiCommands {
                SelectFolderCommand = new SelectFolderCommand(Model, vFolderDialog, this),
                SaveCommand = new SaveCommand(Model)
            };
        }

        public override async Task OnLoadedAsync() {
            await FolderTextChangedAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            await base.OnLoadedAsync();
        }

        public async Task FolderTextChangedAsync(string text) {
            if (Model.Folder.Text == text) { return; }

            Model.Folder.Text = text;
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}