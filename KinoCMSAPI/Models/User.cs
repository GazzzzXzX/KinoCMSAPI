using MongoDB.Bson.Serialization.Attributes;

using System;

namespace KinoCMSAPI.Models
{
	public class User
	{
		[BsonId]
		public String UserName { get; set; }
		public String FIO { get; set; }
		public String Number { get; set; }
		public String Email { get; set; }
		public String Address { get; set; }
		public String NumberCard { get; set; }
		public DateTime Birthday { get; set; }
		public String Role { get; set; }
		public Byte[] Password { get; set; }
	}
}
