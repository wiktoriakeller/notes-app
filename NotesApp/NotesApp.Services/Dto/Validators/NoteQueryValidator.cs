using FluentValidation;

namespace NotesApp.Services.Dto.Validators
{
    public class NoteQueryValidator : AbstractValidator<NoteQuery>
    {
        private int[] allowedPageSizes = new int[] { 20, 30, 50 };

        public NoteQueryValidator()
        {
            RuleFor(p => p.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(p => p.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                    context.AddFailure("PageSize", $"Page size must be in {string.Join(",", allowedPageSizes)}");
            });
        }
    }
}
