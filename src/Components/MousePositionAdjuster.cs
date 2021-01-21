using System;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components {
    public class MousePositionAdjuster : IMousePositionAdjuster {
        public void AdjustMousePosition(ICudotosiApplicationModel model) {
            if (model.DestinationShapeAsIs.IsChecked) {
                if (model.TransformHowManyPercent100.IsChecked) {
                    model.SourceAreaWidth = model.PictureWidth;
                    model.SourceAreaHeight = model.PictureHeight;
                } else {
                    model.SourceAreaWidth = model.PictureWidth / 2;
                    model.SourceAreaHeight = model.PictureHeight / 2;
                }
            } else {
                if (model.TransformHowManyPercent100.IsChecked) {
                    model.SourceAreaWidth = Math.Min(model.PictureWidth, model.PictureHeight);
                    model.SourceAreaHeight = model.SourceAreaWidth;
                } else {
                    model.SourceAreaWidth = Math.Min(model.PictureWidth, model.PictureHeight) / 2;
                    model.SourceAreaHeight = model.SourceAreaWidth;
                }
            }

            if (model.MousePosX + model.SourceAreaWidth > model.PictureWidth) {
                model.MousePosX = model.PictureWidth - model.SourceAreaWidth;
            }
            if (model.MousePosY + model.SourceAreaHeight > model.PictureHeight) {
                model.MousePosY = model.PictureHeight - model.SourceAreaHeight;
            }
            if (model.MousePosX < 0) {
                model.MousePosX = 0;
            }
            if (model.MousePosY < 0) {
                model.MousePosY = 0;
            }
        }
    }
}