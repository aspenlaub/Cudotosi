using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers {
    public class FakeGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<ICudotosiApplicationModel> {
        public ICudotosiApplicationModel Model { get; }

        public FakeGuiAndApplicationSynchronizer(ICudotosiApplicationModel model) {
            Model = model;
        }

        public void OnModelDataChanged() {
        }

        public void IndicateBusy(bool force) {
        }
    }
}
