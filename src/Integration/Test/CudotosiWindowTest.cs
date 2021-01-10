using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    [TestClass]
    public class CudotosiWindowTest : CudotosiIntegrationTestBase {
        [TestMethod]
        public async Task CanOpenCudotosi() {
            using (await CreateCudotosiWindowUnderTestAsync()) { }
        }
    }
}
