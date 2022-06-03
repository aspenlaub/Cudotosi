using System;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components;

public class MousePositionAdjuster : IMousePositionAdjuster {
    private readonly ICutCalculator _CutCalculator;

    public MousePositionAdjuster(ICutCalculator cutCalculator) {
        _CutCalculator = cutCalculator;
    }

    public void AdjustMousePosition(ICudotosiApplicationModel model) {
        if (model.DestinationShapeAsIs.IsChecked) {
            model.SourceAreaWidth = model.PictureWidth;
            model.SourceAreaHeight = model.PictureHeight;
        } else if (model.DestinationShapeSquare.IsChecked) {
            model.SourceAreaWidth = Math.Min(model.PictureWidth, model.PictureHeight);
            model.SourceAreaHeight = model.SourceAreaWidth;
        } else {
            _CutCalculator.TargetSize(model, model.PictureWidth, model.PictureHeight, out var targetWidth, out var targetHeight);
            if (targetHeight == 0 || targetWidth == 0) {
                model.SourceAreaHeight = 0;
                model.SourceAreaWidth = 0;
            } else if (targetWidth * model.PictureHeight >= targetHeight * model.PictureWidth) {
                model.SourceAreaWidth = model.PictureWidth;
                model.SourceAreaHeight = model.PictureWidth * targetHeight / targetWidth;
            } else {
                model.SourceAreaHeight = model.PictureHeight;
                model.SourceAreaWidth = model.PictureHeight * targetWidth / targetHeight;
            }
        }
        if (!model.TransformHowManyPercent100.IsChecked) {
            model.SourceAreaWidth /= 2;
            model.SourceAreaHeight /= 2;
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

        var xPercent = model.PictureWidth > 1 ? (int)(100.0 * model.MousePosX / model.PictureWidth) : 0;
        var yPercent = model.PictureHeight > 1 ? (int)(100.0 * model.MousePosY / model.PictureHeight) : 0;
        model.Status.Text = $"X: {xPercent}%, Y: {yPercent}%";
    }
}