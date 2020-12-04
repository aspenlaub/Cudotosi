using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application {
    public class CudotosiApplication : ApplicationBase<IGuiAndApplicationSynchronizer<ICudotosiApplicationModel>, ICudotosiApplicationModel> {
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

        protected override void CreateCommandsAndHandlers() {
            var pictureHandler = new PictureHandler(Model, this);
            var sourceSizeXlHandler = new SourceSizeXlHandler(Model);
            var jpgFileSelectorHandler = new JpgFileSelectorHandler(Model, this, pictureHandler, sourceSizeXlHandler);
            var folderTextHandler = new FolderTextHandler(Model, this, jpgFileSelectorHandler);
            Handlers = new CudotosiHandlers {
                FolderTextHandler = folderTextHandler,
                JpgFileSelectorHandler = jpgFileSelectorHandler,
                SourceSizeXlHandler = sourceSizeXlHandler,
                SourceSizeLgHandler = new SourceSizeLgHandler(Model),
                SourceSizeMdHandler = new SourceSizeMdHandler(Model),
                SourceSizeSmHandler = new SourceSizeSmHandler(Model),
                TargetSizeLgHandler = new TargetSizeLgHandler(Model),
                TargetSizeMdHandler = new TargetSizeMdHandler(Model),
                TargetSizeSmHandler = new TargetSizeSmHandler(Model),
                TargetSizeXsHandler = new TargetSizeXsHandler(Model),
                PictureHandler = pictureHandler
            };
            Commands = new CudotosiCommands {
                SelectFolderCommand = new SelectFolderCommand(Model, vFolderDialog, folderTextHandler),
                SaveCommand = new SaveCommand(Model)
            };
        }

        public override async Task OnLoadedAsync() {
            await base.OnLoadedAsync();
            await Handlers.FolderTextHandler.TextChangedAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }
    }
}