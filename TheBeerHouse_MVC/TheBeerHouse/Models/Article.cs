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
	/// <summary>
	/// The article object outlined in chapter 6 of the book.
	/// </summary>
	public partial class Article
	{
		/// <summary>
		/// Gets the location.
		/// </summary>
		/// <value>The location.</value>
		public string Location
		{
			get
			{
				string city = this.City ?? String.Empty;
				string state = this.State ?? String.Empty;
				string country = this.Country ?? String.Empty;

				string location = city.Split(';')[0];
				if (state.Length > 0)
				{
					if (location.Length > 0)
						location += ", ";
					location += state.Split(';')[0];
				}
				if (country.Length > 0)
				{
					if (location.Length > 0)
						location += ", ";
					location += country.Split(';')[0];
				}
				return location;
			}
		}

		/// <summary>
		/// Gets the average rating.
		/// </summary>
		/// <value>The average rating.</value>
		public double AverageRating
		{
			get
			{
				if (this.Votes >= 1)
					return ((double)this.TotalRating / (double)this.Votes);
				else
					return 0D;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Article"/> is published.
		/// </summary>
		/// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
		public bool Published
		{
			get
			{
				return (this.Approved && this.ReleaseDate <= DateTime.Now && this.ExpireDate > DateTime.Now);
			}
		}

		/// <summary>
		/// Increments the view count.
		/// </summary>
		public void IncrementViewCount()
		{
			ViewCount++;
		}

		/// <summary>
		/// Rates the specified rating.
		/// </summary>
		/// <param name="rating">The rating.</param>
		/// <returns></returns>
		public void Rate(int rating)
		{
			Votes++;
			TotalRating += rating;
		}
	}
}
