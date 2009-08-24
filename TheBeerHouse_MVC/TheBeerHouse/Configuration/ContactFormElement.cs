using System.Configuration;

namespace TheBeerHouse.Configuration
{
	public class ContactFormElement : ConfigurationElement
	{
		[ConfigurationProperty("mailSubject", DefaultValue = "Mail from TheBeerHouse: {0}")]
		public string MailSubject
		{
			get { return (string)base["mailSubject"]; }
			set { base["mailSubject"] = value; }
		}

		[ConfigurationProperty("mailTo", IsRequired = true)]
		public string MailTo
		{
			get { return (string)base["mailTo"]; }
			set { base["mailTo"] = value; }
		}

		[ConfigurationProperty("mailCC")]
		public string MailCC
		{
			get { return (string)base["mailCC"]; }
			set { base["mailCC"] = value; }
		}
	}
}
