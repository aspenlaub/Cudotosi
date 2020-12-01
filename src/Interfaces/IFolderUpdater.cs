using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface IFolderUpdater {
        Task FolderTextChangedAsync(string text);
    }
}
