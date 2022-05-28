using Xunit;
using FluentValidation;
using System.Collections.Generic;
using NotesApp.Services.Dto;
using NotesApp.Services.Dto.Validators;
using FluentValidation.TestHelper;
using Xunit.Abstractions;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace NotesApp.Tests.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateNoteValidatorTests
    {
        private static IEnumerable<object[]> GetValidNotes()
        {
            yield return new object[]
            {
                new CreateNoteDto
                {
                    NoteName = "Note 5",
                    Content = "lorem ipsum",
                    ImageLink = "",
                    Tags = new List<CreateTagDto> 
                    { 
                        new CreateTagDto { TagName = "Tag" }, 
                        new CreateTagDto { TagName = "Tag 2"}
                    }
                }
            };
            yield return new object[]
            {
                new CreateNoteDto
                {
                    NoteName = "Note 2",
                    Content = "lorem ipsum dorom",
                    ImageLink = "",
                    Tags = new List<CreateTagDto>
                    {
                        new CreateTagDto { TagName = "Tag" },
                        new CreateTagDto { TagName = "Tag 2"}
                    }
                }
            };
        }

        private static IEnumerable<object[]> GetNotesWithTestResult()
        {
            yield return new object[]
            {
                new CreateNoteDto
                {
                    NoteName = "Note 5",
                    Content = "lorem ipsum",
                    ImageLink = "",
                    Tags = new List<CreateTagDto>
                    {
                        new CreateTagDto { TagName = "Tag"},
                        new CreateTagDto { TagName = "Tag"}
                    }
                },
                false
            };
            yield return new object[]
            {
                new CreateNoteDto
                {
                    NoteName = "Note 2",
                    Content = "lorem ipsum dorom",
                    ImageLink = "",
                    Tags = new List<CreateTagDto>
                    {
                        new CreateTagDto { TagName = "Tag" },
                        new CreateTagDto { TagName = "Tag 2"}
                    }
                },
                true
            };
        }

        private CreateNoteDto GetValidNote()
        {
            return new CreateNoteDto
            {
                NoteName = "Note 5",
                Content = "lorem ipsum",
                ImageLink = "",
                Tags = new List<CreateTagDto>
                {
                    new CreateTagDto { TagName = "Tag" },
                    new CreateTagDto { TagName = "Tag 2"}
                }
            };
        }

        private readonly IValidator<CreateNoteDto> _validator;
        private readonly ITestOutputHelper _testOutputHelper;

        public CreateNoteValidatorTests(ITestOutputHelper testOutputHelper)
        {
            _validator = new CreateNoteValidator();
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [MemberData(nameof(GetValidNotes))]
        public void Validate_ForCorrectModel_ReturnsSuccess(CreateNoteDto dto)
        {
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("not empty", true)]
        public void Validate_ForContent(string content, bool passes)
        {
            var note = GetValidNote();
            note.Content = content;
            var result = _validator.TestValidate(note);
            
            if(!passes)
            {
                result.ShouldHaveValidationErrorFor(n => n.Content).Only();
            }
            else
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
        }

        [Theory]
        [InlineData("bardzo duzy string string string string string string string", false)]
        [InlineData("title", true)]
        public void Validate_ForTitle(string name, bool passes)
        {
            var note = GetValidNote();
            note.NoteName = name;
            var result = _validator.TestValidate(note);

            if (!passes)
            {
                result.ShouldHaveValidationErrorFor(n => n.NoteName).Only();
            }
            else
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
        }

        [Theory]
        [MemberData(nameof(GetNotesWithTestResult))]
        public void Validate_ForTags(CreateNoteDto dto, bool passes)
        {
            var result = _validator.Validate(dto);

            if (!passes)
            {
                result.Errors.Should().OnlyContain(e => e.ErrorMessage == "Tags in note should be unique");
            }
            else
            {
                result.Errors.Should().BeEmpty();
            }
        }
    }
}
