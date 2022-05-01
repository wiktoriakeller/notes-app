using NotesApp.Services.Interfaces;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto;
using AutoMapper;

namespace NotesApp.Services.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;
        private readonly IMapper _mapper;

        public NotesService(INotesRepository notesRepository, IMapper mapper)
        {
            _notesRepository = notesRepository;
            _mapper = mapper;
        }

        public async Task AddNote(CreateNoteDto noteDto)
        {
            var note = _mapper.Map<Note>(noteDto);
            await _notesRepository.Add(note);
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            return await _notesRepository.GetAllWithTags();
        }

        public async Task<IEnumerable<Note>> GetNotesByName(string name)
        {
            name = name.ToLower().Trim();
            return await _notesRepository.GetAllWithTagsWhere(n => n.NoteName.ToLower().Contains(name));
        }

        public async Task<IEnumerable<Note>> GetNotesByContent(string content)
        {
            content = content.ToLower().Trim();
            return await _notesRepository.GetAllWithTagsWhere(n => n.Content.ToLower().Contains(content));
        }

        public async Task<IEnumerable<Note>> GetNotesByTags(IEnumerable<string> tags)
        {
            var searchedNotes = new List<Note>();
            var notes = await _notesRepository.GetAllWithTags();
        
            foreach (var note in notes)
            {
                var tagNames = note.Tags.Select(t => t.TagName.ToLower());
                foreach (var tagName in tagNames)
                {
                    if(tags.Any(t => t.ToLower() == tagName))
                        notes.Add(note);
                }
            }

            return notes;
        }
    }
}
