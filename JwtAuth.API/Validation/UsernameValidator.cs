using FluentValidation;

namespace JwtAuth.API.Validation
{
    public class UsernameValidator : AbstractValidator<string>
    {
		public UsernameValidator()
		{
            RuleFor(x => x).NotEmpty().WithMessage("Username can't be empty.");
        }
	}
}