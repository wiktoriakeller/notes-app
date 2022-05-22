using NotesApp.Services.Interfaces;
using NotesApp.Domain.Entities;
using NotesApp.Domain.Interfaces;
using NotesApp.Services.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using NotesApp.Services.Authorization;
using NotesApp.Services.Exceptions;
using HashidsNet;

namespace NotesApp.Services.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _notesRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly IHashids _hashids;

        public NoteService(INoteRepository notesRepository, 
            IMapper mapper,
            IAuthorizationService authorizationService,
            IUserContextService userContextService,
            IHashids hashids)
        {
            _notesRepository = notesRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _hashids = hashids;
        }

        public async Task<NoteDto?> GetNoteById(string hashId)
        {
            var id = GetRawId(hashId);
            var note = await _notesRepository.GetByIdAsync(id, "Tags");
            await CheckAuthorization(note);

            if (note == null)
                throw new NotFoundException($"Resource with id: {id} couldn't be found");

            return _mapper.Map<NoteDto>(note);
        }

        public async Task<IEnumerable<NoteDto>> GetAllNotes()
        {
            var userId = GetUserId();
            var notes = await _notesRepository.GetAllAsync(n => n.UserId == userId, "Tags");
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

        public async Task<PublicLinkDto> GeneratePublicLink(CreatePublicLinkDto dto, string hashId)
        {
            var id = GetRawId(hashId);
            var note = await _notesRepository.GetByIdAsync(id);
            await CheckAuthorization(note);
            string publicHashId = string.Empty;

            if (note == null)
                throw new NotFoundException($"Resource with id: {id} couldn't be found");

            if(!dto.ResetPublicHashId)
            {
                var rng = new Random();
                var salt = rng.Next();
                publicHashId = _hashids.EncodeLong(id + salt);

                note.PublicHashId = publicHashId;
                note.PublicHashIdSalt = salt;
                note.PublicLinkValidTill = DateTimeOffset.Now.AddDays(1);
            }
            else
            {
                note.PublicHashId = string.Empty;
            }

            await _notesRepository.UpdateAsync(note);
            return _mapper.Map<PublicLinkDto>(note);
        }

        public async Task<PublicNoteDto> GetPublicNote(string publicHashId)
        {
            var notes = await _notesRepository.GetAllNotesWithUsersAndTagsAsync(n => n.PublicHashId != string.Empty && n.PublicHashId == publicHashId);
            
            if (notes.Count == 1)
            {
                var note = notes.First();

                if(note.PublicLinkValidTill < DateTimeOffset.Now)
                    throw new NotFoundException($"Resource with hashid: {publicHashId} couldn't be found");

                return _mapper.Map<PublicNoteDto>(note);
            }

            throw new NotFoundException($"Resource with hashid: {publicHashId} couldn't be found");
        }

        public async Task<string> AddNote(CreateNoteDto noteDto)
        {
            var note = _mapper.Map<Note>(noteDto);
            var userId = GetUserId();
            note.UserId = userId;
            await _notesRepository.AddAsync(note);
            note.HashId = EncodeId(note.Id);
            await _notesRepository.UpdateAsync(note);
            return note.HashId;
        }

        public async Task<NoteDto> UpdateNote(UpdateNoteDto noteDto)
        {
            var hashId = noteDto.HashId;
            var id = GetRawId(hashId);
            var note = await _notesRepository.GetByIdAsync(id, "Tags");
            await CheckAuthorization(note, Operation.Update);

            if (note == null)
                throw new NotFoundException($"Resource with id: {id} couldn't be found");

            note.NoteName = noteDto.NoteName;
            note.Content = noteDto.Content;
            note.Tags = _mapper.Map<ICollection<Tag>>(noteDto.Tags);
            await _notesRepository.UpdateAsync(note);

            return _mapper.Map<NoteDto>(note);
        }

        public async Task DeleteNote(string hashid)
        {
            int id = GetRawId(hashid);
            var note = await _notesRepository.GetByIdAsync(id);
            await CheckAuthorization(note, Operation.Delete);

            if (note == null)
                throw new NotFoundException($"Resource with id: {id} couldn't be found");

            await _notesRepository.DeleteAsync(note);
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
                return userId.Value;

            throw new UnauthenticatedException("You are unauthenticated");
        }

        private int GetRawId(string hashId)
        {
            var rawId = _hashids.Decode(hashId);

            if (rawId.Length == 0)
                throw new NotFoundException("Invalid identifier");

            return rawId[0];
        }

        private string EncodeId(int id)
        {
            return _hashids.Encode(id);
        }
    }
}
