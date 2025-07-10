using Xunit;
using FluentAssertions;
using Moq;
using Xunit.Abstractions;

namespace MyProject.Tests
{
    public class BmiCalculatorFacadeTests
    {
        public BmiCalculatorFacadeTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private const string OVERWEIGHT_SUMMARY = "You are a bit overweight";
        private const string NORMAL_SUMMARY = "Your weight is normal, keep it up";
        private readonly ITestOutputHelper _testOutputHelper;

        [Theory]
        [InlineData(BmiClassification.Overweight, OVERWEIGHT_SUMMARY)]
        [InlineData(BmiClassification.Normal, NORMAL_SUMMARY)]
        public void GetResult_ForValidInputs_ReturnsCorrectSummary(BmiClassification bmiClassification, string expectedResult)
        {
            // arrange
            var bmiDeterminatorMock = new Mock<IBmiDeterminator>();

            bmiDeterminatorMock.Setup(m => m.DetermineBmi(It.IsAny<double>()))
                .Returns(bmiClassification);


            var bmiCalculatorFacade = new BmiCalculatorFacade(UnitSystem.Metric, bmiDeterminatorMock.Object);

            // act

            BmiResult result = bmiCalculatorFacade.GetResult(1, 1);

            _testOutputHelper.WriteLine($"For classification: {bmiClassification} the result is: {result.Summary}");

            // assert

            result.Summary.Should().Be(expectedResult);

        }
    }
}
