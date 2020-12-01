using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface ICudotosiApplicationModel : IApplicationModel {
        ITextBox Folder { get; }

        Button SelectFolder { get; }
        Button Save { get; }
    }
}
