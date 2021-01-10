using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public class CudotosiWindowUnderTest : CudotosiWindowUnderTestActions, IDisposable {
        private readonly IStarterAndStopper vCudotosiStarterAndStopper;

        public CudotosiWindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper roustStarterAndStopper) : base(tashAccessor) {
            vCudotosiStarterAndStopper = roustStarterAndStopper;
        }

        public override async Task InitializeAsync() {
            await base.InitializeAsync();
            vCudotosiStarterAndStopper.Start();
        }

        public void Dispose() {
            vCudotosiStarterAndStopper.Stop();
        }
    }
}
