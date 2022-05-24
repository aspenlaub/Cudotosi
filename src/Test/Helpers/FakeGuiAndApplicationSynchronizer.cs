﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Helpers {
    public class FakeGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<ICudotosiApplicationModel> {
        public ICudotosiApplicationModel Model { get; }

        public FakeGuiAndApplicationSynchronizer(ICudotosiApplicationModel model) {
            Model = model;
        }

        public async Task OnModelDataChangedAsync() {
            await Task.CompletedTask;
        }

        public void IndicateBusy(bool force) {
        }

        public void OnWebBrowserLoadCompleted() {
        }

        public async Task<TResult> RunScriptAsync<TResult>(IScriptStatement scriptStatement) where TResult : IScriptCallResponse, new() {
            return await Task.FromResult(new TResult { Success = new YesNoInconclusive { Inconclusive = true, YesNo = false } });
        }

        public async Task WaitUntilNotNavigatingAnymoreAsync() {
            await Task.CompletedTask;
        }
    }
}
