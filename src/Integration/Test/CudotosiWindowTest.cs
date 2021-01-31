using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CudotosiTestResources = Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Properties.Resources;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
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
            var targetFileName = SamplePictureLgFileName();
            Assert.IsFalse(File.Exists(targetFileName));
            var tasks = new List<ControllableProcessTask> {
                sut.CreateSetValueTask(process, nameof(ICudotosiApplicationModel.Folder), TestFolder.FullName),
                sut.CreateSetValueTask(process, nameof(ICudotosiApplicationModel.JpgFile), nameof(CudotosiTestResources.SamplePicture_XL) + ".jpg"),
                sut.CreateVerifyWhetherEnabledTask(process, nameof(ICudotosiApplicationModel.Default), true),
                sut.CreatePressButtonTask(process, nameof(ICudotosiApplicationModel.TransformHowManyPercent50)),
                sut.CreatePressButtonTask(process, nameof(ICudotosiApplicationModel.Default)),
                sut.CreateVerifyWhetherEnabledTask(process, nameof(ICudotosiApplicationModel.Save), true),
                sut.CreatePressButtonTask(process, nameof(ICudotosiApplicationModel.Save))
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
            Assert.IsTrue(File.Exists(targetFileName));
            var targetFileBytes = File.ReadAllBytes(targetFileName).ToList();
            var expectedTargetFileBytes = File.ReadAllBytes(SampleExpectedPictureLgFileName()).ToList();
            Assert.AreEqual(expectedTargetFileBytes.Count, targetFileBytes.Count);
            Assert.IsTrue(expectedTargetFileBytes.Select((b, i) => b == targetFileBytes[i]).All(b => b));
        }
    }
}
