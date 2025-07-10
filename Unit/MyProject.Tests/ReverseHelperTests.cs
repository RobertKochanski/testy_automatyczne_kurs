using Exercise;
using FluentAssertions;
using Xunit;

namespace MyProject.Tests
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
    }
}
