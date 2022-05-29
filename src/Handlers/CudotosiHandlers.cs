using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class CudotosiHandlers : ICudotosiHandlers {
    public ISimpleTextHandler FolderTextHandler { get; set;  }
    public ISimpleSelectorHandler JpgFileSelectorHandler { get; set; }

    public IToggleButtonHandler SourceSizeXlHandler { get; set; }
    public IToggleButtonHandler SourceSizeLgHandler { get; set; }
    public IToggleButtonHandler SourceSizeMdHandler { get; set; }
    public IToggleButtonHandler SourceSizeSmHandler { get; set; }

    public IToggleButtonHandler TargetSizeLgHandler { get; set; }
    public IToggleButtonHandler TargetSizeMdHandler { get; set; }
    public IToggleButtonHandler TargetSizeSmHandler { get; set; }
    public IToggleButtonHandler TargetSizeXsHandler { get; set; }

    public IToggleButtonHandler DestinationShapeAsIsHandler { get; set; }
    public IToggleButtonHandler DestinationShapeSquareHandler { get; set; }
    public IToggleButtonHandler DestinationShapePreviewHandler { get; set; }

    public IToggleButtonHandler TransformHowManyPercent100Handler { get; set; }
    public IToggleButtonHandler TransformHowManyPercent50Handler { get; set; }

    public IPictureHandler PictureHandler { get; set; }
}