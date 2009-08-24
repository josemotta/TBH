using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBeerHouse.Models;
using System.Net.Mail;
using System.Web.Security;
using System.Web.Profile;
using System.Threading;
using TheBeerHouse.Configuration;

namespace TheBeerHouse.Controllers
{
	[HandleError]
	public class NewsletterController : Controller
	{
		/// <summary>
		/// Indexes this instance.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			ViewData["PageTitle"] = "Newsletters";

			return View();
		}

		/// <summary>
		/// Manages Newsletters
		/// </summary>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult ManageNewsletters()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Newsletters.OrderByDescending(n => n.AddedDate);

			ViewData["PageTitle"] = "Manage Newsletters";
			return View(viewData);
		}

		/// <summary>
		/// Gets an update status on a particular id
		/// </summary>
		/// <returns></returns>
		[Service]
		[Authorize(Roles = "Editor")]
		public ActionResult UpdateStatus()
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Newsletters.OrderByDescending(n => n.AddedDate);

			return PartialView("Newsletter/NewsletterStatus", viewData);
		}

		/// <summary>
		/// Creates the Newsletter
		/// </summary>
		/// <param name="subject">Subject</param>
		/// <param name="body">Body Text of Email</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		[ValidateInput(false)]
		public ActionResult CreateNewsletter(string subject, string body)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();

			if (!String.IsNullOrEmpty(subject) && !String.IsNullOrEmpty(body))
			{
				Newsletter newsletter = new Newsletter() {
					AddedDate = DateTime.Now,
					AddedBy = User.Identity.Name,
					Status = "Queued",
					Subject = subject,
					HtmlBody = body,
					PlainTextBody = body
				};

				dc.Newsletters.InsertOnSubmit(newsletter);
				dc.SubmitChanges();

				Thread thread = new Thread(new ParameterizedThreadStart(SendNewsletter));
				thread.Priority = ThreadPriority.BelowNormal;
				thread.Start(new object[] { newsletter, dc });

				return RedirectToAction("ManageNewsletters");
			}

			ViewData["PageTitle"] = "Create Newsletter";
			return View(new Newsletter());
		}

		/// <summary>
		/// Edits the newsletter.
		/// </summary>
		/// <param name="newsletterId">The newsletter id.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="body">The body.</param>
		/// <returns></returns>
		[ValidateInput(false)]
		[Authorize(Roles = "Editor")]
		public ActionResult EditNewsletter(int? newsletterId, string subject, string body)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var newsletter = dc.Newsletters.FirstOrDefault(n => n.NewsletterID == newsletterId);

			if (newsletter == null)
				throw new HttpException(404, "The newsletter could not be found.");

			if (!String.IsNullOrEmpty(subject) && !String.IsNullOrEmpty(body))
			{
				newsletter.Subject = subject;
				newsletter.HtmlBody = body;
				newsletter.PlainTextBody = body;

				dc.SubmitChanges();

				Thread thread = new Thread(new ParameterizedThreadStart(SendNewsletter));
				thread.Priority = ThreadPriority.BelowNormal;
				thread.Start(new object[] { newsletter, dc });

				return RedirectToAction("ManageNewsletters", "Newsletter");
			}

			ViewData["PageTitle"] = "Edit Newsletter";
			return View("CreateNewsletter", newsletter);
		}

		/// <summary>
		/// Removes the newsletter.
		/// </summary>
		/// <param name="newsletterId">The newsletter id.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult RemoveNewsletter(int? newsletterId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var newsletter = dc.Newsletters.FirstOrDefault(n => n.NewsletterID == newsletterId);

			if (newsletter == null)
				throw new HttpException(404, "The newsletter could not be found.");

			dc.Newsletters.DeleteOnSubmit(newsletter);
			dc.SubmitChanges();

			return RedirectToAction("ManageNewsletters");
		}

		/// <summary>
		/// Sends the newsletter
		/// </summary>
		/// <param name="data">object</param>
		/// <returns></returns>
		[NonAction]
		private void SendNewsletter(object data)
		{
			object[] parameters = (object[])data;
			
			// get parameters from the data object passed in
			Newsletter newsletter = (Newsletter)parameters[0];
			TheBeerHouseDataContext dc = (TheBeerHouseDataContext)parameters[1];

			MembershipUserCollection membershipUserCollection = Membership.GetAllUsers();
			ProfileBase profileBase = new ProfileBase();
			NewslettersElement config = Configuration.TheBeerHouseSection.Current.Newsletters;

			// create the message
			MailMessage mailMessage = new MailMessage();
			mailMessage.Body = newsletter.HtmlBody;
			mailMessage.From = new MailAddress(config.FromEmail, config.FromDisplayName);
			mailMessage.Subject = newsletter.Subject;

			// add members to the BCC
			foreach (MembershipUser membershipUser in membershipUserCollection)
			{
				profileBase = ProfileBase.Create(membershipUser.UserName);
				if (profileBase.GetPropertyValue("Subscription").ToString() != "None")
					mailMessage.Bcc.Add(membershipUser.Email);
			}

			// send the e-mail
			SmtpClient smtpClient = new SmtpClient();
			try
			{
				smtpClient.Send(mailMessage);
				newsletter.Status = "Sent";
				newsletter.DateSent = DateTime.Now;
				dc.SubmitChanges();
			}
			catch (Exception ex)
			{
				newsletter.Status = "Failed: " + ex.Message;
				dc.SubmitChanges();
			}
		}
	}
}