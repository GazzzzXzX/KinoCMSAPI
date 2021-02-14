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
	[Route("api/[controller]")]
	[ApiController]
	public class ProducerController : ControllerBase
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Возращение всех продюсеров.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список продюсеров
		/// <remarks>
		/// 
		///			{
		///				"Name": "Имя продюсера"
		///			},
		///			{
		///				"Name": "Имя продюсера"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetProducers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetProducers()
		{
			List<Producer> producers = _db.GetProducers().Result;
			if (producers != null)
			{
				return Ok(producers);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение конкретного продюсера.
		/// </summary>
		/// <param name="name">Ключ продюсера</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает продюсера
		/// <remarks>
		/// 
		///			{
		///				"Name": "Имя продюсера"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetProducer/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetProducer(String name)
		{
			if (name == null)
			{
				return BadRequest();
			}

			Producer producer = _db.GetProducer("Producer", name).Result;

			if (producer != null)
			{
				return Ok(producer);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение всех фильмов по конкретному продюсеру.
		/// </summary>
		/// <param name="name">Ключ продусера</param>
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
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilmsProducer/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetFilmsProducer(String id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			List<FilmsProducer> filmsProducer = _db.GetFilmsProducers(id).Result;

			if (filmsProducer != null)
			{
				List<Film> films = new List<Film>();
				foreach (var item in filmsProducer)
				{
					films.Add(_db.GetFilm("Film", item.idFilm).Result);
				}
				return Ok(films);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращает список продюсер - фильм.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список продюсеров
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ",
		///				"idFilm": "Ключ фильма" 
		///				"idProducer": "Имя продюсера"
		///			},
		///			{
		///				"idProducer": "Имя продюсера"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilmsProducers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetFilmsProducers()
		{

			List<FilmsProducer> filmsProducer = _db.GetFilmsProducers().Result;
			if (filmsProducer != null)
			{
				return Ok(filmsProducer);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращает одно поле продюсер - фильм.
		/// </summary>
		/// <param name="id">Ключ FilmsProducer</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает продюсер-фильм
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ",
		///				"idFilm": "Ключ фильма" 
		///				"idProducer": "Имя продюсера"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetOneFilmsProducer/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetOneFilmsProducer(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			FilmsProducer filmsProducer = _db.GetFilmsProducer(id).Result;
			if (filmsProducer != null)
			{
				return Ok(filmsProducer);
			}
			return NotFound();
		}

		/// <summary>
		/// Создание продюсера. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект продюсера (Name)</param>
		/// <returns></returns>
		/// <remarks>
		///			
		///			PostProducer /ToDo
		///			{
		///				"Name": "Имя продюсера"
		///			}
		///			 
		/// </remarks>
		/// <response code="201">
		/// Возвращает только что созданный объект
		/// <remarks>
		/// 
		///			{
		///				"Name": "Имя продюсера"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostProducer")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostProducer([FromBody]Producer obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.GetType().Name, obj);

			Producer producer = _db.GetProducer("Producer", obj.Name).Result;

			if(producer != null)
			{
				return CreatedAtAction("GetProducer", new { producer.Name }, producer);
			}
			return BadRequest();
		}

		/// <summary>
		/// Добавление продюсера к фильму. Authorize = Administator
		/// </summary>
		/// <param name="obj"></param>
		/// <remarks>
		/// 
		///			PostFilmsProducer /ToDo
		///			{
		///				"idFilm": "Ключ фильма" 
		///				"idProducer": "Имя продюсера"
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
		///				"idProducer": "Имя продюсера"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostFilmsProducer")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostFilmsProducer([FromBody]FilmsProducer obj)
		{
			if (obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.GetType().Name, obj);

			FilmsProducer producer = _db.GetFilmsProducer(obj).Result;

			if (producer != null)
			{
				return CreatedAtAction("GetOneFilmsProducer", new { producer.id }, producer);
			}
			return BadRequest();
		}

		/// <summary>
		/// Удаление продюсера, так же влечет за собой удаление продюсера из всех фильмов. Authorize = Administator
		/// </summary>
		/// <param name="name">Ключ продюсера</param>
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

			_db.RemoveProducer(name);
			_db.RemoveFilmsProducer(name);

			return NoContent();
		}
	}
}
