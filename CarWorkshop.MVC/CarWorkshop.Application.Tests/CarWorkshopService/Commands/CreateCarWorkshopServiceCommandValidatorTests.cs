using CarWorkshop.Application.CarWorkshopService.Commands;
using FluentValidation.TestHelper;

namespace CarWorkshop.Application.Tests.CarWorkshopService.Commands
{
    public class CreateCarWorkshopServiceCommandValidatorTests
    {
        [Fact]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange 

            var validator = new CreateCarWorkshopServiceCommandValidator();

            var command = new CreateCarWorkshopServiceCommand()
            {
                Cost = "100 PLN",
                Description = "Description",
                CarWorkshopEncodedName = "workshop1"
            };

            // act

            var result = validator.TestValidate(command);

            // assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithInvalidCommand_ShouldHaveValidationError()
        {
            // arrange 

            var validator = new CreateCarWorkshopServiceCommandValidator();

            var command = new CreateCarWorkshopServiceCommand()
            {
                Cost = "",
                Description = "",
                CarWorkshopEncodedName = null
            };

            // act

            var result = validator.TestValidate(command);

            // assert

            result.ShouldHaveValidationErrorFor(c => c.Cost);
            result.ShouldHaveValidationErrorFor(c => c.Description);
            result.ShouldHaveValidationErrorFor(c => c.CarWorkshopEncodedName);
        }
    }
}
