using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TheBeerHouse.Models;
using System.Web.Profile;
using System.Collections;
using ManagedFusion.Web.Mvc;

namespace TheBeerHouse.Controllers
{
	/// <summary>
	/// This controller is for chapter 5 of the book.
	/// </summary>
	[HandleError]
	public class UserController : Controller
	{
		/// <summary>
		/// Logouts this instance.
		/// </summary>
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();

			return this.Redirect(307, Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : FormsAuthentication.DefaultUrl);
		}

		/// <summary>
		/// Logins the specified user name.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="persistent">The persistent.</param>
		/// <param name="ReturnUrl">The return URL.</param>
		public ActionResult Login()
		{
			ViewData["PageTitle"] = "Login";
			return View();
		}

        [AcceptVerbs("POST")]
        public ActionResult Login(string userName, string password, bool persistent, string returnUrl)
        {
            if (returnUrl != null && returnUrl.IndexOf("/user/login", StringComparison.OrdinalIgnoreCase) >= 0)
                returnUrl = null;

            if (Membership.ValidateUser(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, persistent);

                if (!String.IsNullOrEmpty(returnUrl))
                    return this.Redirect(303, returnUrl);
                else
                    return this.Redirect(303, FormsAuthentication.DefaultUrl);
            }
            else
                TempData["ErrorMessage"] = "Login failed! Please make sure you are using the correct user name and password.";

            ViewData["PageTitle"] = "Login";
            return View();
        }

		/// <summary>
		/// Forgots the password.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="secretAnswer">The secret answer.</param>
		/// <returns></returns>
		public ActionResult ForgotPassword()
		{
			ViewData["PageTitle"] = "Forgot Password";
            return View(new UserInformation());
		}

        [AcceptVerbs("POST")]
        public ActionResult ForgotPassword(string userName, string secretAnswer)
        {
            if (!String.IsNullOrEmpty(secretAnswer))
            {
                string resetPassword = Membership.Provider.ResetPassword(userName, secretAnswer);
                if (Membership.ValidateUser(userName, resetPassword))
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    return RedirectToAction("ChangePassword", new { resetPassword = resetPassword });
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid Response";
                    return View();
                }
            }

			MembershipUser membershipUser = Membership.GetUser(userName, false);
            UserInformation userinformation = new UserInformation();
			if (membershipUser != null)
			{
                userinformation.SecretQuestion = membershipUser.PasswordQuestion;
                userinformation.UserName = userName;
			}
			else
			{
				TempData["ErrorMessage"] = "The user you have specified is invalid, please recheck your username and try again";
                userinformation.SecretQuestion = String.Empty;
			}

