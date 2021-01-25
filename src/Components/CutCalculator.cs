using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components {
    public class CutCalculator : ICutCalculator {
        public void CutOut(ICudotosiApplicationModel model) {
            model.CutOut.Left = model.PictureWidth > 0 ? 1.0 * model.MousePosX * model.ActualPictureWidth / model.PictureWidth : 0;
            model.CutOut.Top = model.PictureHeight > 0 ? 1.0 * model.MousePosY * model.ActualPictureHeight / model.PictureHeight : 0;
            model.CutOut.Width = 1.0 * model.SourceAreaWidth * model.ActualPictureWidth / model.PictureWidth;
            model.CutOut.Height = 1.0 * model.SourceAreaHeight * model.ActualPictureHeight / model.PictureHeight;
        }
    }
}
