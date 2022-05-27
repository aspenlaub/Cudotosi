using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.GUI;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test {
    public static class CudotosiTestContainerBuilder {
        public static async Task<ContainerBuilder> UseCudotosiVishizhukelNetAndPeghAsync(this ContainerBuilder builder) {
            await builder.UseVishizhukelNetDvinAndPeghAsync(new DummyCsArgumentPrompter(), new LogConfiguration());
            builder.RegisterType<CudotosiApplication>().As<CudotosiApplication>().As<IGuiAndAppHandler<CudotosiApplicationModel>>().SingleInstance();
            builder.RegisterType<CudotosiApplicationModel>().As<CudotosiApplicationModel>().As<ICudotosiApplicationModel>().As<IApplicationModelBase>().As<IBusy>().SingleInstance();
            builder.RegisterType<CudotosiGuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            builder.RegisterType<CutCalculator>().As<ICutCalculator>();
            builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer<CudotosiApplicationModel>>();
            builder.RegisterType<FakeMouseOwner>().As<IMouseOwner>().SingleInstance();
            builder.RegisterType<FakeUserInteraction>().As<IUserInteraction>().SingleInstance();
            builder.RegisterType<JpgFileNameChanger>().As<IJpgFileNameChanger>().SingleInstance();
            builder.RegisterType<MousePositionAdjuster>().As<IMousePositionAdjuster>();
            builder.RegisterType<FakeApplicationLogger>().As<IApplicationLogger>();

            return builder;
        }
    }
}
