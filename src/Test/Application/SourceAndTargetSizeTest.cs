using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application {
    [TestClass]
    public class SourceAndTargetSizeTest : CudotosiApplicationTestBase {
        [TestInitialize]
        public override async Task Initialize() {
            await base.Initialize();
        }

        [TestCleanup]
        public override void Cleanup() {
            base.Cleanup();
        }

        [TestMethod]
        public async Task SourceIsXlWhenFileIsChanged() {
            await Application.Handlers.SourceSizeLgHandler.ToggledAsync(true);
            Assert.IsFalse(Model.SourceSizeXl.IsChecked);
            await SelectFolderAsync(TestFolder.FullName);
            await Application.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(0);
            Assert.IsTrue(Model.SourceSizeXl.IsChecked);
        }

        [TestMethod]
        public async Task SourceSizeIsDisabledWhenFileDoesNotExistInThatSize() {
            foreach (var size in new[] { BootstrapSizes.Lg, BootstrapSizes.Md, BootstrapSizes.Sm }) {
                ToggleButton button;
                switch (size) {
                    case BootstrapSizes.Lg:
                        button = Model.SourceSizeLg;
                    break;
                    case BootstrapSizes.Md:
                        button = Model.SourceSizeMd;
                    break;
                    case BootstrapSizes.Sm:
                        button = Model.SourceSizeSm;
                    break;
                    default:
                        throw new NotFiniteNumberException();
                }
                Assert.IsFalse(button.Enabled);
                await SelectFolderAsync(TestFolder.FullName);
                CreateSamplePictureFile(size);
                await Application.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(0);
                Assert.IsTrue(button.Enabled);
                DeleteSamplePictureFile(size);
                await Application.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(-1);
                Assert.IsFalse(button.Enabled);
                await Application.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(0);
                Assert.IsFalse(button.Enabled);
                await Application.Handlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(-1);
            }
        }
    }
}
