using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagedFusion.Web.Mvc;
using TheBeerHouse.Models;

namespace TheBeerHouse.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Service, HandleError]
	public class ForumController : Controller
	{
		/// <summary>
		/// Indexes the specified page.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Forums.GetForums();

			ViewData["PageTitle"] = "Forums";
			return View(viewData);
		}

		/// <summary>
		/// Views the forum.
		/// </summary>
		/// <param name="forumId">The forum id.</param>
		/// <param name="path">The path.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public ActionResult ViewForum(int forumId, string path, [Default(1)]int page)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var forum = dc.Forums.GetForum(forumId);

			// throw a 404 Not Found if the requested forum is not in the database
			if (forum == null)
				throw new HttpException(404, "The forum could not be found.");

			// SEO: redirect to the correct location if the path is not
			if (!String.Equals(path, forum.Path, StringComparison.OrdinalIgnoreCase))
				return this.RedirectToAction(301, "ViewForum", new { forumId = forum.ForumID, path = forum.Path });

			int count = Configuration.TheBeerHouseSection.Current.Forums.PostsPageSize;
			int index = (page - 1) * count;

			ViewData["PageTitle"] = forum.Title + " Forum";
			ViewData["count"] = count;
			ViewData["index"] = index;

			return View(forum);
		}

		/// <summary>
		/// Views the post.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <param name="path">The path.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public ActionResult ViewPost(int postId, string path, [Default(1)]int page)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Posts.GetPost(postId);

			// throw a 404 Not Found if the requested post is not in the database
			if (viewData == null)
				throw new HttpException(404, "The post could not be found.");

			// SEO: redirect to the correct location if the path is not
			if (!String.Equals(path, viewData.Path, StringComparison.OrdinalIgnoreCase))
				return this.RedirectToAction(301, "ViewPost", new { id = viewData.PostID, path = viewData.Path });

			viewData.ViewCount++;
			dc.SubmitChanges();

			int count = Configuration.TheBeerHouseSection.Current.Forums.ThreadsPageSize;
			int index = (page - 1) * count;

			ViewData["PageTitle"] = viewData.Title;
			ViewData["count"] = count;
			ViewData["index"] = index;

			var vote = dc.Votes.GetVote(postId, User.Identity.Name);
			ViewData["userVote"] = vote == null ? (short)0 : vote.Direction;

			return View(viewData);
		}

		/// <summary>
		/// Votes the specified post id.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		public ActionResult Vote(int postId, int direction)
		{
			if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
				return View(new { error = "not-authenticated" });

			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var post = dc.Posts.GetPost(postId);
			var vote = dc.Votes.GetVote(postId, User.Identity.Name);

			if (vote == null)
			{
				vote = new Vote {
					AddedBy = User.Identity.Name,
					AddedDate = DateTime.Now,
					AddedByIP = Request.UserHostAddress,
					Direction = (direction > 0) ? (short)1 : (short)-1,
				};
				post.Votes.Add(vote);
				post.VoteCount += vote.Direction;

				dc.SubmitChanges();
			}

			return View(new { postId = postId, direction = vote.Direction, voteCount = post.VoteCount });
		}

		/// <summary>
		/// Manages the forums.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult ManageForums()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Forums.GetForums();

			ViewData["PageTitle"] = "Manage Forums";
			return View(viewData);
		}

		/// <summary>
		/// Manages the posts.
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult ManagePosts()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Posts.Where(p => p.Approved == false);

			ViewData["PageTitle"] = "Manage Posts";
			return View(viewData);
		}

		/// <summary>
		/// Creates the forum.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="description">The description.</param>
		/// <param name="order">The order.</param>
		/// <param name="moderated">The moderated.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult CreateForum(string title, string description, int? order, bool? moderated)
		{
			order = order ?? 0;
			moderated = moderated ?? false;

			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();

			if (!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(description))
			{
				try
				{
					Forum forum = new Forum {
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now,
						Title = title,
						Path = title.ToUrlFormat(),
						Description = description,
						Importance = order.Value,
						Moderated = moderated.Value
					};

					dc.Forums.InsertOnSubmit(forum);
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your forum has been created.";
					return RedirectToAction("EditForum", new { forumId = forum.ForumID });
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["title"] = title;
			ViewData["description"] = description;
			ViewData["order"] = order;
			ViewData["moderated"] = moderated;

			ViewData["PageTitle"] = "Create Forum";

			return View("CreateForum");
		}

		/// <summary>
		/// Edits the forum.
		/// </summary>
		/// <param name="forumId">The forum id.</param>
		/// <param name="title">The title.</param>
		/// <param name="description">The description.</param>
		/// <param name="order">The order.</param>
		/// <param name="moderated">The moderated.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult EditForum(int forumId, string title, string description, int? order, bool? moderated)
		{
			order = order ?? 0;
			moderated = moderated ?? false;

			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Forum forum = dc.Forums.GetForum(forumId);

			// throw a 404 Not Found if the requested forum is not in the database
			if (forum == null)
				throw new HttpException(404, "The forum could not be found.");

			if (!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(description))
			{
				try
				{
					forum.Title = title;
					forum.Path = title.ToUrlFormat();
					forum.Description = description;
					forum.Importance = order.Value;
					forum.Moderated = moderated.Value;

					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your forum has been updated.";
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["title"] = title ?? forum.Title;
			ViewData["description"] = description ?? forum.Description;
			ViewData["order"] = order ?? forum.Importance;
			ViewData["moderated"] = moderated ?? forum.Moderated;

			ViewData["PageTitle"] = "Edit Forum";

			return View("CreateForum");
		}

		/// <summary>
		/// Removes the forum.
		/// </summary>
		/// <param name="forumId">The forum id.</param>
		/// <param name="remove">The remove.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult RemoveForum(int forumId, string remove)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Forum forum = dc.Forums.GetForum(forumId);

			// throw a 404 Not Found if the requested forum is not in the database
			if (forum == null)
				throw new HttpException(404, "The forum could not be found.");

			if (String.Equals(remove, "yes", StringComparison.OrdinalIgnoreCase))
			{
				dc.Forums.DeleteOnSubmit(forum);
				dc.SubmitChanges();

				TempData["SuccessMessage"] = "The poll, " + forum.Title + ", has been deleted.";

				forum = null;
			}
			else if (String.Equals(remove, "no", StringComparison.OrdinalIgnoreCase))
			{
				TempData["InformationMessage"] = "The poll, " + forum.Title + ", has NOT been deleted.";
			}
			else
			{
				TempData["WarningMessage"] = "Are you sure you want to delete " + forum.Title + ".  You will not be able to recover this forum if you select YES.";
			}

			ViewData["PageTitle"] = "Remove Forum";

			return View(forum);
		}

		/// <summary>
		/// Adds the post.
		/// </summary>
		/// <param name="forumId">The forum id.</param>
		/// <param name="parentPostId">The parent post id.</param>
		/// <param name="title">The title.</param>
		/// <param name="body">The body.</param>
		/// <returns></returns>
		[Authorize]
		[ValidateInput(false)]
		public ActionResult CreatePost(int? forumId, int? parentPostId, string title, string body)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Forum forum = null;
			Post parentPost = null;

			if (forumId.HasValue)
			{
				forum = dc.Forums.GetForum(forumId.Value);
				TempData["InformationMessage"] = "You are creating a post in the \"" + forum.Title + "\" forum.";
			}

			if (parentPostId.HasValue)
			{
				parentPost = dc.Posts.GetPost(parentPostId.Value);

				// throw a 404 Not Found if the requested parent post is not in the database
				if (parentPost == null)
					throw new HttpException(404, "The post could not be found.");

				forum = parentPost.Forum;
				TempData["InformationMessage"] = "You are replying to \"" + parentPost.Title + "\" in the \"" + forum.Title + "\" forum.";
			}

			// throw a 404 Not Found if the requested forum is not in the database
			if (forum == null)
				throw new HttpException(404, "The forum could not be found.");

			if (!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(body))
			{
				try
				{
					Post post = new Post {
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now,
						AddedByIP = Request.UserHostAddress,
						ParentPostID = parentPostId,
						Title = title,
						Path = title.ToUrlFormat(),
						Body = body,
						ViewCount = 1,
						ReplyCount = 0,
						VoteCount = 0,
						Approved = !forum.Moderated,
						LastPostBy = User.Identity.Name,
						LastPostDate = DateTime.Now
					};

					if (parentPost != null)
					{
						parentPost.LastPostBy = User.Identity.Name;
						parentPost.LastPostDate = DateTime.Now;
						parentPost.ReplyCount++;
						dc.Posts.InsertOnSubmit(post);
					}
					else
					{
						forum.Posts.Add(post);
					}

					dc.SubmitChanges();

					if (forum.Moderated)
						TempData["SuccessMessage"] = "Your post has been created and is awaiting approval from a moderator.";
					else
						TempData["SuccessMessage"] = "Your post has been created.";

					if (parentPost != null)
						return RedirectToAction("ViewPost", new { postId = parentPost.PostID, path = parentPost.Path });
					else if (forum.Moderated)
						return RedirectToAction("ViewForum", new { forumId = forum.ForumID, path = forum.Path });
					else
						return RedirectToAction("ViewPost", new { postId = post.PostID, path = post.Path });
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["title"] = title;
			ViewData["body"] = body;

			ViewData["PageTitle"] = "Create Post";

			return View("CreatePost");
		}

		/// <summary>
		/// Closes the post.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <param name="closed">if set to <c>true</c> [closed].</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult ClosePost(int postId, bool closed)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var post = dc.Posts.GetPost(postId);

			if (post == null)
				throw new HttpException(404, "The post could not be found.");

			// reset all posts to closed for a specific parent post
			dc.ExecuteCommand("update TheBeerHouse.Posts set Closed = {0} where ParentPostID = {1}", new object[] { closed, postId });

			post.Closed = closed;
			dc.SubmitChanges();

			return View(new { postId = postId });
		}

		/// <summary>
		/// Approves the post.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <param name="approved">if set to <c>true</c> [approved].</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult ApprovePost(int postId, bool approved)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var post = dc.Posts.GetPost(postId);

			if (post == null)
				throw new HttpException(404, "The post could not be found.");

			// reset all posts to closed for a specific parent post
			dc.ExecuteCommand("update TheBeerHouse.Posts set Approved = {0} where ParentPostID = {1}", new object[] { approved, postId });

			post.Approved = approved;
			dc.SubmitChanges();

			return View(new { postId = postId });
		}

		/// <summary>
		/// Removes the post.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult RemovePost(int postId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var post = dc.Posts.GetPost(postId);

			if (post == null)
				throw new HttpException(404, "The post could not be found.");

			// delete all replies to the current post for a specific parent post
			dc.ExecuteCommand("delete from TheBeerHouse.Posts where ParentPostID = {0}", new object[] { postId });

			dc.Posts.DeleteOnSubmit(post);
			dc.SubmitChanges();

			return View(new { postId = postId });
		}
	}
}