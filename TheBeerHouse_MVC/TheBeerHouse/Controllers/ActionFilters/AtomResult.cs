using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace TheBeerHouse.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	public class AtomResult : ActionResult
	{
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public object Data
		{
			get;
			set;
		}

		/// <summary>
		/// Executes the result.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			HttpResponseBase response = context.HttpContext.Response;
			HttpRequestBase request = context.HttpContext.Request;

			if (!(Data is Models.Pagination<Models.Article>))
			{
				response.Clear();
				response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
				response.StatusDescription = "Unsupported Media Type";
				response.ContentType = "text/html";
				response.Write("<html><head><title>Unsupported Media Type</title></head><h1>Unsupported Media Type</h1><p>The response is not allowed to be redered to a feed.</p></html>");
				response.Flush();
				response.End();
			}

			response.ContentType = "application/atom+xml";
			response.ContentEncoding = Encoding.UTF8;

			response.Cache.SetCacheability(HttpCacheability.NoCache);
			response.Cache.AppendCacheExtension("no-cache=\"Set-Cookie\", proxy-revalidate");
			response.AppendHeader("X-Robots-Tag", "noindex, follow, noarchive, nosnippet");
			response.ClearContent();

			var articles = Data as Models.Pagination<Models.Article>;

			SyndicationFeed feed = new SyndicationFeed {
				Title = new TextSyndicationContent("The Beer House Articles", TextSyndicationContentKind.Plaintext),
				Description = new TextSyndicationContent("Feed of ideas from The Beer House.", TextSyndicationContentKind.Plaintext),
				LastUpdatedTime = DateTime.Now
			};

			feed.Id = request.Url.GetLeftPart(UriPartial.Path);
			feed.Links.Add(new SyndicationLink {
				Uri = request.Url,
				RelationshipType = "self"
			});
			feed.Links.Add(new SyndicationLink {
				Uri = new Uri(request.Url.GetLeftPart(UriPartial.Path)),
				RelationshipType = "alternate"
			});
			feed.Links.Add(new SyndicationLink {
				Uri = new Uri(request.Url.GetLeftPart(UriPartial.Path) + "?type=atom"),
				RelationshipType = "first"
			});
			if (articles.PageNumber < articles.PageCount)
			{
				feed.Links.Add(new SyndicationLink {
					Uri = new Uri(request.Url.GetLeftPart(UriPartial.Path).TrimEnd('/') + "/page" + (articles.PageNumber + 1) + "?type=atom"),
					RelationshipType = "next"
				});
			}
			if (articles.PageNumber > 1)
			{
				feed.Links.Add(new SyndicationLink {
					Uri = new Uri(request.Url.GetLeftPart(UriPartial.Path).TrimEnd('/') + "/page" + (articles.PageNumber - 1) + "?type=atom"),
					RelationshipType = "previous"
				});
			}
			if (articles.PageCount > 1)
			{
				feed.Links.Add(new SyndicationLink {
					Uri = new Uri(request.Url.GetLeftPart(UriPartial.Path).TrimEnd('/') + "/page" + articles.PageCount + "?type=atom"),
					RelationshipType = "last"
				});
			}

			List<SyndicationItem> items = new List<SyndicationItem>();

			foreach (var article in articles)
			{
				SyndicationItem item = new SyndicationItem {
					Title = new TextSyndicationContent(article.Title, TextSyndicationContentKind.Plaintext),
					Content = new TextSyndicationContent(article.Body, TextSyndicationContentKind.XHtml),
					PublishDate = article.ReleaseDate,
					Id = article.ArticleID.ToString()
				};

				item.Links.Add(new SyndicationLink {
					Uri = new Uri(HttpContext.Current.Request.Url, "/" + article.ArticleID + "/" + article.Path),
					RelationshipType = "alternate"
				});

				item.Authors.Add(new SyndicationPerson {
					Name = article.AddedBy
				});

				item.Categories.Add(new SyndicationCategory {
					Name = article.Category.Title
				});

				items.Add(item);
			}

			feed.Items = items;

			Atom10FeedFormatter atomFormatter = new Atom10FeedFormatter(feed);
			atomFormatter.WriteTo(new XmlTextWriter(response.Output));
		}
	}
}
