using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface IPictureHandler {
        Task MouseDownAsync(int mousePosX, int mousePosY, int pictureWidth, int pictureHeight, int actualPictureWidth, int actualPictureHeight);
        Task PictureSizeChangedAsync(int actualPictureWidth, int actualPictureHeight);
        string FileName();
        Task LoadFromFile(string fileName);
    }
}
