using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test {
    public class CudotosiStarterAndStopper : IStarterAndStopper {
        private const string CudotosiName = "Aspenlaub.Net.GitHub.CSharp.Cudotosi";

        public void Start() {
            var executableFile = typeof(CudotosiWindowUnderTest).Assembly.Location
                .Replace(@"\Integration\Test\", @"\")
                .Replace("Aspenlaub.Net.GitHub.CSharp.Cudotosi.Integration.Test.dll", CudotosiName + ".exe");


            if (!File.Exists(executableFile)) {
                throw new Exception("File '" + executableFile + "' does not exist");
            }

            Stop();

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = executableFile,
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(executableFile) ?? ""
                }
            };
            process.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (Process.GetProcessesByName(CudotosiName).Length != 1) {
                throw new Exception("Cudotosi process could not be started");
            }
        }

        public void Stop() {
            bool again;
            var attempts = 10;
            do {
                again = false;
                try {
                    foreach (var process in Process.GetProcessesByName(CudotosiName)) {
                        process.Kill();
                    }
                } catch {
                    again = --attempts >= 0;
                }
            } while (again);
        }
    }
}