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

using System.Data.Linq;

namespace TheBeerHouse.Models
{
	public static class PollQueries
	{
		/// <summary>
		/// Currents the poll.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static Poll CurrentPoll(this Table<Poll> source)
		{
			return source.FirstOrDefault(p => p.IsCurrent == true);
		}

		/// <summary>
		/// Gets the poll.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static Poll GetPoll(this Table<Poll> source, int id)
		{
			return source.SingleOrDefault(p => p.PollID == id);
		}

		/// <summary>
		/// Gets the poll option.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static PollOption GetPollOption(this Table<PollOption> source, int id)
		{
			return source.SingleOrDefault(o => o.OptionID == id);
		}

		/// <summary>
		/// Gets the polls.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="archived">The archived.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public static Pagination<Poll> GetPolls(this Table<Poll> source, bool? archived, int page)
		{
			int count = Configuration.TheBeerHouseSection.Current.Polls.PageSize;
			int index = (page - 1) * count;

			var query = from p in source
						orderby p.AddedDate descending
						where (archived == null || p.IsArchived == archived.GetValueOrDefault(false))
						select p;

			return new Pagination<Poll>(
				query.Skip(index).Take(count),
				index,
				count,
				query.Count()
			);
		}
	}
}
