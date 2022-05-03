using NotesApp.Services.Interfaces;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto;
using AutoMapper;

namespace NotesApp.Services.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _notesRepository;
        private readonly IMapper _mapper;

        public NoteService(INoteRepository notesRepository, IMapper mapper)
        {
            _notesRepository = notesRepository;
            _mapper = mapper;
        }

        public async Task<NoteDto?> GetNoteById(int id)
        {
            var note = await _notesRepository.GetByIdAsync(id);
            return _mapper.Map<NoteDto>(note);
        }

        public async Task<IEnumerable<NoteDto>> GetAllNotes()
        {
            var notes = await _notesRepository.GetAllAsync("Tags");
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByName(string name)
        {
            name = name.ToLower().Trim();
            var notes = await _notesRepository.GetAllAsync(n => n.NoteName.ToLower().Contains(name), "Tags");
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByContent(string content)
        {
            content = content.ToLower().Trim();
            var notes = await _notesRepository.GetAllAsync(n => n.Content.ToLower().Contains(content), "Tags");
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByTags(IEnumerable<string> tags)
        {
            var searchedNotes = new List<Note>();
            var notes = await _notesRepository.GetAllAsync("Tags");
        
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

        public async Task<int> AddNote(CreateNoteDto noteDto)
        {
            var note = _mapper.Map<Note>(noteDto);
            await _notesRepository.AddAsync(note);
            return note.Id;
        }
    }
}
