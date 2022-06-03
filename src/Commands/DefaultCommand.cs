using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Commands;

public class DefaultCommand : ICommand {
    private readonly ICudotosiApplicationModel _Model;
    private readonly IMouseOwner _MouseOwner;
    private readonly ISimpleLogger _SimpleLogger;
    private readonly IMethodNamesFromStackFramesExtractor _MethodNamesFromStackFramesExtractor;

    public DefaultCommand(ICudotosiApplicationModel model, IMouseOwner mouseOwner, ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        _Model = model;
        _MouseOwner = mouseOwner;
        _SimpleLogger = simpleLogger;
        _MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    public async Task ExecuteAsync() {
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor)))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack("Executing default command", methodNamesFromStack);
            if (!_Model.Default.Enabled) {
                _SimpleLogger.LogInformationWithCallStack("Default command is not enabled", methodNamesFromStack);
                return;
            }

            await _MouseOwner.OnMouseDownAsync(0, 0);
        }
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor)))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack("Checking if default command should be enabled", methodNamesFromStack);
            try {
                return await Task.FromResult(_Model.Picture.BitmapImage.Width > 0 && _Model.Picture.BitmapImage.Height > 1);
            }
            catch {
                return false;
            }
        }
    }
}