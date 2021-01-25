using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class PictureHandler : IPictureHandler {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly IJpgFileNameChanger vJpgFileNameChanger;
        private readonly ISourceAreaHandler vSourceAreaHandler;

        public PictureHandler(ICudotosiApplicationModel model, IGuiAndAppHandler guiAndAppHandler, IJpgFileNameChanger jpgFileNameChanger, ISourceAreaHandler sourceAreaHandler) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vJpgFileNameChanger = jpgFileNameChanger;
            vSourceAreaHandler = sourceAreaHandler;
        }

        public async Task MouseDownAsync(int mousePosX, int mousePosY, int pictureWidth, int pictureHeight, int actualPictureWidth, int actualPictureHeight) {
            if (vModel.MousePosX == mousePosX && vModel.MousePosY == mousePosY && vModel.PictureWidth == pictureWidth && vModel.PictureHeight == pictureHeight) {
                return;
            }

            vModel.MousePosX = mousePosX;
            vModel.MousePosY = mousePosY;
            vModel.PictureWidth = pictureWidth;
            vModel.PictureHeight = pictureHeight;
            vModel.ActualPictureWidth = actualPictureWidth;
            vModel.ActualPictureHeight = actualPictureHeight;
            await vSourceAreaHandler.OnMousePositionChangedAsync();
        }

        public async Task PictureSizeChangedAsync(int actualPictureWidth, int actualPictureHeight) {
            if (vModel.ActualPictureWidth == actualPictureWidth && vModel.ActualPictureHeight == actualPictureHeight) {
                return;
            }

            vModel.ActualPictureWidth = actualPictureWidth;
            vModel.ActualPictureHeight = actualPictureHeight;
            await vSourceAreaHandler.OnMousePositionChangedAsync();
        }

        public string FileName() {
            return vModel.JpgFile.SelectedIndex >= 0 ? new Folder(vModel.Folder.Text).FullName + @"\" + vModel.JpgFile.SelectedItem.Name : "";
        }

        public async Task LoadFromFile(string fileName) {
            if (fileName == "") {
                vModel.Picture.BitmapImage = new BitmapImage();
            } else {
                if (vModel.SourceSizeLg.IsChecked) {
                    fileName = vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Lg);
                } else if (vModel.SourceSizeMd.IsChecked) {
                    fileName = vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Md);
                } else if (vModel.SourceSizeSm.IsChecked) {
                    fileName = vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Sm);
                }
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(fileName);
                image.EndInit();
                vModel.Picture.BitmapImage = image;
            }

            vModel.MousePosX = 0;
            vModel.MousePosY = 0;
            vModel.SourceAreaHeight = 0;
            vModel.SourceAreaWidth = 0;
            vModel.Status.Text = "";
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
