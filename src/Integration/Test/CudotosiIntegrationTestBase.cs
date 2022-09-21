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

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test;

public class CudotosiIntegrationTestBase {
    protected IFolder TestFolder;
    protected readonly IContainer Container;
    protected ControllableProcess ControllableProcess;

    public CudotosiIntegrationTestBase() {
        Container = new ContainerBuilder().RegisterForCudotosiIntegrationTest().Build();
        TestFolder = new Folder(Path.GetTempPath()).SubFolder(GetType().Name);
        TestFolder.CreateIfNecessary();
        CudotosiTestResources.SamplePicture_XXL.Save(SamplePictureXxlFileName());
        CudotosiTestResources.SamplePicture_XL.Save(SampleExpectedPictureXlFileName());
    }

    protected async Task<CudotosiWindowUnderTest> CreateCudotosiWindowUnderTestAsync() {
        var sut = Container.Resolve<CudotosiWindowUnderTest>();
        await sut.InitializeAsync();
        await EnsureControllableProcessAsync(sut);

        var process = ControllableProcess;
        var tasks = new List<ControllableProcessTask> {
            sut.CreateResetTask(process)
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks, false, (_, _) => Task.CompletedTask);
        return sut;
    }

    protected async Task EnsureControllableProcessAsync(CudotosiWindowUnderTest sut) {
        if (ControllableProcess != null) { return; }

        ControllableProcess = await sut.FindIdleProcessAsync();
    }

    protected string SamplePictureXxlFileName() {
        return TestFolder.FullName + @"\" + nameof(CudotosiTestResources.SamplePicture_XXL) + ".jpg";
    }

    protected string SamplePictureXlFileName() {
        return TestFolder.FullName + @"\" + nameof(CudotosiTestResources.SamplePicture_XXL).Replace("XXL", "XL") + ".jpg";
    }

    protected string SampleExpectedPictureXlFileName() {
        return TestFolder.FullName + @"\" + nameof(CudotosiTestResources.SamplePicture_XXL).Replace("XXL", "PXL") + ".jpg";
    }

    public virtual void Cleanup() {
        Assert.IsTrue(TestFolder.Exists());
        var deleter = new FolderDeleter();
        Assert.IsTrue(deleter.CanDeleteFolder(TestFolder));
        deleter.DeleteFolder(TestFolder);
    }
}