using System;
using System.Web;
using System.Web.Mvc;

using ManagedFusion.Web.Mvc;

using TheBeerHouse.Models;

namespace TheBeerHouse.Controllers
{
	/// <summary>
	/// This controller is for chapter 6 of the book.
	/// </summary>
	[Service, HandleError]
	public class ArticleController : BaseController
	{
		/// <summary>
		/// Indexes the specified category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public ActionResult Index(string category, int page)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Articles.GetPublishedArticles(category, page);
			var viewCategory = dc.Categories.GetCategory(category);

			ViewData["Categories"] = dc.Categories.GetCategories();
			ViewData["PageTitle"] = (viewCategory != null ? viewCategory.Title : "All") + " Articles";

			return View(viewData);
		}

		/// <summary>
		/// Categories the index.
		/// </summary>
		/// <returns></returns>
		public ActionResult CategoryIndex()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Categories.GetCategories();

			ViewData["PageTitle"] = "All Categories";

			return View(viewData);
		}

		/// <summary>
		/// Views the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public ActionResult ViewArticle(int id, string path)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Article viewData = dc.Articles.GetArticle(id);

			// throw a 404 Not Found if the requested article is not in the database
			if (viewData == null)
				throw new HttpException(404, "The article could not be found.");

			// SEO: redirect to the correct location if the path is not
			if (!String.Equals(path, viewData.Path, StringComparison.OrdinalIgnoreCase))
				return this.RedirectToAction(301, "View", new { id = viewData.ArticleID, path = viewData.Path });

			// make sure the article is only viewed by members with permissions
			if (viewData.OnlyForMembers
				&& HttpContext.User != null
				&& HttpContext.User.Identity != null
				&& !HttpContext.User.Identity.IsAuthenticated)
				throw new HttpException(401, "The articles is only viewable for members.");

			// update the view count
			try
			{
				viewData.ViewCount++;
				dc.SubmitChanges();
			}
			catch { /* ignore all conflicts because this action isn't critical */ }

			ViewData["PageTitle"] = viewData.Title;

			return View(viewData);
		}

		/// <summary>
		/// Rates the article.
		/// </summary>
		/// <param name="articleId">The article id.</param>
		/// <param name="rating">The rating.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		public ActionResult RateArticle(int articleId, int rating)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Article viewData = dc.Articles.GetArticle(articleId);

			try
			{
				viewData.Rate(rating);
				dc.SubmitChanges();
			}
			catch { /* ignore all conflicts because this action isn't critical */ }

			return View(new { articleId = articleId, averageRating = viewData.AverageRating });
		}

		#region Manage

		/// <summary>
		/// Manages the article.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult ManageArticles(int page)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Articles.GetArticles(null, page);

			ViewData["PageTitle"] = "Manage Articles";

			return View(viewData);
		}

		/// <summary>
		/// Manages the category.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult ManageCategories()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Categories.GetCategories();

			ViewData["PageTitle"] = "Manage Categories";

			return View(viewData);
		}

		/// <summary>
		/// Manages the comments.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult ManageComments(int page)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Comments.GetCommentsForModeration(page);

			ViewData["PageTitle"] = "Manage Comments";

			return View(viewData);
		}

		#endregion

		#region Article

		/// <summary>
		/// Adds the article.
		/// </summary>
		/// <param name="categoryId">The category id.</param>
		/// <param name="title">The title.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="body">The body.</param>
		/// <param name="country">The country.</param>
		/// <param name="state">The state.</param>
		/// <param name="city">The city.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="expireDate">The expire date.</param>
		/// <param name="approved">if set to <c>true</c> [approved].</param>
		/// <param name="listed">if set to <c>true</c> [listed].</param>
		/// <param name="commentsEnabled">if set to <c>true</c> [comments enabled].</param>
		/// <param name="onlyForMembers">if set to <c>true</c> [only for members].</param>
		/// <returns></returns>
		[Authorize(Roles = "Contributor")]
		[ValidateInput(false)]   
		public ActionResult CreateArticle(int? categoryId, string title, string summary, string body, string country, string state, string city, DateTime? releaseDate, DateTime? expireDate, bool? approved, bool? listed, bool? commentsEnabled, bool? onlyForMembers)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var categories = dc.Categories.GetCategories();

			if (categoryId.HasValue
				&& !String.IsNullOrEmpty(title)
				&& !String.IsNullOrEmpty(body))
			{
				try
				{
					Article article = new Article {
						CategoryID = categoryId.Value,
						Title = title,
						Path = title.ToUrlFormat(),
						Abstract = summary,
						Body = body,
						Country = country,
						State = state,
						City = city,
						ReleaseDate = releaseDate ?? DateTime.Today,
						ExpireDate = expireDate,
						Approved = approved ?? false,
						Listed = listed ?? false,
						CommentsEnabled = commentsEnabled ?? false,
						OnlyForMembers = onlyForMembers ?? false,
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now
					};

					dc.Articles.InsertOnSubmit(article);
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your article has been posted.";
					return RedirectToAction("ViewArticle", new { id = article.ArticleID, path = article.Path });
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["categoryId"] = new SelectList(categories, "CategoryID", "Title", categoryId);
			ViewData["title"] = title;
			ViewData["summary"] = summary;
			ViewData["body"] = body;
			ViewData["country"] = new SelectList(Iso3166CountryCodes.CountryDictonary, "Key", "Value", country ?? "US");
			ViewData["state"] = state;
			ViewData["city"] = city;
			ViewData["releaseDate"] = releaseDate;
			ViewData["expireDate"] = expireDate;
			ViewData["approved"] = approved;
			ViewData["listed"] = listed;
			ViewData["commentsEnabled"] = commentsEnabled;
			ViewData["onlyForMembers"] = onlyForMembers;

			ViewData["PageTitle"] = "Create Article";

			return View("CreateArticle");
		}

		/// <summary>
		/// Edits the article.
		/// </summary>
		/// <param name="articleId">The article id.</param>
		/// <param name="categoryId">The category id.</param>
		/// <param name="title">The title.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="body">The body.</param>
		/// <param name="country">The country.</param>
		/// <param name="state">The state.</param>
		/// <param name="city">The city.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="expireDate">The expire date.</param>
		/// <param name="approved">if set to <c>true</c> [approved].</param>
		/// <param name="listed">if set to <c>true</c> [listed].</param>
		/// <param name="commentsEnabled">if set to <c>true</c> [comments enabled].</param>
		/// <param name="onlyForMembers">if set to <c>true</c> [only for members].</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		[ValidateInput(false)]   
		public ActionResult EditArticle(int articleId, int? categoryId, string title, string summary, string body, string country, string state, string city, DateTime? releaseDate, DateTime? expireDate, bool? approved, bool? listed, bool? commentsEnabled, bool? onlyForMembers)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var categories = dc.Categories.GetCategories();

			if (IsPostBack)
			{
				approved = approved ?? false;
				listed = listed ?? false;
				commentsEnabled = commentsEnabled ?? false;
				onlyForMembers = onlyForMembers ?? false;
			}

			Article article = dc.Articles.GetArticle(articleId);

			// throw a 404 Not Found if the requested article is not in the database
			if (article == null)
				throw new HttpException(404, "The article could not be found.");

			if (categoryId.HasValue
				&& !String.IsNullOrEmpty(title)
				&& !String.IsNullOrEmpty(body))
			{
				try
				{
					article.CategoryID = categoryId.Value;
					article.Title = title;
					article.Abstract = summary;
					article.Body = body;
					article.Country = country;
					article.State = state;
					article.City = city;
					article.ReleaseDate = releaseDate ?? article.ReleaseDate;
					article.ExpireDate = expireDate;
					article.Approved = approved ?? false;
					article.Listed = listed ?? false;
					article.CommentsEnabled = commentsEnabled ?? false;
					article.OnlyForMembers = onlyForMembers ?? false;

					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your article has been updated.";
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["categoryId"] = new SelectList(categories, "CategoryID", "Title", categoryId ?? article.CategoryID);
			ViewData["title"] = title ?? article.Title;
			ViewData["summary"] = summary ?? article.Abstract;
			ViewData["body"] = body ?? article.Body;
			ViewData["country"] = new SelectList(Iso3166CountryCodes.CountryDictonary, "Key", "Value", country ?? article.Country ?? "US");
			ViewData["state"] = state ?? article.State;
			ViewData["city"] = city ?? article.City;
			ViewData["releaseDate"] = releaseDate ?? article.ReleaseDate;
			ViewData["expireDate"] = expireDate ?? article.ExpireDate;
			ViewData["approved"] = approved ?? article.Approved;
			ViewData["listed"] = listed ?? article.Listed;
			ViewData["commentsEnabled"] = commentsEnabled ?? article.CommentsEnabled;
			ViewData["onlyForMembers"] = onlyForMembers ?? article.OnlyForMembers;

			ViewData["PageTitle"] = "Edit Article";

			return View("CreateArticle");
		}

		/// <summary>
		/// Removes the article.
		/// </summary>
		/// <param name="articleId">The article id.</param>
		/// <param name="remove">The remove.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult RemoveArticle(int articleId, string remove)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Article article = dc.Articles.GetArticle(articleId);

			// throw a 404 Not Found if the requested article is not in the database
			if (article == null)
				throw new HttpException(404, "The article could not be found.");

			if (String.Equals(remove, "yes", StringComparison.OrdinalIgnoreCase))
			{
				dc.Articles.DeleteOnSubmit(article);
				dc.SubmitChanges();

				TempData["SuccessMessage"] = "The article, " + article.Title + ", has been deleted.";

				article = null;
			}
			else if (String.Equals(remove, "no", StringComparison.OrdinalIgnoreCase))
			{
				TempData["InformationMessage"] = "The article, " + article.Title + ", has NOT been deleted.";
			}
			else
			{
				TempData["WarningMessage"] = "Are you sure you want to delete " + article.Title + ".  You will not be able to recover this article if you select YES.";
			}

			ViewData["PageTitle"] = "Remove Article";

			return View(article);
		}

		#endregion

		#region Category

		/// <summary>
		/// Adds the category.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="importance">The importance.</param>
		/// <param name="imageUrl">The image URL.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		[ValidateInput(false)]   
		public ActionResult CreateCategory(string title, int? importance, string imageUrl, string description)
		{
			if (!String.IsNullOrEmpty(title)
				&& !String.IsNullOrEmpty(imageUrl)
				&& !String.IsNullOrEmpty(description))
			{
				try
				{
					TheBeerHouseDataContext dc = new TheBeerHouseDataContext();

					Category category = new Category {
						Title = title,
						Importance = importance ?? -1,
						ImageUrl = imageUrl,
						Description = description,
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now,
						Path = title.ToUrlFormat()
					};
					dc.Categories.InsertOnSubmit(category);

					// save changes to database
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your category has been created.";
					return RedirectToAction("ManageArticles");
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["title"] = title;
			ViewData["importance"] = importance;
			ViewData["imageUrl"] = imageUrl;
			ViewData["description"] = description;

			ViewData["PageTitle"] = "Create Category";

			return View("CreateCategory");
		}

		/// <summary>
		/// Edits the category.
		/// </summary>
		/// <param name="categoryId">The category id.</param>
		/// <param name="title">The title.</param>
		/// <param name="importance">The importance.</param>
		/// <param name="imageUrl">The image URL.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		[ValidateInput(false)]   
		public ActionResult EditCategory(int categoryId, string title, int? importance, string imageUrl, string description)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Category category = dc.Categories.GetCategory(categoryId);

			// throw a 404 Not Found if the requested category is not in the database
			if (category == null)
				throw new HttpException(404, "The category could not be found.");

			if (!String.IsNullOrEmpty(title)
				&& !String.IsNullOrEmpty(imageUrl)
				&& !String.IsNullOrEmpty(description))
			{
				try
				{
					category.Title = title;
					category.Importance = importance ?? -1;
					category.ImageUrl = imageUrl;
					category.Description = description;

					// save changes to database
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your category has been updated.";
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["title"] = title ?? category.Title;
			ViewData["importance"] = importance ?? category.Importance;
			ViewData["imageUrl"] = imageUrl ?? category.ImageUrl;
			ViewData["description"] = description ?? category.Description;

			ViewData["PageTitle"] = "Edit Category";

			return View("CreateCategory");
		}

		/// <summary>
		/// Removes the category.
		/// </summary>
		/// <param name="categoryId">The category id.</param>
		/// <param name="newCategoryId">The new category id.</param>
		/// <param name="remove">The remove.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult RemoveCategory(int categoryId, int? newCategoryId, string remove)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var categories = dc.Categories.GetCategories();
			Category category = dc.Categories.GetCategory(categoryId);
			bool newCategoryExists = categoryId != newCategoryId && dc.Categories.Exists(newCategoryId ?? -1);

			// throw a 404 Not Found if the requested category is not in the database
			if (category == null)
				throw new HttpException(404, "The category could not be found.");

			if (String.Equals(remove, "yes", StringComparison.OrdinalIgnoreCase) && newCategoryExists)
			{
				foreach (Article article in category.Articles)
					article.CategoryID = newCategoryId.Value;

				dc.Categories.DeleteOnSubmit(category);
				dc.SubmitChanges();

				TempData["SuccessMessage"] = "The category, " + category.Title + ", has been deleted.";

				category = null;
			}
			else if (String.Equals(remove, "no", StringComparison.OrdinalIgnoreCase))
			{
				TempData["InformationMessage"] = "The category, " + category.Title + ", has NOT been deleted.";
			}
			else
			{
				ViewData["newCategoryId"] = new SelectList(categories, "CategoryID", "Title", newCategoryId);
				TempData["WarningMessage"] = "Are you sure you want to delete " + category.Title + ".  If you are sure please select a new category to move all the articles to and then select YES.";
			}

			ViewData["PageTitle"] = "Remove Category";

			return View(category);
		}

		#endregion

		#region Comment

		/// <summary>
		/// Adds the comment.
		/// </summary>
		/// <param name="articleId">The article id.</param>
		/// <param name="name">The name.</param>
		/// <param name="email">The email.</param>
		/// <param name="body">The body.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[ValidateInput(false)]   
		public ActionResult CreateComment(int articleId, string name, string email, string body)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Article article = dc.Articles.GetArticle(articleId);

			// throw a 404 Not Found if the requested article is not in the database
			if (article == null)
				throw new HttpException(404, "The article could not be found.");

			Comment comment = new Comment {
				AddedBy = name,
				AddedByEmail = email,
				AddedByIP = Request.UserHostAddress,
				AddedDate = DateTime.Now,
				Body = body
			};
			article.Comments.Add(comment);

			// save changes to database
			dc.SubmitChanges();

			return View(new {
				commentId = comment.CommentID,
				name = comment.AddedBy,
				body = comment.Body
			});
		}

		/// <summary>
		/// Edits the comment.
		/// </summary>
		/// <param name="commentId">The comment id.</param>
		/// <param name="name">The name.</param>
		/// <param name="body">The body.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		[ServiceOnly, HttpPostOnly]
		[ValidateInput(false)]   
		public ActionResult EditComment(int commentId, string name, string body)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Comment comment = dc.Comments.GetComment(commentId);

			// throw a 404 Not Found if the requested article is not in the database
			if (comment == null)
				throw new HttpException(404, "The comment could not be found.");

			comment.AddedBy = name;
			comment.Body = body;

			// save changes to database
			dc.SubmitChanges();

			return View(new {
				commentId = comment.CommentID,
				name = comment.AddedBy,
				body = comment.Body
			});
		}

		/// <summary>
		/// Removes the comment.
		/// </summary>
		/// <param name="commentId">The comment id.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		[ServiceOnly, HttpPostOnly]
		public ActionResult RemoveComment(int commentId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Comment comment = dc.Comments.GetComment(commentId);

			// throw a 404 Not Found if the requested article is not in the database
			if (comment == null)
				throw new HttpException(404, "The comment could not be found.");

			dc.Comments.DeleteOnSubmit(comment);
			dc.SubmitChanges();

			return View(new { commentId = commentId });
		}

		#endregion
	}
}
