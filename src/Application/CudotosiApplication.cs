using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application {
    public class CudotosiApplication : ApplicationBase<IGuiAndApplicationSynchronizer<ICudotosiApplicationModel>, ICudotosiApplicationModel> {
        public ICudotosiHandlers Handlers { get; private set; }
        public ICudotosiCommands Commands { get; private set; }
        private readonly IJpgFileNameChanger vJpgFileNameChanger;
        public ITashHandler<ICudotosiApplicationModel> TashHandler { get; private set; }
        private readonly ITashAccessor vTashAccessor;
        private readonly ISimpleLogger vSimpleLogger;
        private readonly ILogConfiguration vLogConfiguration;
        private readonly IMousePositionAdjuster vMousePositionAdjuster;
        private readonly ICutCalculator vCutCalculator;
        private readonly IMouseOwner vMouseOwner;
        private readonly IUserInteraction vUserInteraction;

        public CudotosiApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
                IGuiAndApplicationSynchronizer<ICudotosiApplicationModel> guiAndApplicationSynchronizer,
                ICudotosiApplicationModel model, IJpgFileNameChanger jpgFileNameChanger,
                ITashAccessor tashAccessor, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration,
                IMousePositionAdjuster mousePositionAdjuster, ICutCalculator cutCalculator, IMouseOwner mouseOwner,
                IUserInteraction userInteraction)
            : base(buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndApplicationSynchronizer, model) {
            vJpgFileNameChanger = jpgFileNameChanger;
            vTashAccessor = tashAccessor;
            vSimpleLogger = simpleLogger;
            vLogConfiguration = logConfiguration;
            vMousePositionAdjuster = mousePositionAdjuster;
            vCutCalculator = cutCalculator;
            vMouseOwner = mouseOwner;
            vUserInteraction = userInteraction;
        }

        protected override async Task EnableOrDisableButtonsAsync() {
            Model.SelectFolder.Enabled = await Commands.SelectFolderCommand.ShouldBeEnabledAsync();
            Model.Save.Enabled = await Commands.SaveCommand.ShouldBeEnabledAsync();
            Model.Default.Enabled = await Commands.DefaultCommand.ShouldBeEnabledAsync();
        }

        protected override void CreateCommandsAndHandlers() {
            var mousePositionHandler = new SourceAreaHandler(Model, this, vMousePositionAdjuster, vCutCalculator);
            var pictureHandler = new PictureHandler(Model, this, vJpgFileNameChanger, mousePositionHandler, vSimpleLogger, vLogConfiguration);
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
                DestinationShapeAsIsHandler = new DestinationShapeAsIsHandler(Model, mousePositionHandler),
                DestinationShapeSquareHandler = new DestinationShapeSquareHandler(Model, mousePositionHandler),
                TransformHowManyPercent100Handler = new TransformHowManyPercentHandler(Model, mousePositionHandler, Model.TransformHowManyPercent100, 100),
                TransformHowManyPercent50Handler = new TransformHowManyPercentHandler(Model, mousePositionHandler, Model.TransformHowManyPercent50, 50)
            };

            Commands = new CudotosiCommands {
                SelectFolderCommand = new SelectFolderCommand(Model, vUserInteraction, folderTextHandler),
                SaveCommand = new SaveCommand(Model, vCutCalculator, jpgFileSelectorHandler, vJpgFileNameChanger, vUserInteraction, vSimpleLogger, vLogConfiguration),
                DefaultCommand = new DefaultCommand(Model, vMouseOwner, vSimpleLogger, vLogConfiguration)
            };

            var selectors = new Dictionary<string, ISelector> {
                { nameof(ICudotosiApplicationModel.JpgFile), Model.JpgFile }
            };

            var communicator = new TashCommunicatorBase<ICudotosiApplicationModel>(vTashAccessor, vSimpleLogger, vLogConfiguration);
            var selectorHandler = new TashSelectorHandler(Handlers, vSimpleLogger, communicator, selectors);
            var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, vSimpleLogger, selectorHandler, communicator, selectors);
            TashHandler = new TashHandler(vTashAccessor, vSimpleLogger, vLogConfiguration, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator);
        }

        public override async Task OnLoadedAsync() {
            await base.OnLoadedAsync();
            await Handlers.FolderTextHandler.TextChangedAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }

        public ITashTaskHandlingStatus<ICudotosiApplicationModel> CreateTashTaskHandlingStatus() {
            return new TashTaskHandlingStatus<ICudotosiApplicationModel>(Model, Process.GetCurrentProcess().Id);
        }
    }
}