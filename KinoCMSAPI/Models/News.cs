using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	public class News
	{
		[BsonId]
		public String Name { get; set; }
		public String Dsc { get; set; }
		public DateTime DateCreation { get; set; }
		public Boolean Status { get; set; }
		public String UrlVideo { get; set; }
		public String Img { get; set; }
	}
}
