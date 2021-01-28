using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers {
    public class JpgFileSelectorHandler : ISimpleSelectorHandler {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly IPictureHandler vPictureHandler;
        private readonly IToggleButtonHandler vSourceSizeXlHandler;
        private readonly IJpgFileNameChanger vJpgFileNameChanger;

        public JpgFileSelectorHandler(ICudotosiApplicationModel model, IGuiAndAppHandler guiAndAppHandler, IPictureHandler pictureHandler, IToggleButtonHandler sourceSizeXlHandler, IJpgFileNameChanger jpgFileNameChanger) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vPictureHandler = pictureHandler;
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
            var haveSelectedIndex = selectedIndex >= 0;
            if (haveSelectedIndex && vModel.JpgFile.SelectedIndex == selectedIndex) { return; }

            vModel.JpgFile.SelectedIndex = selectedIndex;
            vModel.DestinationShapeAsIs.Enabled = haveSelectedIndex;
            vModel.DestinationShapeSquare.Enabled = haveSelectedIndex;
            vModel.TransformHowManyPercent100.Enabled = haveSelectedIndex;
            vModel.TransformHowManyPercent50.Enabled = haveSelectedIndex;
            var fileName = vPictureHandler.FileName();
            await vPictureHandler.LoadFromFile(fileName);
            vModel.SourceSizeLg.Enabled = File.Exists(vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Lg));
            vModel.SourceSizeMd.Enabled = File.Exists(vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Md));
            vModel.SourceSizeSm.Enabled = File.Exists(vJpgFileNameChanger.ChangeFileName(fileName, BootstrapSizes.Sm));
            await vSourceSizeXlHandler.ToggledAsync(true);
            vModel.Status.Type = StatusType.None;
            vModel.Status.Text = string.IsNullOrEmpty(fileName) ? "" : Properties.Resources.PleaseClickTopLeft;
        }
    }
}
