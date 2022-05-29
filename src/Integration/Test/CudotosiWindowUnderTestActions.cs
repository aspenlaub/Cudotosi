using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test;

public class CudotosiWindowUnderTestActions : WindowUnderTestActionsBase {
    public CudotosiWindowUnderTestActions(ITashAccessor tashAccessor) : base(tashAccessor, "Aspenlaub.Net.GitHub.CSharp.Cudotosi") {
    }
}