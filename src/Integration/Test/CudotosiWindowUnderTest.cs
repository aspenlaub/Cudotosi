using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test;

public class CudotosiWindowUnderTest : CudotosiWindowUnderTestActions, IDisposable {
    private readonly IStarterAndStopper _CudotosiStarterAndStopper;
    private bool _WindowUnderTestInitialized;

    public CudotosiWindowUnderTest(ITashAccessor tashAccessor, IStarterAndStopper roustStarterAndStopper) : base(tashAccessor) {
        _CudotosiStarterAndStopper = roustStarterAndStopper;
        _WindowUnderTestInitialized = false;
    }

    public override async Task InitializeAsync() {
        Assert.IsFalse(_WindowUnderTestInitialized, "Window already has been initialized");
        await base.InitializeAsync();
        _CudotosiStarterAndStopper.Start();
        _WindowUnderTestInitialized = true;
    }

    public void Dispose() {
        _CudotosiStarterAndStopper.Stop();
    }
}