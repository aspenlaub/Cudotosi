using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudotosiTestResources = Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Properties.Resources;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public class CudotosiIntegrationTestBase {
        protected IFolder TestFolder;
        protected readonly IContainer Container;

        public CudotosiIntegrationTestBase() {
            Container = new ContainerBuilder().RegisterForCudotosiIntegrationTest().Build();
            TestFolder = new Folder(Path.GetTempPath()).SubFolder(GetType().Name);
            TestFolder.CreateIfNecessary();
            CudotosiTestResources.SamplePicture_XL.Save(SamplePictureXlFileName());
        }

        protected async Task<CudotosiWindowUnderTest> CreateCudotosiWindowUnderTestAsync() {
            var sut = Container.Resolve<CudotosiWindowUnderTest>();
            await sut.InitializeAsync();
            var process = await sut.FindIdleProcessAsync();
            var tasks = new List<ControllableProcessTask> {
                sut.CreateResetTask(process),
                sut.CreateMaximizeTask(process)
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
            return sut;
        }

        private string SamplePictureXlFileName() {
            return TestFolder.FullName + @"\" + nameof(CudotosiTestResources.SamplePicture_XL) + ".jpg";
        }

        public virtual void Cleanup() {
            Assert.IsTrue(TestFolder.Exists());
            var deleter = new FolderDeleter();
            Assert.IsTrue(deleter.CanDeleteFolder(TestFolder));
            deleter.DeleteFolder(TestFolder);
        }
    }
}
