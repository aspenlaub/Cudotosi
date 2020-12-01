using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities {
    public class CudotosiApplicationModel : ApplicationModelBase, ICudotosiApplicationModel {
        public ITextBox Folder { get; } = new TextBox();
        public ISelector JpgFile { get; } = new ComboBox();

        public Button SelectFolder { get; } = new Button();
        public Button Save { get; } = new Button();
    }
}
