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
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            var note = await _notesService.GetNoteById(id);
            return Ok(note);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _notesService.GetAllNotes();
            return Ok(notes);
        }

        [HttpGet("public/{hashId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicNote([FromRoute] string hashId)
        {
            var note = await _notesService.GetNoteByHashId(hashId);
            return Ok(note);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateNotePublicLink([FromBody] UpdateNoteHashIdDto dto, [FromRoute] int id)
        {
            var hashId = await _notesService.UpdateHashId(dto, id);

            if(hashId != string.Empty)
                return Created($"notes-api/notes/public/{hashId}", null);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            var id = await _notesService.AddNote(dto);
            return Created($"notes-api/notes/{id}", null);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteDto dto)
        {
            var note = await _notesService.UpdateNote(dto);
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            await _notesService.DeleteNote(id);
            return Ok();
        }
    }
}
