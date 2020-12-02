using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands {
    public class SaveCommand : ICommand {
        private readonly ICudotosiApplicationModel vModel;

        public SaveCommand(ICudotosiApplicationModel model) {
            vModel = model;
        }

        public async Task ExecuteAsync() {
            if (!vModel.Save.Enabled) {
                return;
            }

            await Task.Run(() => { }); // TODO: replace
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            return await Task.FromResult(false); // TODO: replace
        }
    }
}
