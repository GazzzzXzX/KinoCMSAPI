using KinoCMSAPI.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KinoCMSAPI.Mocks
{
	public class MockRegistration
	{
		public String UserName { get; set; }
		public String Email { get; set; }
		public String Password { get; set; }
		public String Role { get; set; }

		private enum _Role
		{
			Administator,
			User
		}

		public Byte[] GetHashPasword()
		{
			var md5 = MD5.Create();

			return md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
		}

		public Boolean IsValidPassword()
		{
			var hasNumber = new Regex(@"[0-9]+");
			var hasUpperChar = new Regex(@"[A-Z]+");
			var hasMinimum8Chars = new Regex(@".{8,}");

			return hasNumber.IsMatch(Password) && hasUpperChar.IsMatch(Password) && hasMinimum8Chars.IsMatch(Password);
		}

		public Boolean IsValidEmail()
		{
			string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
			var regex = new Regex(pattern, RegexOptions.IgnoreCase);
			return regex.IsMatch(Email);
		}

		public Boolean CheckNameUser(DataBase db)
		{
			User user = db.GetUser(UserName).Result;

			return user == null;
		}

		public Boolean CheckEmail(DataBase db)
		{
			User user = db.GetUserEmail(Email).Result;

			return user == null;
		}

		public Boolean CheckRole()
		{
			return Role == _Role.Administator.ToString() || Role == _Role.User.ToString();
		}

		public User GetUser(DataBase db)
		{
			User user;
			if (UserName != null)
			{
				user = db.GetUser(UserName).Result;
			}
			else if(Email != null)
			{
				user = db.GetUserEmail(Email).Result;
			}
			else
			{
				return null;
			}

			if(user == null)
			{
				return null;
			}

			Byte[] bPass = GetHashPasword();
			String tempPass = Convert.ToBase64String(bPass);
			String pass = Convert.ToBase64String(user.Password);

			if(tempPass == pass)
			{
				return user;
			}
			return null;
		}
	}
}
