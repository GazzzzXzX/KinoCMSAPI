using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	public class Screenwriter
	{
		[BsonId]
		public String Name { get; set; }
	}
}
