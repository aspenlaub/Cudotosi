﻿using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface ICudotosiHandlers {
        ISimpleTextHandler FolderTextHandler { get; }
        ISimpleSelectorHandler JpgFileSelectorHandler { get; }

        ISimpleToggleButtonHandler SourceSizeXlHandler { get; }
        ISimpleToggleButtonHandler SourceSizeLgHandler { get; }
        ISimpleToggleButtonHandler SourceSizeMdHandler { get; }
        ISimpleToggleButtonHandler SourceSizeSmHandler { get; }

        ISimpleToggleButtonHandler TargetSizeLgHandler { get; }
        ISimpleToggleButtonHandler TargetSizeMdHandler { get; }
        ISimpleToggleButtonHandler TargetSizeSmHandler { get; }
        ISimpleToggleButtonHandler TargetSizeXsHandler { get; }

        ISimpleToggleButtonHandler DestinationShapeAsIsHandler { get; set; }
        ISimpleToggleButtonHandler DestinationShapeSquareHandler { get; set; }

        ISimpleToggleButtonHandler TransformHowManyPercent100Handler { get; set; }
        ISimpleToggleButtonHandler TransformHowManyPercent50Handler { get; set; }

        IImageHandler PictureHandler { get; }
    }
}
