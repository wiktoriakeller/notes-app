using FluentValidation;
using NotesApp.Services.Dto.Validators.Extensions;

namespace NotesApp.Services.Dto.Validators
{
    public class UpdateNoteValidator : AbstractValidator<UpdateNoteDto>
    {
        public UpdateNoteValidator()
        {
            RuleFor(x => x.NoteName)
                .NotEmpty()
                .MaximumLength(40);

            RuleFor(x => x.Content)
                .NotEmpty();

            RuleFor(x => x.Tags.Select(t => t.TagName))
                .TagsMustBeUnique()
                .ForEach(n => n.MaximumLength(10));
        }
    }
}
