using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public class CudotosiWindowUnderTest : CudotosiWindowUnderTestActions, IDisposable {
        private readonly IStarterAndStopper CudotosiStarterAndStopper;
        private bool WindowUnderTestInitialized;

        public CudotosiWindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper roustStarterAndStopper) : base(tashAccessor) {
            CudotosiStarterAndStopper = roustStarterAndStopper;
            WindowUnderTestInitialized = false;
        }

        public override async Task InitializeAsync() {
            Assert.IsFalse(WindowUnderTestInitialized, "Window already has been initialized");
            await base.InitializeAsync();
            CudotosiStarterAndStopper.Start();
            WindowUnderTestInitialized = true;
        }

        public void Dispose() {
            CudotosiStarterAndStopper.Stop();
        }
    }
}
