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
            SetUp(new GenOptions()
            {
                IsCamelCaseEnabled = true,
                IsMapToInterfaceEnabled = true,
                Language = Language.TypeScript
            });

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
            SetUp(new GenOptions()
            {
                IsCamelCaseEnabled = true,
                Language = Language.TypeScript
            });
        }

        private void SetUp(GenOptions options)
        {
            _propText = "public id: number[];";

            var clsOrInt = options.IsMapToInterfaceEnabled ? "interface" : "class";

            _containerText = $"export {clsOrInt} Model {{ public id: number[]; }}";

            _mockLanguageService = new Mock<ILanguageService>();

            _mockLanguageService.Setup(m => m.GetPropertyDeclaration(It.IsAny<string>(), It.IsAny<string>())).Returns(_propText);
            _mockLanguageService.Setup(m => m.GetContainerDeclaration(It.IsAny<string>(), It.IsAny<string>())).Returns(_containerText);

            _sut = new Generator(options, _mockLanguageService.Object);
        }

        private string IgnoreExtraWhiteSpace(string text)
        {
            return Regex.Replace(text, @"[\s\t\n\r]+", " ");
        }
    }
}
