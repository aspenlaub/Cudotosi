using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public class CudotosiWindowUnderTest : CudotosiWindowUnderTestActions, IDisposable {
        private readonly IStarterAndStopper vCudotosiStarterAndStopper;
        private bool vInitialized;

        public CudotosiWindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper roustStarterAndStopper) : base(tashAccessor) {
            vCudotosiStarterAndStopper = roustStarterAndStopper;
            vInitialized = false;
        }

        public override async Task InitializeAsync() {
            Assert.IsFalse(vInitialized, "Window already has been initialized");
            await base.InitializeAsync();
            vCudotosiStarterAndStopper.Start();
            vInitialized = true;
        }

        public void Dispose() {
            vCudotosiStarterAndStopper.Stop();
        }
    }
}
