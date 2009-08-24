using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace TheBeerHouse.Models
{
	public interface IPagination
	{
		/// <summary>
		/// Gets the current page number.
		/// </summary>
		/// <value>The page number.</value>
		int PageNumber { get; }

		/// <summary>
		/// Gets the size of the page.
		/// </summary>
		/// <value>The size of the page.</value>
		int PageSize { get; }

		/// <summary>
		/// Gets the total page count.
		/// </summary>
		/// <value>The total page count.</value>
		int PageCount { get; }

		/// <summary>
		/// Gets the start index of the item.
		/// </summary>
		/// <value>The start index of the item.</value>
		int StartIndex { get; }

		/// <summary>
		/// Gets the total item count.
		/// </summary>
		/// <value>The total item count.</value>
		int TotalCount { get; }
	}
}
