using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NotesApp.Domain.Interfaces;
using NotesApp.Domain.Entities;

namespace NotesApp.Services.Dto.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginDto>
    {
        public LoginUserValidator(IUserRepository usersRepository, IPasswordHasher<User> _passwordHasher)
        {
            RuleFor(x => x.Login)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20);
        
            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    var users = usersRepository.GetAllWhere(u => u.Login == dto.Login);
                    var failedAuthentication = false;

                    if(users.Count == 0)
                        failedAuthentication = true;

                    var user = users.First();
                    if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
                        failedAuthentication = true;

                    if (failedAuthentication)
                        context.AddFailure("Provided username or password is invalid");
                });
        }
    }
}
