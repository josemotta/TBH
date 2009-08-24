<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TheBeerHouse.Models.Poll>" %>

<div id="poll-<%= ViewData.Model.PollID %>" class="poll">
	<h2><%= Html.Encode(Model.QuestionText) %>></h2>
	
	<% var total = ViewData.Model.PollOptions.Sum(o => o.Votes); %>	
	<ul class="poll-options">
	<% foreach(var option in ViewData.Model.PollOptions) {
		var percentValue = Math.Round(((decimal)option.Votes / (decimal)total) * 100M); %>
		<li class="option" id="option-<%= option.OptionID %>">
			<h3><%= Html.Encode(option.OptionText) %></h3>
			<div class="graph"><img src="/Content/images/poll-graph.gif" height="10" width="<%= Math.Floor(percentValue) %>%" /></div>
			<div class="values"><%= Math.Floor(percentValue) %>% (<%= option.Votes %> votes)</div>
		</li>
	<% } %>
	</ul>
	<div class="total">There are <%= total %> total votes for this poll.</div>
</div>