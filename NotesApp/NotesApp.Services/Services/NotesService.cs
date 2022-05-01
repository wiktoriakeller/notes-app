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

        public async Task<NoteDto?> GetNoteById(int id)
        {
            var note = await _notesRepository.GetById(id);
            return _mapper.Map<NoteDto>(note);
        }

        public async Task<IEnumerable<NoteDto>> GetAllNotes()
        {
            var notes = await _notesRepository.GetAllWithTags();
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByName(string name)
        {
            name = name.ToLower().Trim();
            var notes = await _notesRepository.GetAllWithTagsWhere(n => n.NoteName.ToLower().Contains(name));
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByContent(string content)
        {
            content = content.ToLower().Trim();
            var notes = await _notesRepository.GetAllWithTagsWhere(n => n.Content.ToLower().Contains(content));
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByTags(IEnumerable<string> tags)
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

            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<int> AddNote(NoteDto noteDto)
        {
            var note = _mapper.Map<Note>(noteDto);
            await _notesRepository.Add(note);
            return note.Id;
        }
    }
}
