using AutoMapper;
using NotesApp.Domain.Entities;
using NotesApp.Services.Dto;
using HashidsNet;

namespace NotesApp.Services.MappingProfiles
{
    public class MapperNotesProfile : Profile
    {
        public MapperNotesProfile()
        {
            CreateMap<NoteDto, Note>().ReverseMap();
            CreateMap<CreateNoteDto, Note>().ReverseMap();
            CreateMap<UpdateNoteDto, Note>().ReverseMap();
            CreateMap<Note, PublicNoteDto>()
                .ForMember(dest => dest.Author,
                    opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"));
            CreateMap<Note, PublicLinkDto>();

            CreateMap<TagDto, Tag>().ReverseMap();
            CreateMap<CreateTagDto, Tag>().ReverseMap();

            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<RegisterUserDto, User>().ReverseMap();
        }
    }
}
