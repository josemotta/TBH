using System;
using System.Web.Mvc;
using System.Globalization;

namespace TheBeerHouse.Controllers
{
	[HandleError]
    public class LocalizationController : Controller
    {
        public ActionResult TestLocalization()
        {
            Decimal amount = new Decimal(5);
            ViewData["CurrencyExample"] = String.Format(CultureInfo.CurrentCulture.NumberFormat, "{0:c}", amount);
            ViewData["PercentageExample"] = String.Format(CultureInfo.CurrentCulture.NumberFormat, "{0:p}", amount);
            ViewData["NumberExample"] = String.Format(CultureInfo.CurrentCulture.NumberFormat, "{0:N}", amount);
            ViewData["DateExample"] = Convert.ToDateTime(DateTime.Now, CultureInfo.CurrentCulture.DateTimeFormat);

            return View("TestLocalization");
        }
    }
}
