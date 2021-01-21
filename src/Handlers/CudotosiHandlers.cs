﻿using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class CudotosiHandlers : ICudotosiHandlers {
        public ISimpleTextHandler FolderTextHandler { get; set;  }
        public ISimpleSelectorHandler JpgFileSelectorHandler { get; set; }

        public ISimpleToggleButtonHandler SourceSizeXlHandler { get; set; }
        public ISimpleToggleButtonHandler SourceSizeLgHandler { get; set; }
        public ISimpleToggleButtonHandler SourceSizeMdHandler { get; set; }
        public ISimpleToggleButtonHandler SourceSizeSmHandler { get; set; }

        public ISimpleToggleButtonHandler TargetSizeLgHandler { get; set; }
        public ISimpleToggleButtonHandler TargetSizeMdHandler { get; set; }
        public ISimpleToggleButtonHandler TargetSizeSmHandler { get; set; }
        public ISimpleToggleButtonHandler TargetSizeXsHandler { get; set; }

        public ISimpleToggleButtonHandler DestinationShapeAsIsHandler { get; set; }
        public ISimpleToggleButtonHandler DestinationShapeSquareHandler { get; set; }

        public ISimpleToggleButtonHandler TransformHowManyPercent100Handler { get; set; }
        public ISimpleToggleButtonHandler TransformHowManyPercent50Handler { get; set; }

        public IPictureHandler PictureHandler { get; set; }
    }
}
