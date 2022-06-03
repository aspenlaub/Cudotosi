using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class SaveCommand : ICommand {
    private readonly ICudotosiApplicationModel _Model;
    private readonly ICutCalculator _CutCalculator;
    private readonly ISimpleSelectorHandler _JpgFileSelectorHandler;
    private readonly IJpgFileNameChanger _JpgFileNameChanger;
    private readonly IUserInteraction _UserInteraction;
    private string _TargetFileName;
    private readonly ISimpleLogger _SimpleLogger;
    private readonly IMethodNamesFromStackFramesExtractor _MethodNamesFromStackFramesExtractor;

    public SaveCommand(ICudotosiApplicationModel model, ICutCalculator cutCalculator, ISimpleSelectorHandler jpgFileSelectorHandler,
            IJpgFileNameChanger jpgFileNameChanger, IUserInteraction userInteraction, ISimpleLogger simpleLogger,
            IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        _Model = model;
        _CutCalculator = cutCalculator;
        _JpgFileSelectorHandler = jpgFileSelectorHandler;
        _JpgFileNameChanger = jpgFileNameChanger;
        _UserInteraction = userInteraction;
        _TargetFileName = "";
        _SimpleLogger = simpleLogger;
        _MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    public async Task ExecuteAsync() {
        var executionResult  = Execute();
        var executed = executionResult.YesNo && !executionResult.Inconclusive;
        _Model.Status.Type = executed ? StatusType.Success : executionResult.Inconclusive ? StatusType.None : StatusType.Error;
        _Model.Status.Text = executed
            ? string.Format(Properties.Resources.TargetFileSaved, _TargetFileName)
            : executionResult.Inconclusive
                ? Properties.Resources.NoTargetFileWasSaved
                : Properties.Resources.TargetFileCouldNotBeSaved;
        if (!executionResult.Inconclusive) {
            await _JpgFileSelectorHandler.UpdateSelectableValuesAsync();
            var icon = _Model.Status.Type == StatusType.Error ? MessageBoxImage.Error : MessageBoxImage.Information;
            _UserInteraction.ShowMessageBox(_Model.Status.Text, MessageBoxButton.OK, icon);
        }
    }

    private YesNoInconclusive Execute() {
        var result = new YesNoInconclusive { Inconclusive = false, YesNo = false };
        if (!_Model.Save.Enabled) {
            return result;
        }

        using var image = Image.FromFile(_Model.Folder.Text + @"\" + _Model.JpgFile.SelectedItem.Name);

        _CutCalculator.CutOut(_Model, image.Width, image.Height, out var cutOutLeft, out var cutOutTop, out var cutOutWidth, out var cutOutHeight);
        _CutCalculator.TargetSize(_Model, image.Width, image.Height, out var targetWidth, out var targetHeight);

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

        _TargetFileName = _Model.JpgFile.SelectedItem.Name;
        if (_Model.TargetSizeLg.IsChecked) {
            _TargetFileName = _JpgFileNameChanger.ChangeFileName(_TargetFileName, BootstrapSizes.Lg, _Model.DestinationShapePreview.IsChecked);
        } else if (_Model.TargetSizeMd.IsChecked) {
            _TargetFileName = _JpgFileNameChanger.ChangeFileName(_TargetFileName, BootstrapSizes.Md, _Model.DestinationShapePreview.IsChecked);
        } else if (_Model.TargetSizeSm.IsChecked) {
            _TargetFileName = _JpgFileNameChanger.ChangeFileName(_TargetFileName, BootstrapSizes.Sm, _Model.DestinationShapePreview.IsChecked);
        } else if (_Model.TargetSizeXs.IsChecked) {
            _TargetFileName = _JpgFileNameChanger.ChangeFileName(_TargetFileName, BootstrapSizes.Xs, _Model.DestinationShapePreview.IsChecked);
        } else {
            return result;
        }

        if (_TargetFileName == _Model.JpgFile.SelectedItem.Name) {
            return result;
        }

        var targetFileFullName = _Model.Folder.Text + @"\" + _TargetFileName;
        if (File.Exists(targetFileFullName)) {
            var mbResult = _UserInteraction.ShowMessageBox(string.Format(Properties.Resources.TargetFileExistsOverwrite, _TargetFileName),
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
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor)))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack("Checking if save command should be enabled", methodNamesFromStack);
            return await Task.FromResult(_Model.JpgFile.SelectedIndex >= 0 && _Model.PictureHeight > 1 && _Model.PictureWidth > 1);
        }
    }
}