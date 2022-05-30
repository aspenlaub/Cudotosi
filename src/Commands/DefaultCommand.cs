using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class DefaultCommand : ICommand {
    private readonly ICudotosiApplicationModel Model;
    private readonly IMouseOwner MouseOwner;
    private readonly ISimpleLogger SimpleLogger;

    public DefaultCommand(ICudotosiApplicationModel model, IMouseOwner mouseOwner, ISimpleLogger simpleLogger) {
        Model = model;
        MouseOwner = mouseOwner;
        SimpleLogger = simpleLogger;
    }

    public async Task ExecuteAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), SimpleLogger.LogId))) {
            SimpleLogger.LogInformation("Executing default command");
            if (!Model.Default.Enabled) {
                SimpleLogger.LogInformation("Default command is not enabled");
                return;
            }

            await MouseOwner.OnMouseDownAsync(0, 0);
        }
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), SimpleLogger.LogId))) {
            SimpleLogger.LogInformation("Checking if default command should be enabled");
            try {
                return await Task.FromResult(Model.Picture.BitmapImage.Width > 0 && Model.Picture.BitmapImage.Height > 1);
            }
            catch {
                return false;
            }
        }
    }
}