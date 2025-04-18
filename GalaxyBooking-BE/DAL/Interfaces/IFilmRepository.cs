using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IFilmRepository
    {
        Task<List<Film>>GetallFilms();
        Task<Film> GetFilmByTitle(string title);
        Task<Film> AddFilm(Film film);
        Task<Film> UpdateFilm(string title,Film film);
        Task DeleteFilm(string title);

    }
}
