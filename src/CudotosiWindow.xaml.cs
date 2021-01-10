using System;
using System.ComponentModel;
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

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi {
    /// <summary>
    /// Interaction logic for CudotosiWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class CudotosiWindow : IFolderDialog, IDisposable {
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

            var commands = vCudotosiApp.Commands;
            guiToAppGate.WireButtonAndCommand(SelectFolder, commands.SelectFolderCommand, buttonNameToCommandMapper);
            guiToAppGate.WireButtonAndCommand(Save, commands.SaveCommand, buttonNameToCommandMapper);

            guiToAppGate.RegisterAsyncTextBoxCallback(Folder, t => vCudotosiApp.Handlers.FolderTextHandler.TextChangedAsync(t));
            guiToAppGate.RegisterAsyncSelectorCallback(JpgFile, t => vCudotosiApp.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(t));

            guiToAppGate.RegisterAsyncToggleButtonCallback(SourceSizeXl, b => vCudotosiApp.Handlers.SourceSizeXlHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(SourceSizeLg, b => vCudotosiApp.Handlers.SourceSizeLgHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(SourceSizeMd, b => vCudotosiApp.Handlers.SourceSizeMdHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(SourceSizeSm, b => vCudotosiApp.Handlers.SourceSizeSmHandler.ToggledAsync(b));

            guiToAppGate.RegisterAsyncToggleButtonCallback(TargetSizeLg, b => vCudotosiApp.Handlers.TargetSizeLgHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(TargetSizeMd, b => vCudotosiApp.Handlers.TargetSizeMdHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(TargetSizeSm, b => vCudotosiApp.Handlers.TargetSizeSmHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(TargetSizeXs, b => vCudotosiApp.Handlers.TargetSizeXsHandler.ToggledAsync(b));

            guiToAppGate.RegisterAsyncToggleButtonCallback(DestinationShapeAsIs, b => vCudotosiApp.Handlers.DestinationShapeAsIsHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(DestinationShapeSquare, b => vCudotosiApp.Handlers.DestinationShapeSquareHandler.ToggledAsync(b));

            guiToAppGate.RegisterAsyncToggleButtonCallback(TransformHowManyPercent100, b => vCudotosiApp.Handlers.TransformHowManyPercent100Handler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(TransformHowManyPercent50, b => vCudotosiApp.Handlers.TransformHowManyPercent50Handler.ToggledAsync(b));

            vTashTimer = new TashTimer<ICudotosiApplicationModel>(Container.Resolve<ITashAccessor>(), vCudotosiApp.TashHandler, guiToAppGate);
            if (!await vTashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.CudotosiWindowTitle)) {
                Close();
            }

            vTashTimer.CreateAndStartTimer(vCudotosiApp.CreateTashTaskHandlingStatus());
        }

        public string PromptForFolder(string folder) {
            var folderBrowserDialog = new VistaFolderBrowserDialog {
                SelectedPath = folder,
                ShowNewFolderButton = true
            };
            return folderBrowserDialog.ShowDialog() != true ? folder : folderBrowserDialog.SelectedPath;
        }

        private async void Picture_OnMouseDown(object sender, MouseButtonEventArgs e) {
            var width = (int) Picture.ActualWidth;
            var height = (int) Picture.ActualHeight;
            var p = e.GetPosition(Picture);
            var x = (int)(Picture.Source.Width * p.X / width);
            var y = (int)(Picture.Source.Height * p.Y / height);
            await vCudotosiApp.Handlers.PictureHandler.MouseDownAsync(x, y, width, height);
        }

        public void Dispose() {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }

        private void OnClosing(object sender, CancelEventArgs e) {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }
    }
}
