using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands {
    public class SaveCommand : ICommand {
        private readonly ICudotosiApplicationModel vModel;
        private readonly ICutCalculator vCutCalculator;
        private readonly ISimpleSelectorHandler vJpgFileSelectorHandler;
        private readonly IJpgFileNameChanger vJpgFileNameChanger;
        private readonly IUserInteraction vUserInteraction;
        private string vTargetFileName;
        private readonly ISimpleLogger vSimpleLogger;
        private readonly ILogConfiguration vLogConfiguration;

        public SaveCommand(ICudotosiApplicationModel model, ICutCalculator cutCalculator, ISimpleSelectorHandler jpgFileSelectorHandler,
                IJpgFileNameChanger jpgFileNameChanger, IUserInteraction userInteraction, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration) {
            vModel = model;
            vCutCalculator = cutCalculator;
            vJpgFileSelectorHandler = jpgFileSelectorHandler;
            vJpgFileNameChanger = jpgFileNameChanger;
            vUserInteraction = userInteraction;
            vTargetFileName = "";
            vSimpleLogger = simpleLogger;
            vLogConfiguration = logConfiguration;
        }

        public async Task ExecuteAsync() {
            var executionResult  = Execute();
            var executed = executionResult.YesNo && !executionResult.Inconclusive;
            vModel.Status.Type = executed ? StatusType.Success : executionResult.Inconclusive ? StatusType.None : StatusType.Error;
            vModel.Status.Text = executed
                ? string.Format(Properties.Resources.TargetFileSaved, vTargetFileName)
                : executionResult.Inconclusive
                    ? Properties.Resources.NoTargetFileWasSaved
                    : Properties.Resources.TargetFileCouldNotBeSaved;
            if (!executionResult.Inconclusive) {
                await vJpgFileSelectorHandler.UpdateSelectableValuesAsync();
                var icon = vModel.Status.Type == StatusType.Error ? MessageBoxImage.Error : MessageBoxImage.Information;
                vUserInteraction.ShowMessageBox(vModel.Status.Text, MessageBoxButton.OK, icon);
            }
        }

        private IYesNoInconclusive Execute() {
            var result = new YesNoInconclusive { Inconclusive = false, YesNo = false };
            if (!vModel.Save.Enabled) {
                return result;
            }

            using var image = Image.FromFile(vModel.Folder.Text + @"\" + vModel.JpgFile.SelectedItem.Name);

            vCutCalculator.CutOut(vModel, image.Width, image.Height, out var cutOutLeft, out var cutOutTop, out var cutOutWidth, out var cutOutHeight);
            vCutCalculator.TargetSize(vModel, image.Width, image.Height, out var targetWidth, out var targetHeight);

            var targetRectangle = new Rectangle(0, 0, targetWidth, targetHeight);
            var targetImage = new Bitmap(targetWidth, targetHeight);
            targetImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(targetImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var mode = new ImageAttributes();
            mode.SetWrapMode(WrapMode.TileFlipXY);

            graphics.DrawImage(image, targetRectangle, cutOutLeft, cutOutTop, cutOutWidth, cutOutHeight, GraphicsUnit.Pixel, mode);

            vTargetFileName = vModel.JpgFile.SelectedItem.Name;
            if (vModel.TargetSizeLg.IsChecked) {
                vTargetFileName = vJpgFileNameChanger.ChangeFileName(vTargetFileName, BootstrapSizes.Lg);
            } else if (vModel.TargetSizeMd.IsChecked) {
                vTargetFileName = vJpgFileNameChanger.ChangeFileName(vTargetFileName, BootstrapSizes.Md);
            } else if (vModel.TargetSizeSm.IsChecked) {
                vTargetFileName = vJpgFileNameChanger.ChangeFileName(vTargetFileName, BootstrapSizes.Sm);
            } else if (vModel.TargetSizeXs.IsChecked) {
                vTargetFileName = vJpgFileNameChanger.ChangeFileName(vTargetFileName, BootstrapSizes.Xs);
            } else {
                return result;
            }

            if (vTargetFileName == vModel.JpgFile.SelectedItem.Name) {
                return result;
            }

            var targetFileFullName = vModel.Folder.Text + @"\" + vTargetFileName;
            if (File.Exists(targetFileFullName)) {
                var mbResult = vUserInteraction.ShowMessageBox(string.Format(Properties.Resources.TargetFileExistsOverwrite, vTargetFileName),
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (mbResult != MessageBoxResult.Yes) {
                    result.Inconclusive = true;
                    return result;
                }
            }
            targetImage.Save(targetFileFullName, ImageFormat.Jpeg);
            result.YesNo = true;
            return result;
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), vLogConfiguration.LogId))) {
                vSimpleLogger.LogInformation("Checking if save command should be enabled");
                return await Task.FromResult(vModel.PictureHeight > 0 && vModel.PictureWidth > 0);
            }
        }
    }
}
