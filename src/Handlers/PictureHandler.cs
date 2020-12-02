using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class PictureHandler : IImageHandler {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;

        public PictureHandler(ICudotosiApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
        }

        public async Task MouseDownAsync(int mousePosX, int mousePosY, int pictureWidth, int pictureHeight) {
            if (vModel.MousePosX == mousePosX && vModel.MousePosY == mousePosY && vModel.PictureWidth == pictureWidth && vModel.PictureHeight == pictureHeight) {
                return;
            }

            vModel.MousePosX = mousePosX;
            vModel.MousePosY = mousePosY;
            vModel.PictureWidth = pictureWidth;
            vModel.PictureHeight = pictureHeight;
            await Task.Run(() => { }); // TODO: replace or remove
        }

        public async Task LoadFromFile(string fileName) {
            vModel.Picture.BitmapImage = fileName == "" ? new BitmapImage() : new BitmapImage(new Uri(fileName));
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
