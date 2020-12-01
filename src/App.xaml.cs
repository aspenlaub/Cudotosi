using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi {
    /// <summary>
    /// Interaction logic for CudotosiApp.xaml
    /// </summary>
    public partial class CudotosiApp {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            ExceptionHandlerUpSetter.SetUp(this);
        }
    }
}
