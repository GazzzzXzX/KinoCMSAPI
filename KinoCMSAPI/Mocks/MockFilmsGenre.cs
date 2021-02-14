using KinoCMSAPI.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Mocks
{
	struct MockFilmsGenre
	{
		public List<Film> GetFilmsGenre(String name)
		{
			DataBase db = Singleton.GetInstance().Context;

			List<FilmsGenre> filmsGenres = db.GetFilmsGenre(name).Result;
			List<Film> films = new List<Film>();

			foreach (var item in filmsGenres)
			{
				films.Add(db.GetFilm("Film", item.idFilm).Result);
			}

			return films;
		}
	}
}
