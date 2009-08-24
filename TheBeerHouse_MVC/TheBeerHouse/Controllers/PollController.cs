using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.Security;

using ManagedFusion.Web.Mvc;

using TheBeerHouse.Models;

namespace TheBeerHouse.Controllers
{
	[Service, HandleError]
	public class PollController : Controller
	{
		/// <summary>
		/// Indexes the specified page.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public ActionResult Index(bool? archived, int page)
		{
			// if archived isn't set then we are only showing non-archived polls
			archived = archived ?? false;

			// make sure the current user is allowed to view archived polls
			if (archived.Value
				&& !Configuration.TheBeerHouseSection.Current.Polls.ArchiveIsPublic
				&& !Roles.IsUserInRole("Editor"))
				throw new HttpException(401, "The archived polls are only available to editors of the site.");

			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Polls.GetPolls(archived, page);

			ViewData["PageTitle"] = "Polls";
			return View(viewData);
		}

		/// <summary>
		/// Votes the specified id.
		/// </summary>
		/// <param name="optionId">The option id.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		public ActionResult Vote(int optionId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var option = dc.PollOptions.GetPollOption(optionId);

			if (option == null)
				throw new HttpException(404, "The poll option could not be found.");

			var poll = option.Poll;
			var options = new List<object>();

			if (poll.IsArchived || Request.Cookies["poll_" + poll.PollID] == null)
			{
				option.Votes++;
				dc.SubmitChanges();
			}

			foreach (var o in poll.PollOptions)
				options.Add(new {
					optionId = o.OptionID,
					text = o.OptionText,
					votes = o.Votes
				});

			return View(new {
				pollId = poll.PollID,
				total = poll.PollOptions.Sum(o => o.Votes),
				question = poll.QuestionText,
				options = options
			});
		}

		/// <summary>
		/// Manages the polls.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult ManagePolls([Default(1)]int page)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var viewData = dc.Polls.GetPolls(null, page);

			ViewData["PageTitle"] = "Manage Polls";
			return View(viewData);
		}

		/// <summary>
		/// Creates the poll.
		/// </summary>
		/// <param name="question">The question.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult CreatePoll(string question)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();

			if (!String.IsNullOrEmpty(question))
			{
				try
				{
					Poll poll = new Poll {
						QuestionText = question,
						Path = question.ToUrlFormat(),
						AddedBy = User.Identity.Name,
						AddedDate = DateTime.Now
					};

					dc.Polls.InsertOnSubmit(poll);
					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your poll has been created.";
					return RedirectToAction("EditPoll", new { pollId = poll.PollID });
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["question"] = question;

			ViewData["PageTitle"] = "Create Poll";

			return View("CreatePoll");
		}

		/// <summary>
		/// Edits the poll.
		/// </summary>
		/// <param name="pollId">The poll id.</param>
		/// <param name="question">The question.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult EditPoll(int pollId, string question)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Poll poll = dc.Polls.GetPoll(pollId);

			// throw a 404 Not Found if the requested poll is not in the database
			if (poll == null)
				throw new HttpException(404, "The poll could not be found.");

			if (!String.IsNullOrEmpty(question))
			{
				try
				{
					poll.QuestionText = question;
					poll.Path = question.ToUrlFormat();

					dc.SubmitChanges();

					TempData["SuccessMessage"] = "Your poll has been updated.";
				}
				catch (Exception exc)
				{
					TempData["ErrorMessage"] = exc.Message;
				}
			}

			ViewData["pollId"] = pollId;
			ViewData["question"] = question ?? poll.QuestionText;
			ViewData["options"] = poll.PollOptions.ToList();

			ViewData["PageTitle"] = "Edit Poll";

			return View("CreatePoll");
		}

		/// <summary>
		/// Removes the poll.
		/// </summary>
		/// <param name="pollId">The poll id.</param>
		/// <param name="remove">The remove.</param>
		/// <returns></returns>
		[Authorize(Roles = "Editor")]
		public ActionResult RemovePoll(int pollId, string remove)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			Poll poll = dc.Polls.GetPoll(pollId);

			// throw a 404 Not Found if the requested poll is not in the database
			if (poll == null)
				throw new HttpException(404, "The poll could not be found.");

			if (String.Equals(remove, "yes", StringComparison.OrdinalIgnoreCase))
			{
				dc.Polls.DeleteOnSubmit(poll);
				dc.SubmitChanges();

				TempData["SuccessMessage"] = "The poll, " + poll.QuestionText + ", has been deleted.";

				poll = null;
			}
			else if (String.Equals(remove, "no", StringComparison.OrdinalIgnoreCase))
			{
				TempData["InformationMessage"] = "The poll, " + poll.QuestionText + ", has NOT been deleted.";
			}
			else
			{
				TempData["WarningMessage"] = "Are you sure you want to delete " + poll.QuestionText + ".  You will not be able to recover this poll if you select YES.";
			}

			ViewData["PageTitle"] = "Remove Poll";

			return View(poll);
		}

		/// <summary>
		/// Adds the option.
		/// </summary>
		/// <param name="option">The option.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult AddOption(int pollId, string text)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var poll = dc.Polls.GetPoll(pollId);

			if (poll == null)
				throw new HttpException(404, "The poll could not be found.");

			var option = new PollOption {
				AddedBy = User.Identity.Name,
				AddedDate = DateTime.Now,
				OptionText = text,
				Votes = 0
			};
			poll.PollOptions.Add(option);
			dc.SubmitChanges();

			return View(new { optionId = option.OptionID, text = text });
		}

		/// <summary>
		/// Edits the option.
		/// </summary>
		/// <param name="optionId">The option id.</param>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult EditOption(int optionId, string text)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var option = dc.PollOptions.GetPollOption(optionId);

			if (option == null)
				throw new HttpException(404, "The poll option could not be found.");

			option.OptionText = text;
			dc.SubmitChanges();

			return View(new { optionId = option.OptionID, text = text });
		}

		/// <summary>
		/// Removes the option.
		/// </summary>
		/// <param name="optionId">The option id.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult RemoveOption(int optionId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var option = dc.PollOptions.GetPollOption(optionId);

			if (option == null)
				throw new HttpException(404, "The poll option could not be found.");

			dc.PollOptions.DeleteOnSubmit(option);
			dc.SubmitChanges();

			return View(new { optionId = optionId });
		}

		/// <summary>
		/// Sets the current.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult SetCurrent(int pollId)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var poll = dc.Polls.GetPoll(pollId);

			if (poll == null)
				throw new HttpException(404, "The poll could not be found.");

			// reset all polls to not current
			dc.ExecuteCommand("update TheBeerHouse.Polls set IsCurrent = 0;", new object[0]);

			poll.IsCurrent = true;
			dc.SubmitChanges();

			return View(new { pollId = pollId });
		}

		/// <summary>
		/// Sets the archived.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		[ServiceOnly, HttpPostOnly]
		[Authorize(Roles = "Editor")]
		public ActionResult SetArchived(int pollId, bool archive)
		{
			TheBeerHouseDataContext dc = new TheBeerHouseDataContext();
			var poll = dc.Polls.GetPoll(pollId);

			if (poll == null)
				throw new HttpException(404, "The poll could not be found.");

			poll.IsArchived = archive;
			poll.ArchivedDate = archive ? DateTime.Now : (DateTime?)null;
			dc.SubmitChanges();

			return View(new { pollId = pollId, isArchived = poll.IsArchived });
		}
	}
}