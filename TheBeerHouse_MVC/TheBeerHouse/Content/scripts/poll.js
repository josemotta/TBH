/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
* Poll
***********************************************************************************************************/

function RenderPoll(obj, data) {
	var poll = $(obj),
		total = data.object.total,
		item, percentValue, rightValue, leftValue;
	
	// clears all child nodes
	poll.empty();
	poll.append("<h2>" + data.object.question + "</h2>");
	poll.append("<ul class=\"poll-options\">");
	
	// go through each option and render it
	for(var i = 0; i < data.object.options.length; i++) {
		item = data.object.options[i];
		percentValue = Math.round(item.votes / total * 100);

		poll.append("<li class=\"option\" id=\"option-" + item.optionId + "\">"
		+ "<h3>" + item.text + "</h3>"
		+ "<div class=\"graph\"><img src=\"/Content/images/poll-graph.gif\" height=\"10\" width=\"" + percentValue + "%\" /></div>"
		+ "<div class=\"values\">" + percentValue + "% (" + item.votes + " votes)</div>"
		+ "</li>");
	}
	
	poll.append("</ul>");
	poll.append("<div class=\"total\">There are " + total + " total votes for this poll.</div>");
}

$(".poll form").submit(function() {
	var selection = $(this).find(":checked").val();

	if (selection != undefined) {
		$.post(
			"/poll/vote",
			{ optionId: selection },
			function(data, textStatus) {
				SetCookie("poll_" + data.object.pollId, selection, 30);
				// render the poll for the given data
				RenderPoll($("#poll-" + data.object.pollId), data);
			},
			"json"
		);
	}

	return false;
});