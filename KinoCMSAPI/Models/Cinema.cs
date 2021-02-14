using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	public class Cinema
	{
		[BsonId]
		public String Name { get; set; }
		public String Desc { get; set; }
		public String Conditions { get; set; }
		public String Img { get; set; }
	}
}