			ViewData["PageTitle"] = "Forgot Password";
            return View(userinformation);
		}

		/// <summary>
		/// Registers the specified user name.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="confirmPassword">The confirm password.</param>
		/// <param name="email">The email.</param>
		/// <param name="secretQuestion">The secret question.</param>
		/// <param name="secretAnswer">The secret answer.</param>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns></returns>

        public ActionResult Register()
        {
            ViewData["PageTitle"] = "Register New User";
            return View();
        }

        [AcceptVerbs("POST")]
        public ActionResult Register(UserInformation userInformation)
        {
            if (userInformation.Password != userInformation.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Registration failed! Your passwords must match, please re-enter and try again";
            }
            else
            {
                MembershipCreateStatus membershipCreateStatus = new MembershipCreateStatus();
                try
                {
                    Membership.CreateUser(userInformation.UserName, userInformation.Password, userInformation.Email, userInformation.SecretQuestion, userInformation.SecretAnswer, true, out membershipCreateStatus);
                    if (membershipCreateStatus == MembershipCreateStatus.Success)
                    {
                        if (Membership.ValidateUser(userInformation.UserName, userInformation.Password))
                        {
                            FormsAuthentication.SetAuthCookie(userInformation.UserName, false);
                            if (!String.IsNullOrEmpty(userInformation.ReturnUrl))
                                return this.Redirect(303, userInformation.ReturnUrl);
                            else
                                return this.Redirect(303, FormsAuthentication.DefaultUrl);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Login failed! Please make sure you are using the correct user name and password.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = GetErrorMessage(membershipCreateStatus);
                    }
                }
                catch (Exception exception)
                {
                    TempData["ErrorMessage"] = exception.Message;
                }
            }

            ViewData["PageTitle"] = "Register New User";
            return View("Register", userInformation);
        }


		/// <summary>
		/// Changes the password.
		/// </summary>
		/// <param name="oldPassword">The old password.</param>
		/// <param name="newPassword">The new password.</param>
		/// <param name="confirmNewPassword">The confirm new password.</param>
		/// <param name="resetPassword">The reset password.</param>
		/// <returns></returns>
        public ActionResult ChangePassword(string resetPassword)
        {
            UserInformation userInformation = new UserInformation();
            if (!string.IsNullOrEmpty(resetPassword))
            {
                userInformation.Password = resetPassword;
            }
            ViewData["PageTitle"] = "Change Password";
            return View(userInformation);
        }

        [AcceptVerbs("POST")]
        public ActionResult ChangePassword(UserInformation userInformation)
        {
            if (userInformation.ChangePassword != userInformation.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Your new passwords do not match, please retype them and try again";
                return View(userInformation);
            }

            try
            {
                MembershipUser membershipUser = Membership.GetUser(HttpContext.User.Identity.Name, false);
                membershipUser.ChangePassword(userInformation.Password, userInformation.ChangePassword);
                TempData["SuccessMessage"] = "Your password has been sucessfully changed";
                ViewData["PageTitle"] = "Change Password";
                return View(userInformation);
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = "Password change has failed because: " + exception.Message;
                return View(userInformation);
            }
        }

		/// <summary>
		/// Users the profile.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="firstName">The first name.</param>
		/// <param name="lastName">The last name.</param>
		/// <param name="genderType">Type of the gender.</param>
		/// <param name="birthDate">The birth date.</param>
		/// <param name="occupation">The occupation.</param>
		/// <param name="website">The website.</param>
		/// <param name="language">The language.</param>
		/// <param name="country">The country.</param>
		/// <param name="subscriptionType">Type of the subscription.</param>
		/// <param name="street">The street.</param>
		/// <param name="city">The city.</param>
		/// <param name="state">The state.</param>
		/// <param name="zipcode">The zipcode.</param>
		/// <returns></returns>
        [Authorize]
        public ActionResult UserProfile()
        {
            string id = HttpContext.User.Identity.Name.ToString();
            
            ProfileBase profileBase;
            if (!String.IsNullOrEmpty(id))
            {
                profileBase = ProfileBase.Create(id);
            }
            else
            {
                profileBase = HttpContext.Profile as ProfileBase;
            }

            ViewData["subscriptionType"] = ProfileInformation.GetSubscriptionList(profileBase.GetPropertyValue("Subscription").ToString());
            ViewData["genderType"] = ProfileInformation.GetGenderList(profileBase.GetPropertyValue("PersonalInformation.Gender").ToString()); 
            ViewData["country"] = ProfileInformation.GetLanguageList(profileBase.GetPropertyValue("ContactInformation.Country").ToString());
            ViewData["occupation"] = ProfileInformation.GetOccupationList(profileBase.GetPropertyValue("PersonalInformation.Occupation").ToString());
            ViewData["language"] = ProfileInformation.GetLanguageList(profileBase.GetPropertyValue("Language").ToString()); 

            ProfileInformation profileInformation = new ProfileInformation()
            {
                FirstName = profileBase.GetPropertyValue("PersonalInformation.FirstName").ToString(),
                LastName = profileBase.GetPropertyValue("PersonalInformation.LastName").ToString(),
                BirthDate = (DateTime)profileBase.GetPropertyValue("PersonalInformation.BirthDate"),
                Website = profileBase.GetPropertyValue("PersonalInformation.Website").ToString(),
                Street = profileBase.GetPropertyValue("ContactInformation.Street").ToString(),
                City = profileBase.GetPropertyValue("ContactInformation.City").ToString(),
                State = profileBase.GetPropertyValue("ContactInformation.State").ToString(),
                Zipcode = profileBase.GetPropertyValue("ContactInformation.ZipCode").ToString()
            };

            return View(profileInformation); 
        }


        [Authorize]
        [AcceptVerbs("POST")]
        public ActionResult UserProfile(ProfileInformation profileInformation)
        {
            ProfileBase profileBase = HttpContext.Profile as ProfileBase;
            profileBase.SetPropertyValue("Subscription", profileInformation.SubscriptionType);
            profileBase.SetPropertyValue("Language", profileInformation.Language);

            profileBase.SetPropertyValue("PersonalInformation.FirstName", profileInformation.FirstName);
            profileBase.SetPropertyValue("PersonalInformation.LastName", profileInformation.LastName);
            profileBase.SetPropertyValue("PersonalInformation.Gender", profileInformation.GenderType);
            if (profileInformation.BirthDate != null)
	        {
                profileBase.SetPropertyValue("PersonalInformation.BirthDate", profileInformation.BirthDate);
	        }
            profileBase.SetPropertyValue("PersonalInformation.Occupation", profileInformation.Occupation);
            profileBase.SetPropertyValue("PersonalInformation.Website", profileInformation.Website);

            profileBase.SetPropertyValue("ContactInformation.Street", profileInformation.Street);
            profileBase.SetPropertyValue("ContactInformation.City", profileInformation.City);
            profileBase.SetPropertyValue("ContactInformation.State", profileInformation.State);
            profileBase.SetPropertyValue("ContactInformation.ZipCode", profileInformation.Zipcode);
            profileBase.SetPropertyValue("ContactInformation.Country", profileInformation.Country);
	        profileBase.Save();

            ViewData["subscriptionType"] = ProfileInformation.GetSubscriptionList(profileBase.GetPropertyValue("Subscription").ToString());
            ViewData["genderType"] = ProfileInformation.GetGenderList(profileBase.GetPropertyValue("PersonalInformation.Gender").ToString());
            ViewData["country"] = ProfileInformation.GetLanguageList(profileBase.GetPropertyValue("ContactInformation.Country").ToString());
            ViewData["occupation"] = ProfileInformation.GetOccupationList(profileBase.GetPropertyValue("PersonalInformation.Occupation").ToString());
            ViewData["language"] = ProfileInformation.GetLanguageList(profileBase.GetPropertyValue("Language").ToString());

            TempData["SuccessMessage"] = "Your profile information has been saved"; 
	        ViewData["PageTitle"] = "My Profile";
            return View(profileInformation);
        }

		/// <summary>
		/// Gets the error message.
		/// </summary>
		/// <param name="membershipCreateStatus">The membership create status.</param>
		/// <returns></returns>
		[NonAction]
		private string GetErrorMessage(MembershipCreateStatus membershipCreateStatus)
		{
			switch (membershipCreateStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "Username already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A username for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}

		#region Administrative Section

		/// <summary>
		/// Manages the user.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="searchType">Type of the search.</param>
		/// <param name="searchInput">The search input.</param>
		/// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult ManageUser(string searchType, string searchInput)
        {
	        List<SelectListItem> searchOptionList = new List<SelectListItem>() 
            {
                new SelectListItem() { Value = "UserName", Text = "UserName" },
                new SelectListItem() { Value = "Email", Text = "Email" }
            };

	        ViewData["searchOptionList"] = new SelectList(searchOptionList, "Value", "Text", searchType ?? "UserName");
	        ViewData["searchInput"] = searchInput ?? string.Empty;
	        ViewData["UsersOnlineNow"] = Membership.GetNumberOfUsersOnline().ToString();
	        ViewData["RegisteredUsers"] = Membership.GetAllUsers().Count.ToString();

	        MembershipUserCollection viewData;

	        if (String.IsNullOrEmpty(searchInput))
		        viewData = Membership.GetAllUsers();
	        else if (searchType == "Email")
		        viewData = Membership.FindUsersByEmail(searchInput);
	        else
		        viewData = Membership.FindUsersByName(searchInput);

	        ViewData["PageTitle"] = "Account Management";
	        return View(viewData);
        }

        [Service, HttpPostOnly]
        public ActionResult DeleteUser(string id)
        {
            Membership.DeleteUser(id);
            return View(new { id = id });
        }


		/// <summary>
		/// Manages the role.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="newRole">The new role.</param>
		/// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRole()
        {		
	        ViewData["TotalRoles"] = Roles.GetAllRoles().Count();
	        ViewData["PageTitle"] = "Role Management";
	        return View();
        }

        [Authorize(Roles = "Admin")]
        [AcceptVerbs("POST")]
        public ActionResult CreateRole(string newRole)
        {
            Roles.CreateRole(newRole);
            return RedirectToAction("ManageRole"); 
        }

        [Authorize(Roles = "Admin")]
        [Service, HttpPostOnly]
        public ActionResult DeleteRole(string id)
        {
            Roles.DeleteRole(id);
            return View(new { id = id });
        }

		/// <summary>
		/// Edits the user.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="approved">The approved.</param>
		/// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            ViewData["roles"] = (String[])Roles.GetAllRoles();
            MembershipUser membershipUser = Membership.GetUser(id);

            ViewData["PageTitle"] = "Edit " + id;
            return View(membershipUser);
        }

        [AcceptVerbs("POST")]
        public ActionResult EditUser(string id, bool approved)
        {
            //Is a list of all the user roles
            ArrayList removeRoleList = new ArrayList(Roles.GetAllRoles());

            //We are requesting the form variables directly from the form
            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("role."))
                {
                    String userRole = key.Substring(5, key.Length - 5);
                    removeRoleList.Remove(userRole);
                    if (!Roles.IsUserInRole(id, userRole))
                    {
                        Roles.AddUserToRole(id, userRole);
                    }
                }
            }

            foreach (string removeRole in removeRoleList)
                Roles.RemoveUserFromRole(id, removeRole);

            MembershipUser membershipUser = Membership.GetUser(id);
            membershipUser.IsApproved = approved;
            Membership.UpdateUser(membershipUser);

            TempData["SuccessMessage"] = "User Information has been updated";
            ViewData["roles"] = (String[])Roles.GetAllRoles();
            ViewData["PageTitle"] = "Edit " + id;
            return View(membershipUser);
        }

		#endregion
	}
}