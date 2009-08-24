/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
* Manage
***********************************************************************************************************/

$(document).ready(function() {
	$("#polls :input").hide("fast");
});

$(".set-current").click(function() {
	var id = $(this).attr("meta:id");

	$.post(
		"/poll/setcurrent",
		{ pollId: id },
		function(data) {
			var poll = $("#set-current-" + data.object.pollId);
			poll.remove();
		},
		"json"
	);

	return false;
});

$(".set-archived").click(function() {
	var id = $(this).attr("meta:id"),
		archived = $(this).hasClass("archived");

	$.post(
		"/poll/setarchived",
		{ pollId: id, archive: !archived },
		function(data) {
			var poll = $("#set-archived-" + data.object.pollId);
			poll.removeClass("archived");
			if (data.object.isArchived)
				poll.addClass("archived");
			poll.text(data.object.isArchived ? "Allow Voting" : "Archive");
		},
		"json"
	);

	return false;
});

/************************************************************************************************************
 * Polls
 ***********************************************************************************************************/

$("#question").focus(function() { ShowMessage(this, "Enter the question you would like to ask in the poll."); });

function ValidateQuestion() {
	return VerifyRequiredField("#question", "required");
}

function ValidatePoll() {
	return ValidateQuestion();
}

$("form.poll-create").validate(ValidatePoll);

function EditOption() {
	var id = $(this).attr("meta:id"),
		option = $("#option-" + id),
		text = option.find(".text").text();
		
	// hide all the childrend	
	option.children().hide();

	var optionForm = $("<form><span class=\"field\"><input type=\"text\" id=\"text-" + id + "\" class=\"edit-text\" value=\"" + text + "\" /> <button type=\"button\" class=\"update\" meta:id=\"" + id + "\">Update</button>&nbsp;<button type=\"button\" class=\"cancel\">Cancel</button></span></form>");
	
	// update the form
	optionForm.find(".update").click(function () {
		var id = $(this).attr("meta:id"),
			formText = $(this).prevAll(".edit-text").val();

		$.post(
			"/poll/editoption",
			{ optionId: id, text: formText },
			function (data) {
				var comment = $("#option-" + data.object.optionId);
				comment.children("form").remove();
				comment.children(".text").text(data.object.text);
				comment.children().show();
			},
			"json"
		);
	});
	
	// cancel the update
	optionForm.find(".cancel").click(function () {
		$(this).parents(".option").children(":hidden").show();
		$(this).parents("form").remove();
	});
	
	// add the form to the current comment
	option.append(optionForm);

	return false;
}

$(".edit-option-button").click(EditOption);

function DeleteOption () {
	var id = $(this).attr("meta:id");
	
	$.post(
		"/poll/removeoption",
		{ optionId: id },
		function (data) {
			$("#option-" + data.object.optionId).fadeOut("slow", function () { $(this).remove() });
		},
		"json"
	);
	
	return false;
}

$(".delete-option-button").click(DeleteOption);

function AddOptionSuccess(data) {
	var optionItem = $("<li id=\"option-" + data.object.optionId + "\" class=\"option\"><span class=\"text\">" + data.object.text + "</span> <button type=\"button\" class=\"edit-option-button\" meta:id=\"" + data.object.optionId + "\">Edit</button>&nbsp;<button type=\"button\" class=\"delete-option-button\" meta:id=\"" + data.object.optionId + "\">Delete</button></li>");
	optionItem.find(".edit-option-button").click(EditOption);
	optionItem.find(".delete-option-button").click(DeleteOption);

	// clear the option box
	$("#option").val("");

	// add the new option to the other options
	optionItem.appendTo("#poll-options");
}

$("form.poll-options-create").submit(function() {
	var option = $("#option").val(),
		pollId = $("#pollId").val();

	$.post(
		"/poll/addoption",
		{ pollId: pollId, text: option },
		AddOptionSuccess,
		"json"
	);

	return false;
});