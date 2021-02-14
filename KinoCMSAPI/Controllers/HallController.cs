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
	public class HallController : Controller
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Возвращает один конеретно зал.
		/// </summary>
		/// <param name="id">Ключ Hall</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает объект зала
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"Name": "Название зала",
		///				"DateCreation": "Дата"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetHall/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetHall(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			Hall hall = _db.GetHall(id).Result;
			if(hall != null)
			{
				return Ok(hall);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение список всех залов по конкретному кинотеатру.
		/// </summary>
		/// <param name="name">Название кинотеатра</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список залаов
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"idCinema": "Ключ Cinema",
		///				"idHall": "Ключ зала"
		///			},
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"idCinema": "Ключ Cinema",
		///				"idHall": "Ключ зала"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetHallsCinema/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetHallsCinema(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			List<HallCinema> hallCinemas = _db.GetHallsCinema(name).Result;

			if(hallCinemas.Count != 0)
			{
				return Ok(hallCinemas);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение один зал конкретного кинотеатра.
		/// </summary>
		/// <param name="id">Ключ HallCinema</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает объект зала
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"idCinema": "Ключ Cinema",
		///				"idHall": "Ключ зала"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetHallCinema/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetHallCinema(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			HallCinema hallCinema = _db.GetHallCinema(id).Result;

			if(hallCinema != null)
			{
				return Ok(hallCinema);
			}
			return NotFound();
		}

		/// <summary>
		/// Создает зал. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект Hall(Name, DateCreation)</param>
		/// <remarks>
		/// 
		///			{
		///				"Name": "Название зала",
		///				"DateCreation": "Дата"
		///			}
		///			 
		/// </remarks>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает объект зала
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"Name": "Название зала",
		///				"DateCreation": "Дата"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostHall")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostHall([FromBody]Hall obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.GetType().Name, obj);

			Hall hall = _db.GetHall(obj).Result;

			if(hall != null)
			{
				return CreatedAtAction("GetHall", new { hall.id }, hall);
			}
			return BadRequest();
		}

		/// <summary>
		/// Добавление зала к конкретному кинотеатру. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект HallCinema(idCinema, idHall)</param>
		/// <remarks>
		/// 
		///			{
		///				"idCinema": "Ключ Cinema",
		///				"idHall": "Ключ зала"
		///			}
		///			 
		/// </remarks>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает объект зала
		/// <remarks>
		/// 
		///			{
		///				"id": "60292c8c5f3d9b3af81083cb",
		///				"idCinema": "Ключ Cinema",
		///				"idHall": "Ключ зала"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostHallCinema")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostHallCinema([FromBody]HallCinema obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.GetType().Name, obj);

			HallCinema hallCinema = _db.GetHallCinema(obj).Result;

			if (hallCinema != null)
			{
				return CreatedAtAction("GetHallCinema", new { hallCinema.id }, hallCinema);
			}
			return BadRequest();
		}

		/// <summary>
		/// Удаление зала из таблицы Hall, а так же удаляется из таблицы HallCinema. Authorize = Administator
		/// </summary>
		/// <param name="id">Ключ Hall</param>
		/// <returns></returns>
		/// <response code="204">Сообщает что объект был удален из базы</response>
		/// <response code="400">Если по каим либо причинам объект не был удален</response>
		[HttpDelete("DeleteHall/{id}")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult RemoveHall(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			_db.RemoveHall(id);
			_db.RemoveHallCinema(id);

			return NoContent();
		}
	}
}
