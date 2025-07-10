using Exercise;
using FluentAssertions;
using Xunit;

namespace ExerciseTests
{
    public class ReverseHelperTests
    {
        [Fact]
        public void ReverseWords_ForString_ReturnsReversedWords()
        {
            // Arrange

            string text = "Ala ma kota";

            // Act

            var result = StringHelper.ReverseWords(text);

            // Assert

            result.Should().Be("kota ma Ala");
        }

        [Theory]
        [InlineData("kajak", true)]
        [InlineData("abs", false)]
        [InlineData("aba", true)]
        [InlineData("ajk", false)]
        [InlineData("true", false)]
        public void IsPalindrome_CorrectString_ReturnTrue(string text, bool expected)
        {
            // Arrange

            // Act

            var result = StringHelper.IsPalindrome(text);

            // Assert

            result.Should().Be(expected);
        }
    }
}
