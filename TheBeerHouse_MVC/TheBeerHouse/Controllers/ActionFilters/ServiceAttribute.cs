using System;
using System.Web.Mvc;

using ManagedFusion.Web.Mvc;

namespace TheBeerHouse.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceAttribute"/> class.
		/// </summary>
		public ServiceAttribute()
		{
			Order = 0;
		}

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string type = filterContext.HttpContext.Request.QueryString["type"];
			ResponseType responseType = ResponseType.Html;

			// check to see if we should try to parse it to an enum
			if (!String.IsNullOrEmpty(type))
				responseType = ManagedFusion.Utility.ParseEnum<ResponseType>(type);

			// if the response type is still the default HTML check the Accept header
			// if the requestion is an XMLHttpRequest
			if (responseType == ResponseType.Html
				&& filterContext.HttpContext.Request.AcceptTypes != null
				&& String.Equals(filterContext.HttpContext.Request.Headers["x-requested-with"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
			{
				foreach (string accept in filterContext.HttpContext.Request.AcceptTypes)
				{
					switch (accept.ToLower())
					{
						case "application/json":
						case "application/x-json": responseType = ResponseType.Json; break;

						case "application/javascript":
						case "application/x-javascript":
						case "text/javascript": responseType = ResponseType.JS; break;

						case "application/xml":
						case "text/xml": responseType = ResponseType.Xml; break;

						case "application/atom+xml": responseType = ResponseType.Atom; break;
					}

					if (responseType != ResponseType.Html)
						break;
				}
			}

			if (filterContext.RouteData.Values.ContainsKey("responseType"))
				filterContext.RouteData.Values.Remove("responseType");

			// set the value in the route data so it can be used in the methods
			filterContext.RouteData.Values.Add("responseType", responseType);
		}

		/// <summary>
		/// Called when [action executed].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Result is ViewResult)
			{
				ViewResult result = filterContext.Result as ViewResult;

				switch ((ResponseType)filterContext.RouteData.Values["responseType"])
				{
					case ResponseType.Html:
						goto default;

					case ResponseType.JS:
						filterContext.Result = new JavaScriptCallbackResult {
							Data = result.ViewData.Model
						};
						break;

					case ResponseType.Json:
						filterContext.Result = new ManagedFusion.Web.Mvc.JsonResult {
							Data = result.ViewData.Model
						};
						break;

					case ResponseType.Xml:
						filterContext.Result = new XmlResult {
							Data = result.ViewData.Model
						};
						break;

					case ResponseType.Atom:
						filterContext.Result = new AtomResult {
							Data = result.ViewData.Model
						};
						break;

					default:
						filterContext.Result = result;
						break;
				}
			}
		}
	}
}
