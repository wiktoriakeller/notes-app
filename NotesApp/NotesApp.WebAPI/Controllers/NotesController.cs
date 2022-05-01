using Microsoft.AspNetCore.Mvc;
using NotesApp.Services.Interfaces;
using NotesApp.Services.Dto;

namespace NotesApp.WebAPI.Controllers
{
    [Route("note/")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var note = await _notesService.GetNoteById(id);
            
            if(note == null)
                return NotFound();

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteDto noteDto)
        {
            var id = _notesService.AddNote(noteDto);
            return Created($"/note/{id}", null);
        }
    }
}
