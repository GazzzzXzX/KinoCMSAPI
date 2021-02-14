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
	public class ScheduleController : Controller
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Возвращение всех расписаний.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список расписаний
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ"
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			},
		///			{
		///				"id": "Ключ"
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetSchedules")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetSchedules()
		{
			List<TimeTableFilms> timeTableFilms = _db.GetTimetableFilms().Result;
			if (timeTableFilms.Count != 0)
			{
				return Ok(timeTableFilms);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение конкретное расписание.
		/// </summary>
		/// <param name="id">ключ расписания</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает расписание
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ"
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetFilmSchedule/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetFilmSchedule(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}
			TimeTableFilms timeTableFilms = _db.GetTimeTableFilm(id).Result;
			if (timeTableFilms != null)
			{
				return Ok(timeTableFilms);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение расписание по конкретному фильму.
		/// </summary>
		/// <param name="id">Ключ фильма</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает расписание
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ"
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			},
		///			{
		///				"id": "Ключ"
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetMovieSchedule/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetMovieSchedule(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}
			List<TimeTableFilms> timeTables = _db.GetTimetableFilms(id).Result;
			if (timeTables.Count != 0)
			{
				return Ok(timeTables);
			}
			return NotFound();
		}

		/// <summary>
		/// Запись одного расписания. Authorize = Administator
		/// </summary>
		/// <param name="obj"></param>
		/// <remarks>
		/// 
		///			{
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			}
		///			 
		/// </remarks>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает расписание
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ"
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostSchedule")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult PostSchedule([FromBody] TimeTableFilms obj)
		{
			if (obj == null)
			{
				return BadRequest();
			}
			_db.SetValue(obj.GetType().Name, obj);

			TimeTableFilms timeTable = _db.GetTimeTableFilm(obj).Result;

			return CreatedAtAction("GetFilmSchedule", new { timeTable.id }, timeTable);
		}

		/// <summary>
		/// Обновление конкретного расписания. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект TimeTableFilms</param>
		/// <returns></returns>
		/// <remarks>
		/// 
		///			{
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			}
		///			 
		/// </remarks>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает расписание
		/// <remarks>
		/// 
		///			{
		///				"id": "Ключ"
		///				"dateTime": "Дата",
		///				"idFilm": "Название фильма"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPut("PutSchedule")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult PutSchedule([FromBody]TimeTableFilms obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}
			TimeTableFilms timeTable = _db.GetTimeTableFilm(obj.id).Result;
			if(timeTable == null)
			{
				return NotFound();
			}
			timeTable.idFilm = obj.idFilm;
			timeTable.dateTime = obj.dateTime;

			_db.Save(timeTable.GetType().Name, timeTable.id, timeTable);

			return Ok(timeTable);
		}

		/// <summary>
		/// Удаление одной записи из таблицы расписания. Authorize = Administator
		/// </summary>
		/// <param name="id">Ключ расписания</param>
		/// <returns></returns>
		/// <response code="204">Сообщает что объект был удален из базы</response>
		/// <response code="400">Если по каим либо причинам объект не был удален</response>
		[HttpDelete("DeleteSchedule/{id}")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult DeleteSchedule(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}
			_db.RemoveTimeTableFilm(id);
			return NoContent();
		}
	}
}
