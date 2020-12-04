using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class JpgFileSelectorHandler : ISimpleSelectorHandler {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly IImageHandler vImageHandler;
        private readonly ISimpleToggleButtonHandler vSourceSizeXlHandler;
        private readonly IJpgFileNameChanger vJpgFileNameChanger;

        public JpgFileSelectorHandler(ICudotosiApplicationModel model, IGuiAndAppHandler guiAndAppHandler, IImageHandler imageHandler, ISimpleToggleButtonHandler sourceSizeXlHandler, IJpgFileNameChanger jpgFileNameChanger) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vImageHandler = imageHandler;
            vSourceSizeXlHandler = sourceSizeXlHandler;
            vJpgFileNameChanger = jpgFileNameChanger;
        }

        public async Task UpdateSelectableValuesAsync() {
            var shortFileNames = vModel.Folder.Type == StatusType.None
                ? Directory.GetFiles(vModel.Folder.Text, "*_XL.jpg", SearchOption.TopDirectoryOnly).OrderBy(x => x).ToList()
                : new List<string>();
            var selectables = shortFileNames.Select(f => new Selectable { Guid = f, Name = f.Substring(f.LastIndexOf('\\') + 1) }).ToList();
            if (vModel.JpgFile.AreSelectablesIdentical(selectables)) { return; }

            vModel.JpgFile.UpdateSelectables(selectables);
            await SelectedIndexChangedAsync(-1);
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task SelectedIndexChangedAsync(int selectedIndex) {
            if (selectedIndex >= 0 && vModel.JpgFile.SelectedIndex == selectedIndex) { return; }

            vModel.JpgFile.SelectedIndex = selectedIndex;
            var fileName = selectedIndex >= 0 ? new Folder(vModel.Folder.Text).FullName + @"\" + vModel.JpgFile.SelectedItem.Name : "";
            await vImageHandler.LoadFromFile(fileName);
            vModel.SourceSizeLg.Enabled = File.Exists(vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Lg));
            vModel.SourceSizeMd.Enabled = File.Exists(vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Md));
            vModel.SourceSizeSm.Enabled = File.Exists(vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Sm));
            await vSourceSizeXlHandler.ToggledAsync(true);
        }
    }
}
