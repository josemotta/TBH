<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Security.MembershipUser>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form method="post" action="EditUser" class="manage-user">
<!-- The hidden control manages the username -->
<%= Html.Hidden("id") %>

<!-- Shows detailed information about member from MembershipUser object -->
<p><b>UserName:</b><%= Html.Encode(Model.UserName) %> </p>
<p><b>E-Mail:</b> <%= Html.Encode(Model.Email) %></p>
<p><b>Registered:</b> <%= Model.CreationDate.ToLocalTime()%></p>
<p><b>Last Login:</b> <%= Model.LastLoginDate.ToLocalTime()%></p>
<p><b>Last Activity:</b> <%= Model.LastActivityDate.ToLocalTime()%></p>
<p><b>Online Now:</b> <%= Html.CheckBox("onlineNow", Model.IsOnline, new { disabled = "true" })%></p>
<p><b>Approved:</b> <%= Html.CheckBox("approved", Model.IsApproved)%></p>
<p><b>Locked Out:</b> <%= Html.CheckBox("lockedOut", Model.IsLockedOut, new { disabled = "true" })%></p>
<hr />

<!-- This portion allows you to actually edit the roles of the user -->
<h2>Edit User Roles</h2>
<ul>
<% foreach (string role in (string[])ViewData["roles"]){ %>
	<li><%= Html.CheckBox("role." + role,  Roles.IsUserInRole(Model.UserName, role))%>
	<label for="role.<%= role %>"><%= Html.Encode(role) %></label></li>
<% } %>
</ul>

<p>
	<button type="submit" id="user-editUser-button">Update User</button>
	<button type="button" onclick="location.href='/User/ManageUser'" style="margin-left: 2em;">Return</button>
</p>

</form>
</asp:Content>