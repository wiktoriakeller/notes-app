using Microsoft.AspNetCore.Authorization;

namespace NotesApp.Services.Authorization
{
    public enum Operation
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public Operation Operation { get; }

        public ResourceOperationRequirement(Operation operation)
        {
            Operation = operation;
        }
    }
}
