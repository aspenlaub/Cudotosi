using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Handlers;

public class TashSelectorHandler : TashSelectorHandlerBase<ICudotosiApplicationModel> {
    private readonly ICudotosiHandlers CudotosiHandlers;

    public TashSelectorHandler(ICudotosiHandlers cudotosiHandlers, ISimpleLogger simpleLogger, ITashCommunicator<ICudotosiApplicationModel> tashCommunicator, Dictionary<string, ISelector> selectors)
        : base(simpleLogger, tashCommunicator, selectors) {
        CudotosiHandlers = cudotosiHandlers;
    }

    public override async Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<ICudotosiApplicationModel> status) {
        var controlName = status.TaskBeingProcessed.ControlName;
        SimpleLogger.LogInformation($"{controlName} is a valid selector");
        var selector = Selectors[controlName];

        await SelectedIndexChangedAsync(status, controlName, -1, false);
        if (status.TaskBeingProcessed.Status == ControllableProcessTaskStatus.BadRequest) { return; }

        var itemToSelect = status.TaskBeingProcessed.Text;
        await SelectItemAsync(status, selector, itemToSelect, controlName);
    }

    protected override async Task SelectedIndexChangedAsync(ITashTaskHandlingStatus<ICudotosiApplicationModel> status, string controlName, int selectedIndex, bool selectablesChanged) {
        if (selectedIndex < 0) { return; }

        SimpleLogger.LogInformation($"Changing selected index for {controlName} to {selectedIndex}");
        switch (controlName) {
            case nameof(status.Model.JpgFile):
                await CudotosiHandlers.JpgFileSelectorHandler.SelectedIndexChangedAsync(selectedIndex);
                break;
            default:
                var errorMessage = $"Do not know how to select for {status.TaskBeingProcessed.ControlName}";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                break;
        }
    }
}