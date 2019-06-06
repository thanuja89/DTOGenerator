﻿using Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace DTGen.Tests
{
    public class GeneratorTests
    {
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
@"export class Model {
    public id: number[];
}");

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
@"export interface Model {
    public id: number[];
}");

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
}";

            // Act
            var result = await _sut.GenerateAsync(source);

            // Assert

            string expected = IgnoreExtraWhiteSpace(
@"export class Model {
    public id: number[];
}");

            string actual = IgnoreExtraWhiteSpace(result);

            Assert.Equal(expected
                , actual);
        }

        private void SetUp()
        {
            _sut = new Generator(new GenOptions()
            {
                IsCamelCaseEnabled = true,
                Language = Language.TypeScript
            });
        }

        private void SetUp(GenOptions options)
        {
            _sut = new Generator(options);
        }

        private string IgnoreExtraWhiteSpace(string text)
        {
            return Regex.Replace(text, @"[\s\t\n\r]+", " ");
        }
    }
}
