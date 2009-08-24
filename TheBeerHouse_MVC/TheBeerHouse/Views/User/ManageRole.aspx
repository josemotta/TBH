<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>

<form  method="post" action="CreateRole" class="manage-role">
<p><b>Total Number of Roles: </b><%= ViewData["TotalRoles"].ToString()%></p>
<hr />

<!-- Functionality to add new roles -->
<p>Add New Role: <%= Html.TextBox("newRole")%> &nbsp;&nbsp;<button type="submit" id="user-manageRole-button" style="height:28px">Add Role</button></p>
<hr />

<!-- Table showing old rows -->
<table width="50%" cellpadding="2" cellspacing="0" align="left" summary="User Grid" border="1">
<tr style="font-weight:bold; background-color:#A8C3CB; ">
<td align="center">Role Name</td>
<td>&nbsp;</td>
</tr>
<% foreach(String role in Roles.GetAllRoles()) { %>
<tr id="role-<%= role %>">
<td><%= Html.Encode(role) %></td>
<td align="center"><a class="delete-role-button" href="#" meta:id="<%= role %>"><img border="0" alt="Delete Role"  src="/content/images/DeleteSymbol.png" title="Delete Role" align="middle"/></a></td>


</tr>
<% } %>
</table>		
</form>

</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/manage-roles.js"></script>
</asp:Content>
