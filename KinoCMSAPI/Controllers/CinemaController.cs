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
	public class CinemaController : Controller
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Возвращает все кинотеатры.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список кинотеатров
		/// <remarks>
		/// 
		///			 {
		///				"name": "Дюк",
		///				"desc": "Лучший кинотеатр",
		///				"conditions": "Приходить в маске!",
		///				"img": "url"
		///			 },
		///			 {
		///				"name": "Дюк2",
		///				"desc": "Лучший кинотеатр2",
		///				"conditions": "Приходить в маске!2",
		///				"img": "url"
		///			 }
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в вополнении запроса</response>
		/// <response code="400">База данных пуста</response> 
		[HttpGet("GetCinemas")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetCinemas()
		{
			List<Cinema> cinemas = _db.GetCinemas().Result;
			if (cinemas.Count != 0)
			{
				return Ok(cinemas);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращает конкретный кинотеатр.
		/// </summary>
		/// <param name="name">Ключ объекта Cinema (String)</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает конкретный кинотеатр
		/// <remarks>
		/// 
		///			 {
		///				"name": "Дюк",
		///				"desc": "Лучший кинотеатр",
		///				"conditions": "Приходить в маске!",
		///				"img": "url"
		///			 }
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в вополнении запроса</response>     
		[HttpGet("GetCinema/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult GetCinema(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			Cinema cinema = _db.GetCinema(name).Result;

			if(cinema != null)
			{
				return Ok(cinema);
			}
			return NotFound();
		}

		/// <summary>
		/// Добавление кинотеатра. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект Cinema(Name, Desc, Conditions, Img)</param>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает только что созданный объект
		/// <remarks>
		/// 
		///			 {
		///				"name": "Дюк",
		///				"desc": "Лучший кинотеатр",
		///				"conditions": "Приходить в маске!",
		///				"img": "url"
		///			 }
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка при выполнении запроса</response>       
		/// <response code="400">Данное поле отсутсвует в безе</response> 
		[HttpPost("PostCinema")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> PostCinema([FromBody]Cinema obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}
			await _db.SetValue(obj.GetType().Name, obj);

			Cinema cinema = _db.GetCinema(obj.Name).Result;
			if (cinema != null)
			{
				return CreatedAtAction("GetCinema", new { cinema.Name }, cinema);
			}
			return BadRequest();
		}

		/// <summary>
		/// Изменение конеретного кинотеатра. Authorize = Administator
		/// </summary>
		/// <param name="obj">Объект Cinema(Name, Desc, Conditions, Img)</param>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает только что изменившийся объект
		/// <remarks>
		/// 
		///			 {
		///				"name": "Дюк",
		///				"desc": "Лучший кинотеатр",
		///				"conditions": "Приходить в маске!",
		///				"img": "url"
		///			 }
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Если по каим либо причинам объект не был изменен</response>
		[HttpPost("PutCinema")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PutCinema([FromBody] Cinema obj)
		{
			if (obj == null)
			{
				return BadRequest();
			}
			Cinema cinema = _db.GetCinema(obj.Name).Result;

			cinema.Desc = obj.Desc;
			cinema.Conditions = obj.Conditions;
			cinema.Img = obj.Img;

			_db.Save(cinema.GetType().Name, cinema.Name, cinema);

			if (cinema != null)
			{
				return CreatedAtAction("GetCinema", new { cinema.Name }, cinema);
			}
			return BadRequest();
		}

		/// <summary>
		/// Удаление конкретного кинотеатра. Authorize = Administator
		/// </summary>
		/// <param name="name">Ключ объекта Cinema</param>
		/// <returns></returns>
		/// <response code="204">Сообщает что объект был удален из базы</response>
		/// <response code="400">Если по каим либо причинам объект не был удален</response>
		[HttpDelete("DeleteCinema/{name}")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult DeleteCinema(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			_db.RemoveCinema(name);

			return NoContent();
		}
	}
}
