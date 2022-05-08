using Microsoft.AspNetCore.Authorization;
using NotesApp.Domain.Entities;
using System.Security.Claims;

namespace NotesApp.Services.Authorization
{
    public class NotesAuthorizationHandler : AuthorizationHandler<ResourceOperationRequirement, object>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, object noteObj)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (noteObj is IEnumerable<Note>)
            {
                var notes = (IEnumerable<Note>) noteObj;
                var forbidden = false;

                foreach (var note in notes)
                {
                    if(note.UserId != userId)
                    {
                        forbidden = true;
                        break;
                    }
                }

                if (!forbidden)
                    context.Succeed(requirement);
            }
            else if(noteObj is Note)
            {
                var note = (Note) noteObj;
                
                if (requirement.Operation == Operation.Create)
                    context.Succeed(requirement);

                if (note.UserId == userId)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
