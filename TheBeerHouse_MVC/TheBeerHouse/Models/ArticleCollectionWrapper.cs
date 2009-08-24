using System;
using System.Collections.Generic;
using System.Web;

namespace TheBeerHouse.Models
{
	/// <summary>
	/// 
	/// </summary>
	internal class ArticleCollectionWrapper : IEnumerable<Article>
	{
		private IEnumerable<Article> _articles;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArticleCollectionWrapper"/> class.
		/// </summary>
		/// <param name="articles">The articles.</param>
		public ArticleCollectionWrapper(IEnumerable<Article> articles)
		{
			_articles = articles;
		}

		#region IEnumerable<Article> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<Article> GetEnumerator()
		{
			bool isAuthenticated = false;
			HttpContext context = HttpContext.Current;

			if (context.User != null && context.User.Identity != null)
				isAuthenticated = context.User.Identity.IsAuthenticated;

			foreach (Article article in _articles)
			{
				// make sure that only members see articles marked as members only
				if (article.OnlyForMembers && !isAuthenticated)
					continue;

				yield return article;
			}
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
