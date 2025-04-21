using AutoMapper;
using DAL.Models;

namespace BAL.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //register mapper here
            CreateMap<Projection, ProjectionDto>();
            CreateMap<Film, FilmDto>();
        }
    }
}
