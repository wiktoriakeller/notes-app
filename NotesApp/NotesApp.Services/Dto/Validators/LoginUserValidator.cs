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
                .NotEmpty();
        
            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    var user = usersRepository.GetFirstOrDefault(u => u.Login == dto.Login);
                    var failedAuthentication = false;

                    if(user == null)
                        failedAuthentication = true;

                    if (user != null && _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
                        failedAuthentication = true;

                    if (failedAuthentication)
                        context.AddFailure("WrongCredentials", "Login or password is incorrect");
                });
        }
    }
}
