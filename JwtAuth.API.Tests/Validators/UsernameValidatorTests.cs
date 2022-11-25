using JwtAuth.API.Validation;

namespace JwtAuth.API.Tests.Validators
{
    public class UsernameValidatorTests
	{
		[Fact]
        public void WhenCallingUsernameValidatorWithEmptyUsernameShouldReturnEmptyMessageError()
        {
            // Arrange
            var validator = new UsernameValidator();
            var username = string.Empty;

            // Act
            var sut = validator.Validate(username);

            // Assert
            Assert.Equal(1, sut.Errors?.Count);
            Assert.Equal("Username can't be empty.", sut.Errors?.SingleOrDefault()?.ErrorMessage);
        }
    }
}