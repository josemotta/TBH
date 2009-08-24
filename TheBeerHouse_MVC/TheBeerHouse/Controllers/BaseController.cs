using System;
using System.Web.Mvc;

namespace TheBeerHouse.Controllers
{
	public abstract class BaseController : Controller
	{
		/// <summary>
		/// Gets a value indicating whether this instance is post back.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if this instance is post back; otherwise, <see langword="false"/>.
		/// </value>
		public virtual bool IsPostBack
		{
			get
			{
				return Uri.Compare(
					Request.UrlReferrer,
					Request.Url,
					UriComponents.Path,
					UriFormat.Unescaped,
					StringComparison.OrdinalIgnoreCase
					) == 0 && Request.HttpMethod == "POST";
			}
		}
	}
}
