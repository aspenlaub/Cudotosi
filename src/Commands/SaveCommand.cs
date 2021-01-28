using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands {
    public class SaveCommand : ICommand {
        private readonly ICudotosiApplicationModel vModel;
        private readonly ICutCalculator vCutCalculator;
        private readonly ISimpleSelectorHandler vJpgFileSelectorHandler;
        private readonly IJpgFileNameChanger vJpgFileNameChanger;

        public SaveCommand(ICudotosiApplicationModel model, ICutCalculator cutCalculator, ISimpleSelectorHandler jpgFileSelectorHandler, IJpgFileNameChanger jpgFileNameChanger) {
            vModel = model;
            vCutCalculator = cutCalculator;
            vJpgFileSelectorHandler = jpgFileSelectorHandler;
            vJpgFileNameChanger = jpgFileNameChanger;
        }

        public async Task ExecuteAsync() {
            var executed = Execute();
            vModel.Status.Type = executed ? StatusType.Success : StatusType.Error;
            vModel.Status.Text = executed ? Properties.Resources.TargetFileSaved : Properties.Resources.TargetFileNotSaved;
            if (executed) {
                await vJpgFileSelectorHandler.UpdateSelectableValuesAsync();
            }
        }

        private bool Execute() {
            if (!vModel.Save.Enabled) {
                return false;
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

            var targetFileName = vModel.JpgFile.SelectedItem.Name;
            if (vModel.TargetSizeLg.IsChecked) {
                targetFileName = vJpgFileNameChanger.ChangeFileName(targetFileName, BootstrapSizes.Lg);
            } else if (vModel.TargetSizeMd.IsChecked) {
                targetFileName = vJpgFileNameChanger.ChangeFileName(targetFileName, BootstrapSizes.Md);
            } else if (vModel.TargetSizeSm.IsChecked) {
                targetFileName = vJpgFileNameChanger.ChangeFileName(targetFileName, BootstrapSizes.Sm);
            } else if (vModel.TargetSizeXs.IsChecked) {
                targetFileName = vJpgFileNameChanger.ChangeFileName(targetFileName, BootstrapSizes.Xs);
            } else {
                return false;
            }

            if (targetFileName == vModel.JpgFile.SelectedItem.Name) {
                return false;
            }

            targetImage.Save(vModel.Folder.Text + @"\" + targetFileName, ImageFormat.Jpeg);
            return true;
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            return await Task.FromResult(vModel.PictureHeight > 0 && vModel.PictureWidth > 0);
        }
    }
}
