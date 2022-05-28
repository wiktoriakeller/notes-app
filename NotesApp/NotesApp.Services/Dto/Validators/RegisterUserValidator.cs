using FluentValidation;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto.Validators.Extensions;

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

            RuleFor(x => x.Email).EmailMustBeUnique(usersRepository);

            RuleFor(x => x.Login).LoginMustBeUnique(usersRepository);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password).WithMessage("Passwords should match");
        }
    }
}
