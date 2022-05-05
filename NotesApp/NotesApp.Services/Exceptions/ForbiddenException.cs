using System;

namespace NotesApp.Services.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {

        }
    }
}
