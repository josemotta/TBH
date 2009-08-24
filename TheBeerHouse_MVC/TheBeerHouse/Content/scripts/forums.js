/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
* Forum Post
***********************************************************************************************************/

$("#title").focus(function() { ShowMessage(this, "Enter the title for your post."); });
$("#body").focus(function() { ShowMessage(this, "Enter the body of your post."); });

function ValidateTitle() {
	return VerifyRequiredField("#title", "required");
}

function ValidateBody() {
	return VerifyRequiredField("#body", "required");
}

function ValidatePost() {
	return ValidateTitle()
		&& ValidateBody();
}

$("form.post-create").validate(ValidatePost);

/************************************************************************************************************
* Rich Text Editor
***********************************************************************************************************/

var bodyEditor;

$(document).ready(function() {
	bodyEditor = new tinymce.Editor("body", __editorConfig);
	bodyEditor.onChange.add(function(ed) { bodyEditor.save(); });
	bodyEditor.onClick.add(function(ed) { ShowMessage("#body", "Enter the body of your article."); });
	bodyEditor.render();
});

// clears the message from the description when another input gets focus
$(":input")
	.focus(function() { HideMessage("#body"); })
	.blur(function() { HideMessage("#body"); });

/************************************************************************************************************
* Forum
***********************************************************************************************************/

function VoteSuccess(data) {
	if (data.object.error) {
		alert("You must be logged in to vote.");
		return;
	}

	var button = $(".post .vote-" + (data.object.direction > 0 ? "up" : "down"));
	var number = $(".post strong");

	// remove current selections and select correct button
	$(".post .vote-button a").removeClass("selected");
	button.addClass("selected");

	// set new count value
	number.text(data.object.voteCount);
}

$(".post .vote-button a").click(function() {
	var postId = $("#postId").val();
	var href = $(this).attr("href");
	var direction = (href == "#up") ? 1 : -1;

	$.post(
		"/forum/vote",
		{ postId: postId, direction: direction },
		VoteSuccess,
		"json"
	);

	return false;
});