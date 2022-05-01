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

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _notesService.GetAllNotes();
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto noteDto)
        {
            var id = await _notesService.AddNote(noteDto);
            return Created($"/note/{id}", null);
        }
    }
}
