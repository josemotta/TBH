<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.UserInformation>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="<%= Url.Action("ChangePassword", "User") %>">
		
    <% if (Model.Password == null){ %>
	    <p class="field input"><label for="Password">Old Password</label><br />
			<%= Html.Password("Password")%>
        <span class="input-message"></span></p>
	<% } else { %>
	    <%= Html.Hidden("Password")%>
	<% } %>	       
        <p class="field input"><label for="ChangePassword">New Password</label><br />
			<%= Html.Password("ChangePassword")%>
        <span class="input-message"></span></p>
        
        <p class="field input"><label for="ConfirmPassword">Confirm New Password</label><br />
			<%= Html.Password("ConfirmPassword")%>
        <span class="input-message"></span></p>

	<hr />
	<p><button type="submit" id="user-login-button">Submit</button></p>
		
</form>

</asp:Content>
