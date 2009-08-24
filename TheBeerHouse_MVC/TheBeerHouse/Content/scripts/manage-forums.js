/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
* Forum Post
***********************************************************************************************************/

$("#title").focus(function() { ShowMessage(this, "Enter the title for your forum."); });
$("#description").focus(function() { ShowMessage(this, "Enter the description of your forum."); });
$("#moderated").focus(function() { ShowMessage(this, "Do you want this forum to be moderated?"); });
$("#order").focus(function() { ShowMessage(this, "The order you want this forum to appear in, compared to the other forums."); });

function ValidateTitle() {
	return VerifyRequiredField("#title", "required");
}

function ValidateDescription() {
	return VerifyRequiredField("#description", "required");
}

function ValidateForum() {
	return ValidateTitle()
		&& ValidateDescription();
}

$("form.forum-create").validate(ValidateForum);

/************************************************************************************************************
* Admin Functions
***********************************************************************************************************/

$(".post .toggle-body").click(function() {
	$(this).next(".body").slideToggle("normal");

	return false;
});

$(".post .admin .close").click(function() {
	var postId = $(this).attr("meta:id");

	$.post(
		"/forum/closepost",
		{ postId: postId, closed: true },
		function(data) {
			$("#post-" + data.object.postId)
				.fadeOut("normal", function() {
					var title = $(this).find("h3");
					title.text(title.text() + " [closed]");
					$(".admin .close", this).remove();
				})
				.fadeIn("normal");
		},
		"json"
	);

	return false;
});

$(".post .admin .approve").click(function() {
	var postId = $(this).attr("meta:id");

	$.post(
		"/forum/approvepost",
		{ postId: postId, approved: true },
		function(data) {
			$("#post-" + data.object.postId).fadeOut("normal", function() {
				$(this).remove();
			});
		},
		"json"
	);

	return false;
});

$(".post .admin .remove").click(function() {
	var postId = $(this).attr("meta:id");

	$.post(
		"/forum/removepost",
		{ postId: postId },
		function(data) {
			$("#post-" + data.object.postId).fadeOut("normal", function() {
				$(this).remove();
			});
		},
		"json"
	);

	return false;
});

$(".reply .admin .remove").click(function() {
	var postId = $(this).attr("meta:id");

	$.post(
			"/forum/removepost",
			{ postId: postId },
			function(data) {
				$("#reply-" + data.object.postId).fadeOut("normal", function() {
					$(this).remove();
				});
			},
			"json"
		);

	return false;
});