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

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Application;

public abstract class CudotosiApplicationTestBase {
    protected IFolder TestFolder;
    protected IContainer Container;
    protected CudotosiApplication Application;
    protected ICudotosiApplicationModel Model;
    protected IJpgFileNameChanger JpgFileNameChanger;

    public virtual async Task Initialize() {
        Container = (await new ContainerBuilder().UseCudotosiVishizhukelNetAndPeghAsync()).Build();
        Application = Container.Resolve<CudotosiApplication>();
        Assert.IsNotNull(Application);
        Model = Container.Resolve<ICudotosiApplicationModel>();
        Assert.IsNotNull(Model);
        await Application.OnLoadedAsync();
        TestFolder = new Folder(Path.GetTempPath()).SubFolder(GetType().Name);
        TestFolder.CreateIfNecessary();
        Properties.Resources.SamplePicture_XXL.Save(SamplePictureXxlFileName());
        JpgFileNameChanger = Container.Resolve<IJpgFileNameChanger>();
    }

    private string SamplePictureXxlFileName() {
        return TestFolder.FullName + @"\" + nameof(Properties.Resources.SamplePicture_XXL) + ".jpg";
    }

    protected void CreateSamplePictureFile(BootstrapSizes size) {
        File.Copy(SamplePictureXxlFileName(), JpgFileNameChanger.ChangeFileName(SamplePictureXxlFileName(), size, false));
    }

    protected void DeleteSamplePictureFile(BootstrapSizes size) {
        var fileName = JpgFileNameChanger.ChangeFileName(SamplePictureXxlFileName(), size, false);
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
        return new() {
            { BootstrapSizes.Xxl, Model.SourceSizeXxl },
            { BootstrapSizes.Xl, Model.SourceSizeXl },
            { BootstrapSizes.Lg, Model.SourceSizeLg },
            { BootstrapSizes.Md, Model.SourceSizeMd },
            { BootstrapSizes.Sm, Model.SourceSizeSm }
        };
    }

    protected Dictionary<BootstrapSizes, IToggleButtonHandler> SourceSizesToHandlers() {
        return new() {
            { BootstrapSizes.Xxl, Application.Handlers.SourceSizeXxlHandler },
            { BootstrapSizes.Xl, Application.Handlers.SourceSizeXlHandler },
            { BootstrapSizes.Lg, Application.Handlers.SourceSizeLgHandler },
            { BootstrapSizes.Md, Application.Handlers.SourceSizeMdHandler },
            { BootstrapSizes.Sm, Application.Handlers.SourceSizeSmHandler }
        };
    }

    protected Dictionary<BootstrapSizes, ToggleButton> TargetSizesToButtons() {
        return new() {
            { BootstrapSizes.Xl, Model.TargetSizeXl },
            { BootstrapSizes.Lg, Model.TargetSizeLg },
            { BootstrapSizes.Md, Model.TargetSizeMd },
            { BootstrapSizes.Sm, Model.TargetSizeSm },
            { BootstrapSizes.Xs, Model.TargetSizeXs }
        };
    }
}