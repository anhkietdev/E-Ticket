using AutoMapper;
using BAL.DTOs;
using DAL.Models;

namespace BAL.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Film mappings
            CreateMap<FilmRequestDto, Film>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.FilmGenres, opt => opt.Ignore())
                .ForMember(dest => dest.Projections, opt => opt.Ignore());

            CreateMap<Film, FilmResponseDto>()
                .ForMember(dest => dest.FilmGenres, opt => opt.MapFrom(src => src.FilmGenres))
                .ForMember(dest => dest.Projections, opt => opt.MapFrom(src => src.Projections));

            CreateMap<FilmGenre, FilmGenreDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre));

            CreateMap<Genre, GenreDto>();

            CreateMap<Projection, ProjectionDto>();

            // Projection mappings
            CreateMap<ProjectionRequestDto, Projection>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Film, opt => opt.Ignore())
                .ForMember(dest => dest.Room, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());

            CreateMap<Projection, ProjectionResponseDto>();
            CreateMap<Film, FilmDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<Ticket, TicketResponseDTO>();

            // Room mappings
            CreateMap<RoomRequestDto, Room>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Seats, opt => opt.Ignore())
                .ForMember(dest => dest.Projections, opt => opt.Ignore());

            CreateMap<Room, RoomResponseDto>();

            // Seat mappings
            CreateMap<SeatRequestDto, Seat>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Room, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());

            CreateMap<Seat, SeatResponseDto>();
            CreateMap<Seat, SeatDto>();

            // Genre mappings
            CreateMap<GenreRequestDto, Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.FilmGenres, opt => opt.Ignore());

            CreateMap<Genre, GenreResponseDto>();

            // FilmGenre mappings
            CreateMap<FilmGenreRequestDto, FilmGenre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Film, opt => opt.Ignore())
                .ForMember(dest => dest.Genre, opt => opt.Ignore());

            CreateMap<FilmGenre, FilmGenresResponseDto>()
                .ForMember(dest => dest.FilmId, opt => opt.MapFrom(src => src.FilmId))
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                .ForMember(dest => dest.Film, opt => opt.MapFrom(src => src.Film))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre));
        }
    }
}