using Exercise;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTests
{
    public class ValidatorTest
    {
        private List<List<DateRange>> _rangeList = new List<List<DateRange>>()
        {
            new List<DateRange>
            {
                new DateRange(new DateTime(2020, 1, 1), new DateTime(2020, 1, 15)),
                new DateRange(new DateTime(2020, 2, 1), new DateTime(2020, 2, 15))
            },
            new List<DateRange>
            {
                new DateRange(new DateTime(2020, 1, 15), new DateTime(2020, 1, 25))
            },
            new List<DateRange>
            {
                new DateRange(new DateTime(2020, 1, 8), new DateTime(2020, 1, 25))
            },
            new List<DateRange>
            {
                new DateRange(new DateTime(2020, 1, 12), new DateTime(2020, 1, 14))
            },
    };


        [Theory]
        //[InlineData(0)]
        //[InlineData(1)]
        //[InlineData(2)]
        [InlineData(3)]
        public void ValidatorOverlapping_ForOverlappingDataRanges_ResturnsFalse(int index)
        {
            // Arrange
            List<DateRange> ranges = _rangeList[index];

            DateRange input = new DateRange(new DateTime(2020, 1, 10), new DateTime(2020, 1, 20));

            Validator validator = new Validator();

            // Act

            var result = validator.ValidateOverlapping(ranges, input);

            // Assert 

            result.Should().BeFalse();
        }
    }
}
