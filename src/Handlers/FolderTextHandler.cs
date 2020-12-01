using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class FolderTextHandler : ISimpleTextHandler {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly ISimpleSelectorHandler vJpgFileSelectorHandler;

        public FolderTextHandler(ICudotosiApplicationModel model, IGuiAndAppHandler guiAndAppHandler, ISimpleSelectorHandler jpgFileSelectorHandler) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vJpgFileSelectorHandler = jpgFileSelectorHandler;
        }

        public async Task TextChangedAsync(string text) {
            if (vModel.Folder.Text == text) { return; }

            vModel.Folder.Text = text;
            await vJpgFileSelectorHandler.UpdateSelectableValuesAsync();
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
