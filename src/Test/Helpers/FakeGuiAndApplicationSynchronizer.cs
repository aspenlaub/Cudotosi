using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers;

public class FakeGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<CudotosiApplicationModel> {
    public CudotosiApplicationModel Model { get; }

    public FakeGuiAndApplicationSynchronizer(CudotosiApplicationModel model) {
        Model = model;
    }

    public async Task OnModelDataChangedAsync() {
        await Task.CompletedTask;
    }

    public void IndicateBusy(bool force) {
    }
}