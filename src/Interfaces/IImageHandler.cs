using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface IImageHandler {
        Task MouseDownAsync(int mousePosX, int mousePosY, int pictureWidth, int pictureHeight);
        Task LoadFromFile(string fileName);
    }
}
