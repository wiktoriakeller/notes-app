using Microsoft.AspNetCore.Mvc;
using NotesApp.Services.Interfaces;
using NotesApp.Services.Dto;
using Microsoft.AspNetCore.Authorization;

namespace NotesApp.WebAPI.Controllers
{
    [Route("notes-api/notes")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _notesService;

        public NoteController(INoteService notesService)
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
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            var id = await _notesService.AddNote(dto);
            return Created($"/note/{id}", null);
        }
    }
}
