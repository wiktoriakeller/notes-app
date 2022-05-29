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
using System.Collections.Generic;

namespace NotesApp.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class ForgotPasswordRequestValidatorTests
    {
        private static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new User {},
                true
            };
            yield return new object[]
            {
                null,
                false
            };
        }

        [Theory]
        [InlineData("email", false)]
        [InlineData("email@email.com", true)]
        public void Validate_ForCorrectEmail(string email, bool passes)
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new ForgotPasswordRequestValidator(userRepositoryMock.Object);

            userRepositoryMock.Setup(m => m.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new User { });

            var result = validator.TestValidate(new ForgotPasswordRequestDto { Email = email});

            if(passes)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveValidationErrorFor(x => x.Email).Only();
            }
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void Validate_ForExistingEmail(User? user, bool passes)
        {
            var email = "email@email.com";
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new ForgotPasswordRequestValidator(userRepositoryMock.Object);

            userRepositoryMock.Setup(m => m.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user);

            var result = validator.TestValidate(new ForgotPasswordRequestDto { Email = email });
            if(passes)
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
            else
            {
                result.ShouldHaveValidationErrorFor(x => x.Email).Only();
            }
        }
    }
}
