using KinoCMSAPI.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Mocks
{
	struct MockFilmsTimeTable
	{
		private static DataBase _db = Singleton.GetInstance().Context;

		public List<Film> GetFilmsDate(DateTime dateTime)
		{
			List<TimeTableFilms> timeTableFilms = _db.GetTimetableFilms().Result;
			List<Film> films = new List<Film>();
			foreach (var item in timeTableFilms)
			{
				if (item.dateTime.Date == dateTime.Date)
				{
					films.Add(_db.GetFilm("Film", item.idFilm).Result);
				}
			}

			return films;
		}
	}
}
