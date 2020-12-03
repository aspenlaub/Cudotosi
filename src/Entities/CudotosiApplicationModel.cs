using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities {
    public class CudotosiApplicationModel : ApplicationModelBase, ICudotosiApplicationModel {
        public ITextBox Folder { get; } = new TextBox();
        public ISelector JpgFile { get; } = new ComboBox();

        public ToggleButton SourceSizeXl { get; } = new ToggleButton("SourceSize") { IsChecked = true };
        public ToggleButton SourceSizeLg { get; } = new ToggleButton("SourceSize");
        public ToggleButton SourceSizeMd { get; } = new ToggleButton("SourceSize");
        public ToggleButton SourceSizeSm { get; } = new ToggleButton("SourceSize");

        public ToggleButton TargetSizeLg { get; } = new ToggleButton("SourceSize") { IsChecked = true };
        public ToggleButton TargetSizeMd { get; } = new ToggleButton("SourceSize");
        public ToggleButton TargetSizeSm { get; } = new ToggleButton("SourceSize");
        public ToggleButton TargetSizeXs { get; } = new ToggleButton("SourceSize");

        public Button SelectFolder { get; } = new Button();
        public Button Save { get; } = new Button();

        public IImage Picture { get; } = new Image { BitmapImage = new BitmapImage() };
        public int MousePosX { get; set; }
        public int MousePosY { get; set; }
        public int PictureWidth { get; set; }
        public int PictureHeight { get; set; }
    }
}
