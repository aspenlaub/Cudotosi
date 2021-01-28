namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces {
    public interface ICutCalculator {
        void CutOut(ICudotosiApplicationModel model);
        void CutOut(ICudotosiApplicationModel model, int imageWidth, int imageHeight, out int x, out int y, out int width, out int height);
        void TargetSize(ICudotosiApplicationModel model, int imageWidth, int imageHeight, out int targetWidth, out int targetHeight);
    }
}
