using Core;
using Core.Abstractions;
using Moq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace DTGen.Tests
{
    public class GeneratorTests
    {
        private string _propText;
        private string _containerText;
        private Mock<ILanguageService> _mockLanguageService;
        private Generator _sut;

        [Fact]
        public async Task GenerateAsync_WhenCalledWithSingleClass_ReturnsCorrectTSClass()
        {
            // Arrange
            SetUp();

            var source =
@"public class Model
{
    public List<int> Id { get; set; }
}";

            // Act
            var result = await _sut.GenerateAsync(source);

            // Assert

            string expected = IgnoreExtraWhiteSpace(
$@"export class Model {{
    { _propText }
}}");

            string actual = IgnoreExtraWhiteSpace(result);

            Assert.Equal(expected
                , actual);
        }

        [Fact]
        public async Task GenerateAsync_WhenCalledWithSingleClassAndIsMappedToInterfaceEnabled_ReturnsCorrectTSInterface()
        {
            // Arrange
            SetUpWithMapToInterfaceEnabled();

            var source =
@"public class Model
{
    public List<int> Id { get; set; }
}";

            // Act
            var result = await _sut.GenerateAsync(source);

            // Assert

            string expected = IgnoreExtraWhiteSpace(
$@"export interface Model {{
    { _propText }
}}");

            string actual = IgnoreExtraWhiteSpace(result);

            Assert.Equal(expected
                , actual);
        }

        [Fact]
        public async Task GenerateAsync_WhenCalledWithMultipleClasses_ReturnsCorrectTSClass()
        {
            // Arrange
            SetUp();

            var source =
@"public class Model
{
    public List<int> Id { get; set; }
}

public class Model
{
    public List<int> Id { get; set; }
}";

            // Act
            var result = await _sut.GenerateAsync(source);

            // Assert

            string expected = IgnoreExtraWhiteSpace(
$@"export class Model {{
    { _propText }
}}

export class Model {{
    { _propText }
}}");

            string actual = IgnoreExtraWhiteSpace(result);

            Assert.Equal(expected
                , actual);
        }

        private void SetUp()
        {
            _propText = "public id: number[];";

            _containerText = $"export class Model {{ public id: number[]; }}";

            _mockLanguageService = new Mock<ILanguageService>();

            _mockLanguageService.Setup(m => m.GetPropertyDeclaration(It.IsAny<string>(), It.IsAny<string>())).Returns(_propText);
            _mockLanguageService.Setup(m => m.GetContainerDeclaration(It.IsAny<string>(), It.IsAny<string>())).Returns(_containerText);

            _sut = new Generator(new GenOptions()
            {
                IsCamelCaseEnabled = true,
                IsMapToInterfaceEnabled = false,
                Language = Language.TypeScript
            }, _mockLanguageService.Object);
        }

        private void SetUpWithMapToInterfaceEnabled()
        {
            _propText = "public id: number[];";

            _containerText = $"export interface Model {{ public id: number[]; }}";

            _mockLanguageService = new Mock<ILanguageService>();

            _mockLanguageService.Setup(m => m.GetPropertyDeclaration(It.IsAny<string>(), It.IsAny<string>())).Returns(_propText);
            _mockLanguageService.Setup(m => m.GetContainerDeclaration(It.IsAny<string>(), It.IsAny<string>())).Returns(_containerText);

            _sut = new Generator(new GenOptions()
            {
                IsCamelCaseEnabled = true,
                IsMapToInterfaceEnabled = true,
                Language = Language.TypeScript
            }, _mockLanguageService.Object);
        }

        private string IgnoreExtraWhiteSpace(string text)
        {
            return Regex.Replace(text, @"[\s\t\n\r]+", " ");
        }
    }
}
