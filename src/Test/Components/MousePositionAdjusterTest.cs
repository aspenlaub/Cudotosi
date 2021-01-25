using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Components;
using Aspenlaub.Net.GitHub.CSharp.Cudotosi.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Aspenlaub.Net.GitHub.CSharp.Cudotosi.Test.Components {
    [TestClass]
    public class MousePositionAdjusterTest {

        [TestMethod]
        public void CanAdjustMousePosition() {
            TestScenario(1, 1, 4, 4, true, true, 0, 0, 4, 4);
            TestScenario(3, 5, 4, 8, true, false, 2, 4, 2, 4);
            TestScenario(3, 5, 8, 4, true, false, 3, 2, 4, 2);
            TestScenario(5, 3, 8, 4, true, false, 4, 2, 4, 2);
            TestScenario(1, 3, 4, 8, false, true, 0, 3, 4, 4);
            TestScenario(1, 3, 8, 4, false, false, 1, 2, 2, 2);
            TestScenario(-1, -3, 8, 4, false, false, 0, 0, 2, 2);
        }

        protected void TestScenario(int mousePosX, int mousePosY, int pictureWidth, int pictureHeight, bool asIs, bool hundredPercent,
                int expectedMousePosX, int expectedMousePosY, int expectedSourceAreaWidth, int expectedSourceAreaHeight) {
            var modelMock = new Mock<ICudotosiApplicationModel>();
            modelMock.SetupAllProperties();
            var model = modelMock.Object;
            model.MousePosX = mousePosX;
            model.MousePosY = mousePosY;
            model.PictureWidth = pictureWidth;
            model.PictureHeight = pictureHeight;

            var shapeAsIsButton = new ToggleButton("Shape") { IsChecked = asIs };
            modelMock.SetupGet(m => m.DestinationShapeAsIs).Returns(shapeAsIsButton);
            var shapeSquareButton = new ToggleButton("Shape") { IsChecked = !asIs };
            modelMock.SetupGet(m => m.DestinationShapeSquare).Returns(shapeSquareButton);

            var transForm100Button = new ToggleButton("Shape") { IsChecked = hundredPercent };
            modelMock.SetupGet(m => m.TransformHowManyPercent100).Returns(transForm100Button);
            var transForm50Button = new ToggleButton("Shape") { IsChecked = !hundredPercent };
            modelMock.SetupGet(m => m.TransformHowManyPercent50).Returns(transForm50Button);

            var textBox = new TextBox();
            modelMock.SetupGet(m => m.Status).Returns(textBox);

            var sut = new MousePositionAdjuster();
            sut.AdjustMousePosition(model);
            Assert.AreEqual(expectedMousePosX, model.MousePosX);
            Assert.AreEqual(expectedMousePosY, model.MousePosY);
            Assert.AreEqual(expectedSourceAreaWidth, model.SourceAreaWidth);
            Assert.AreEqual(expectedSourceAreaHeight, model.SourceAreaHeight);
        }
    }
}
