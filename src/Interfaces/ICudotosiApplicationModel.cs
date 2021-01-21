using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface ICudotosiApplicationModel : IApplicationModel {
        ITextBox Folder { get; }
        ISelector JpgFile { get; }

        ToggleButton SourceSizeXl { get; }
        ToggleButton SourceSizeLg { get; }
        ToggleButton SourceSizeMd { get; }
        ToggleButton SourceSizeSm { get; }

        ToggleButton TargetSizeLg { get; }
        ToggleButton TargetSizeMd { get; }
        ToggleButton TargetSizeSm { get; }
        ToggleButton TargetSizeXs { get; }

        ToggleButton DestinationShapeAsIs { get; }
        ToggleButton DestinationShapeSquare { get; }

        ToggleButton TransformHowManyPercent100 { get; }
        ToggleButton TransformHowManyPercent50 { get; }

        Button SelectFolder { get; }
        Button Save { get; }

        IImage Picture { get; }
        int MousePosX { get; set; }
        int MousePosY { get; set; }
        int SourceAreaWidth { get; set; }
        int SourceAreaHeight { get; set; }
        int PictureWidth { get; set; }
        int PictureHeight { get; set; }

        ITextBox Status { get; }
    }
}
