namespace ShoppingLikeFlies.Api.Security.Validators
{
    public class RegistrationValidator : AbstractValidator<RegisterRequest>
    {
        public RegistrationValidator()
        {
            RuleFor(x => x.username).NotNull().NotEmpty().MinimumLength(5).MaximumLength(20);
            RuleFor(x => x.password).NotNull().NotEmpty().MinimumLength(8);

            RuleFor(x => x.username).Must((model, un) => !un.Any(c => !char.IsLetterOrDigit(c))).WithMessage("Username must not contain special characters");


            RuleFor(x => x.password).Must((model, pw) => pw.Any(c => char.IsLetterOrDigit(c)));
            RuleFor(x => x.password).Must((model, pw) => pw.Any(c => char.IsLower(c)));
            RuleFor(x => x.password).Must((model, pw) => pw.Any(c => char.IsUpper(c)));
            RuleFor(x => x.password).Must((model, pw) => pw.Any(c => !char.IsLetterOrDigit(c)));

            RuleFor(x => x.passwordConfirm).NotNull().NotEmpty().MinimumLength(8);

            RuleFor(x => x.passwordConfirm).Must((model, pwConf) => model.password == pwConf).WithMessage("Passwords must match.");

            RuleFor(x => x.firstname).NotNull().NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(x => x.lastname).NotNull().NotEmpty().MinimumLength(3).MaximumLength(20);

            RuleFor(x => x.firstname).Must((model, fn) => !fn.Any(c => !char.IsLetter(c)));
            RuleFor(x => x.lastname).Must((model, fn) => !fn.Any(c => !char.IsLetter(c)));
        }
    }
}
