using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands {
    public class DefaultCommand : ICommand {
        private readonly ICudotosiApplicationModel vModel;
        private readonly IMouseOwner vMouseOwner;
        private readonly ISimpleLogger vSimpleLogger;
        private readonly ILogConfiguration vLogConfiguration;

        public DefaultCommand(ICudotosiApplicationModel model, IMouseOwner mouseOwner, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration) {
            vModel = model;
            vMouseOwner = mouseOwner;
            vSimpleLogger = simpleLogger;
            vLogConfiguration = logConfiguration;
        }

        public async Task ExecuteAsync() {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), vLogConfiguration.LogId))) {
                vSimpleLogger.LogInformation("Executing default command");
                if (!vModel.Default.Enabled) {
                    vSimpleLogger.LogInformation("Default command is not enabled");
                    return;
                }

                await vMouseOwner.OnMouseDownAsync(0, 0);
            }
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), vLogConfiguration.LogId))) {
                vSimpleLogger.LogInformation("Checking if default command should be enabled");
                try {
                    return await Task.FromResult(vModel.Picture.BitmapImage.Width > 0 && vModel.Picture.BitmapImage.Height > 0);
                }
                catch {
                    return false;
                }
            }
        }
    }
}