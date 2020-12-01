using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application {
    [TestClass]
    public class CudotosiApplicationTest {
        private CudotosiApplication vApplication;
        private ICudotosiApplicationModel vModel;

        [TestInitialize]
        public void Initialize() {
            var container = new ContainerBuilder()
                .UseCudotosiVishizhukelNetAndPegh()
                .Build();
            vApplication = container.Resolve<CudotosiApplication>();
            Assert.IsNotNull(vApplication);
            vModel = container.Resolve<ICudotosiApplicationModel>();
            Assert.IsNotNull(vModel);
            vApplication.RegisterTypes();
        }

        [TestMethod]
        public void CannotSave() {
            Assert.IsFalse(vModel.Save.Enabled);
        }
    }
}