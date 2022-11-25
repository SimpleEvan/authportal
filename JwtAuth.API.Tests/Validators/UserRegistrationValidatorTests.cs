using JwtAuth.API.APIModels;
using JwtAuth.API.Validation;

namespace JwtAuth.API.Tests.Validators
{
    public class UserRegistrationValidatorTests
	{
        [Fact]
        public void WhenCallingUserRegistrationValidatorWithEmptyUsernameShouldReturnEmptyMessageError()
        {
            // Arrange
            var validator = new UserRegistrationValidator();
            var userRequest = new UserRequest
            {
                Username = string.Empty,
                Password = "Password1234!"
           };

            // Act
            var sut = validator.Validate(userRequest);

            // Assert
            Assert.Equal(1, sut.Errors?.Count);
            Assert.Equal("Please specify a username", sut.Errors?.SingleOrDefault()?.ErrorMessage);
        }

        [Fact]
        public void WhenCallingUserRegistrationValidatorWithoutCorrectPasswordFormatShouldReturnPasswordErrorMessages()
        {
            // Arrange
            var validator = new UserRegistrationValidator();
            var userRequest = new UserRequest
            {
                Username = "Username",
                Password = string.Empty
            };

            // Act
            var sut = validator.Validate(userRequest);

            // Assert
            Assert.Equal(4, sut.Errors?.Count);
            Assert.Equal("Password needs to have at least 8 characters.", sut.Errors?[0].ErrorMessage);
            Assert.Equal("Your password must contain at least one uppercase letter.", sut.Errors?[1]?.ErrorMessage);
            Assert.Equal("Your password must contain at least one number.", sut.Errors?[2]?.ErrorMessage);
            Assert.Equal("Your password must contain at least one special character.", sut.Errors?[3]?.ErrorMessage);
        }
    }
}