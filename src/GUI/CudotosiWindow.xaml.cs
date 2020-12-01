using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Ookii.Dialogs.Wpf;
using IContainer = Autofac.IContainer;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.GUI {
    /// <summary>
    /// Interaction logic for CudotosiWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class CudotosiWindow : IFolderDialog {
        private static IContainer Container { get; set; }

        private CudotosiApplication vCudotosiApp;

        public CudotosiWindow() {
            InitializeComponent();

            var builder = new ContainerBuilder().UseCudotosiVishizhukelNetAndPegh(this);
            Container = builder.Build();

            Title = Properties.Resources.CudotosiWindowTitle;
            Name = Properties.Resources.CudotosiWindowName;
            AutomationProperties.SetAutomationId(this, Name);
            AutomationProperties.SetName(this, Name);
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private void RegisterTypes() {
            vCudotosiApp = Container.Resolve<CudotosiApplication>();
            vCudotosiApp.RegisterTypes();
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
            RegisterTypes();

            await vCudotosiApp.OnLoadedAsync();

            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();

            var commands = vCudotosiApp.Commands;
            guiToAppGate.WireButtonAndCommand(SelectFolder, commands.SelectFolderCommand, buttonNameToCommandMapper);
            guiToAppGate.WireButtonAndCommand(Save, commands.SaveCommand, buttonNameToCommandMapper);

            guiToAppGate.RegisterAsyncTextBoxCallback(Folder, t => vCudotosiApp.FolderTextChangedAsync(t));
        }

        public string PromptForFolder(string folder) {
            var folderBrowserDialog = new VistaFolderBrowserDialog {
                SelectedPath = folder,
                ShowNewFolderButton = true
            };
            return folderBrowserDialog.ShowDialog() != true ? folder : folderBrowserDialog.SelectedPath;
        }
    }
}
