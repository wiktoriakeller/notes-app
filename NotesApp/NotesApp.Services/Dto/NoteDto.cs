using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Services.Dto
{
    public class NoteDto
    {
        public string NoteName { get; set; }
        public string Content { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
