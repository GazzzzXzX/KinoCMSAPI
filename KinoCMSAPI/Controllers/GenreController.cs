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
	public class GenreController : Controller
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Возвращает объект коткретного жанра.
		/// </summary>
		/// <param name="name">Ключ объекта</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает объект жанра
		/// <remarks>
		/// 
		///			{
		///				"name": "Название жанра" // Является так же ключем.
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetGenre/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetGenre(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			Genre genre = _db.GetGenre("Genre", name).Result;
			if(genre != null)
			{
				return Ok(genre);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращает все жанры.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список жанров
		/// <remarks>
		/// 
		///			{
		///				"name": "Название жанра" // Является так же ключем.
		///			}
		///			{
		///				"name": "Название жанра" // Является так же ключем.
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetGenre")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetGenres()
		{
			List<Genre> genres = _db.GetGenre().Result;
			if(genres.Count != 0)
			{
				return Ok(genres);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращает все фильмы с данным жанром.
		/// </summary>
		/// <param name="name">Ключ объекта жанра</param>
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
		[HttpGet("GetFilmsGenre/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetFilmsGenre(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			Genre genre = _db.GetGenre("Genre", name).Result;

			if(genre == null)
			{
				ModelState.AddModelError("Genre", "Такого жанра не существует.");
				return NotFound(ModelState);
			}

			MockFilmsGenre mockFilms;
			List<Film> films = mockFilms.GetFilmsGenre(name);

			if (films.Count != 0)
			{
				return Ok(films);
			}
			ModelState.AddModelError("Film", "Нет ни одного фильма с данным жанром.");
			return NotFound(ModelState);
		}

		/// <summary>
		/// Возвращает объект FilmsGenre
		/// </summary>
		/// <param name="id">Ключ объекта</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает объект жанра
		/// <remarks>
		/// 
		///			{
		///				"id": "60269d11c44061f618fa8ffd"
		///				"idFilm": "Название фильма"
		///				"idGenre": "Название жанра"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetObjFilmsGenre/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetObjFilmsGenre(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			FilmsGenre filmsGenre = _db.GetFilmGenre(id).Result;

			if(filmsGenre != null)
			{
				return Ok(filmsGenre);
			}
			ModelState.AddModelError("FilmsGenre", "Данного объекта не существует.");
			return NotFound(ModelState);
		}

		/// <summary>
		/// Сохранение жанра.
		/// </summary>
		/// <param name="obj">Объект жанра("Name")</param>
		/// <remarks>
		/// 
		///			{
		///				"name": "Название жанра" // Так же является ключем.
		///			}
		///			 
		/// </remarks>
		/// <returns></returns>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает только что созданный объект
		/// <remarks>
		/// 
		///			{
		///				"name": "Название жанра" // Так же является ключем.
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был добавлен в базу</response>
		[HttpPost("PostGenre")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult PostGenre([FromBody]Genre obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}

			
			String temp = _db.SetValue(obj.GetType().Name, obj).Result;
			if (temp != null)
			{
				ModelState.AddModelError("Genre", temp);
				return BadRequest(ModelState);
			}
			

			Genre genre = _db.GetGenre("Genre", obj.Name).Result;
			if(genre == null)
			{
				ModelState.AddModelError("Genre", "Объект не был записан в базу данных.");
				return BadRequest(ModelState);
			}

			return CreatedAtAction("GetGenre", new { genre.Name}, genre);
		}

		/// <summary>
		/// Добавляет фильму жанр
		/// </summary>
		/// <param name="obj">Объект FilmsGenre("idFilm", "idGenre")</param>
		/// <remarks>
		/// 
		///			{
		///				"idFilm": "Название фильма"
		///				"idGenre": "Название жанра"
		///			}
		///			 
		/// </remarks>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает объект жанра
		/// <remarks>
		/// 
		///			{
		///				"id": "60269d11c44061f618fa8ffd"
		///				"idFilm": "Название фильма"
		///				"idGenre": "Название жанра"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был найден</response>
		[HttpPost("PostFilmGenre")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostFilmGenre([FromBody]FilmsGenre obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.GetType().Name, obj);

			FilmsGenre filmsGenre = _db.GetFilmsGenre(obj).Result;

			return CreatedAtAction("GetObjFilmsGenre", new { filmsGenre.id }, filmsGenre);
		}
	}
}
