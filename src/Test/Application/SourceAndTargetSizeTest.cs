using System.Threading.Tasks;
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
    }
}
