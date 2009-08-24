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

namespace TheBeerHouse.Controllers
{
	public enum ResponseType
	{
		/// <summary>
		/// 
		/// </summary>
		None,
		/// <summary>
		/// 
		/// </summary>
		Html,
		/// <summary>
		/// 
		/// </summary>
		Xml,
		/// <summary>
		/// 
		/// </summary>
		Json,
		/// <summary>
		/// 
		/// </summary>
		JS,
		/// <summary>
		/// 
		/// </summary>
		Atom
	}
}
