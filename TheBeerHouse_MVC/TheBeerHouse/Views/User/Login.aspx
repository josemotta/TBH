<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action("Login", "User") %>" class="user-login">
		
	<input type="hidden" name="returnUrl" value="<%= Html.If(!String.IsNullOrEmpty(Request.QueryString["returnUrl"]), () => Request.QueryString["returnUrl"])
														 .ElseIf(Request.UrlReferrer != null, () => Request.UrlReferrer.ToString())
														 .Else(() => String.Empty) %>" />
		
	<p class="field input"><label for="userName">User Name</label><br />
			<%= Html.TextBox("UserName")%>
        <span class="input-message"></span></p>
		
	<p class="field input"><label for="password">Password</label><br />
	        <%= Html.Password("Password")%>
		<span class="input-message"></span></p>	
		
	<p class="field">
		<%= Html.CheckBox("persistent")%> Remember Me?</p>
	
	<p>
		<%= Html.ActionLink("Forgot Password", "ForgotPassword") %> | 
		<%= Html.ActionLink("Join Now", "Register") %>
	</p>
	
	<hr />
	<p><button type="submit" id="user-login-button">Submit</button></p>

		
</form>

</asp:Content>
