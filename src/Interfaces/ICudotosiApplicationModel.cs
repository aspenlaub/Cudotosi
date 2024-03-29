﻿using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

public interface ICudotosiApplicationModel : IApplicationModelBase {
    ITextBox Folder { get; }
    ISelector JpgFile { get; }

    ToggleButton SourceSizeXxl { get; }
    ToggleButton SourceSizeXl { get; }
    ToggleButton SourceSizeLg { get; }
    ToggleButton SourceSizeMd { get; }
    ToggleButton SourceSizeSm { get; }

    ToggleButton TargetSizeXl { get; }
    ToggleButton TargetSizeLg { get; }
    ToggleButton TargetSizeMd { get; }
    ToggleButton TargetSizeSm { get; }
    ToggleButton TargetSizeXs { get; }

    ToggleButton DestinationShapeAsIs { get; }
    ToggleButton DestinationShapeSquare { get; }
    ToggleButton DestinationShapePreview { get; }

    ToggleButton TransformHowManyPercent100 { get; }
    ToggleButton TransformHowManyPercent50 { get; }

    Button SelectFolder { get; }
    Button Save { get; }
    Button Default { get; }

    IImage Picture { get; }

    IRectangle CutOut { get; }

    int MousePosX { get; set; }
    int MousePosY { get; set; }
    int SourceAreaWidth { get; set; }
    int SourceAreaHeight { get; set; }
    int PictureWidth { get; set; }
    int PictureHeight { get; set; }
    int ActualPictureWidth { get; set; }
    int ActualPictureHeight { get; set; }
}