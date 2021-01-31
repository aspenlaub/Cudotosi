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
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Ookii.Dialogs.Wpf;
using IContainer = Autofac.IContainer;
using MessageBox = System.Windows.MessageBox;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi {
    /// <summary>
    /// Interaction logic for CudotosiWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class CudotosiWindow : IMouseOwner, IUserInteraction, IDisposable {
        private static IContainer Container { get; set; }

        private CudotosiApplication vCudotosiApp;
        private ITashTimer<ICudotosiApplicationModel> vTashTimer;

        public CudotosiWindow() {
            InitializeComponent();

            var builder = new ContainerBuilder().UseCudotosiVishizhukelNetAndPegh(this);
            Container = builder.Build();

            Title = Properties.Resources.CudotosiWindowTitle;
            Name = Properties.Resources.CudotosiWindowName;
            AutomationProperties.SetAutomationId(this, Name);
            AutomationProperties.SetName(this, Name);
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
            vCudotosiApp = Container.Resolve<CudotosiApplication>();
            await vCudotosiApp.OnLoadedAsync();

            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();
            var toggleButtonNameToHandlerMapper = Container.Resolve<IToggleButtonNameToHandlerMapper>();

            var commands = vCudotosiApp.Commands;
            guiToAppGate.WireButtonAndCommand(SelectFolder, commands.SelectFolderCommand, buttonNameToCommandMapper);
            guiToAppGate.WireButtonAndCommand(Save, commands.SaveCommand, buttonNameToCommandMapper);
            guiToAppGate.WireButtonAndCommand(Default, commands.DefaultCommand, buttonNameToCommandMapper);

            guiToAppGate.RegisterAsyncTextBoxCallback(Folder, t => vCudotosiApp.Handlers.FolderTextHandler.TextChangedAsync(t));
            guiToAppGate.RegisterAsyncSelectorCallback(JpgFile, t => vCudotosiApp.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(t));

            guiToAppGate.WireToggleButtonAndHandler(SourceSizeXl, vCudotosiApp.Handlers.SourceSizeXlHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(SourceSizeLg, vCudotosiApp.Handlers.SourceSizeLgHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(SourceSizeMd, vCudotosiApp.Handlers.SourceSizeMdHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(SourceSizeSm, vCudotosiApp.Handlers.SourceSizeSmHandler, toggleButtonNameToHandlerMapper);

            guiToAppGate.WireToggleButtonAndHandler(TargetSizeLg, vCudotosiApp.Handlers.TargetSizeLgHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(TargetSizeMd, vCudotosiApp.Handlers.TargetSizeMdHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(TargetSizeSm, vCudotosiApp.Handlers.TargetSizeSmHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(TargetSizeXs, vCudotosiApp.Handlers.TargetSizeXsHandler, toggleButtonNameToHandlerMapper);

            guiToAppGate.WireToggleButtonAndHandler(DestinationShapeAsIs, vCudotosiApp.Handlers.DestinationShapeAsIsHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(DestinationShapeSquare, vCudotosiApp.Handlers.DestinationShapeSquareHandler, toggleButtonNameToHandlerMapper);

            guiToAppGate.WireToggleButtonAndHandler(TransformHowManyPercent100, vCudotosiApp.Handlers.TransformHowManyPercent100Handler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(TransformHowManyPercent50, vCudotosiApp.Handlers.TransformHowManyPercent50Handler, toggleButtonNameToHandlerMapper);

            vTashTimer = new TashTimer<ICudotosiApplicationModel>(Container.Resolve<ITashAccessor>(), vCudotosiApp.TashHandler, guiToAppGate);
            if (!await vTashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.CudotosiWindowTitle)) {
                Close();
            }

            vTashTimer.CreateAndStartTimer(vCudotosiApp.CreateTashTaskHandlingStatus());

            AdjustCanvasAndImageSync();
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
            await vCudotosiApp.Handlers.PictureHandler.MouseDownAsync((int)x, (int)y, pictureSourceWidth, pictureSourceHeight, actualPictureWidth, actualPictureHeight);
        }

        public void Dispose() {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }

        private void OnClosing(object sender, CancelEventArgs e) {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }

        private void OnStateChanged(object sender, EventArgs e) {
            vCudotosiApp.OnWindowStateChanged(WindowState);
        }

        private async void OnSizeChanged(object sender, SizeChangedEventArgs e) {
            await AdjustCanvasAndImageAsync();
        }

        public async Task AdjustCanvasAndImageAsync() {
            var adjuster = Container.Resolve<ICanvasAndImageSizeAdjuster>();
            adjuster.AdjustCanvasAndImage(CanvasContainer, Canvas, Picture);
            if (vCudotosiApp?.Handlers?.PictureHandler == null) { return; }

            UpdateLayout();
            await vCudotosiApp.Handlers.PictureHandler.PictureSizeChangedAsync((int)Picture.ActualWidth, (int)Picture.ActualHeight);
        }

        public void AdjustCanvasAndImageSync() {
            var adjuster = Container.Resolve<ICanvasAndImageSizeAdjuster>();
            adjuster.AdjustCanvasAndImage(CanvasContainer, Canvas, Picture);
        }

        public MessageBoxResult ShowMessageBox(string text, MessageBoxButton button, MessageBoxImage icon) {
            return MessageBox.Show(text, Properties.Resources.CudotosiWindowTitle, button, icon);
        }
    }
}
