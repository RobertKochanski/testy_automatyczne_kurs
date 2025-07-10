using CarWorkshop.Domain.Entities;
using FluentAssertions;

namespace CarWorkshow.Domain.Tests.Entities
{
    public class CarWorkshopTests
    {
        [Fact]
        public void EncodeName_ShouldSetEncodeName()
        {
            // arrange 

            var carWorkshop = new CarWorkshopEntity();
            carWorkshop.Name = "Name workshop";

            // act 

            carWorkshop.EncodeName();

            // assert

            carWorkshop.EncodedName.Should().Be("name-workshop");
        }

        [Fact]
        public void EncodeName_ShouldThrowException_WhenNameIsNull()
        {
            // arrange 

            var carWorkshop = new CarWorkshopEntity();

            // act 

            Action action = () => carWorkshop.EncodeName();

            // assert

            action.Invoking(a => a.Invoke()).Should().Throw<NullReferenceException>();
        }
    }
}
