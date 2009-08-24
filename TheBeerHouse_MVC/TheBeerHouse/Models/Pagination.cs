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

using System.Collections.Generic;

namespace TheBeerHouse.Models
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Pagination<T> : IEnumerable<T>, IPagination
	{
		private IEnumerable<T> _collection;

		/// <summary>
		/// Initializes a new instance of the <see cref="PageableCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="requestedCount">The requested count.</param>
		/// <param name="totalCount">The total count.</param>
		public Pagination(IEnumerable<T> collection, int startIndex, int requestedCount, int totalCount)
		{
			_collection = collection;
			StartIndex = startIndex;
			PageSize = requestedCount;
			TotalCount = totalCount;
		}

		/// <summary>
		/// Gets the total item count.
		/// </summary>
		/// <value>The total item count.</value>
		public virtual int TotalCount { get; protected internal set; }

		/// <summary>
		/// Gets the size of the page.
		/// </summary>
		/// <value>The size of the page.</value>
		public virtual int PageSize { get; protected internal set; }

		/// <summary>
		/// Gets or sets the start index.
		/// </summary>
		/// <value>The start index.</value>
		public virtual int StartIndex { get; protected internal set; }

		/// <summary>
		/// Gets the current page number.
		/// </summary>
		/// <value>The page number.</value>
		public virtual int PageNumber
		{
			get { return (StartIndex / PageSize) + 1; }
		}

		/// <summary>
		/// Gets the total page count.
		/// </summary>
		/// <value>The total page count.</value>
		public virtual int PageCount
		{
			get { return (int)Math.Ceiling((double)TotalCount / (double)PageSize); }
		}

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}