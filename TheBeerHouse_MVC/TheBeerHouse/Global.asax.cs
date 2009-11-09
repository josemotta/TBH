using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;

namespace TheBeerHouse
{
	/// <summary>
	/// 
	/// </summary>
	public class GlobalApplication : System.Web.HttpApplication
	{
		/// <summary>
		/// Registers the routes.
		/// </summary>
		/// <param name="routes">The routes.</param>
		public static void RegisterRoutes(RouteCollection routes)
		{
			#region Security - Chapter 5

			#endregion
			#region Articles - Chapter 6

			routes.MapRoute(
				"ArticleView",
				"articles/{id}/{*path}",
				new { controller = "Article", action = "ViewArticle", id = (string)null, path = (string)null },
				new { id = "[0-9]+", path = "[a-zA-Z0-9\\-]*" }
			);

			routes.MapRoute(
				"ArticleCategoryViewIndex",
				"articles/categories/{category}",
				new { controller = "Article", action = "Index", category = (string)null, page = 1 },
				new { category = "[a-zA-Z0-9\\-]+", page = "[0-9]+" }
			);

			routes.MapRoute(
				"ArticleCategoryViewIndexPaged",
				"articles/categories/{category}/page{page}",
				new { controller = "Article", action = "Index", category = (string)null, page = (int?)null },
				new { category = "[a-zA-Z0-9\\-]+", page = "[0-9]+" }
			);

			routes.MapRoute(
				"ArticleCategoryIndex",
				"articles/categories",
				new { controller = "Article", action = "CategoryIndex" }
			);

			routes.MapRoute(
				"ArticleIndex",
                //"",                   // home não é mais aqui
                "articles",
                new { controller = "Article", action = "Index", category = (string)null, page = 1 }
			);

			routes.MapRoute(
				"ArticleIndexPaged",
				"articles/page{page}",
				new { controller = "Article", action = "Index", category = (string)null, page = (int?)null },
				new { page = "[0-9]+" }
			);

			#region Admin

			routes.MapRoute(
				"ArticleCreate",
				"admin/articles/create",
				new { controller = "Article", action = "CreateArticle" }
			);

			routes.MapRoute(
				"ArticleEdit",
				"admin/articles/edit/{articleId}",
				new { controller = "Article", action = "EditArticle", articleId = (int?)null },
				new { articleId = "[0-9]+" }
			);

			routes.MapRoute(
				"ArticleRemove",
				"admin/articles/remove/{articleId}",
				new { controller = "Article", action = "RemoveArticle", articleId = (int?)null },
				new { articleId = "[0-9]+" }
			);

			routes.MapRoute(
				"ArticleManage",
				"admin/articles",
				new { controller = "Article", action = "ManageArticles", page = 1 }
			);

			routes.MapRoute(
				"ArticleManagePaged",
				"admin/articles/page{page}",
				new { controller = "Article", action = "ManageArticles", page = (int?)null },
				new { page = "[0-9]+" }
			);

			routes.MapRoute(
				"ArticleCategoryCreate",
				"admin/articles/categories/create",
				new { controller = "Article", action = "CreateCategory" }
			);

			routes.MapRoute(
				"ArticleCategoryEdit",
				"admin/articles/categories/edit/{categoryId}",
				new { controller = "Article", action = "EditCategory", categoryId = (int?)null },
				new { categoryId = "[0-9]+" }
			);

			routes.MapRoute(
				"ArticleCategoryRemove",
				"admin/articles/categories/remove/{categoryId}",
				new { controller = "Article", action = "RemoveCategory", categoryId = (int?)null },
				new { categoryId = "[0-9]+" }
			);

			routes.MapRoute(
				"ArticleCategoryManage",
				"admin/articles/categories",
				new { controller = "Article", action = "ManageCategories" }
			);

			routes.MapRoute(
				"ArticleCommentManage",
				"admin/articles/comments",
				new { controller = "Article", action = "ManageComments", page = 1 }
			);

			routes.MapRoute(
				"ArticleCommentManagePaged",
				"admin/articles/comments/page{page}",
				new { controller = "Article", action = "ManageComments", page = (int?)null },
				new { page = "[0-9]+" }
			);

			#endregion

			#endregion
			#region Polls - Chapter 7

			routes.MapRoute(
				"PollsIndex",
				"polls",
				new { controller = "Poll", action = "Index", page = 1 }
			);

			routes.MapRoute(
				"PollsIndexPaged",
				"polls/page{page}",
				new { controller = "Poll", action = "Index", page = (int?)null },
				new { page = "[0-9]+" }
			);

			#region Admin

			routes.MapRoute(
				"PollCreate",
				"admin/polls/create",
				new { controller = "Poll", action = "CreatePoll" }
			);

			routes.MapRoute(
				"PollEdit",
				"admin/polls/edit/{pollId}",
				new { controller = "Poll", action = "EditPoll", pollId = (int?)null },
				new { pollId = "[0-9]+" }
			);

			routes.MapRoute(
				"PollRemove",
				"admin/polls/remove/{pollId}",
				new { controller = "Poll", action = "RemovePoll", pollId = (int?)null },
				new { pollId = "[0-9]+" }
			);

			routes.MapRoute(
				"PollManager",
				"admin/polls",
				new { controller = "Poll", action = "ManagePolls", page = 1 }
			);

			routes.MapRoute(
				"PollManagerPaged",
				"admin/polls/page{page}",
				new { controller = "Poll", action = "ManagePolls", page = (int?)null },
				new { page = "[0-9]+" }
			);

			#endregion

			#endregion
			#region Newsletter - Chapter 8

			routes.MapRoute(
				"NewsletterIndex",
                //"news",               //altera home page para News
                "",
                new { controller = "Newsletter", action = "Index" }
			);

			#region Admin

			routes.MapRoute(
				"NewsletterCreate",
				"admin/newsletters/create",
				new { controller = "Newsletter", action = "CreateNewsletter" }
			);

			routes.MapRoute(
				"NewsletterEdit",
				"admin/newsletters/edit/{newsletterId}",
				new { controller = "Newsletter", action = "EditNewsletter", newsletterId = (int?)null },
				new { newsletterId = "[0-9]+" }
			);

			routes.MapRoute(
				"NewsletterRemove",
				"admin/newsletters/remove/{newsletterId}",
				new { controller = "Newsletter", action = "RemoveNewsletter", newsletterId = (int?)null },
				new { newsletterId = "[0-9]+" }
			);

			routes.MapRoute(
				"NewsletterManage",
				"admin/newsletters",
				new { controller = "Newsletter", action = "ManageNewsletters" }
			);

			#endregion

			#endregion
			#region Forums - Chapter 9

			routes.MapRoute(
				"ForumPostCreate",
				"forums/{forumId}/post",
				new { controller = "Forum", action = "CreatePost", forumId = (int?)null },
				new { forumId = "[0-9]+" }
			);

			routes.MapRoute(
				"ForumPostReply",
				"forums/posts/{parentPostId}/reply",
				new { controller = "Forum", action = "CreatePost", parentPostId = (int?)null },
				new { parentPostId = "[0-9]+" }
			);

			routes.MapRoute(
				"ForumsIndex",
				"forums",
				new { controller = "Forum", action = "Index" }
			);

			routes.MapRoute(
				"Forum",
				"forums/{forumId}/{*path}",
				new { controller = "Forum", action = "ViewForum", forumId = (int?)null, path = (string)null, page = 1 },
				new { forumId = "[0-9]+" }
			);

			routes.MapRoute(
				"ForumPaged",
				"forums/{forumId}/{path}/page{page}",
				new { controller = "Forum", action = "ViewForum", forumId = (int?)null, path = (string)null, page = (int?)null },
				new { forumId = "[0-9]+" }
			);

			routes.MapRoute(
				"ForumPost",
				"forums/posts/{postId}/{*path}",
				new { controller = "Forum", action = "ViewPost", postId = (int?)null, path = (string)null, page = 1 },
				new { postId = "[0-9]+" }
			);

			routes.MapRoute(
				"ForumPostPaged",
				"forums/posts/{postId}/{path}/page{page}",
				new { controller = "Forum", action = "ViewPost", postId = (int?)null, path = (string)null, page = (int?)null },
				new { postId = "[0-9]+" }
			);

			#region Admin

			routes.MapRoute(
				"ForumCreate",
				"admin/forums/create",
				new { controller = "Forum", action = "CreateForum" }
			);

			routes.MapRoute(
				"ForumEdit",
				"admin/forums/edit/{forumId}",
				new { controller = "Forum", action = "EditForum", forumId = (int?)null },
				new { forumId = "[0-9]+" }
			);

			routes.MapRoute(
				"ForumRemove",
				"admin/forums/remove/{forumId}",
				new { controller = "Forum", action = "RemoveForum", forumId = (int?)null },
				new { forumId = "[0-9]+" }
			);

			routes.MapRoute(
				"ForumPostsManager",
				"admin/forums/posts",
				new { controller = "Forum", action = "ManagePosts" }
			);

			routes.MapRoute(
				"ForumManager",
				"admin/forums",
				new { controller = "Forum", action = "ManageForums" }
			);

			#endregion

			#endregion
			#region Commerce - Chapter 10

			routes.MapRoute(
				"CommerceIndex",
				"store",
				new { controller = "Commerce", action = "Index" }
			);

			routes.MapRoute(
				"CommerceDepartment",
				"store/departments/{departmentId}",
				new { controller = "Commerce", action = "ViewDepartment", departmentId = (int?)null }
			);

			routes.MapRoute(
				"CommerceProduct",
				"store/products/{productId}",
				new { controller = "Commerce", action = "ViewProduct", productId = (int?)null }
			);

			routes.MapRoute(
				"CommerceCart",
				"store/cart",
				new { controller = "Commerce", action = "ViewShoppingCart" }
			);

			routes.MapRoute(
				"CommerceCompleted",
				"store/order/completed",
				new { controller = "Commerce", action = "CompleteOrder" }
			);

			#region Admin

			routes.MapRoute(
				"CommerceManageStore",
				"admin/store",
				new { controller = "Commerce", action = "ManageStore" }
			);

			routes.MapRoute(
				"CommerceManageDepartments",
				"admin/store/departments",
				new { controller = "Commerce", action = "ManageDepartments" }
			);

			routes.MapRoute(
				"CommerceCreateDepartment",
				"admin/store/departments/create",
				new { controller = "Commerce", action = "CreateDepartment" }
			);

			routes.MapRoute(
				"CommerceEditDepartment",
				"admin/store/departments/edit/{departmentId}",
				new { controller = "Commerce", action = "EditDepartment", departmentId = (int?)null }
			);

			routes.MapRoute(
				"CommerceManageProducts",
				"admin/store/products",
				new { controller = "Commerce", action = "ManageProducts" }
			);

			routes.MapRoute(
				"CommerceCreateProduct",
				"admin/store/products/create",
				new { controller = "Commerce", action = "CreateProduct" }
			);

			routes.MapRoute(
				"CommerceEditProduct",
				"admin/store/products/edit/{productId}",
				new { controller = "Commerce", action = "EditProduct", productId = (int?)null }
			);

			routes.MapRoute(
				"CommerceManageOrders",
				"admin/store/orders",
				new { controller = "Commerce", action = "ManageOrders" }
			);

			routes.MapRoute(
				"CommerceOrderDetail",
				"admin/store/orders/{orderId}",
				new { controller = "Commerce", action = "OrderDetail", orderId = (int?)null }
			);

			routes.MapRoute(
				"CommerceManageShipping",
				"admin/store/shipping",
				new { controller = "Commerce", action = "ManageShipping" }
			);


			#endregion

			#endregion

			routes.MapRoute(
			  "Default",
			  "{controller}/{action}",
			  new { controller = (string)null, action = (string)null }
			);
		}

		/// <summary>
		/// Handles the OnAuthenticate event of the FormsAuthentication control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="args">The <see cref="System.Web.Security.FormsAuthenticationEventArgs"/> instance containing the event data.</param>
		public void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
		{
			if (FormsAuthentication.CookiesSupported)
			{
				if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
				{
					try
					{
						FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

						args.User = new GenericPrincipal(
							new FormsIdentity(ticket),
							new string[0] // don't worry about roles they are handled by the RoleManager
						);

						args.Context.User = args.User;
					}
					catch (Exception) { /* decryption failed */ }
				}
			}
			else
			{
				throw new HttpException("Cookieless Forms Authentication is not supported for this application.");
			}
		}

		#region Application Events

		/// <summary>
		/// Application_s the start.
		/// </summary>
		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
		}

		/// <summary>
		/// Handles the BeginReques event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		public void Application_BeginReques(object sender, EventArgs e)
		{
			Response.AppendHeader("X-TheBeerHouse", "TheBeerHouse/2.0 (Wrox; Berardi; Katawazi; +thebeerhouseexample.com; +thebeerhouse.codeplex.com)");
		}

		#endregion
	}
}