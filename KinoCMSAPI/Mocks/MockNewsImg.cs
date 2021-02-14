using KinoCMSAPI.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoCMSAPI.Mocks
{
	public class MockNewsImg
	{
		
		public News news { get; set; }
		
		public List<ImgNews> Imgs { get; set; }

		public MockNewsImg()
		{
			Imgs = new List<ImgNews>();
		}

		public MockNewsImg(News news, List<ImgNews> imgs)
		{
			this.news = news;
			Imgs = imgs;
		}
	}
}
