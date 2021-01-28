using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components {
    public class CutCalculator : ICutCalculator {
        public void CutOut(ICudotosiApplicationModel model) {
            model.CutOut.Left = model.PictureWidth > 0 ? 1.0 * model.MousePosX * model.ActualPictureWidth / model.PictureWidth : 0;
            model.CutOut.Top = model.PictureHeight > 0 ? 1.0 * model.MousePosY * model.ActualPictureHeight / model.PictureHeight : 0;
            model.CutOut.Width = 1.0 * model.SourceAreaWidth * model.ActualPictureWidth / model.PictureWidth;
            model.CutOut.Height = 1.0 * model.SourceAreaHeight * model.ActualPictureHeight / model.PictureHeight;
        }

        public void CutOut(ICudotosiApplicationModel model, int imageWidth, int imageHeight, out int x, out int y, out int width, out int height) {
            x = model.PictureWidth > 0 ? model.MousePosX * imageWidth / model.PictureWidth : 0;
            y = model.PictureHeight > 0 ? model.MousePosY * imageHeight / model.PictureHeight : 0;
            width = model.SourceAreaWidth * imageWidth / model.PictureWidth;
            height = model.SourceAreaHeight * imageHeight / model.PictureHeight;
        }

        public void TargetSize(ICudotosiApplicationModel model, int imageWidth, int imageHeight, out int targetWidth, out int targetHeight) {
            if (model.TargetSizeLg.IsChecked) {
                targetWidth = 1140;
            } else if (model.TargetSizeMd.IsChecked) {
                targetWidth = 960;
            } else if (model.TargetSizeSm.IsChecked) {
                targetWidth = 768;
            } else if (model.TargetSizeXs.IsChecked) {
                targetWidth = 576;
            } else {
                targetWidth = 0;
            }

            targetHeight = model.DestinationShapeSquare.IsChecked ? targetWidth : targetWidth * imageHeight / imageWidth;
        }
    }
}
