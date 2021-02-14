using KinoCMSAPI.Mocks;
using KinoCMSAPI.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KinoCMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private DataBase _db = Singleton.GetInstance().Context;

        /// <summary>
        /// Возращение пользователя. Authorization
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <response code="200">
        /// Возвращает имя пользователя
        /// <remarks>
        /// 
        ///			{
        ///				"UserName": "Имя пользователя",
        ///				"Email": "string",
        ///				"Role": "Роль",
        ///				"FIO": "ФИО",
        ///				"Number":"Номер",
        ///				"Address": "Адресс",
        ///				"NumberCard": "Номер карты",
        ///				"Birthday": "Дата"
        ///			}
        ///			 
        /// </remarks>
        /// </response>
        /// <response code="400">Ошибка в запросе</response>
        [Authorize]
        [HttpGet("GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetUser(String userName)
		{
            if(userName == null)
			{
                return BadRequest();
			}

            User user = _db.GetUser(userName).Result;

            if(user == null)
			{
                ModelState.AddModelError("User", "Ошибка при вополнении запроса");
                return BadRequest(ModelState);
            }

            return Ok(user);
		}

        /// <summary>
        /// Авторизация пользователя, доступна по нескольким параметрам. Email или UserName, Password.
        /// </summary>
        /// <param name="obj">Email: String or UserName: String
        /// Password: String</param>
        /// <returns></returns>
        /// <remarks>
		/// 
		///			Authorization /ToDo
		///			{
		///				"UserName": "Имя пользователя" // "Email": "string" 
		///				"Password": "Пароль"
		///			}
		///			 
		/// </remarks>x
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает имя пользователя и токен
		/// <remarks>
		/// 
		///			{
		///				"access_token": "String"
		///				"username": "Пароль"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
        [HttpPost("Authorization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Authorization([FromBody] MockRegistration obj)
        {
            User user = obj.GetUser(_db);

            if (user == null)
            {
                ModelState.AddModelError("User:", "Пользователь не найден или пароль был введен не верно!");
                return BadRequest(ModelState);
            }

            ClaimsIdentity identity = GetIdentity(user.UserName);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username." });
            }

            return Token(identity);
        }

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        /// <param name="obj">Объект MockRegistration:
        /// UserName: String
        /// Email: String
        /// Password: String
        /// Role: String</param>
        /// <returns></returns>
        /// <remarks>
		/// 
		///			Registration /ToDo
		///			{
		///				"UserName": "Имя пользователя",
        ///				"Email": "string",
        ///				"Role": "Роль"
		///				"Password": "Пароль"
		///			}
		///			 
		/// </remarks>x
		/// <returns></returns>
		/// <response code="201">
		/// Возвращает имя пользователя и токен
		/// <remarks>
		/// 
		///			{
		///				"access_token": "String"
		///				"username": "Пароль"
		///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
        [HttpPost("Registration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Registration([FromBody] MockRegistration obj)
		{
            if(obj == null)
			{
                return BadRequest();
			}

            if(!obj.IsValidPassword())
			{
                ModelState.AddModelError("Password", "Пароль должен содержать 8 символов, иметь хотя бы одну заклавную букву, одну цифру, один спецальный символ.");
                return BadRequest(ModelState);
			}
           
            if(!obj.IsValidEmail())
			{
                ModelState.AddModelError("Email", "Email введен не верно.");
                return BadRequest(ModelState);
            }

            if(!obj.CheckNameUser(_db))
			{
                ModelState.AddModelError("UserName", "Пользователь с таким ником уже существует!");
                return BadRequest(ModelState);
            }

            if (!obj.CheckEmail(_db))
            {
                ModelState.AddModelError("UserName", "Данный Email уже существует!");
                return BadRequest(ModelState);
            }

            if (!obj.CheckRole())
            {
                ModelState.AddModelError("Role", "Данной роли не существует!");
                return BadRequest(ModelState);
            }

            User user = new User();

            user.Email = obj.Email;
            user.UserName = obj.UserName;
            user.Password = obj.GetHashPasword();
            user.Role = obj.Role;

            await _db.SetValue(user.GetType().Name, user);

            var identity = GetIdentity(user.UserName);

            return Token(identity);
        }

        /// <summary>
        /// Изменение или добавление отсальных записей к пользователю. Authorize
        /// </summary>
        /// <param name="obj">UserName: String - не изменяется нужен для идентификации пользователя.
        /// FIO: String
        /// Number: String
        /// Address: String
        /// NumberCard: String
        /// Birthday: DateTime</param>
        /// <returns></returns>
        /// <remarks>
        /// 
        ///			PutUser /ToDo
        ///			{
        ///				"UserName": "Имя пользователя",
        ///				"Email": "string",
        ///				"Role": "Роль",
        ///				"FIO": "ФИО",
        ///				"Number":"Номер",
        ///				"Address": "Адресс",
        ///				"NumberCard": "Номер карты",
        ///				"Birthday": "Дата"
        ///			}
        ///			 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">
        /// Возвращает имя пользователя
        /// <remarks>
        /// 
        ///			{
        ///				"UserName": "Имя пользователя",
        ///				"Email": "string",
        ///				"Role": "Роль",
        ///				"FIO": "ФИО",
        ///				"Number":"Номер",
        ///				"Address": "Адресс",
        ///				"NumberCard": "Номер карты",
        ///				"Birthday": "Дата"
        ///			}
        ///			 
        /// </remarks>
        /// </response>
        /// <response code="400">Ошибка в запросе</response>
        [Authorize]
        [HttpPut("PutUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PutUser([FromBody] User obj)
		{
            if(obj == null)
			{
                return BadRequest();
			}

            User user = _db.GetUser(obj.UserName).Result;

            user.FIO = obj.FIO;
            user.Birthday = obj.Birthday;
            user.Address = obj.Address;
            user.Number = obj.Number;
            user.NumberCard = obj.NumberCard;

            _db.Save(user.GetType().Name, user.UserName, user);

            return Ok(user);
		}

        /// <summary>
        /// Изменение роли пользователя. Authorize - Administator
        /// </summary>
        /// <param name="obj">UserName: String - не изменяется нужен для идентификации пользователя.
        /// Role: String (принимает значение Administator или User)</param>
        /// <returns></returns>
        /// <remarks>
		/// 
		///			PutRole /ToDo
		///			{
		///				"UserName": "Имя пользователя"
		///				"Role": "Роль"
		///			}
		///			 
		/// </remarks>x
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает имя пользователя и токен
		/// <remarks>
		/// 
		///			{
        ///				"UserName": "Имя пользователя",
        ///				"Email": "string",
        ///				"Role": "Роль",
        ///				"FIO": "ФИО",
        ///				"Number":"Номер",
        ///				"Address": "Адресс",
        ///				"Birthday": "Дата"
        ///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
        [Authorize(Roles = "Administator")]
        [HttpPut("PutRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PutRole([FromBody] MockRegistration obj)
		{
            if (obj == null)
            {
                return BadRequest();
            }

            User user = _db.GetUser(obj.UserName).Result;

            if (!obj.CheckRole())
            {
                ModelState.AddModelError("Role", "Данной роли не существует!");
                return BadRequest(ModelState);
            }

            user.Role = obj.Role;

            _db.Save(user.GetType().Name, user.UserName, user);
            user.NumberCard = "";

            return Ok(user);
        }

        /// <summary>
        /// Изменение пароля. Authorize
        /// </summary>
        /// <param name="obj">UserName: String - не изменяется нужен для идентификации пользователя.
        /// Password: String (принимает значение Administator или User)</param>
        /// <returns></returns>
        /// <remarks>
		/// 
		///			PutPassword /ToDo
		///			{
		///				"UserName": "Имя пользователя"
		///				"Password": "Пароль"
		///			}
		///			 
		/// </remarks>x
		/// <returns></returns>
		/// <response code="200">
		/// Возвращает имя пользователя и токен
		/// <remarks>
		/// 
		///			{
        ///				"UserName": "Имя пользователя",
        ///				"Email": "string",
        ///				"Role": "Роль",
        ///				"FIO": "ФИО",
        ///				"Number":"Номер",
        ///				"Address": "Адресс",
        ///				"NumberCard": "Номер карты",
        ///				"Birthday": "Дата"
        ///			}
		///			 
		/// </remarks>
		/// </response>
		/// <response code="400">Ошибка в запросе</response>
        [Authorize]
        [HttpPut("PutPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PutPassword([FromBody] MockRegistration obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }

            User user = _db.GetUser(obj.UserName).Result;

            if (!obj.IsValidPassword())
            {
                ModelState.AddModelError("Password", "Пароль должен содержать 8 символов, иметь хотя бы одну заклавную букву, одну цифру, один спецальный символ.");
                return BadRequest(ModelState);
            }
            user.Password = obj.GetHashPasword();

            _db.Save(user.GetType().Name, user.UserName, user);

            return Ok(user);
        }

        private JsonResult Token(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username)
        {
            User person = _db.GetUser(username).Result;
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}

