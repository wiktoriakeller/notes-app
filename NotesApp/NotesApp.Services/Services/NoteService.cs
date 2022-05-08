using NotesApp.Services.Interfaces;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using NotesApp.Services.Authorization;
using NotesApp.Services.Exceptions;

namespace NotesApp.Services.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _notesRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public NoteService(INoteRepository notesRepository, 
            IMapper mapper,
            IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _notesRepository = notesRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public async Task<NoteDto?> GetNoteById(int id)
        {
            var note = await _notesRepository.GetByIdAsync(id);
            await CheckAuthorization(note);

            if (note == null)
                throw new NotFoundException($"Resource with id: {id} couldn't be found");

            return _mapper.Map<NoteDto>(note);
        }

        public async Task<IEnumerable<NoteDto>> GetAllNotes()
        {
            var userId = GetUserId();
            var notes = await _notesRepository.GetAllAsync(n => n.UserId == userId,"Tags");
            await CheckAuthorization(notes);
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByName(string name)
        {
            name = name.ToLower().Trim();
            var userId = GetUserId();
            var notes = await _notesRepository.GetAllAsync(n => n.NoteName.ToLower().Contains(name) && n.UserId == userId, "Tags");
            await CheckAuthorization(notes);
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByContent(string content)
        {
            content = content.ToLower().Trim();
            var userId = GetUserId();
            var notes = await _notesRepository.GetAllAsync(n => n.Content.ToLower().Contains(content) && n.UserId == userId, "Tags");
            await CheckAuthorization(notes);
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }

        public async Task<IEnumerable<NoteDto>> GetNotesByTags(IEnumerable<string> tags)
        {
            var userId = GetUserId();
            var searchedNotes = new List<Note>();
            var notes = await _notesRepository.GetAllAsync(n => n.UserId == userId, "Tags");
            await CheckAuthorization(notes);

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
            var userId = GetUserId();
            note.UserId = userId;
            await _notesRepository.AddAsync(note);
            return note.Id;
        }

        private async Task CheckAuthorization(IEnumerable<Note> notes, Operation operation = Operation.Read)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, notes,
               new ResourceOperationRequirement(operation));

            if (!authorizationResult.Succeeded)
                throw new ForbiddenException("You don't have access to this resource");
        }

        private async Task CheckAuthorization(Note note, Operation operation = Operation.Read)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User, note,
               new ResourceOperationRequirement(operation));

            if (!authorizationResult.Succeeded)
                throw new ForbiddenException("You don't have access to this resource");
        }

        private int GetUserId()
        {
            var userId = _userContextService.GetUserId;

            if (userId.HasValue)
            {
                return userId.Value;
            }

            throw new UnauthenticatedException("You are unauthenticated");
        }
    }
}
