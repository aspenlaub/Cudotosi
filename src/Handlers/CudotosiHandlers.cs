using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class CudotosiHandlers : ICudotosiHandlers {
        public ISimpleTextHandler FolderTextHandler { get; set;  }
        public ISimpleSelectorHandler JpgFileSelectorHandler { get; set; }
        public IImageHandler PictureHandler { get; set; }
    }
}
