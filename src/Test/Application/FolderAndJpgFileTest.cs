using System;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application {
    [TestClass]
    public class FolderAndJpgFileTest : CudotosiApplicationTestBase {
        [TestInitialize]
        public override async Task Initialize() {
            await base.Initialize();
        }

        [TestCleanup]
        public override void Cleanup() {
            base.Cleanup();
        }

        [TestMethod]
        public void CanSelectFolderButCannotSave() {
            Assert.IsTrue(Model.SelectFolder.Enabled);
            Assert.IsFalse(Model.Save.Enabled);
        }

        [TestMethod]
        public async Task JpgFileComboContainsAFileWhenTestFolderIsEntered() {
            var folderDialog = Container.Resolve<IFolderDialog>() as FakeFolderDialog;
            Assert.IsNotNull(folderDialog);
            folderDialog.FolderToReturn = TestFolder.FullName;
            await Application.Commands.SelectFolderCommand.ExecuteAsync();
            Assert.AreEqual(TestFolder.FullName, Model.Folder.Text);
            var shortFileNames = Model.JpgFile.Selectables.Select(s => s.Name).ToList();
            Assert.AreEqual(1, shortFileNames.Count);
            Assert.AreEqual(nameof(Properties.Resources.SamplePicture_XL) + ".jpg", shortFileNames[0]);
        }

        [TestMethod]
        public async Task JpgFileComboContainsNoFileWhenEnteredFolderDoesNotExist() {
            var folderDialog = Container.Resolve<IFolderDialog>() as FakeFolderDialog;
            Assert.IsNotNull(folderDialog);
            var folder = Guid.NewGuid().ToString();
            folderDialog.FolderToReturn = folder;
            await Application.Commands.SelectFolderCommand.ExecuteAsync();
            Assert.AreEqual(folder, Model.Folder.Text);
            Assert.AreEqual(StatusType.Error, Model.Folder.Type);
            Assert.IsFalse(Model.JpgFile.Selectables.Any());
        }
    }
}