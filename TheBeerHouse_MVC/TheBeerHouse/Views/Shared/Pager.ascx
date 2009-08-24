<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="TheBeerHouse.Views.Shared.Pager" %>

<div class="pager">
<% if (this.ViewData.Model.TotalCount > this.ViewData.Model.PageSize) { %>
	<% if (this.ViewData.Model.PageNumber > 1) { 
	%><a class="cap" href="<%= GetPageUrl(this.ViewData.Model.PageNumber - 1) %>" rel="prev">Previous</a><%
	 } else { %>	<a class="cap disabled" rel="prev">Previous</a>	<% }

	int middleStart, middleEnd;
	GetStartAndEndPage(out middleStart, out middleEnd);

	if (middleStart > 1) { 
	%><a href="<%= GetPageUrl(1) %>">1</a><%
	 if (middleStart > 2) { 
	%><span>...</span><% 
	 } }

	for (int i = middleStart; i <= middleEnd; i++) {
		if (ViewData.Model.PageNumber == i) {
	%><a class="selected" rel="self"><%= i%></a><%
	 } else {
	%><a href="<%= GetPageUrl(i) %>"><%= i.ToString()%></a><%
		}
	}
	   
	if (middleEnd < ViewData.Model.PageCount) {
		if (middleEnd < ViewData.Model.PageCount - 1) { %>
	<span>...</span>
	<% } 
	%><a href="<%= GetPageUrl(ViewData.Model.PageCount) %>"><%= ViewData.Model.PageCount %></a><%
	 }
 
	if (this.ViewData.Model.PageNumber < this.ViewData.Model.PageCount) { 
	%><a class="cap" href="<%= GetPageUrl(this.ViewData.Model.PageNumber + 1) %>" rel="next">Next</a><%
	 } else {
	%><a class="cap disabled" rel="next">Next</a><%
} } %>
</div>