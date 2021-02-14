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
	[Route("api/[controller]")]
	[ApiController]
	public class ScreenwriterController : ControllerBase
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Возращение всех сценаристов.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список сценаристов
		/// <remarks>
		/// 
		///			{
		///				"Name": "Имя сценариста"
		///			},
		///			{
		///				"Name": "Имя сценариста"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetScreenwriters")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetScreenwiters()
		{
			List<Screenwriter> screenwriter = _db.GetScreenwriter().Result;
			if (screenwriter != null)
			{
				return Ok(screenwriter);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение конкретного сценариста.
		/// </summary>
		/// <param name="name">Ключ сценариста</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает продюсера
		/// <remarks>
		/// 
		///			{
		///				"Name": "Имя сценариста"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetScreenwriter/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetScreenwriter(String name)
		{
			if (name == null)
			{
				return BadRequest();
			}

			Screenwriter screenwriter = _db.GetScreenwriter("Producer", name).Result;

			if (screenwriter != null)
			{
				return Ok(screenwriter);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение всех фильмов по конкретному сценаристу.
		/// </summary>
		/// <param name="name">Ключ сценариста</param>
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
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilmsScreenwriter/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetFilmsScreenwriter(String id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			List<FilmsScreenwriter> filmsScreenwriters = _db.GetFilmsScreenwiter(id).Result;

			if (filmsScreenwriters != null)
			{
				List<Film> films = new List<Film>();
				foreach (var item in filmsScreenwriters)
				{
					films.Add(_db.GetFilm("Film", item.idFilm).Result);
				}
				return Ok(films);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращает список сценариста - фильм.
		/// </summary>
		/// <response code="200">
		/// Возвращает список продюсеров
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ",
		///				"idFilm": "Ключ фильма" 
		///				"idScreenwriter": "Имя сценариста"
		///			},
		///			{
		///				"idScreenwriter": "Имя сценариста"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilmsScreenwriter")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetFilmsScreenwriter()
		{

			List<FilmsScreenwriter> filmsScreenwriters = _db.GetFilmsScreenwiter().Result;
			if (filmsScreenwriters != null)
			{
				return Ok(filmsScreenwriters);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращает одно поле сценарист - фильм.
		/// </summary>
		/// <param name="id">Ключ FilmsScreenwriter</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает сценарист-фильм
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ",
		///				"idFilm": "Ключ фильма" 
		///				"idScreenwriter": "Имя сценариста"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetOneFilmsScreenwriter/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetOneFilmsScreenwriter(String id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			FilmsScreenwriter filmsScreenwriter = _db.GetOneFilmsScreenwiter(id).Result;
			if (filmsScreenwriter != null)
			{
				return Ok(filmsScreenwriter);
			}
			return NotFound();
		}

		/// <summary>
		/// Создание сценариста. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект сценариста (Name)</param>
		/// <returns></returns>
		/// <remarks>
		///			
		///			PostScreenwriter /ToDo
		///			{
		///				"Name": "Имя сценариста"
		///			}
		///			 
		/// </remarks>
		/// <response code="201">
		/// Возвращает только что созданный объект
		/// <remarks>
		/// 
		///			{
		///				"Name": "Имя сценариста"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostScreenwriter")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostScreenwriter([FromBody]Screenwriter obj)
		{
			if (obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.GetType().Name, obj);

			Screenwriter screenwriter = _db.GetScreenwriter("Screenwriter", obj.Name).Result;

			if (screenwriter != null)
			{
				return CreatedAtAction("GetScreenwriter", new { screenwriter.Name }, screenwriter);
			}
			return BadRequest();
		}

		/// <summary>
		/// Добавление сценариста к фильму. Authorize = Administator
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		/// <remarks>
		/// 
		///			PostFilmsScreenwriter /ToDo
		///			{
		///				"idFilm": "Ключ фильма" 
		///				"idScreenwriter": "Имя сценариста"
		///			}
		///			 
		/// </remarks>x
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает продюсер-фильм
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ",
		///				"idFilm": "Ключ фильма" 
		///				"idScreenwriter": "Имя сценариста"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostFilmsScreenwriter")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostFilmsScreenwriter([FromBody] FilmsScreenwriter obj)
		{
			if (obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.GetType().Name, obj);

			FilmsScreenwriter filmsScreenwriter = _db.GetFilmsScreenwriter(obj).Result;

			if (filmsScreenwriter != null)
			{
				return CreatedAtAction("GetOneFilmsScreenwriter", new { filmsScreenwriter.id }, filmsScreenwriter);
			}
			return BadRequest();
		}

		/// <summary>
		/// Удаление сценариста, так же влечет за собой удаление сценариста из всех фильмов где он встречается. Authorize = Administator
		/// </summary>
		/// <param name="name">Ключ сценариста</param>
		/// <returns></returns>
		/// <response code="204">Сообщает что объект был удален из базы</response>
		/// <response code="400">Если по каим либо причинам объект не был удален</response>
		[HttpDelete("DeleteProducer/{name}")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult DeleteProducer(String name)
		{
			if (name == null)
			{
				return BadRequest();
			}

			_db.RemoveScreenwriter(name);
			_db.RemoveFilmsScreenwriter(name);

			return NoContent();
		}
	}
}
