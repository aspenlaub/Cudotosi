﻿using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.GUI;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi {
    public static class CudotosiContainerBuilder {
        public static ContainerBuilder UseCudotosiVishizhukelNetAndPegh(this ContainerBuilder builder, CudotosiWindow cudotosiWindow) {
            builder.UseVishizhukelNetAndPegh(new DummyCsArgumentPrompter());
            builder.RegisterInstance(cudotosiWindow).As<IFolderDialog>();
            builder.RegisterType<CudotosiApplication>().As<CudotosiApplication>().As<IGuiAndAppHandler>().SingleInstance();
            builder.RegisterType<CudotosiApplicationModel>().As<CudotosiApplicationModel>().As<ICudotosiApplicationModel>().As<IApplicationModel>().As<IBusy>().SingleInstance();
            builder.RegisterType<CudotosiGuiAndApplicationSynchronizer>().WithParameter((p, c) => p.ParameterType == typeof(CudotosiWindow), (p, c) => cudotosiWindow).As<IGuiAndApplicationSynchronizer<ICudotosiApplicationModel>>();
            builder.RegisterType<CudotosiGuiToApplicationGate>().As<IGuiToApplicationGate>().SingleInstance();

            return builder;
        }
    }
}
