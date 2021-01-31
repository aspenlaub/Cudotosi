using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Application;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application {
    public abstract class CudotosiApplicationTestBase {
        protected IFolder TestFolder;
        protected IContainer Container;
        protected CudotosiApplication Application;
        protected ICudotosiApplicationModel Model;
        protected IJpgFileNameChanger JpgFileNameChanger;

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
            Properties.Resources.SamplePicture_XL.Save(SamplePictureXlFileName());
            JpgFileNameChanger = Container.Resolve<IJpgFileNameChanger>();
        }

        private string SamplePictureXlFileName() {
            return TestFolder.FullName + @"\" + nameof(Properties.Resources.SamplePicture_XL) + ".jpg";
        }

        protected void CreateSamplePictureFile(BootstrapSizes size) {
            File.Copy(SamplePictureXlFileName(), JpgFileNameChanger.ChangeFileName(SamplePictureXlFileName(), size));
        }

        protected void DeleteSamplePictureFile(BootstrapSizes size) {
            var fileName = JpgFileNameChanger.ChangeFileName(SamplePictureXlFileName(), size);
            if (!File.Exists(fileName)) { return; }

            File.Delete(fileName);
        }

        public virtual void Cleanup() {
            Assert.IsTrue(TestFolder.Exists());
            var deleter = new FolderDeleter();
            Assert.IsTrue(deleter.CanDeleteFolder(TestFolder));
            deleter.DeleteFolder(TestFolder);
        }

        protected async Task SelectFolderAsync(string folder) {
            var fakeUserInteraction = Container.Resolve<IUserInteraction>() as FakeUserInteraction;
            Assert.IsNotNull(fakeUserInteraction);
            fakeUserInteraction.FolderToReturn = folder;
            await Application.Commands.SelectFolderCommand.ExecuteAsync();
            Assert.AreEqual(folder, Model.Folder.Text);
        }

        protected Dictionary<BootstrapSizes, ToggleButton> SourceSizesToButtons() {
            return new Dictionary<BootstrapSizes, ToggleButton> {
                { BootstrapSizes.Xl, Model.SourceSizeXl },
                { BootstrapSizes.Lg, Model.SourceSizeLg },
                { BootstrapSizes.Md, Model.SourceSizeMd },
                { BootstrapSizes.Sm, Model.SourceSizeSm }
            };
        }

        protected Dictionary<BootstrapSizes, IToggleButtonHandler> SourceSizesToHandlers() {
            return new Dictionary<BootstrapSizes, IToggleButtonHandler> {
                { BootstrapSizes.Xl, Application.Handlers.SourceSizeXlHandler },
                { BootstrapSizes.Lg, Application.Handlers.SourceSizeLgHandler },
                { BootstrapSizes.Md, Application.Handlers.SourceSizeMdHandler },
                { BootstrapSizes.Sm, Application.Handlers.SourceSizeSmHandler }
            };
        }

        protected Dictionary<BootstrapSizes, ToggleButton> TargetSizesToButtons() {
            return new Dictionary<BootstrapSizes, ToggleButton> {
                { BootstrapSizes.Lg, Model.TargetSizeLg },
                { BootstrapSizes.Md, Model.TargetSizeMd },
                { BootstrapSizes.Sm, Model.TargetSizeSm },
                { BootstrapSizes.Xs, Model.TargetSizeXs }
            };
        }
    }
}
