using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class CudotosiCommands : ICudotosiCommands {
    public ICommand SelectFolderCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand DefaultCommand { get; set; }
}