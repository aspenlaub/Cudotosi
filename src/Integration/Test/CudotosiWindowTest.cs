using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudotosiTestResources = Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Properties.Resources;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    [TestClass]
    public class CudotosiWindowTest : CudotosiIntegrationTestBase {
        [TestCleanup]
        public override void Cleanup() {
            base.Cleanup();
        }

        [TestMethod]
        public async Task CanOpenCudotosi() {
            using (await CreateCudotosiWindowUnderTestAsync()) {}
        }

        [TestMethod]
        public async Task CanCutOutAnAreaFromPicture() {
            using var sut = await CreateCudotosiWindowUnderTestAsync();
            await sut.InitializeAsync();
            var process = await sut.FindIdleProcessAsync();
            var tasks = new List<ControllableProcessTask> {
                sut.CreateSetValueTask(process, nameof(ICudotosiApplicationModel.Folder), TestFolder.FullName),
                sut.CreateSetValueTask(process, nameof(ICudotosiApplicationModel.JpgFile), nameof(CudotosiTestResources.SamplePicture_XL) + ".jpg"),
                sut.CreatePressButtonTask(process, nameof(ICudotosiApplicationModel.TransformHowManyPercent50))
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
        }
    }
}
