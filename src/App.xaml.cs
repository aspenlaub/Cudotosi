using System.Linq;
using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi {
    /// <summary>
    /// Interaction logic for CudotosiApp.xaml
    /// </summary>
    public partial class CudotosiApp {
        public static bool IsIntegrationTest { get; private set; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            IsIntegrationTest = e.Args.Any(a => a == "/UnitTest");
        }
    }
}
