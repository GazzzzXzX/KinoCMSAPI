using KinoCMSAPI.Mocks;
using KinoCMSAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NewsController : Controller
	{
		private DataBase _db = Singleton.GetInstance().Context;

		/// <summary>
		/// Возращение всех новостей.
		/// </summary>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список новостей
		/// <remarks>
		/// 
		///			{
		///				"Name": "Название зала",
		///				"Dsc": "Описание",
		///				"DateCreation": "Дата",
		///				"Status": true/false,
		///				"UrlVideo": "url",
		///				"Img": "url"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetNews")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetNews()
		{
			List<News> news = _db.GetNews().Result;
			if (news != null)
			{
				return Ok(news);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение конкретной новости и со всеми картинками.
		/// </summary>
		/// <param name="name">Ключ новости</param>
		/// <returns></returns>
		///  Возвращает конкретной новости и список изображений
		/// <remarks>
		/// 
		///			{
		///				"Name": "Название зала",
		///				"Dsc": "Описание",
		///				"DateCreation": "Дата",
		///				"Status": true/false,
		///				"UrlVideo": "url",
		///				"Img": "url"
		///			},
		///			{
		///				"id": "ключ"
		///				"Img": "url"
		///				"idNews": "Название новости"
		///			},
		///			{
		///				"id": "ключ"
		///				"Img": "url"
		///				"idNews": "Название новости"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetOneNews/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetOneNews(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			News news = _db.GetNews(name).Result;
			List<ImgNews> imgNews = _db.GetImgsNews(name).Result;

			if(news != null)
			{
				MockNewsImg mockNewsImg = new MockNewsImg(news, imgNews);
				return Ok(mockNewsImg);
			}
			return NotFound();
		}

		/// <summary>
		/// Возвращение всех изображений по конкретной новости.
		/// </summary>
		/// <param name="name">Ключ новости</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает список изображений
		/// <remarks>
		/// 
		///			{
		///				"id": "ключ"
		///				"Img": "url"
		///				"idNews": "Название новости"
		///			},
		///			{
		///				"id": "ключ"
		///				"Img": "url"
		///				"idNews": "Название новости"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("GetImgsNews/{name}")]
		public ActionResult GetImgsNews(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			List<ImgNews> imgNews = _db.GetImgsNews(name).Result;

			if(imgNews != null)
			{
				return Ok(imgNews);
			}
			return NotFound();
		}

		/// <summary>
		/// Возращение конкретного изображения.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// Возвращает изображение
		/// <response code="200">
		/// <remarks>
		/// 
		///			{
		///				"id": "ключ"
		///				"Img": "url"
		///				"idNews": "Название новости"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpGet("GetImgNews/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult GetImgNews(String id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			ImgNews imgNews = _db.GetImgNews(id).Result;
			if(imgNews != null)
			{
				return Ok(imgNews);
			}
			return NotFound();
		}

		/// <summary>
		/// Создание новости и одновременно сохранение картинок. Authorize = Administator
		/// </summary>
		/// <param name="obj">
		/// Прослойка которая хронит в себе 2 объекта News и Array(ImgNews).
		/// News: (Name, Dsc, DateCreation, Status, UrlVideo, Img)
		/// Array(Imgs): (Img, idNews)
		/// </param>
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает список изображений и новость
		/// <remarks>
		/// 
		///			{//News
		///				"Name": "Название зала",
		///				"Dsc": "Описание",
		///				"DateCreation": "Дата",
		///				"Status": true/false,
		///				"UrlVideo": "url",
		///				"Img": "url"
		///			},
		///			{//NewsImg
		///				"id": "ключ"
		///				"Img": "url"
		///				"idNews": "Название новости"
		///			},
		///			{//NewsImg
		///				"id": "ключ"
		///				"Img": "url"
		///				"idNews": "Название новости"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		[HttpPost("PostNews")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> PostNews([FromBody]MockNewsImg obj)
		{
			
			if(obj == null)
			{
				return BadRequest();
			}

			await _db.SetValue(obj.news.GetType().Name, obj.news);
			foreach (var item in obj.Imgs)
			{
				await _db.SetValue(item.GetType().Name, item);
			}
			

			News news = _db.GetNews(obj.news.Name).Result;
			List<ImgNews> imgNews = _db.GetImgsNews(obj.news.Name).Result;
			if (news != null)
			{
				MockNewsImg mockNewsImg = new MockNewsImg(news, imgNews);

				return CreatedAtAction("GetOneNews", new { mockNewsImg.news.Name }, mockNewsImg);
			}
			return BadRequest();
		}

		/// <summary>
		/// Удаление новости, так же удаляються все картинки связанные с этой новостью. Authorize = Administator
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <response code="204">Сообщает что объект был удален из базы</response>
		/// <response code="400">Если по каим либо причинам объект не был удален</response>
		[HttpDelete("DeleteNews/{name}")]
		[Authorize(Roles = "Administator")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult DeleteNews(String name)
		{
			if(name == null)
			{
				return BadRequest();
			}

			_db.RemoveNews(name);
			_db.RemoveImgNews(name);

			return NoContent();
		}

		/// <summary>
		/// Обновление новости
		/// </summary>
		/// <param name="obj">Объект новость: (Name, Dsc, DateCreation, Status, UrlVideo, Img)</param>
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает новость
		/// <remarks>
		/// 
		///			{
		///				"Name": "Название зала",
		///				"Dsc": "Описание",
		///				"DateCreation": "Дата",
		///				"Status": true/false,
		///				"UrlVideo": "url",
		///				"Img": "url"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
		/// <response code="404">Если по каим либо причинам объект не был найден</response>
		[HttpPut("PutNews")]
		[Authorize(Roles = "Administator")]
		public ActionResult PutNews([FromBody]News obj)
		{
			if(obj == null)
			{
				return BadRequest();
			}

			News news = _db.GetNews(obj.Name).Result;

			news.Dsc = obj.Dsc;
			news.DateCreation = obj.DateCreation;
			news.Img = obj.Img;
			news.Status = obj.Status;
			news.UrlVideo = obj.UrlVideo;

			_db.Save(news.GetType().Name, news.Name, news);

			return Ok(news);
		}
	}
}
