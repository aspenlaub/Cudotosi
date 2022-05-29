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
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class SaveCommand : ICommand {
    private readonly ICudotosiApplicationModel Model;
    private readonly ICutCalculator CutCalculator;
    private readonly ISimpleSelectorHandler JpgFileSelectorHandler;
    private readonly IJpgFileNameChanger JpgFileNameChanger;
    private readonly IUserInteraction UserInteraction;
    private string TargetFileName;
    private readonly ISimpleLogger SimpleLogger;
    private readonly ILogConfiguration LogConfiguration;

    public SaveCommand(ICudotosiApplicationModel model, ICutCalculator cutCalculator, ISimpleSelectorHandler jpgFileSelectorHandler,
        IJpgFileNameChanger jpgFileNameChanger, IUserInteraction userInteraction, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration) {
        Model = model;
        CutCalculator = cutCalculator;
        JpgFileSelectorHandler = jpgFileSelectorHandler;
        JpgFileNameChanger = jpgFileNameChanger;
        UserInteraction = userInteraction;
        TargetFileName = "";
        SimpleLogger = simpleLogger;
        LogConfiguration = logConfiguration;
    }

    public async Task ExecuteAsync() {
        var executionResult  = Execute();
        var executed = executionResult.YesNo && !executionResult.Inconclusive;
        Model.Status.Type = executed ? StatusType.Success : executionResult.Inconclusive ? StatusType.None : StatusType.Error;
        Model.Status.Text = executed
            ? string.Format(Properties.Resources.TargetFileSaved, TargetFileName)
            : executionResult.Inconclusive
                ? Properties.Resources.NoTargetFileWasSaved
                : Properties.Resources.TargetFileCouldNotBeSaved;
        if (!executionResult.Inconclusive) {
            await JpgFileSelectorHandler.UpdateSelectableValuesAsync();
            var icon = Model.Status.Type == StatusType.Error ? MessageBoxImage.Error : MessageBoxImage.Information;
            UserInteraction.ShowMessageBox(Model.Status.Text, MessageBoxButton.OK, icon);
        }
    }

    private YesNoInconclusive Execute() {
        var result = new YesNoInconclusive { Inconclusive = false, YesNo = false };
        if (!Model.Save.Enabled) {
            return result;
        }

        using var image = Image.FromFile(Model.Folder.Text + @"\" + Model.JpgFile.SelectedItem.Name);

        CutCalculator.CutOut(Model, image.Width, image.Height, out var cutOutLeft, out var cutOutTop, out var cutOutWidth, out var cutOutHeight);
        CutCalculator.TargetSize(Model, image.Width, image.Height, out var targetWidth, out var targetHeight);

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

        TargetFileName = Model.JpgFile.SelectedItem.Name;
        if (Model.TargetSizeLg.IsChecked) {
            TargetFileName = JpgFileNameChanger.ChangeFileName(TargetFileName, BootstrapSizes.Lg, Model.DestinationShapePreview.IsChecked);
        } else if (Model.TargetSizeMd.IsChecked) {
            TargetFileName = JpgFileNameChanger.ChangeFileName(TargetFileName, BootstrapSizes.Md, Model.DestinationShapePreview.IsChecked);
        } else if (Model.TargetSizeSm.IsChecked) {
            TargetFileName = JpgFileNameChanger.ChangeFileName(TargetFileName, BootstrapSizes.Sm, Model.DestinationShapePreview.IsChecked);
        } else if (Model.TargetSizeXs.IsChecked) {
            TargetFileName = JpgFileNameChanger.ChangeFileName(TargetFileName, BootstrapSizes.Xs, Model.DestinationShapePreview.IsChecked);
        } else {
            return result;
        }

        if (TargetFileName == Model.JpgFile.SelectedItem.Name) {
            return result;
        }

        var targetFileFullName = Model.Folder.Text + @"\" + TargetFileName;
        if (File.Exists(targetFileFullName)) {
            var mbResult = UserInteraction.ShowMessageBox(string.Format(Properties.Resources.TargetFileExistsOverwrite, TargetFileName),
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), LogConfiguration.LogId))) {
            SimpleLogger.LogInformation("Checking if save command should be enabled");
            return await Task.FromResult(Model.JpgFile.SelectedIndex >= 0 && Model.PictureHeight > 1 && Model.PictureWidth > 1);
        }
    }
}