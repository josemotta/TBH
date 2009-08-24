<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TheBeerHouse.Models.Poll>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Message.ascx"); %>
		
<% if (ViewData.Model != null) { %>
<form method="post" action="<%= Url.Action("RemovePoll", "Poll", new { pollId = ViewData.Model.PollID }) %>" class="poll-remove">

	<button type="submit" name="remove" value="yes" class="yes">yes</button>
	<button type="submit" name="remove" value="no" class="no">no</button>

</form>
<% } %>
		
</asp:Content>

<asp:Content ID="SidebarContent" ContentPlaceHolderID="SidebarContent" runat="server">
<% Html.RenderPartial("~/Views/Shared/Poll/AdminSidebar.ascx"); %>
</asp:Content>
