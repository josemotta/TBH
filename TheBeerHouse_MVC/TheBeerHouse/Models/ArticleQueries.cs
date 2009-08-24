using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;

namespace TheBeerHouse.Models
{
	/// <summary>
	/// This data queries are for chapter 6 of the book.
	/// </summary>
	public static class ArticleQueries
	{
		/// <summary>
		/// Gets the article.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static Article GetArticle(this Table<Article> source, int id)
		{
			return source.SingleOrDefault(a => a.ArticleID == id);
		}

		/// <summary>
		/// Gets the comment.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static Category GetCategory(this Table<Category> source, int id)
		{
			return source.SingleOrDefault(c => c.CategoryID == id);
		}

		/// <summary>
		/// Gets the category.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public static Category GetCategory(this Table<Category> source, string path)
		{
			return source.SingleOrDefault(c => c.Path == path);
		}

		/// <summary>
		/// Gets the comment.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static Comment GetComment(this Table<Comment> source, int id)
		{
			return source.SingleOrDefault(c => c.CommentID == id);
		}


		/// <summary>
		/// Checks to see if the specified Category exists.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static bool Exists(this Table<Category> source, int id)
		{
			return source.Count(c => c.CategoryID == id) > 0;
		}

		/// <summary>
		/// Gets the categories.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static IEnumerable<Category> GetCategories(this Table<Category> source)
		{
			return from c in source
				   orderby c.Importance, c.Title
				   select c;
		}

		/// <summary>
		/// Gets the published articles by category.
		/// </summary>
		/// <param name="dataContext">The data context.</param>
		/// <param name="categoryId">The category id.</param>
		/// <returns></returns>
		private static IQueryable<Article> GetPublishedArticles(TheBeerHouseDataContext dataContext, string category)
		{
			// make sure that category evaluates to null incase it comes in as an empty string
			category = String.IsNullOrEmpty(category) ? null : category;

			bool isAuthenticated = false;
			HttpContext context = HttpContext.Current;

			if (context.User != null && context.User.Identity != null)
				isAuthenticated = context.User.Identity.IsAuthenticated;

			var query = from a in dataContext.Articles
						orderby a.ReleaseDate descending
						where a.Approved == true
							&& a.Listed == true
							&& (isAuthenticated == true || a.OnlyForMembers == false)
							&& a.ReleaseDate <= DateTime.Now
							&& (a.ExpireDate == null || a.ExpireDate > DateTime.Now)
							&& (category == null || a.Category.Path == category)
						select a;

			return query;
		}

		/// <summary>
		/// Gets the published articles by category.
		/// </summary>
		/// <param name="dataContext">The data context.</param>
		/// <param name="categoryId">The category id.</param>
		/// <returns></returns>
		private static IQueryable<Article> GetArticles(TheBeerHouseDataContext dataContext, string category)
		{
			// make sure that category always evaluates to null incase it comes in as an empty string
			category = String.IsNullOrEmpty(category) ? null : category;

			var query = from a in dataContext.Articles
						orderby a.ReleaseDate descending
						where category == null || a.Category.Path == category
						select a;

			return query;
		}

		/// <summary>
		/// Gets the articles.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public static Pagination<Article> GetPublishedArticles(this Table<Article> source, int page)
		{
			int count = Configuration.TheBeerHouseSection.Current.Articles.PageSize;
			int index = (page -1) * count;

			return new Pagination<Article>(
				new ArticleCollectionWrapper(GetPublishedArticles(source.Context as TheBeerHouseDataContext, null).Skip(index).Take(count)),
				index,
				count,
				GetPublishedArticlesCount(source)
			);
		}

		/// <summary>
		/// Gets the published articles by category.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="categoryId">The category id.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public static Pagination<Article> GetPublishedArticles(this Table<Article> source, string category, int page)
		{
			int count = Configuration.TheBeerHouseSection.Current.Articles.PageSize;
			int index = (page - 1) * count;

			return new Pagination<Article>(
				new ArticleCollectionWrapper(GetPublishedArticles(source.Context as TheBeerHouseDataContext, category).Skip(index).Take(count)),
				index,
				count,
				GetPublishedArticlesCount(source, category)
			);
		}

		/// <summary>
		/// Gets the articles count.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static int GetPublishedArticlesCount(this Table<Article> source)
		{
			return GetPublishedArticles(source.Context as TheBeerHouseDataContext, null).Count();
		}

		/// <summary>
		/// Gets the published articles by category count.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="categoryId">The category id.</param>
		/// <returns></returns>
		public static int GetPublishedArticlesCount(this Table<Article> source, string category)
		{
			return GetPublishedArticles(source.Context as TheBeerHouseDataContext, category).Count();
		}

		/// <summary>
		/// Gets the articles.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public static Pagination<Article> GetArticles(this Table<Article> source, int page)
		{
			int count = Configuration.TheBeerHouseSection.Current.Articles.PageSize;
			int index = (page - 1) * count;

			return new Pagination<Article>(
				new ArticleCollectionWrapper(
					GetArticles(
						source.Context as TheBeerHouseDataContext, 
						null
						).Skip(index).Take(count)),
				index,
				count,
				GetArticlesCount(source)
			);
		}

		/// <summary>
		/// Gets the published articles by category.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="categoryId">The category id.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public static Pagination<Article> GetArticles(this Table<Article> source, string category, int page)
		{
			int count = Configuration.TheBeerHouseSection.Current.Articles.PageSize;
			int index = (page - 1) * count;

			return new Pagination<Article>(
				new ArticleCollectionWrapper(GetArticles(source.Context as TheBeerHouseDataContext, category).Skip(index).Take(count)),
				index,
				count,
				GetArticlesCount(source, category)
			);
		}

		/// <summary>
		/// Gets the articles count.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static int GetArticlesCount(this Table<Article> source)
		{
			return GetArticles(source.Context as TheBeerHouseDataContext, null).Count();
		}

		/// <summary>
		/// Gets the published articles by category count.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="categoryId">The category id.</param>
		/// <returns></returns>
		public static int GetArticlesCount(this Table<Article> source, string category)
		{
			return GetArticles(source.Context as TheBeerHouseDataContext, category).Count();
		}

		/// <summary>
		/// Gets the comments for moderation.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public static Pagination<Comment> GetCommentsForModeration(this Table<Comment> source, int page)
		{
			int count = Configuration.TheBeerHouseSection.Current.Articles.PageSize;
			int index = (page - 1) * count;

			var query = from c in source
						orderby c.AddedDate descending
						select c;

			return new Pagination<Comment>(
				query.Skip(index).Take(count),
				index,
				count,
				source.Count()
			);
		}
	}
}
