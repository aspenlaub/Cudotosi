using System.IO;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application {
    [TestClass]
    public class CudotosiApplicationTest {
        private IContainer vContainer;
        private CudotosiApplication vApplication;
        private ICudotosiApplicationModel vModel;

        [TestInitialize]
        public void Initialize() {
            vContainer = new ContainerBuilder()
                .UseCudotosiVishizhukelNetAndPegh()
                .Build();
            vApplication = vContainer.Resolve<CudotosiApplication>();
            Assert.IsNotNull(vApplication);
            vModel = vContainer.Resolve<ICudotosiApplicationModel>();
            Assert.IsNotNull(vModel);
            vApplication.RegisterTypes();
            vApplication.EnableOrDisableButtonsThenSyncGuiAndAppAsync().Wait();
        }

        [TestMethod]
        public void CanSelectFolderButCannotSave() {
            Assert.IsTrue(vModel.SelectFolder.Enabled);
            Assert.IsFalse(vModel.Save.Enabled);
        }

        [TestMethod]
        public async Task CanSelectAFolder() {
            var folder = Path.GetTempPath();
            var folderDialog = vContainer.Resolve<IFolderDialog>() as FakeFolderDialog;
            Assert.IsNotNull(folderDialog);
            folderDialog.FolderToReturn = folder;
            await vApplication.Commands.SelectFolderCommand.ExecuteAsync();
            Assert.AreEqual(folder, vModel.Folder.Text);
        }
    }
}