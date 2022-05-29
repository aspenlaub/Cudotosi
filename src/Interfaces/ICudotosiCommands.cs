using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

public interface ICudotosiCommands {
    ICommand SelectFolderCommand { get; set; }
    ICommand SaveCommand { get; set; }
    ICommand DefaultCommand { get; set; }
}