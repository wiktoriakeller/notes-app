using FluentValidation;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Services.Dto.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(IUserRepository usersRepository)
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(20)
                .Matches(@"[A-Za-z]*");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(20)
                .Matches(@"[A-Za-z]*");

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = usersRepository.GetFirstOrDefault(u => u.Email == value) != null;

                    if (emailInUse)
                        context.AddFailure("Email", $"Email is already");
                });

            RuleFor(x => x.Login)
                .Custom((value, context) =>
                {
                    var loginInUse = usersRepository.GetFirstOrDefault(u => u.Login == value) != null;

                    if (loginInUse)
                        context.AddFailure("Login", $"Login is already taken");
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password).WithMessage("Passwords should match");
        }
    }
}
