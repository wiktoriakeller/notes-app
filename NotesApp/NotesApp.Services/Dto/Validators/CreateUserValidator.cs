using FluentValidation;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Services.Dto.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator(IUserRepository usersRepository)
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
                .Matches(@"[A-Za-z]*");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .Matches(@"[A-Za-z]*");

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var users = usersRepository.GetAll();
                    var emailInUse = users.Any(u => u.Email == value);

                    if (emailInUse)
                        context.AddFailure("Email", $"Email {value} is already in use");
                });

            RuleFor(x => x.Login)
                .Custom((value, context) =>
                {
                    var users = usersRepository.GetAll();
                    var loginInUse = users.Any(u => u.Login == value);

                    if (loginInUse)
                        context.AddFailure("Login", $"Login {value} is already in use");
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
        }
    }
}
