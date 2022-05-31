using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class DefaultCommand : ICommand {
    private readonly ICudotosiApplicationModel Model;
    private readonly IMouseOwner MouseOwner;
    private readonly ISimpleLogger SimpleLogger;
    private readonly IMethodNamesFromStackFramesExtractor MethodNamesFromStackFramesExtractor;

    public DefaultCommand(ICudotosiApplicationModel model, IMouseOwner mouseOwner, ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        Model = model;
        MouseOwner = mouseOwner;
        SimpleLogger = simpleLogger;
        MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    public async Task ExecuteAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack("Executing default command", methodNamesFromStack);
            if (!Model.Default.Enabled) {
                SimpleLogger.LogInformationWithCallStack("Default command is not enabled", methodNamesFromStack);
                return;
            }

            await MouseOwner.OnMouseDownAsync(0, 0);
        }
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack("Checking if default command should be enabled", methodNamesFromStack);
            try {
                return await Task.FromResult(Model.Picture.BitmapImage.Width > 0 && Model.Picture.BitmapImage.Height > 1);
            }
            catch {
                return false;
            }
        }
    }
}