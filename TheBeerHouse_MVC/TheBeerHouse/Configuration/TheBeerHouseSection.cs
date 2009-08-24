using System.Configuration;
using System.Web.Configuration;

namespace TheBeerHouse.Configuration
{
	public class TheBeerHouseSection : ConfigurationSection
	{
		public readonly static TheBeerHouseSection Current = (TheBeerHouseSection)WebConfigurationManager.GetSection("theBeerHouse");

		[ConfigurationProperty("contactForm", IsRequired = true)]
		public ContactFormElement ContactForm
		{
			get { return (ContactFormElement)base["contactForm"]; }
		}

		[ConfigurationProperty("articles", IsRequired = true)]
		public ArticlesElement Articles
		{
			get { return (ArticlesElement)base["articles"]; }
		}

		[ConfigurationProperty("polls", IsRequired = true)]
		public PollsElement Polls
		{
			get { return (PollsElement)base["polls"]; }
		}

		[ConfigurationProperty("newsletters", IsRequired = true)]
		public NewslettersElement Newsletters
		{
			get { return (NewslettersElement)base["newsletters"]; }
		}

		[ConfigurationProperty("forums", IsRequired = true)]
		public ForumsElement Forums
		{
			get { return (ForumsElement)base["forums"]; }
		}

		[ConfigurationProperty("commerce", IsRequired = true)]
		public CommerceElement Commerce
		{
			get { return (CommerceElement)base["commerce"]; }
		}
	}
}
