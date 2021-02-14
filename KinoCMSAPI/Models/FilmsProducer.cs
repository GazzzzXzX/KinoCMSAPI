﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	/// <summary>
	/// Связь один-многие. (Многие - фильм, Один - Продюсер)
	/// </summary>
	public class FilmsProducer
	{
		[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
		public String id { get; set; }
		public String idFilm { get; set; }
		public String idProducer { get; set; }
	}
}
