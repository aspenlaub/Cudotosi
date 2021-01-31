using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Helpers {
    public class FakeMouseOwner : IMouseOwner {
        public async Task OnMouseDownAsync(double x, double y) {
            await Task.CompletedTask;
        }
    }
}
