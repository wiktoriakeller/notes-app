using AutoMapper;
using NotesApp.Domain.Entities;
using NotesApp.Services.Dto;

namespace NotesApp.Services.MappingProfiles
{
    public class MapperNotesProfile : Profile
    {
        public MapperNotesProfile()
        {
            CreateMap<NoteDto, Note>().ReverseMap();
            CreateMap<TagDto, Tag>().ReverseMap();
            CreateMap<CreateNoteDto, Note>().ReverseMap();  
            CreateMap<CreateTagDto, Tag>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<Note, PublicNoteDto>()
                .ForMember(dest => dest.Author,
                            opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));
        }
    }
}
