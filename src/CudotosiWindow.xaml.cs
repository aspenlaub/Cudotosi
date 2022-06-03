using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Input;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Ookii.Dialogs.Wpf;
using IContainer = Autofac.IContainer;
using MessageBox = System.Windows.MessageBox;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi;

/// <summary>
/// Interaction logic for CudotosiWindow.xaml
/// </summary>
// ReSharper disable once UnusedMember.Global
public partial class CudotosiWindow : IMouseOwner, IUserInteraction, IAsyncDisposable {
    private static IContainer Container { get; set; }

    private CudotosiApplication _CudotosiApp;
    private ITashTimer<ICudotosiApplicationModel> _TashTimer;

    public CudotosiWindow() {
        InitializeComponent();

        Title = Properties.Resources.CudotosiWindowTitle;
        Name = Properties.Resources.CudotosiWindowName;
        AutomationProperties.SetAutomationId(this, Name);
        AutomationProperties.SetName(this, Name);
    }

    private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
        await BuildContainerIfNecessaryAsync();

        _CudotosiApp = Container.Resolve<CudotosiApplication>();
        await _CudotosiApp.OnLoadedAsync();

        var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
        var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();
        var toggleButtonNameToHandlerMapper = Container.Resolve<IToggleButtonNameToHandlerMapper>();

        var commands = _CudotosiApp.Commands;
        guiToAppGate.WireButtonAndCommand(SelectFolder, commands.SelectFolderCommand, buttonNameToCommandMapper);
        guiToAppGate.WireButtonAndCommand(Save, commands.SaveCommand, buttonNameToCommandMapper);
        guiToAppGate.WireButtonAndCommand(Default, commands.DefaultCommand, buttonNameToCommandMapper);

        guiToAppGate.RegisterAsyncTextBoxCallback(Folder, t => _CudotosiApp.Handlers.FolderTextHandler.TextChangedAsync(t));
        guiToAppGate.RegisterAsyncSelectorCallback(JpgFile, t => _CudotosiApp.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(t));

        guiToAppGate.WireToggleButtonAndHandler(SourceSizeXl, _CudotosiApp.Handlers.SourceSizeXlHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(SourceSizeLg, _CudotosiApp.Handlers.SourceSizeLgHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(SourceSizeMd, _CudotosiApp.Handlers.SourceSizeMdHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(SourceSizeSm, _CudotosiApp.Handlers.SourceSizeSmHandler, toggleButtonNameToHandlerMapper);

        guiToAppGate.WireToggleButtonAndHandler(TargetSizeLg, _CudotosiApp.Handlers.TargetSizeLgHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(TargetSizeMd, _CudotosiApp.Handlers.TargetSizeMdHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(TargetSizeSm, _CudotosiApp.Handlers.TargetSizeSmHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(TargetSizeXs, _CudotosiApp.Handlers.TargetSizeXsHandler, toggleButtonNameToHandlerMapper);

        guiToAppGate.WireToggleButtonAndHandler(DestinationShapeAsIs, _CudotosiApp.Handlers.DestinationShapeAsIsHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(DestinationShapeSquare, _CudotosiApp.Handlers.DestinationShapeSquareHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(DestinationShapePreview, _CudotosiApp.Handlers.DestinationShapePreviewHandler, toggleButtonNameToHandlerMapper);

        guiToAppGate.WireToggleButtonAndHandler(TransformHowManyPercent100, _CudotosiApp.Handlers.TransformHowManyPercent100Handler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(TransformHowManyPercent50, _CudotosiApp.Handlers.TransformHowManyPercent50Handler, toggleButtonNameToHandlerMapper);

        _TashTimer = new TashTimer<ICudotosiApplicationModel>(Container.Resolve<ITashAccessor>(), _CudotosiApp.TashHandler, guiToAppGate);
        if (!await _TashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.CudotosiWindowTitle)) {
            Close();
        }

        _TashTimer.CreateAndStartTimer(_CudotosiApp.CreateTashTaskHandlingStatus());

        AdjustCanvasAndImageSync();

        await ExceptionHandler.RunAsync(WindowsApplication.Current, TimeSpan.FromSeconds(7));
    }

    private async Task BuildContainerIfNecessaryAsync() {
        var builder = await new ContainerBuilder().UseCudotosiVishizhukelNetAndPeghAsync(this);
        Container = builder.Build();
    }

    public string PromptForFolder(string folder) {
        var folderBrowserDialog = new VistaFolderBrowserDialog {
            SelectedPath = folder,
            ShowNewFolderButton = true
        };
        return folderBrowserDialog.ShowDialog() != true ? folder : folderBrowserDialog.SelectedPath;
    }

    private async void Picture_OnMouseDown(object sender, MouseButtonEventArgs e) {
        var p = e.GetPosition(Picture);
        await OnMouseDownAsync(p.X, p.Y);

    }

    public async Task OnMouseDownAsync(double x, double y) {
        var actualPictureWidth = (int)Picture.ActualWidth;
        var actualPictureHeight = (int)Picture.ActualHeight;
        var pictureSourceWidth = (int)Picture.Source.Width;
        var pictureSourceHeight = (int)Picture.Source.Height;
        x = pictureSourceWidth * x / actualPictureWidth;
        y = pictureSourceHeight * y / actualPictureHeight;
        await _CudotosiApp.Handlers.PictureHandler.MouseDownAsync((int)x, (int)y, pictureSourceWidth, pictureSourceHeight, actualPictureWidth, actualPictureHeight);
    }

    public async ValueTask DisposeAsync() {
        if (_TashTimer == null) { return; }

        await _TashTimer.StopTimerAndConfirmDeadAsync(false);
    }

    private async void OnClosing(object sender, CancelEventArgs e) {
        e.Cancel = true;

        if (_TashTimer == null) { return; }

        await _TashTimer.StopTimerAndConfirmDeadAsync(false);
        WindowsApplication.Current.Shutdown();
    }

    private void OnStateChanged(object sender, EventArgs e) {
        _CudotosiApp.OnWindowStateChanged(WindowState);
    }

    private async void OnSizeChanged(object sender, SizeChangedEventArgs e) {
        await AdjustCanvasAndImageAsync();
    }

    public async Task AdjustCanvasAndImageAsync() {
        await BuildContainerIfNecessaryAsync();

        var adjuster = Container.Resolve<ICanvasAndImageSizeAdjuster>();
        adjuster.AdjustCanvasAndImage(CanvasContainer, Canvas, Picture);
        if (_CudotosiApp?.Handlers?.PictureHandler == null) { return; }

        UpdateLayout();
        await _CudotosiApp.Handlers.PictureHandler.PictureSizeChangedAsync((int)Picture.ActualWidth, (int)Picture.ActualHeight);
    }

    public void AdjustCanvasAndImageSync() {
        var adjuster = Container.Resolve<ICanvasAndImageSizeAdjuster>();
        adjuster.AdjustCanvasAndImage(CanvasContainer, Canvas, Picture);
    }

    public MessageBoxResult ShowMessageBox(string text, MessageBoxButton button, MessageBoxImage icon) {
        return MessageBox.Show(text, Properties.Resources.CudotosiWindowTitle, button, icon);
    }
}