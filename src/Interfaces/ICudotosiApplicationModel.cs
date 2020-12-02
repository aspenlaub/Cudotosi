using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface ICudotosiApplicationModel : IApplicationModel {
        ITextBox Folder { get; }
        ISelector JpgFile { get; }

        Button SelectFolder { get; }
        Button Save { get; }

        IImage Picture { get; }
        int MousePosX { get; set; }
        int MousePosY { get; set; }
        int PictureWidth { get; set; }
        int PictureHeight { get; set; }
    }
}
