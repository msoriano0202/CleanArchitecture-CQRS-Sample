using AutoMapper;
using CleanArchitecture.Application.Features.Actors.Queries.Vms;
using CleanArchitecture.Application.Features.Directors.Commands.CreateDirector;
using CleanArchitecture.Application.Features.Directors.Queries.Vms;
using CleanArchitecture.Application.Features.Streamers.Commands;
using CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer;
using CleanArchitecture.Application.Features.Streamers.Queries.Vms;
using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;
using CleanArchitecture.Application.Features.Videos.Queries.Vms;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Video, VideosVm>();
            CreateMap<Video, VideosWithIncludesVm>()
                .ForMember(dest => dest.DirectorNombreCompleto, src => src.MapFrom(v => v.Director!.NombreCompleto))
                .ForMember(dest => dest.StreamerNombre, src => src.MapFrom(v => v.Streamer!.Nombre))
                .ForMember(dest => dest.Actores, src => src.MapFrom(v => v.Actores));

            CreateMap<Actor, ActorVm>();
            CreateMap<Director, DirectorVm>();
            CreateMap<Streamer, StreamersVm>();

            CreateMap<CreateStreamerCommand, Streamer>();
            CreateMap<UpdateStreamerCommand, Streamer>();
            CreateMap<CreateDirectorCommand, Director>();
        }
    }
}
