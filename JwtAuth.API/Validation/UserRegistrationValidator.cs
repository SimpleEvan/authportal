using FluentValidation;
using JwtAuth.API.APIModels;

namespace JwtAuth.API.Validation
{
    public class UserRegistrationValidator : AbstractValidator<UserRequest>
    {
		public UserRegistrationValidator()
		{
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please specify a username.");
			RuleFor(x => x.Password).MinimumLength(8).WithMessage("Password needs to have at least 8 characters.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one special character.");
        }
	}
}