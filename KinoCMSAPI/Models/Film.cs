using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	public class Film
	{
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public String id { get; set; }
        public String Name { get; set; }
        public String Desc { get; set; }
        public String Img { get; set; }
        public UInt16 Price { get; set; }
        public String UrlVideo { get; set; }
        public Boolean D3 { get; set; }
        public Boolean D2 { get; set; }
        public Boolean Imax { get; set; }
    }
}
