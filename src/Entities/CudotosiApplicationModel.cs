using System.IO;
using System.Windows;
using System.Windows.Media;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities {
    public class CudotosiApplicationModel : ApplicationModelBase, ICudotosiApplicationModel {
        public ITextBox Folder { get; } = new TextBox();
        public ISelector JpgFile { get; } = new ComboBox();

        public ToggleButton SourceSizeXl { get; } = new("SourceSize") { IsChecked = true, Enabled = true };
        public ToggleButton SourceSizeLg { get; } = new("SourceSize") { Enabled = false };
        public ToggleButton SourceSizeMd { get; } = new("SourceSize") { Enabled = false };
        public ToggleButton SourceSizeSm { get; } = new("SourceSize") { Enabled = false };

        public ToggleButton TargetSizeLg { get; } = new("TargetSize") { IsChecked = true, Enabled = false };
        public ToggleButton TargetSizeMd { get; } = new("TargetSize") { Enabled = false };
        public ToggleButton TargetSizeSm { get; } = new("TargetSize") { Enabled = false };
        public ToggleButton TargetSizeXs { get; } = new("TargetSize") { Enabled = false };

        public ToggleButton DestinationShapeAsIs { get; } = new("DestinationShape") { IsChecked = true, Enabled = false };
        public ToggleButton DestinationShapeSquare { get; } = new("DestinationShape") { Enabled = false };
        public ToggleButton DestinationShapePreview { get; } = new("DestinationShape") { Enabled = false };

        public ToggleButton TransformHowManyPercent100 { get; } = new("TransformHowManyPercent") { IsChecked = true, Enabled = false };
        public ToggleButton TransformHowManyPercent50 { get; } = new("TransformHowManyPercent") { Enabled = false };

        public Button SelectFolder { get; } = new();
        public Button Save { get; } = new();
        public Button Default { get; } = new();

        public IImage Picture { get; } = new Image {
            BitmapImage = new MemoryStream(Properties.Resources.DefaultImage).ToBitmapImage()
        };

        public IRectangle CutOut { get; } = new Rectangle { Stroke = new LinearGradientBrush(Colors.DarkGray, Colors.White, new Point(0.5, 1), new Point(0.5, 0)) };

        public int MousePosX { get; set; }
        public int MousePosY { get; set; }
        public int SourceAreaWidth { get; set; }
        public int SourceAreaHeight { get; set; }
        public int PictureWidth { get; set; }
        public int PictureHeight { get; set; }
        public int ActualPictureWidth { get; set; }
        public int ActualPictureHeight { get; set; }
    }
}
