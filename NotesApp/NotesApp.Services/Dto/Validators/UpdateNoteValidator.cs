using FluentValidation;
using NotesApp.Domain.Interfaces;

namespace NotesApp.Services.Dto.Validators
{
    public class UpdateNoteValidator : AbstractValidator<UpdateNoteDto>
    {
        public UpdateNoteValidator(INoteRepository notesRepository)
        {
            RuleFor(x => x.NoteName)
                .NotEmpty()
                .MaximumLength(40);

            RuleFor(x => x.Content)
                .NotEmpty();

            RuleFor(x => x)
                .Custom((value, context) =>
                {
                    var notes = notesRepository.GetAll(n => n.NoteName == value.NoteName && n.Id != value.Id);

                    if (notes.Count > 0)
                        context.AddFailure("NoteName", $"Note with that name already exists");
                });

            RuleFor(x => x.Tags)
                .Custom((value, context) =>
                {
                    if (value.Where(tag => tag.TagName.Length > 10).Count() > 0)
                        context.AddFailure("Tags", $"Tag name shouldn't be longer than 10 characters");

                    var namesDict = new Dictionary<string, int>();
                    foreach (var tag in value)
                    {
                        if (!namesDict.ContainsKey(tag.TagName))
                            namesDict.Add(tag.TagName, 1);
                        else
                            namesDict[tag.TagName]++;
                    }

                    if (namesDict.Where(kvp => kvp.Value > 1).Count() > 0)
                        context.AddFailure("Tags", $"Tag names shouldn't be duplicated");
                });
        }
    }
}
