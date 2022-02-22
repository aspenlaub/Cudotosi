using System;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
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
            await SelectFolderAsync(TestFolder.FullName);
            var shortFileNames = Model.JpgFile.Selectables.Select(s => s.Name).ToList();
            Assert.AreEqual(1, shortFileNames.Count);
            Assert.AreEqual(nameof(Properties.Resources.SamplePicture_XL) + ".jpg", shortFileNames[0]);
        }

        [TestMethod]
        public async Task JpgFileComboContainsNoFileWhenEnteredFolderDoesNotExist() {
            await SelectFolderAsync(Guid.NewGuid().ToString());
            Assert.AreEqual(StatusType.Error, Model.Folder.Type);
            Assert.IsFalse(Model.JpgFile.Selectables.Any());
        }

        [TestMethod]
        public async Task PictureIsUpdatedWhenFileIsChanged() {
            await SelectFolderAsync(TestFolder.FullName);
            Assert.IsFalse(HasBitmapBeenLoaded());
            await Application.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(0);
            Assert.IsTrue(HasBitmapBeenLoaded());
        }

        private bool HasBitmapBeenLoaded() {
            double height;

            try {
                height = Model.Picture.BitmapImage.Height;
            } catch (Exception e) {
                Assert.IsTrue(e.Message.Contains("not been initialized"));
                return false;
            }

            return height > 1;
        }
    }
}