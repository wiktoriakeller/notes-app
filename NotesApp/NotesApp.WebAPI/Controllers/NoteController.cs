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

        [HttpGet("{hashId}")]
        public async Task<IActionResult> GetNote([FromRoute] string hashId)
        {
            var note = await _notesService.GetNoteById(hashId);
            return Ok(note);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes([FromQuery] NoteQuery query)
        {
            var notes = await _notesService.GetNotes(query);
            return Ok(notes);
        }

        [HttpGet("public/{hashId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPublicNote([FromRoute] string hashId)
        {
            var note = await _notesService.GetPublicNote(hashId);
            return Ok(note);
        }

        [HttpPatch("{hashId}")]
        public async Task<IActionResult> GeneratePublicLink([FromBody] CreatePublicLinkDto dto, [FromRoute] string hashId)
        {
            var linkDto = await _notesService.GeneratePublicLink(dto, hashId);

            if (linkDto.PublicHashId != string.Empty)
                return Ok(linkDto);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            var hashId = await _notesService.AddNote(dto);
            return Created($"notes-api/notes/{hashId}", null);
        }

        [HttpPut("{hashId}")]
        public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteDto dto, string hashId)
        {
            var note = await _notesService.UpdateNote(dto, hashId);
            return Ok(note);
        }

        [HttpDelete("{hashId}")]
        public async Task<IActionResult> DeleteNote([FromRoute] string hashId)
        {
            await _notesService.DeleteNote(hashId);
            return Ok();
        }
    }
}
