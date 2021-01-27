using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var tasks = new List<ControllableProcessTask>();
            await sut.RemotelyProcessTaskListAsync(process, tasks);
        }
    }
}
