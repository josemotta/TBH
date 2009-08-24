<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.UserInformation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action("ForgotPassword", "User") %>">

	<% if (!String.IsNullOrEmpty(Model.SecretQuestion)){ %>
        <%= Html.Hidden("UserName") %>
	
	    <p class="field input"><label for="secretAnswer"><%= Model.SecretQuestion %></label><br />
			<%= Html.TextBox("SecretAnswer")%>
        <span class="input-message"></span></p>
	<% } else { %>
	<p class="field input"><label for="userName">Please enter your username</label><br />
			<%= Html.TextBox("UserName")%>
        <span class="input-message"></span></p>	
	<% } %>	

	<hr />
	<p><button type="submit" id="user-login-button">Submit</button></p>
		
</form>

</asp:Content>
