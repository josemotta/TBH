using System.Configuration;

namespace TheBeerHouse.Configuration
{
	public class CommerceElement : ConfigurationElement
	{
		[ConfigurationProperty("payPalServer", DefaultValue = "https://www.paypal.com/cgi-bin/webscr")]
		public string PayPalServer
		{
			get { return (string)base["payPalServer"]; }
			set { base["payPalServer"] = value; }
		}

		[ConfigurationProperty("payPalAccount", IsRequired = true)]
		public string PayPalAccount
		{
			get { return (string)base["payPalAccount"]; }
			set { base["payPalAccount"] = value; }
		}

		[ConfigurationProperty("payPalIdentityToken", IsRequired = true)]
		public string PayPalIdentityToken
		{
			get { return (string)base["payPalIdentityToken"]; }
			set { base["payPalIdentityToken"] = value; }
		}
	}
}