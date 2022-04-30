﻿using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Entities;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.GUI {
    public class CudotosiGuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<CudotosiApplicationModel, CudotosiWindow> {
        public CudotosiGuiAndApplicationSynchronizer(CudotosiApplicationModel model, CudotosiWindow window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
        }

        public override void OnImageChanged(Image image) {
            base.OnImageChanged(image);
            var source = image.Source as BitmapSource;
            if (source?.ToString().Contains(".jpg") != true) { return; }

            Window.AdjustCanvasAndImageSync();
        }
    }
}
