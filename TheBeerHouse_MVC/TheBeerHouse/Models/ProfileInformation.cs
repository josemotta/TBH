using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace TheBeerHouse.Models
{
    public class ProfileInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GenderType { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Occupation { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string SubscriptionType { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        public static SelectList GetSubscriptionList(String subscription)
        {
            List<SelectListItem> subscriptionList = new List<SelectListItem>() 
            {
                new SelectListItem() { Value = "HTML", Text = "Subscribe to HTML Version" },
                new SelectListItem() { Value = "Plain", Text = "Subscribe to Plain Text Version" },
                new SelectListItem() { Value = "None", Text = "No Thanks" }
            };
            return new SelectList(subscriptionList, "Value", "Text", subscription ?? "HTML");
        }

        public static SelectList GetGenderList(String gender)
        {
            List<SelectListItem> genderList = new List<SelectListItem>() 
            {
                new SelectListItem() { Value = "M", Text = "Male" },
                new SelectListItem() { Value = "F", Text = "Female" }
            };
            return new SelectList(genderList, "Value", "Text", gender ?? "M");
        }

        public static SelectList GetCountryList(String country)
        {
            return new SelectList(Iso3166CountryCodes.CountryDictonary, "Key", "Value", country ?? "US");
        }

        public static SelectList GetOccupationList(String occupation)
        {
            TheBeerHouseDataContext dataContext = new TheBeerHouseDataContext();
            var Occupations = from occupationList in dataContext.Occupations
                              orderby occupationList.OccupationName
                              select occupationList;
            return new SelectList(Occupations, "OccupationName", "OccupationName", occupation ?? "Business Owner");
        }

        public static SelectList GetLanguageList(String language)
        {
            TheBeerHouseDataContext dataContext = new TheBeerHouseDataContext();
            var Languages = from languageList in dataContext.Languages
                            orderby languageList.LanguageName
                            select languageList;
            return new SelectList(Languages, "LanguageName", "LanguageName", language ?? "English");
        }
    }
}
