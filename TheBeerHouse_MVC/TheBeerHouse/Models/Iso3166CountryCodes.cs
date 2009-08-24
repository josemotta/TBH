using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheBeerHouse.Models
{
	public static class Iso3166CountryCodes
	{
		public static readonly IDictionary<string, string> CountryDictonary;

		static Iso3166CountryCodes()
		{
			string[] iso3166 = Properties.Resources.Iso3166CountryCodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, string> countires = new Dictionary<string, string>(iso3166.Length);

			foreach (string country in iso3166)
				try { countires.Add(country, (new RegionInfo(country)).NativeName); }
				catch { continue; }

			CountryDictonary = countires;
		}
	}
}
