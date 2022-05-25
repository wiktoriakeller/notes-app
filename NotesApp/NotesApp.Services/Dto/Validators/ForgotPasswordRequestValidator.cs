using FluentValidation;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Services.Dto.Validators
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        public ForgotPasswordRequestValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var user = userRepository.GetFirstOrDefault(u => u.Email == value);

                    if (user == null)
                        context.AddFailure("Email", "There is no account with that email");
                });
        }
    }
}
