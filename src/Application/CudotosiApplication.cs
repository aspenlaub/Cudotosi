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
        private readonly IJpgFileNameChanger vJpgFileNameChanger;

        public CudotosiApplication(IButtonNameToCommandMapper buttonNameToCommandMapper,
                IGuiAndApplicationSynchronizer<ICudotosiApplicationModel> guiAndApplicationSynchronizer,
                ICudotosiApplicationModel model,
                IFolderDialog folderDialog,
                IJpgFileNameChanger jpgFileNameChanger)
            : base(buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) {
            vFolderDialog = folderDialog;
            vJpgFileNameChanger = jpgFileNameChanger;
        }

        protected override async Task EnableOrDisableButtonsAsync() {
            Model.SelectFolder.Enabled = await Commands.SelectFolderCommand.ShouldBeEnabledAsync();
            Model.Save.Enabled = await Commands.SaveCommand.ShouldBeEnabledAsync();
        }

        protected override void CreateCommandsAndHandlers() {
            var pictureHandler = new PictureHandler(Model, this, vJpgFileNameChanger);
            var sourceSizeXlHandler = new SourceSizeXlHandler(Model, pictureHandler);
            var jpgFileSelectorHandler = new JpgFileSelectorHandler(Model, this, pictureHandler, sourceSizeXlHandler, vJpgFileNameChanger);
            var folderTextHandler = new FolderTextHandler(Model, this, jpgFileSelectorHandler);
            var targetSizeMdHandler = new TargetSizeMdHandler(Model);
            var targetSizeSmHandler = new TargetSizeSmHandler(Model);
            var targetSizeXsHandler = new TargetSizeXsHandler(Model);
            Handlers = new CudotosiHandlers {
                FolderTextHandler = folderTextHandler,
                JpgFileSelectorHandler = jpgFileSelectorHandler,
                SourceSizeXlHandler = sourceSizeXlHandler,
                SourceSizeLgHandler = new SourceSizeLgHandler(Model, targetSizeMdHandler, pictureHandler),
                SourceSizeMdHandler = new SourceSizeMdHandler(Model, targetSizeSmHandler, pictureHandler),
                SourceSizeSmHandler = new SourceSizeSmHandler(Model, targetSizeXsHandler, pictureHandler),
                TargetSizeLgHandler = new TargetSizeLgHandler(Model),
                TargetSizeMdHandler = targetSizeMdHandler,
                TargetSizeSmHandler = targetSizeSmHandler,
                TargetSizeXsHandler = targetSizeXsHandler,
                PictureHandler = pictureHandler,
                DestinationShapeAsIsHandler = new DestinationShapeAsIsHandler(Model),
                DestinationShapeSquareHandler = new DestinationShapeSquareHandler(Model),
                TransformHowManyPercent100Handler = new TransformHowManyPercentHandler(Model, Model.TransformHowManyPercent100, 100),
                TransformHowManyPercent50Handler = new TransformHowManyPercentHandler(Model, Model.TransformHowManyPercent50, 50)
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