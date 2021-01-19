using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public class CudotosiIntegrationTestBase {
        protected readonly IContainer Container;

        public CudotosiIntegrationTestBase() {
            Container = new ContainerBuilder().RegisterForCudotosiIntegrationTest().Build();
        }

        protected async Task<CudotosiWindowUnderTest> CreateCudotosiWindowUnderTestAsync() {
            var sut = Container.Resolve<CudotosiWindowUnderTest>();
            await sut.InitializeAsync();
            var process = await sut.FindIdleProcessAsync();
            var tasks = new List<ControllableProcessTask> {
                sut.CreateResetTask(process),
                sut.CreateMaximizeTask(process)
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
            return sut;
        }
    }
}
