<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl" %>

<%= Html.If(!String.IsNullOrEmpty(TempData["ErrorMessage"] as string), () => "<div class=\"error\">" + TempData["ErrorMessage"] + "</div>")
		.ElseIf(!String.IsNullOrEmpty(TempData["SuccessMessage"] as string), () => "<div class=\"success\">" + TempData["SuccessMessage"] + "</div>")
		.ElseIf(!String.IsNullOrEmpty(TempData["WarningMessage"] as string), () => "<div class=\"warning\">" + TempData["WarningMessage"] + "</div>")
		.ElseIf(!String.IsNullOrEmpty(TempData["InformationMessage"] as string), () => "<div class=\"info\">" + TempData["InformationMessage"] + "</div>")
		.Else(() => String.Empty) %>