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
        public static ContainerBuilder UseCudotosiVishizhukelNetAndPegh(this ContainerBuilder builder) {
            builder.UseVishizhukelNetDvinAndPegh(new DummyCsArgumentPrompter(), new LogConfiguration());
            builder.RegisterType<ApplicationLogger>().As<ApplicationLogger>().As<IApplicationLogger>().SingleInstance();
            builder.RegisterType<FakeFolderDialog>().As<IFolderDialog>().SingleInstance();
            builder.RegisterType<CudotosiApplication>().As<CudotosiApplication>().As<IGuiAndAppHandler>().SingleInstance();
            builder.RegisterType<CudotosiApplicationModel>().As<CudotosiApplicationModel>().As<ICudotosiApplicationModel>().As<IApplicationModel>().As<IBusy>().SingleInstance();
            builder.RegisterType<FakeGuiAndApplicationSynchronizer>().As<IGuiAndApplicationSynchronizer<ICudotosiApplicationModel>>();
            builder.RegisterType<CudotosiGuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();
            builder.RegisterType<JpgFileNameChanger>().As<IJpgFileNameChanger>().SingleInstance();

            return builder;
        }
    }
}
