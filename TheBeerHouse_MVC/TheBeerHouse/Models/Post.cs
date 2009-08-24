using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace TheBeerHouse.Models
{
	public partial class Post
	{
		/// <summary>
		/// Get the avatar for the AddedBy user.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public string GetAddedByAvatarUrl(int size)
		{
			MembershipUser membershipUser = Membership.GetUser(AddedBy, false);
			string identity = AddedByIP;

			if (membershipUser != null && membershipUser.Email != null)
				identity = membershipUser.Email.ToLower();

			return String.Format("http://www.gravatar.com/avatar/{0}?s={1}&d=identicon", identity.ToHashString("MD5"), size);
		}

		/// <summary>
		/// Get the avatar for the LastPostBy user.
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public string GetLastPostByAvatarUrl(int size)
		{
			MembershipUser membershipUser = Membership.GetUser(LastPostBy, false);
			string identity = AddedByIP;

			if (membershipUser != null && membershipUser.Email != null)
				identity = membershipUser.Email.ToLower();

			return String.Format("http://www.gravatar.com/avatar/{0}?s={1}&d=identicon", identity.ToHashString("MD5"), size);
		}
	}
}
