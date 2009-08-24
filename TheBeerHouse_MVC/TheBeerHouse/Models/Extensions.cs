using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheBeerHouse.Models
{
	public static class Extensions
	{
		public static Pagination<T> AsPagination<T>(this IEnumerable<T> collection, int startIndex, int requestedCount)
		{
			return new Pagination<T>(
				collection,
				startIndex,
				requestedCount,
				collection.Count()
			);
		}
	}
}
