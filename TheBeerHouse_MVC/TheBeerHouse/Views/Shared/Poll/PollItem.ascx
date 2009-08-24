<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<TheBeerHouse.Models.Poll>" %>

<div id="poll-<%= ViewData.Model.PollID %>" class="poll">
	<h2><%= Html.Encode(Model.QuestionText) %></h2>
	
	<form action="#" method="post">
		<ul class="poll-options">
		<% foreach(var option in ViewData.Model.PollOptions) { %>
			<li class="option" id="option-<%= option.OptionID %>"><input type="radio" id="option-<%= option.OptionID %>" name="post-<%= ViewData.Model.PollID %>" value="<%= option.OptionID %>" /><label class="text" for="option-<%= option.OptionID %>"><%= Html.Encode(option.OptionText) %></label></li>
		<% } %>
		</ul>
	
		<button type="submit" name="poll-submit">Vote</button>
	</form>
</div>