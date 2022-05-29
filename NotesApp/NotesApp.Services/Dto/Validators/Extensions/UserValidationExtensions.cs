using FluentValidation;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Services.Dto.Validators.Extensions
{
    public static class UserValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> EmailMustBeUnique<T>(this IRuleBuilder<T, string> ruleBuilder, 
            IUserRepository usersRepository)
        {
            return ruleBuilder.Must((rootObject, email, context) =>
            {
                var emailInUse = usersRepository.GetFirstOrDefault(u => u.Email == email) != null;
                if (emailInUse)
                    return false;

                return true;
            })
            .WithMessage("Email is already taken");
        }

        public static IRuleBuilderOptions<T, string> LoginMustBeUnique<T>(this IRuleBuilder<T, string> ruleBuilder,
            IUserRepository usersRepository)
        {
            return ruleBuilder.Must((rootObject, login, context) =>
            {
                var loginInUse = usersRepository.GetFirstOrDefault(u => u.Login == login) != null;
                if (loginInUse)
                    return false;

                return true;
            })
            .WithMessage("Login is already taken");
        }
    }
}
