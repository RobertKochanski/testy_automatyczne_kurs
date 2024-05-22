using Exercise;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTests
{
    public class ListHelperTests
    {
        [Fact]
        public void FilterOddNumber_IntigerList_ReturnOddNumbers()
        {
            // Arrange

            var list = new List<int>() { 1, 2, 3, 3, 2, 1, 2, 2, 3, 12, 11, 52, 32, 31};

            // Act

            var result = ListHelper.FilterOddNumber(list);

            // Assert

            Assert.Equal(result, new List<int>() { 1, 3, 3, 1, 3, 11, 31 });
        }

    }
}
