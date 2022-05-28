using Xunit;
using Moq;
using System;
using NotesApp.Services.Dto;
using NotesApp.Services.Dto.Validators;
using FluentValidation.TestHelper;
using System.Diagnostics.CodeAnalysis;
using NotesApp.Domain.Interfaces;
using NotesApp.Domain.Entities;
using System.Linq.Expressions;
using FluentValidation;

namespace NotesApp.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class RegisterUserValidatorTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IValidator<RegisterUserDto> _validator;

        public RegisterUserValidatorTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(m => m.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
                    .Returns<User?>(null);
            _validator = new RegisterUserValidator(_userRepositoryMock.Object);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("aa", false)]
        [InlineData("aaa", true)]
        [InlineData("login", true)]
        [InlineData("VeryLongLoginThatExceedsTwentyCharacters", false)]
        public void Validate_ForCorrectLoginLength(string login, bool passes)
        {
            var dto = GetValidRegisterUserDto();
            dto.Login = login;
            var result = _validator.TestValidate(dto);
            if(passes)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveValidationErrorFor(e => e.Login);
            }
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("email", false)]
        [InlineData("email@email.com", true)]
        public void Validate_ForCorrectEmail(string email, bool passes)
        {
            var dto = GetValidRegisterUserDto();
            dto.Email = email;
            var result = _validator.TestValidate(dto);
            if (passes)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveValidationErrorFor(e => e.Email);
            }
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("@##@%$#", false)]
        [InlineData("...", false)]
        [InlineData("VeryLongNameThatExceeds20Characters", false)]
        [InlineData("name", true)]
        [InlineData("n", true)]
        public void Validate_ForCorrectName(string name, bool passes)
        {
            var dto = GetValidRegisterUserDto();
            dto.Name = name;
            var result = _validator.TestValidate(dto);
            if (passes)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveValidationErrorFor(e => e.Name);
            }
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("@@@@@!!!", false)]
        [InlineData("....", false)]
        [InlineData("VeryLongSurnameThatExceeds20Characters", false)]
        [InlineData("surname", true)]
        [InlineData("s", true)]
        public void Validate_ForCorrectSurname(string surname, bool passes)
        {
            var dto = GetValidRegisterUserDto();
            dto.Surname = surname;
            var result = _validator.TestValidate(dto);
            if (passes)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveValidationErrorFor(e => e.Surname);
            }
        }


        [Theory]
        [InlineData("", "", false)]
        [InlineData("pas", "pas", false)]
        [InlineData("longerPassword", "", false)]
        [InlineData("VeryLongPasswordThatExceeds20Characters", "VeryLongPasswordThatExceeds20Characters", false)]
        [InlineData("Password", "Password", false)]
        [InlineData("Password123@@", "Password123@@", true)]
        public void Validate_ForCorrectPasswords(string password, string confirm, bool passes)
        {
            var dto = GetValidRegisterUserDto();
            dto.Password = password;
            dto.ConfirmPassword = confirm;
            var result = _validator.TestValidate(dto);
            if (passes)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveAnyValidationError();
            }
        }

        [Fact]
        public void Validate_ForUniqueLoginAndEmail_ReturnsFailure()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(m => m.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
                    .Returns(new User { });
            var validator = new RegisterUserValidator(userRepositoryMock.Object);

            var dto = GetValidRegisterUserDto();
            var result = validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(e => e.Login);
            result.ShouldHaveValidationErrorFor(e => e.Email);
        }

        private RegisterUserDto GetValidRegisterUserDto()
        {
            return new RegisterUserDto
            {
                Login = "login",
                Email = "email@email.com",
                Name = "name",
                Surname = "surname",
                Password = "Password@@123",
                ConfirmPassword = "Password@@123"
            };
        }
    } 
}
