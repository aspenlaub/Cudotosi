﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Enums;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class TashHandler : TashHandlerBase<ICudotosiApplicationModel> {
    public TashHandler(ITashAccessor tashAccessor, ISimpleLogger simpleLogger, IButtonNameToCommandMapper buttonNameToCommandMapper,
            IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper, IGuiAndAppHandler<CudotosiApplicationModel> guiAndAppHandler,
            ITashVerifyAndSetHandler<ICudotosiApplicationModel> tashVerifyAndSetHandler, ITashSelectorHandler<ICudotosiApplicationModel> tashSelectorHandler,
            ITashCommunicator<ICudotosiApplicationModel> tashCommunicator, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor)
        : base(tashAccessor, simpleLogger, buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndAppHandler, tashVerifyAndSetHandler, tashSelectorHandler,
                tashCommunicator, methodNamesFromStackFramesExtractor) {
    }

    protected override async Task ProcessSingleTaskAsync(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor)))) {
            var s = string.IsNullOrEmpty(status.TaskBeingProcessed.ControlName)
                ? $"Processing a task of type {status.TaskBeingProcessed.Type} in {nameof(TashHandler)}"
                : $"Processing a task of type {status.TaskBeingProcessed.Type} on {status.TaskBeingProcessed.ControlName} in {nameof(TashHandler)}";
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack(s, methodNamesFromStack);

            switch (status.TaskBeingProcessed.Type) {
                case ControllableProcessTaskType.Reset:
                    await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
                    break;
                default:
                    await base.ProcessSingleTaskAsync(status);
                    break;
            }
        }
    }
}