using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities {
    public class CudotosiApplicationModel : ApplicationModelBase, ICudotosiApplicationModel {
        public ITextBox Folder { get; } = new TextBox();
        public ISelector JpgFile { get; } = new ComboBox();

        public ToggleButton SourceSizeXl { get; } = new ToggleButton("SourceSize") { IsChecked = true, Enabled = true };
        public ToggleButton SourceSizeLg { get; } = new ToggleButton("SourceSize") { Enabled = false };
        public ToggleButton SourceSizeMd { get; } = new ToggleButton("SourceSize") { Enabled = false };
        public ToggleButton SourceSizeSm { get; } = new ToggleButton("SourceSize") { Enabled = false };

        public ToggleButton TargetSizeLg { get; } = new ToggleButton("TargetSize") { IsChecked = true, Enabled = false };
        public ToggleButton TargetSizeMd { get; } = new ToggleButton("TargetSize") { Enabled = false };
        public ToggleButton TargetSizeSm { get; } = new ToggleButton("TargetSize") { Enabled = false };
        public ToggleButton TargetSizeXs { get; } = new ToggleButton("TargetSize") { Enabled = false };

        public ToggleButton DestinationShapeAsIs { get; } = new ToggleButton("DestinationShape") { IsChecked = true, Enabled = false };
        public ToggleButton DestinationShapeSquare { get; } = new ToggleButton("DestinationShape") { Enabled = false };

        public ToggleButton TransformHowManyPercent100 { get; } = new ToggleButton("TransformHowManyPercent") { IsChecked = true, Enabled = false };
        public ToggleButton TransformHowManyPercent50 { get; } = new ToggleButton("TransformHowManyPercent") { Enabled = false };

        public Button SelectFolder { get; } = new Button();
        public Button Save { get; } = new Button();

        public IImage Picture { get; } = new Image { BitmapImage = new BitmapImage() };

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
