using System.Configuration;

namespace TheBeerHouse.Configuration
{
	public class ForumsElement : ConfigurationElement
	{
		[ConfigurationProperty("postReplyPageSize", DefaultValue = "10")]
		public int ThreadsPageSize
		{
			get { return (int)base["postReplyPageSize"]; }
			set { base["postReplyPageSize"] = value; }
		}

		[ConfigurationProperty("forumPageSize", DefaultValue = "25")]
		public int PostsPageSize
		{
			get { return (int)base["forumPageSize"]; }
			set { base["forumPageSize"] = value; }
		}
	}
}
