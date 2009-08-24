<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<TheBeerHouse.Models.Newsletter>>" %>

<div id="Newsletter_Status_Table">

<table width="100%" cellpadding="2" cellspacing="0" summary="User Grid" border="1">
<tr style="font-weight:bold; background-color:#A8C3CB; ">
<td>&nbsp;</td>
<td align="center">Date Sent</td>
<td align="center">Status</td>
<td>&nbsp;</td>
</tr>

<% foreach (var newsletter in ViewData.Model) { %>
<tr>
<td><%= Html.ActionLink(newsletter.Subject, "EditNewsletter", new { newsletterId = newsletter.NewsletterID }) %></td>
<td><%= newsletter.DateSent %></td>
<td><%= newsletter.Status %></td>
<td align="center"><a href="<%= Url.Action("RemoveNewsletter", new { newsletterId = newsletter.NewsletterID }) %>"><img border="0" alt="Delete Newsletter" src="/content/images/DeleteSymbol.png" title="Delete Newsletter" align="middle"/></a></td>
</tr>
<% } %>
</table>

</div>

