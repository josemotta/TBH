using System.Configuration;

namespace TheBeerHouse.Configuration
{
	public class NewslettersElement : ConfigurationElement
	{
		[ConfigurationProperty("fromEmail", IsRequired = true)]
		public string FromEmail
		{
			get { return (string)base["fromEmail"]; }
			set { base["fromEmail"] = value; }
		}

		[ConfigurationProperty("fromDisplayName", IsRequired = true)]
		public string FromDisplayName
		{
			get { return (string)base["fromDisplayName"]; }
			set { base["fromDisplayName"] = value; }
		}
	}
}
