<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Security.MembershipUserCollection>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form  method="post" action="ManageUser" class="manage-user">

<p><b>Total Registered Users: </b><%= ViewData["RegisteredUsers"]%></p>
<p><b>Users Online Now: </b><%= ViewData["UsersOnlineNow"] %></p>
<hr />

<!-- user search options -->
<p>Search Members: <%= Html.TextBox("searchInput",ViewData["searchInput"])%>&nbsp;&nbsp; <%= Html.DropDownList("searchType", (SelectList)ViewData["searchOptionList"])%>&nbsp;&nbsp;<button type="submit" id="user-manageUser-button" style="height:28px">Search</button></p>
<hr />

<!-- the user grid -->
<table cellpadding="2" cellspacing="0" align="left" summary="User Grid" border="1">
<thead>
<tr style="font-weight:bold; background-color:#A8C3CB;">
	<th align="center">Username</th>
	<th align="center">E-Mail</th>
	<th align="center">Created</th>
	<th align="center">Last Used</th>
	<th align="center">Approved</th>
	<th>&nbsp;</th>
	<th>&nbsp;</th>
</tr>
</thead>
<tbody>
<% foreach(MembershipUser membershipUser in ViewData.Model) { %>
<tr  id="user-<%= membershipUser.UserName %>">
	<td><%= Html.Encode(membershipUser.UserName) %></td>
	<td><%= Html.Encode(membershipUser.Email) %>></td>
	<td align="center"><%= membershipUser.CreationDate.ToLocalTime() %></td>
	<td align="center"><%= membershipUser.LastActivityDate.ToLocalTime() %></td>
	<td align="center"><%= Html.CheckBox("approved" ,membershipUser.IsApproved, new {disabled="true"}) %></td>
	<td><a href="EditUser?id=<%= membershipUser.UserName %>"><img border="0" alt="Edit User" src="/content/images/EditSymbol.png" title="Modify User" align="middle"/></a></td>
	<td align="center"><a class="delete-user-button" href="#" meta:id="<%= membershipUser.UserName %>"><img border="0" alt="Delete User"  src="/content/images/DeleteSymbol.png" title="Delete User" align="middle"/></a></td>
</tr>
<% } %>
</tbody>
</table>

</form>
</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-users.js"></script>
</asp:Content>
