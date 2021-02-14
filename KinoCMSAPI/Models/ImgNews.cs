using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	public class ImgNews
	{
		[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
		public String id { get; set; }
		public String Img { get; set; }
		public String idNews { get; set; }
	}
}
