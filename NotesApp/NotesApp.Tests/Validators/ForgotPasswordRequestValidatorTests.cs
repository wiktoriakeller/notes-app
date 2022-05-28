using Xunit;
using Moq;
using FluentValidation;
using System.Collections.Generic;
using NotesApp.Services.Dto;
using NotesApp.Services.Dto.Validators;
using FluentValidation.TestHelper;
using Xunit.Abstractions;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using NotesApp.Domain.Interfaces;
using System;
using NotesApp.Domain.Entities;
using System.Linq.Expressions;

namespace NotesApp.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class ForgotPasswordRequestValidatorTests
    {
        [Theory]
        [InlineData("email", false)]
        [InlineData("email@.com", false)]
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

        [Fact]
        public void Validate_ForExistingEmail_ReturnsFailure( )
        {
            var email = "email@email.com";
            var userRepositoryMock = new Mock<IUserRepository>();
            var validator = new ForgotPasswordRequestValidator(userRepositoryMock.Object);

            userRepositoryMock.Setup(m => m.GetFirstOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns<User?>(null);

            var result = validator.TestValidate(new ForgotPasswordRequestDto { Email = email });
            result.ShouldHaveValidationErrorFor(x => x.Email).Only();
        }
    }
}
