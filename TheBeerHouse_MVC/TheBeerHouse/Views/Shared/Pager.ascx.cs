using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheBeerHouse.Views.Shared
{
	public partial class Pager : ViewUserControl<Models.IPagination>
	{
		protected const int PageBuffer = 3;
		private string _currentAction = String.Empty;

		/// <summary>
		/// Gets the current action.
		/// </summary>
		/// <value>The current action.</value>
		public string CurrentAction
		{
			get
			{
				if (String.IsNullOrEmpty(_currentAction))
					_currentAction = Html.ViewContext.RouteData.Values["action"] as string;

				return _currentAction;
			}
		}

		/// <summary>
		/// Gets the page URL.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public string GetPageUrl(int page)
		{
			if (page < 1)
				throw new ArgumentOutOfRangeException("page", "'page' must be greater than or equal to 1");

			return Url.Action(
				this.ViewContext.RouteData.Values["action"] as string,
				new { page = page }
			);
		}

		/// <summary>
		/// Gets the start and end page.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		public void GetStartAndEndPage(out int start, out int end)
		{
			if (ViewData.Model.PageCount <= PageBuffer)
			{
				start = 1;
				end = ViewData.Model.PageCount;
			}
			else
			{
				start = Math.Max(ViewData.Model.PageNumber - PageBuffer, 1);
				end = Math.Min(ViewData.Model.PageNumber + PageBuffer, ViewData.Model.PageCount);
			}
		}
	}
}
