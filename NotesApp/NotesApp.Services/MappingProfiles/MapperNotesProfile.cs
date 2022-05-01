using AutoMapper;
using NotesApp.Domain.Entities;
using NotesApp.Services.Dto;

namespace NotesApp.Services.MappingProfiles
{
    public class MapperNotesProfile : Profile
    {
        public MapperNotesProfile()
        {
            CreateMap<NoteDto, Note>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(t => t.Tags.Select(p => new Tag() { TagName = p })))
                .ReverseMap();
        }
    }
}
