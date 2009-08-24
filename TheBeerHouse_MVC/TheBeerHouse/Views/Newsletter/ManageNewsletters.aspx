<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Newsletter>>" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<% Html.RenderPartial("~/Views/Shared/Newsletter/NewsletterStatus.ascx", ViewData.Model); %>
<p><%= Html.ActionLink("Create Newsletter", "CreateNewsletter") %></p>

</asp:Content>

<asp:Content ID="ScriptContent" ContentPlaceHolderID="ScriptContent" runat="server">
<script type="text/javascript" src="/content/scripts/tiny_mce/tiny_mce_src.js"></script>
<script type="text/javascript" src="/content/scripts/manage-newsletter.js"></script>
<script type="text/javascript">
	setInterval(UpdateStatus, 4000);
</script>
</asp:Content>
