using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudotosiTestResources = Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Properties.Resources;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test;

[TestClass]
public class CudotosiWindowTest : CudotosiIntegrationTestBase {
    [TestCleanup]
    public override void Cleanup() {
        base.Cleanup();
    }

    [TestMethod]
    public async Task CanOpenCudotosi() {
        using (await CreateCudotosiWindowUnderTestAsync()) {}
    }

    [TestMethod]
    public async Task CanCutOutAnAreaFromPicture() {
        using var sut = await CreateCudotosiWindowUnderTestAsync();
        await EnsureControllableProcessAsync(sut);
        var process = ControllableProcess;
        var targetFileName = SamplePictureXlFileName();
        Assert.IsFalse(File.Exists(targetFileName));
        var tasks = new List<ControllableProcessTask> {
            sut.CreateSetValueTask(process, nameof(ICudotosiApplicationModel.Folder), TestFolder.FullName),
            sut.CreateSetValueTask(process, nameof(ICudotosiApplicationModel.JpgFile), nameof(CudotosiTestResources.SamplePicture_XXL) + ".jpg"),
            sut.CreateVerifyWhetherEnabledTask(process, nameof(ICudotosiApplicationModel.Default), true),
            sut.CreatePressButtonTask(process, nameof(ICudotosiApplicationModel.TransformHowManyPercent50)),
            sut.CreatePressButtonTask(process, nameof(ICudotosiApplicationModel.Default)),
            sut.CreateVerifyWhetherEnabledTask(process, nameof(ICudotosiApplicationModel.Save), true),
            sut.CreatePressButtonTask(process, nameof(ICudotosiApplicationModel.Save))
        };
        await sut.RemotelyProcessTaskListAsync(process, tasks, false, (_, _) => Task.CompletedTask);
        Assert.IsTrue(File.Exists(targetFileName));
        var targetFileBytes = (await File.ReadAllBytesAsync(targetFileName)).ToList();
        var expectedTargetFileBytes = (await File.ReadAllBytesAsync(SampleExpectedPictureXlFileName())).ToList();
        var alternativelyExpectedTargetFileBytes = (await File.ReadAllBytesAsync(SampleExpectedPictureXlFileName2())).ToList();
        Assert.IsTrue(expectedTargetFileBytes.Count == targetFileBytes.Count || alternativelyExpectedTargetFileBytes.Count == targetFileBytes.Count);
        Assert.IsTrue(expectedTargetFileBytes.Select((b, i) => b == targetFileBytes[i]).All(b => b)
            || alternativelyExpectedTargetFileBytes.Select((b, i) => b == targetFileBytes[i]).All(b => b));
    }
}