using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public static class CudotosiIntegrationTestContainerBuilder {
        public static ContainerBuilder RegisterForCudotosiIntegrationTest(this ContainerBuilder builder) {
            builder.UseDvinAndPegh(new DummyCsArgumentPrompter());
            builder.RegisterType<CudotosiStarterAndStopper>().As<IStarterAndStopper>();
            builder.RegisterType<CudotosiWindowUnderTest>();
            builder.RegisterType<LogConfiguration>().As<ILogConfiguration>();
            builder.RegisterType<TashAccessor>().As<ITashAccessor>();
            return builder;
        }
    }
}
