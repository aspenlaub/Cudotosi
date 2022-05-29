using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application;

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
        foreach (var sizeToButton in SourceSizesToButtons()) {
            var size = sizeToButton.Key;
            if (size == BootstrapSizes.Xl) { continue; }

            var button = sizeToButton.Value;
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

    [TestMethod]
    public async Task OnlyTargetSizesSmallerThanSourceSizeAreEnabled() {
        await SelectFolderAsync(TestFolder.FullName);
        foreach (var sourceSizeToButton in SourceSizesToHandlers()) {
            var sourceSize = sourceSizeToButton.Key;
            var handler = sourceSizeToButton.Value;
            await handler.ToggledAsync(false);
            await handler.ToggledAsync(true);
            foreach (var targetSizeToButton in TargetSizesToButtons()) {
                var targetSize = targetSizeToButton.Key;
                var targetButton = targetSizeToButton.Value;
                var expectedEnabled = (int) sourceSize > (int) targetSize;
                Assert.AreEqual(expectedEnabled, targetButton.Enabled);
            }
        }
    }
}