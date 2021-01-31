using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface IMouseOwner {
        Task OnMouseDownAsync(double x, double y);
    }
}
