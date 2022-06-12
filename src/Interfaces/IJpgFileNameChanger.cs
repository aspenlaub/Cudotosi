using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

public interface IJpgFileNameChanger {
    string ChangeFileName(string xxlFileName, BootstrapSizes size, bool preview);
}