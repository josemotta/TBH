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
	public partial class Comment
	{
		/// <summary>
		/// Gets the encoded body.
		/// </summary>
		/// <value>The encoded body.</value>
		public string EncodedBody
		{
			get { return HttpContext.Current.Server.HtmlEncode(Body); }
		}
	}
}
