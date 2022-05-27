using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.GUI;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi {
    public static class CudotosiContainerBuilder {
        public static async Task<ContainerBuilder> UseCudotosiVishizhukelNetAndPeghAsync(this ContainerBuilder builder, CudotosiWindow cudotosiWindow) {
            await builder.UseVishizhukelNetDvinAndPeghAsync(new DummyCsArgumentPrompter(), new LogConfiguration());
            if (CudotosiApp.IsIntegrationTest) {
                builder.RegisterInstance(cudotosiWindow).As<IMouseOwner>();
                builder.RegisterType<FakeUserInteraction>().As<IUserInteraction>().SingleInstance();

            } else {
                builder.RegisterInstance(cudotosiWindow).As<IMouseOwner>().As<IUserInteraction>();
            }
            builder.RegisterType<CudotosiApplication>().As<CudotosiApplication>().As<IGuiAndAppHandler<CudotosiApplicationModel>>().SingleInstance();
            builder.RegisterType<CudotosiApplicationModel>()
                .As<CudotosiApplicationModel>().As<ICudotosiApplicationModel>().As<CudotosiApplicationModel>()
                .As<IApplicationModelBase>().As<IBusy>().SingleInstance();
            builder.RegisterType<CudotosiGuiAndApplicationSynchronizer>().WithParameter((p, _) => p.ParameterType == typeof(CudotosiWindow), (_, _) => cudotosiWindow)
                .As<IGuiAndApplicationSynchronizer<CudotosiApplicationModel>>();
            builder.RegisterType<CudotosiGuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            builder.RegisterType<CutCalculator>().As<ICutCalculator>();
            builder.RegisterType<JpgFileNameChanger>().As<IJpgFileNameChanger>().SingleInstance();
            builder.RegisterType<MousePositionAdjuster>().As<IMousePositionAdjuster>();
            builder.RegisterType<FakeApplicationLogger>().As<IApplicationLogger>();

            return builder;
        }
    }
}
