using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Models
{
	public class Singleton
	{
		private Singleton()
		{
		}

		private static Singleton _instance;

		public static Singleton GetInstance()
		{
			if (_instance != null)
			{
				return _instance;
			}

			_instance = new Singleton
			{
				Context = new DataBase()
			};

			return _instance;
		}

		public DataBase Context { get; set; }
	}
}
