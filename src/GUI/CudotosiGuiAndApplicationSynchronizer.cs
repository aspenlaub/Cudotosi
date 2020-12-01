using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.GUI {
    public class CudotosiGuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<ICudotosiApplicationModel, CudotosiWindow> {
        public CudotosiGuiAndApplicationSynchronizer(ICudotosiApplicationModel model, CudotosiWindow window) : base(model, window) {
        }
    }
}
