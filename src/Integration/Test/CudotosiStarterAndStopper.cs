using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public class CudotosiStarterAndStopper : StarterAndStopperBase {
        protected override string ProcessName => "Aspenlaub.Net.GitHub.CSharp.Cudotosi";
        protected override List<string> AdditionalProcessNamesToStop => new List<string>();
        protected override string ExecutableFile() {
            return typeof(CudotosiWindowUnderTest).Assembly.Location
                .Replace(@"\Integration\Test\", @"\")
                .Replace("Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test.dll", ProcessName + ".exe");
        }
   }
}