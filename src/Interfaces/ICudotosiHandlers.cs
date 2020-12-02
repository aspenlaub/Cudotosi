using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface ICudotosiHandlers {
        ISimpleTextHandler FolderTextHandler { get; }
        ISimpleSelectorHandler JpgFileSelectorHandler { get; }
        IImageHandler PictureHandler { get; }
    }
}
