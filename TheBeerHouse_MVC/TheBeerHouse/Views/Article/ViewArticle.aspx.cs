using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheBeerHouse.Views.Article
{
	public partial class View : ViewPage<Models.Article>
	{
		public string ImageRatingUrl
		{
			get
			{
				double value = ViewData.Model.AverageRating;
				string url = "/Content/images/stars{0}.gif";
				if (value <= 1.3)
					url = String.Format(url, "10");
				else if (value <= 1.8)
					url = String.Format(url, "15");
				else if (value <= 2.3)
					url = String.Format(url, "20");
				else if (value <= 2.8)
					url = String.Format(url, "25");
				else if (value <= 3.3)
					url = String.Format(url, "30");
				else if (value <= 3.8)
					url = String.Format(url, "35");
				else if (value <= 4.3)
					url = String.Format(url, "40");
				else if (value <= 4.8)
					url = String.Format(url, "45");
				else
					url = String.Format(url, "50");

				return url;
			}
		}
	}
}
