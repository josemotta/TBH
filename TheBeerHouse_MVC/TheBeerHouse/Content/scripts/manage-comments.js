/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

$(".edit-comment").click(function () {
	var id = $(this).attr("meta:id"),
		comment = $("#comment-" + id),
		bodyText = comment.find(".body").text(),
		nameText = comment.find(".name").text();

	// hide all the childrend	
	comment.children().hide();

	var commentText = "";
	commentText += "<form><div class=\"comment-header field\"><label for=\"name-" + id + "\">Commentor's Name</label><br/><input type=\"text\" id=\"name-" + id + "\" class=\"edit-name\" value=\"" + nameText + "\" /></div>";
	commentText += "<div class=\"field\"><label for=\"body-" + id + "\">Comment Body</label><br/><textarea class=\"edit-body\" id=\"body-" + id + "\">" + bodyText + "</textarea><br/><button type=\"button\" class=\"update\" meta:id=\"" + id + "\">Update</button>&nbsp;<button type=\"button\" class=\"cancel\">Cancel</button></div></form>";

	var commentForm = $(commentText);
	
	// update the form
	commentForm.find(".update").click(function () {
		var id = $(this).attr("meta:id");
		var nameFormText = $(this).prevAll(".edit-name").val();
		var bodyFormText = $(this).prevAll(".edit-body").val();

		$.post(
			"/article/editcomment",
			{ commentId: id, name: nameFormText, body: bodyFormText },
			function (data) {
				var comment = $("#comment-" + data.object.commentId);
				comment.children("form").remove();
				comment.children(".body").text(data.object.body);
				comment.children(".name").text(data.object.name);
				comment.children().show();
			},
			"json"
		);
	});
	
	// cancel the update
	commentForm.find(".cancel").click(function () {
		$(this).parents(".comment").children(":hidden").show();
		$(this).parents("form").remove();
	});
	
	// add the form to the current comment
	comment.append(commentForm);

	return false;
});

$(".remove-comment").click(function () {
	var id = $(this).attr("meta:id");
	
	$.post(
		"/article/removecomment",
		{ commentId: id },
		function (data) {
			$("#comment-" + data.object.commentId).next(".admin").fadeOut("slow", function () { $(this).remove() });
			$("#comment-" + data.object.commentId).fadeOut("slow", function () { $(this).remove() });
		},
		"json"
	);
	
	return false;
});