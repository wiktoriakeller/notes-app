using FluentValidation;

namespace NotesApp.Services.Dto.Validators.Extensions
{
    public static class NoteValidatorExtensions
    {
        public static IRuleBuilderOptions<T, IEnumerable<string>> TagsMustBeUnique<T>(this IRuleBuilder<T, IEnumerable<string>> ruleBuilder)
        {
            return ruleBuilder.Must((rootObject, tags, context) =>
            {
                var namesDict = new Dictionary<string, int>();
                foreach (var tag in tags)
                {
                    if (!namesDict.ContainsKey(tag))
                        namesDict.Add(tag, 1);
                    else
                        namesDict[tag]++;
                }

                if (namesDict.Where(kvp => kvp.Value > 1).Any())
                    return false;

                return true;
            });
        }
    }
}
