using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface ISourceAreaHandler {
        Task OnMousePositionChangedAsync();
        Task OnDestinationShapeChangedAsync();
        Task OnTransformHowManyPercentChangedAsync();
    }
}
