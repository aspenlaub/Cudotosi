using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.GUI;

public class CudotosiGuiToApplicationGate : GuiToApplicationGateBase<CudotosiApplication, CudotosiApplicationModel> {
    public CudotosiGuiToApplicationGate(IBusy busy, CudotosiApplication application) : base(busy, application) {
    }
}