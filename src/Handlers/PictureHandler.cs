using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class PictureHandler : IPictureHandler {
    private readonly ICudotosiApplicationModel Model;
    private readonly IGuiAndAppHandler<CudotosiApplicationModel> GuiAndAppHandler;
    private readonly IJpgFileNameChanger JpgFileNameChanger;
    private readonly ISourceAreaHandler SourceAreaHandler;
    private readonly ISimpleLogger SimpleLogger;

    public PictureHandler(ICudotosiApplicationModel model, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler, IJpgFileNameChanger jpgFileNameChanger,
        ISourceAreaHandler sourceAreaHandler, ISimpleLogger simpleLogger) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
        JpgFileNameChanger = jpgFileNameChanger;
        SourceAreaHandler = sourceAreaHandler;
        SimpleLogger = simpleLogger;
    }

    public async Task MouseDownAsync(int mousePosX, int mousePosY, int pictureWidth, int pictureHeight, int actualPictureWidth, int actualPictureHeight) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), SimpleLogger.LogId))) {
            SimpleLogger.LogInformation($"Mouse is down at {mousePosX}, {mousePosY}");
            if (Model.MousePosX == mousePosX && Model.MousePosY == mousePosY && Model.PictureWidth == pictureWidth && Model.PictureHeight == pictureHeight) {
                SimpleLogger.LogInformation("Same as previous position");
                return;
            }

            Model.MousePosX = mousePosX;
            Model.MousePosY = mousePosY;
            Model.PictureWidth = pictureWidth;
            Model.PictureHeight = pictureHeight;
            Model.ActualPictureWidth = actualPictureWidth;
            Model.ActualPictureHeight = actualPictureHeight;
            await SourceAreaHandler.OnMousePositionChangedAsync();
        }
    }

    public async Task PictureSizeChangedAsync(int actualPictureWidth, int actualPictureHeight) {
        if (Model.ActualPictureWidth == actualPictureWidth && Model.ActualPictureHeight == actualPictureHeight) {
            return;
        }

        Model.ActualPictureWidth = actualPictureWidth;
        Model.ActualPictureHeight = actualPictureHeight;
        await SourceAreaHandler.OnMousePositionChangedAsync();
    }

    public string FileName() {
        return Model.JpgFile.SelectedIndex >= 0 ? new Folder(Model.Folder.Text).FullName + @"\" + Model.JpgFile.SelectedItem.Name : "";
    }

    public async Task LoadFromFile(string fileName) {
        BitmapImage image;

        if (fileName == "") {
            image = new BitmapImage();
        } else {
            if (Model.SourceSizeLg.IsChecked) {
                fileName = JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Lg, false);
            } else if (Model.SourceSizeMd.IsChecked) {
                fileName = JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Md, false);
            } else if (Model.SourceSizeSm.IsChecked) {
                fileName = JpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Sm, false);
            }
            image = new BitmapImage();
        }

        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.UriSource = fileName != "" ? new Uri(fileName) : new Uri(@"Images\blank.jpg", UriKind.Relative);
        image.EndInit();
        Model.Picture.BitmapImage = image;

        Model.MousePosX = 0;
        Model.MousePosY = 0;
        Model.SourceAreaHeight = 0;
        Model.SourceAreaWidth = 0;
        Model.Status.Text = "";
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}