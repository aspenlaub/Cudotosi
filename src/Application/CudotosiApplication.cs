using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;

public class CudotosiApplication : ApplicationBase<IGuiAndApplicationSynchronizer<CudotosiApplicationModel>, CudotosiApplicationModel> {
    public ICudotosiHandlers Handlers { get; private set; }
    public ICudotosiCommands Commands { get; private set; }
    private readonly IJpgFileNameChanger _JpgFileNameChanger;
    public ITashHandler<ICudotosiApplicationModel> TashHandler { get; private set; }
    private readonly ITashAccessor _TashAccessor;
    private readonly IMousePositionAdjuster _MousePositionAdjuster;
    private readonly ICutCalculator _CutCalculator;
    private readonly IMouseOwner _MouseOwner;
    private readonly IUserInteraction _UserInteraction;
    private readonly IMethodNamesFromStackFramesExtractor _MethodNamesFromStackFramesExtractor;

    public CudotosiApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        IGuiAndApplicationSynchronizer<CudotosiApplicationModel> guiAndApplicationSynchronizer,
        CudotosiApplicationModel model, IJpgFileNameChanger jpgFileNameChanger,
        ITashAccessor tashAccessor, ISimpleLogger simpleLogger,
        IMousePositionAdjuster mousePositionAdjuster, ICutCalculator cutCalculator, IMouseOwner mouseOwner,
        IUserInteraction userInteraction, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor)
        : base(buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndApplicationSynchronizer, model, simpleLogger) {
        _JpgFileNameChanger = jpgFileNameChanger;
        _TashAccessor = tashAccessor;
        _MousePositionAdjuster = mousePositionAdjuster;
        _CutCalculator = cutCalculator;
        _MouseOwner = mouseOwner;
        _UserInteraction = userInteraction;
        _MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    protected override async Task EnableOrDisableButtonsAsync() {
        Model.SelectFolder.Enabled = await Commands.SelectFolderCommand.ShouldBeEnabledAsync();
        Model.Save.Enabled = await Commands.SaveCommand.ShouldBeEnabledAsync();
        Model.Default.Enabled = await Commands.DefaultCommand.ShouldBeEnabledAsync();
    }

    protected override void CreateCommandsAndHandlers() {
        var mousePositionHandler = new SourceAreaHandler(Model, this, _MousePositionAdjuster, _CutCalculator);
        var pictureHandler = new PictureHandler(Model, this, _JpgFileNameChanger, mousePositionHandler, SimpleLogger, _MethodNamesFromStackFramesExtractor);
        var sourceSizeXlHandler = new SourceSizeXlHandler(Model, pictureHandler);
        var jpgFileSelectorHandler = new JpgFileSelectorHandler(Model, this, pictureHandler, sourceSizeXlHandler, _JpgFileNameChanger);
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
            DestinationShapePreviewHandler = new DestinationShapePreviewHandler(Model, mousePositionHandler),
            TransformHowManyPercent100Handler = new TransformHowManyPercentHandler(Model, mousePositionHandler, Model.TransformHowManyPercent100, 100),
            TransformHowManyPercent50Handler = new TransformHowManyPercentHandler(Model, mousePositionHandler, Model.TransformHowManyPercent50, 50)
        };

        Commands = new CudotosiCommands {
            SelectFolderCommand = new SelectFolderCommand(Model, _UserInteraction, folderTextHandler),
            SaveCommand = new SaveCommand(Model, _CutCalculator, jpgFileSelectorHandler, _JpgFileNameChanger, _UserInteraction, SimpleLogger, _MethodNamesFromStackFramesExtractor),
            DefaultCommand = new DefaultCommand(Model, _MouseOwner, SimpleLogger, _MethodNamesFromStackFramesExtractor)
        };

        var selectors = new Dictionary<string, ISelector> {
            { nameof(ICudotosiApplicationModel.JpgFile), Model.JpgFile }
        };

        var communicator = new TashCommunicatorBase<ICudotosiApplicationModel>(_TashAccessor, SimpleLogger, _MethodNamesFromStackFramesExtractor);
        var selectorHandler = new TashSelectorHandler(Handlers, SimpleLogger, communicator, selectors, _MethodNamesFromStackFramesExtractor);
        var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, SimpleLogger, selectorHandler, communicator, selectors, _MethodNamesFromStackFramesExtractor);
        TashHandler = new TashHandler(_TashAccessor, SimpleLogger, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator, _MethodNamesFromStackFramesExtractor);
    }

    public override async Task OnLoadedAsync() {
        await base.OnLoadedAsync();
        await Handlers.FolderTextHandler.TextChangedAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
    }

    public ITashTaskHandlingStatus<ICudotosiApplicationModel> CreateTashTaskHandlingStatus() {
        return new TashTaskHandlingStatus<ICudotosiApplicationModel>(Model, Process.GetCurrentProcess().Id);
    }
}