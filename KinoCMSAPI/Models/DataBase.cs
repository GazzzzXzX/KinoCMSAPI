using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	public class DataBase
	{
		private IMongoDatabase _db;

		public DataBase()
		{
			String con = ConfigurationManager.ConnectionStrings["DatabaseMongoDb"].ConnectionString;
			String nameDb = ConfigurationManager.ConnectionStrings["NameDatabase"].ConnectionString;
			var client = new MongoClient(con);
			_db = client.GetDatabase(nameDb);
		}

		/// <summary>
		/// Создание какого либо объекта связаного с БД.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="table">Имя таблицы</param>
		/// <param name="record">Объект относящийся к бд</param>
		public async Task<String> SetValue<T>(String table, T record)
		{
			var collection = _db.GetCollection<T>(table);
			try
			{
				await collection.InsertOneAsync(record);
			}
			catch(Exception)
			{
				return "Объект не был записан в базу данных по причине что уже такое поле существует.";
			}
			return null;
		}

		/// <summary>
		/// Обновление полей в бд.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="table">Имя таблицы</param>
		/// <param name="id">Ключ записи</param>
		/// <param name="record">Объект относящийся к бд</param>
		public async void Save<T>(String table, String id, T record)
		{
			var collection = _db.GetCollection<T>(table);

			var result = await collection.ReplaceOneAsync(
					new BsonDocument("_id", id),
					record,
					new UpdateOptions { IsUpsert = true });
		}

		public async Task<User> GetUser(String id)
		{
			var collection = _db.GetCollection<User>("User");

			return await collection.Find(p => p.UserName == id).FirstOrDefaultAsync();
		}

		public async Task<User> GetUserEmail(String id)
		{
			var collection = _db.GetCollection<User>("User");

			return await collection.Find(p => p.Email == id).FirstOrDefaultAsync();
		}

		public async Task<Film> GetFilm(Film obj)
		{
			var collection = _db.GetCollection<Film>(obj.GetType().Name);

			return await collection.Find(p => p.Name == obj.Name && p.Desc == obj.Desc && p.Img == obj.Img && p.Price == obj.Price).FirstOrDefaultAsync();
		}

		public async Task<Film> GetFilm(String table, String id)
		{
			var collection = _db.GetCollection<Film>(table);

			return await collection.Find(p => p.id == id).FirstOrDefaultAsync();
		}

		public async Task<List<Film>> GetFilm()
		{
			var collection = _db.GetCollection<Film>("Film");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async void RemoveFilm(String id)
		{
			var collection = _db.GetCollection<Film>("Film");

			var filter = Builders<Film>.Filter.Eq("id", id);

			await collection.DeleteOneAsync(filter);
		}

		public async void RemoveTimeTableFilm(String id)
		{
			var collection = _db.GetCollection<TimeTableFilms>("TimeTableFilms");

			var filter = Builders<TimeTableFilms>.Filter.Eq("id", id);

			await collection.DeleteOneAsync(filter);
		}

		public async void RemoveTimeTableFilms(String id)
		{
			var collection = _db.GetCollection<TimeTableFilms>("TimeTableFilms");

			var filter = Builders<TimeTableFilms>.Filter.Eq("idFilm", id);

			await collection.DeleteManyAsync(filter);
		}

		public async Task<Genre> GetGenre(String table, String id)
		{
			var collection = _db.GetCollection<Genre>(table);

			return await collection.Find(p => p.Name == id).FirstOrDefaultAsync();
		}

		public async Task<List<Genre>> GetGenre()
		{
			var collection = _db.GetCollection<Genre>("Genre");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async void RemoveFilmsGenre(String id)
		{
			var collection = _db.GetCollection<FilmsGenre>("FilmsGenre");

			var filter = Builders<FilmsGenre>.Filter.Eq("idFilm", id);

			await collection.DeleteManyAsync(filter);
		}

		public async Task<FilmsGenre> GetFilmGenre(String id)
		{
			var collection = _db.GetCollection<FilmsGenre>("FilmsGenre");

			return await collection.Find(p => p.id == id).FirstOrDefaultAsync();
		}

		public async Task<List<FilmsGenre>> GetFilmsGenre(String name)
		{
			var collection = _db.GetCollection<FilmsGenre>("FilmsGenre");

			return await collection.Find(p => p.idGenre == name).ToListAsync();
		}

		public async Task<FilmsGenre> GetFilmsGenre(FilmsGenre obj)
		{
			var collection = _db.GetCollection<FilmsGenre>("FilmsGenre");

			return await collection.Find(p => p.idGenre == obj.idGenre && p.idFilm == obj.idFilm).FirstOrDefaultAsync();
		}

		public async Task<Screenwriter> GetScreenwriter(String table, String id)
		{
			var collection = _db.GetCollection<Screenwriter>(table);

			return await collection.Find(p => p.Name == id).FirstOrDefaultAsync();
		}

		public async Task<List<Screenwriter>> GetScreenwriter()
		{
			var collection = _db.GetCollection<Screenwriter>("Screenwriter");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task<Producer> GetProducer(String table, String id)
		{
			var collection = _db.GetCollection<Producer>(table);

			return await collection.Find(p => p.Name == id).FirstOrDefaultAsync();
		}

		public async Task<List<Producer>> GetProducers()
		{
			var collection = _db.GetCollection<Producer>("Producer");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task<List<FilmsProducer>> GetFilmsProducers(String id)
		{
			var collection = _db.GetCollection<FilmsProducer>("FilmsProducer");

			return await collection.Find(p => p.idProducer == id).ToListAsync();
		}

		public async Task<List<FilmsProducer>> GetFilmsProducers()
		{
			var collection = _db.GetCollection<FilmsProducer>("FilmsProducer");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task<FilmsProducer> GetFilmsProducer(String id)
		{
			var collection = _db.GetCollection<FilmsProducer>("FilmsProducer");

			return await collection.Find(p => p.id == id).FirstOrDefaultAsync();
		}

		public async Task<FilmsProducer> GetFilmsProducer(FilmsProducer obj)
		{
			var collection = _db.GetCollection<FilmsProducer>("FilmsProducer");

			return await collection.Find(p => p.idFilm == obj.idFilm && p.idProducer == obj.idProducer).FirstOrDefaultAsync();
		}

		public async void RemoveProducer(String name)
		{
			var collection = _db.GetCollection<Producer>("Producer");

			var filter = Builders<Producer>.Filter.Eq("Name", name);

			await collection.DeleteOneAsync(filter);
		}

		public async void RemoveFilmsProducer(String name)
		{
			var collection = _db.GetCollection<FilmsProducer>("FilmsProducer");

			var filter = Builders<FilmsProducer>.Filter.Eq("idProducer", name);

			await collection.DeleteManyAsync(filter);
		}

		public async Task<TimeTableFilms> GetTimeTableFilm(String id)
		{
			var collection = _db.GetCollection<TimeTableFilms>("TimeTableFilms");

			return await collection.Find(p => p.id == id).FirstOrDefaultAsync();
		}

		public async Task<TimeTableFilms> GetTimeTableFilm(TimeTableFilms obj)
		{
			var collection = _db.GetCollection<TimeTableFilms>(obj.GetType().Name);

			return await collection.Find(p => p.dateTime == obj.dateTime && p.idFilm == obj.idFilm).FirstOrDefaultAsync();
		}

		public async Task<List<TimeTableFilms>> GetTimetableFilms()
		{
			var collection = _db.GetCollection<TimeTableFilms>("TimeTableFilms");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		/// <summary>
		/// Возвращает коллекцию расписание по конкретному фильму.
		/// </summary>
		/// <param name="id">Ключ фильма который вам необходимо</param>
		/// <returns></returns>
		public async Task<List<TimeTableFilms>> GetTimetableFilms(String id)
		{
			var collection = _db.GetCollection<TimeTableFilms>("TimeTableFilms");

			return await collection.Find(p => p.idFilm == id).ToListAsync();
		}

		public async Task<TimeTableFilms> GetTimetableFilms(String table, String id)
		{
			var collection = _db.GetCollection<TimeTableFilms>(table);

			return await collection.Find(p => p.id == id).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Возвращает коллекцию фильмов по конкретному сценаристу.
		/// </summary>
		/// <param name="id">Ключ сценариста который вам необходимо</param>
		/// <returns></returns>
		public async Task<List<FilmsScreenwriter>> GetFilmsScreenwiter(String id)
		{
			var collection = _db.GetCollection<FilmsScreenwriter>("FilmsScreenwriter");

			return await collection.Find(p => p.idScreenwriter == id).ToListAsync();
		}

		public async Task<FilmsScreenwriter> GetOneFilmsScreenwiter(String id)
		{
			var collection = _db.GetCollection<FilmsScreenwriter>("FilmsScreenwriter");

			return await collection.Find(p => p.id == id).FirstOrDefaultAsync();
		}

		public async Task<List<FilmsScreenwriter>> GetFilmsScreenwiter()
		{
			var collection = _db.GetCollection<FilmsScreenwriter>("FilmsScreenwriter");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task<FilmsScreenwriter> GetFilmsScreenwriter(FilmsScreenwriter obj)
		{
			var collection = _db.GetCollection<FilmsScreenwriter>("FilmsScreenwriter");

			return await collection.Find(p => p.idFilm == obj.idFilm && p.idScreenwriter == obj.idScreenwriter).FirstOrDefaultAsync();
		}

		public async void RemoveScreenwriter(String name)
		{
			var collection = _db.GetCollection<Screenwriter>("Screenwriter");

			var filter = Builders<Screenwriter>.Filter.Eq("Name", name);

			await collection.DeleteOneAsync(filter);
		}

		public async void RemoveFilmsScreenwriter(String name)
		{
			var collection = _db.GetCollection<FilmsScreenwriter>("FilmsScreenwriter");

			var filter = Builders<FilmsScreenwriter>.Filter.Eq("idScreenwriter", name);

			await collection.DeleteManyAsync(filter);
		}

		public async Task<List<Cinema>> GetCinemas()
		{
			var collection = _db.GetCollection<Cinema>("Cinema");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task<Cinema> GetCinema(String id)
		{
			var collection = _db.GetCollection<Cinema>("Cinema");

			return await collection.Find(p => p.Name == id).FirstOrDefaultAsync();
		}

		public async void RemoveCinema(String id)
		{
			var collection = _db.GetCollection<Cinema>("Cinema");

			var filter = Builders<Cinema>.Filter.Eq("Name", id);

			await collection.DeleteOneAsync(filter);
		}

		public async Task<Hall>GetHall(String id)
		{
			var collection = _db.GetCollection<Hall>("Hall");

			return await collection.Find(p => p.id == id).FirstOrDefaultAsync();
		}

		public async Task<HallCinema> GetHallCinema(String name)
		{
			var collection = _db.GetCollection<HallCinema>("HallCinema");

			return await collection.Find(p => p.id == name).FirstOrDefaultAsync();
		}

		public async Task<List<HallCinema>> GetHallsCinema(String name)
		{
			var collection = _db.GetCollection<HallCinema>("HallCinema");

			return await collection.Find(p => p.idCinema == name).ToListAsync();
		}

		public async Task<HallCinema> GetHallCinema(HallCinema hall)
		{
			var collection = _db.GetCollection<HallCinema>("HallCinema");

			return await collection.Find(p => p.idCinema == hall.idCinema && p.idHall == hall.idHall).FirstOrDefaultAsync();
		}

		public async Task<Hall> GetHall(Hall obj)
		{
			var collection = _db.GetCollection<Hall>("Hall");

			return await collection.Find(p => p.Name == obj.Name && p.DateCreation == obj.DateCreation).FirstOrDefaultAsync();
		}

		public async void RemoveHall(String id)
		{
			var collection = _db.GetCollection<Hall>("Hall");

			var filter = Builders<Hall>.Filter.Eq("id", id);

			await collection.DeleteOneAsync(filter);
		}

		public async void RemoveHallCinema(String id)
		{
			var collection = _db.GetCollection<HallCinema>("HallCinema");

			var filter = Builders<HallCinema>.Filter.Eq("idHall", id);

			await collection.DeleteOneAsync(filter);
		}

		public async Task<List<News>> GetNews()
		{
			var collection = _db.GetCollection<News>("News");

			return await collection.Find(new BsonDocument()).ToListAsync();
		}

		public async Task<News> GetNews(String name)
		{
			var collection = _db.GetCollection<News>("News");

			return await collection.Find(p => p.Name == name).FirstOrDefaultAsync();
		}

		public async Task<List<ImgNews>> GetImgsNews(String name)
		{
			var collection = _db.GetCollection<ImgNews>("ImgNews");

			return await collection.Find(p => p.idNews == name).ToListAsync();
		}

		public async Task<ImgNews> GetImgNews(String name)
		{
			var collection = _db.GetCollection<ImgNews>("ImgNews");

			return await collection.Find(p => p.id == name).FirstOrDefaultAsync();
		}

		public async void RemoveNews(String name)
		{
			var collection = _db.GetCollection<News>("News");

			var filter = Builders<News>.Filter.Eq("Name", name);

			await collection.DeleteOneAsync(filter);
		}

		public async void RemoveImgNews(String name)
		{
			var collection = _db.GetCollection<ImgNews>("ImgNews");

			var filter = Builders<ImgNews>.Filter.Eq("idNews", name);

			await collection.DeleteManyAsync(filter);
		}
	}
}
