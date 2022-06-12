using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class PictureHandler : IPictureHandler {
    private readonly ICudotosiApplicationModel _Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> _GuiAndAppHandler;
    private readonly IJpgFileNameChanger _JpgFileNameChanger;
    private readonly ISourceAreaHandler _SourceAreaHandler;
    private readonly ISimpleLogger _SimpleLogger;
    private readonly IMethodNamesFromStackFramesExtractor _MethodNamesFromStackFramesExtractor;

    public PictureHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, IJpgFileNameChanger jpgFileNameChanger,
            ISourceAreaHandler sourceAreaHandler, ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
        _JpgFileNameChanger = jpgFileNameChanger;
        _SourceAreaHandler = sourceAreaHandler;
        _SimpleLogger = simpleLogger;
        _MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    public async Task MouseDownAsync(int mousePosX, int mousePosY, int pictureWidth, int pictureHeight, int actualPictureWidth, int actualPictureHeight) {
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor)))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack($"Mouse is down at {mousePosX}, {mousePosY}", methodNamesFromStack);
            if (_Model.MousePosX == mousePosX && _Model.MousePosY == mousePosY && _Model.PictureWidth == pictureWidth && _Model.PictureHeight == pictureHeight) {
                _SimpleLogger.LogInformationWithCallStack("Same as previous position", methodNamesFromStack);
                return;
            }

            _Model.MousePosX = mousePosX;
            _Model.MousePosY = mousePosY;
            _Model.PictureWidth = pictureWidth;
            _Model.PictureHeight = pictureHeight;
            _Model.ActualPictureWidth = actualPictureWidth;
            _Model.ActualPictureHeight = actualPictureHeight;
            await _SourceAreaHandler.OnMousePositionChangedAsync();
        }
    }

    public async Task PictureSizeChangedAsync(int actualPictureWidth, int actualPictureHeight) {
        if (_Model.ActualPictureWidth == actualPictureWidth && _Model.ActualPictureHeight == actualPictureHeight) {
            return;
        }

        _Model.ActualPictureWidth = actualPictureWidth;
        _Model.ActualPictureHeight = actualPictureHeight;
        await _SourceAreaHandler.OnMousePositionChangedAsync();
    }

    public string FileName() {
        return _Model.JpgFile.SelectedIndex >= 0 ? new Folder(_Model.Folder.Text).FullName + @"\" + _Model.JpgFile.SelectedItem.Name : "";
    }

    public async Task LoadFromFile(string fileName) {
        BitmapImage image;

        if (fileName == "") {
            image = new BitmapImage();
        } else {
            if (_Model.SourceSizeXl.IsChecked) {
                fileName = _JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Xl, false);
            } else if (_Model.SourceSizeLg.IsChecked) {
                fileName = _JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Lg, false);
            } else if (_Model.SourceSizeMd.IsChecked) {
                fileName = _JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Md, false);
            } else if (_Model.SourceSizeSm.IsChecked) {
                fileName = _JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Sm, false);
            }
            image = new BitmapImage();
        }

        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.UriSource = fileName != "" ? new Uri(fileName) : new Uri(@"Images\blank.jpg", UriKind.Relative);
        image.EndInit();
        _Model.Picture.BitmapImage = image;

        _Model.MousePosX = 0;
        _Model.MousePosY = 0;
        _Model.SourceAreaHeight = 0;
        _Model.SourceAreaWidth = 0;
        _Model.Status.Text = "";
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}