using KinoCMSAPI.Mocks;
using KinoCMSAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FilmController : Controller
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Запись объекта "фильм" в бд. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект фильм</param>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает только что созданный объект
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был добавлен в базу</response>       
		[HttpPost("PostFilm")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult PostFilm([FromBody]Film obj)
		{
			if (obj == null)
			{
				return BadRequest();
			}

			_db.SetValue(obj.GetType().Name, obj);

			Film film = _db.GetFilm(obj).Result;

			if (film != null)
			{
				return CreatedAtAction("GetFilm", new { film.id }, film);
			}
			return NotFound();
		}

		/// <summary>
		/// Получение всех фильмов которые есть на данный момент.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список фильмов
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			},
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilms")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetFilms()
		{
			List<Film> films = _db.GetFilm().Result;
			if (films.Count != 0)
			{
				return Ok(films);
			}
			return NotFound();
		}

		/// <summary>
		/// Получение конкретного фильма.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает объект фильма
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilm/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetFilm(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			Film film = _db.GetFilm("Film", id).Result;

			if (film != null)
			{
				return Ok(film);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение всех фильмов которые будет транслироваться сегодня.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список фильмов
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			},
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilmToday")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetFilmToday()
		{
			MockFilmsTimeTable MockFilm;

			List<Film> timeTableFilms = MockFilm.GetFilmsDate(DateTime.Today);

			if (timeTableFilms.Count != 0)
			{
				return Ok(timeTableFilms);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение всех фильмов которые будет транслироваться в определенную дату.
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список фильмов
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			},
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilmDate/{dateTime}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetFilmDate(String dateTime)
		{
			if(dateTime == null)
			{
				return BadRequest();
			}
			DateTime date;
			try
			{
				date = Convert.ToDateTime(dateTime);
			}
			catch
			{
				ModelState.AddModelError("dateTime", "Недопустимое значение для даты.");
				return BadRequest(ModelState);
			}

			MockFilmsTimeTable MockFilm;

			List<Film> timeTableFilms = MockFilm.GetFilmsDate(date);

			if (timeTableFilms.Count != 0)
			{
				return Ok(timeTableFilms);
			}
			return NotFound();
		}

		/// <summary>
		/// Обновление данных фильма. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект фильма</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает только что созданный объект
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"name": "Название фильма",
		///				"desc": "Описание",
		///				"img": "url",
		///				"price": 100,
		///				"urlVideo": "UrlVideo",
		///				"d3": true,
		///				"d2": true,
		///				"imax": true
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был изменен</response>       
		[HttpPut("PutFilm")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult PutFilm([FromBody]Film obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}

			Film film = _db.GetFilm("Film", obj.id).Result;

			if(film == null)
			{
				return NotFound();
			}

			film.Desc = obj.Desc;
			film.D2 = obj.D2;
			film.D3 = obj.D3;
			film.Imax = obj.Imax;
			film.Img = obj.Img;
			film.Name = obj.Name;
			film.Price = obj.Price;
			film.UrlVideo = obj.UrlVideo;

			_db.Save(film.GetType().Name, film.id, film);
			return Ok(film);
		}

		/// <summary>
		/// Удаление фильма из таблицы, так же влечет за собой удаление всех расписаний связаных с этим фильмом и удаление фильмов из жанров. Authorize = Administator
		/// </summary>
		/// <param name="id">Ключ фильма</param>
		/// <returns></returns>
		/// <response code="204">Сообщает что объект был удален из базы</response>
		/// <response code="400">Если по каим либо причинам объект не был удален</response>
		[HttpDelete("DeleteFilm/{id}")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult DeleteFilm(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			_db.RemoveFilm(id);
			_db.RemoveTimeTableFilms(id);
			_db.RemoveFilmsGenre(id);

			return NoContent();
		}
	}
}
