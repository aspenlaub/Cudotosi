using System.IO;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application {
    public abstract class CudotosiApplicationTestBase {
        protected IFolder TestFolder;
        protected IContainer Container;
        protected CudotosiApplication Application;
        protected ICudotosiApplicationModel Model;

        public virtual async Task Initialize() {
            Container = new ContainerBuilder()
                .UseCudotosiVishizhukelNetAndPegh()
                .Build();
            Application = Container.Resolve<CudotosiApplication>();
            Assert.IsNotNull(Application);
            Model = Container.Resolve<ICudotosiApplicationModel>();
            Assert.IsNotNull(Model);
            await Application.OnLoadedAsync();
            TestFolder = new Folder(Path.GetTempPath()).SubFolder(GetType().Name);
            TestFolder.CreateIfNecessary();
            Properties.Resources.SamplePicture_XL.Save(TestFolder.FullName + @"\" + nameof(Properties.Resources.SamplePicture_XL) + ".jpg");
        }

        public virtual void Cleanup() {
            var deleter = new FolderDeleter();
            deleter.DeleteFolder(TestFolder);
        }
    }
}
