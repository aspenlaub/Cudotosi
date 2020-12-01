using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application {
    [TestClass]
    public class CudotosiApplicationTest {
        private IFolder vTestFolder;
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
            vTestFolder = new Folder(Path.GetTempPath()).SubFolder(nameof(CudotosiApplicationTest));
            vTestFolder.CreateIfNecessary();
            Properties.Resources.SamplePicture_XL.Save(vTestFolder.FullName + @"\" + nameof(Properties.Resources.SamplePicture_XL) + ".jpg");
        }

        [TestCleanup]
        public void Cleanup() {
            var deleter = new FolderDeleter();
            deleter.DeleteFolder(vTestFolder);
        }

        [TestMethod]
        public void CanSelectFolderButCannotSave() {
            Assert.IsTrue(vModel.SelectFolder.Enabled);
            Assert.IsFalse(vModel.Save.Enabled);
        }

        [TestMethod]
        public async Task JpgFileComboContainsAFileWhenTestFolderIsEntered() {
            var folderDialog = vContainer.Resolve<IFolderDialog>() as FakeFolderDialog;
            Assert.IsNotNull(folderDialog);
            folderDialog.FolderToReturn = vTestFolder.FullName;
            await vApplication.Commands.SelectFolderCommand.ExecuteAsync();
            Assert.AreEqual(vTestFolder.FullName, vModel.Folder.Text);
            var shortFileNames = vModel.JpgFile.Selectables.Select(s => s.Name).ToList();
            Assert.AreEqual(1, shortFileNames.Count);
            Assert.AreEqual(nameof(Properties.Resources.SamplePicture_XL) + ".jpg", shortFileNames[0]);
        }
    }
}