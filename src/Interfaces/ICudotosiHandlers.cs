using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

public interface ICudotosiHandlers {
    ISimpleTextHandler FolderTextHandler { get; }
    ISimpleSelectorHandler JpgFileSelectorHandler { get; }

    IToggleButtonHandler SourceSizeXxlHandler { get; }
    IToggleButtonHandler SourceSizeXlHandler { get; }
    IToggleButtonHandler SourceSizeLgHandler { get; }
    IToggleButtonHandler SourceSizeMdHandler { get; }
    IToggleButtonHandler SourceSizeSmHandler { get; }

    IToggleButtonHandler TargetSizeXlHandler { get; }
    IToggleButtonHandler TargetSizeLgHandler { get; }
    IToggleButtonHandler TargetSizeMdHandler { get; }
    IToggleButtonHandler TargetSizeSmHandler { get; }
    IToggleButtonHandler TargetSizeXsHandler { get; }

    IToggleButtonHandler DestinationShapeAsIsHandler { get; set; }
    IToggleButtonHandler DestinationShapeSquareHandler { get; set; }
    IToggleButtonHandler DestinationShapePreviewHandler { get; set; }

    IToggleButtonHandler TransformHowManyPercent100Handler { get; set; }
    IToggleButtonHandler TransformHowManyPercent50Handler { get; set; }

    IPictureHandler PictureHandler { get; }
}