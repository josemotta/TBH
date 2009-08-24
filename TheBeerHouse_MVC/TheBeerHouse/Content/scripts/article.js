/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
 * Rate
 ***********************************************************************************************************/

function RateArticleSuccess (data) {
	var value = data.object.averageRating;
	var imagePosition = "50";
	
	if (value <= 1.3)
		imagePosition = "10";
	else if (value <= 1.8)
		imagePosition = "15";
	else if (value <= 2.3)
		imagePosition = "20";
	else if (value <= 2.8)
		imagePosition = "25";
	else if (value <= 3.3)
		imagePosition = "30";
	else if (value <= 3.8)
		imagePosition = "35";
	else if (value <= 4.3)
		imagePosition = "40";
	else if (value <= 4.8)
		imagePosition = "45";
		
	$("#article-rating-value")
		.replaceWith("<img src=\"/Content/images/stars" + imagePosition + ".gif\" alt=\"" + value + "\" />");
		
	$("form.rate-article :input").attr("disabled", "true");
	$("form.rate-article").append("Your rating has been applied!");
}

$("form.rate-article").submit(function () {
	$.post(
		"/article/ratearticle",
		{	articleId: $("#articleId").val(),
			rating: $("#rating").val() },
		RateArticleSuccess,
		"json"
	);

	// don't allow submit because this is an ajax request	
	return false;
});

/************************************************************************************************************
 * Comments
 ***********************************************************************************************************/

function ValidateCommentName () {
	return VerifyRequiredField("#comment-name", "required");
}

function ValidateCommentEmail () {
	return VerifyRequiredField("#comment-email", "required");
}

function ValidateCommentBody () {
	return VerifyRequiredField("#comment-body", "required");
}

function CreateCommentSuccess (data, textStatus) {
	$(".new-comment").removeClass("new-comment").show("normal");

	var commentText = "";
	commentText += "<div id=\"comment-" + data.object.commentId + "\" class=\"comment new-comment\">";
	commentText += "<div class=\"comment-header\">Comment posted by " + data.object.name + " 0 sec ago</div>";
	commentText += "<blockquote>" + data.object.body + "</blockquote>";
	commentText += "</div>";

	var comment = $(commentText);

	// clear the body box
	$("#comment-body").val("");

	// add the new comment to the other comments
	comment
		.hide()
		.appendTo("#article-comments")
		.slideDown("slow");
}

$("form.comment-create").submit(function () {
	var valid = ValidateCommentName()
			&& ValidateCommentEmail()
			&& ValidateCommentBody();
			
	if (valid) {
		$.post(
			"/article/createcomment",
			{	articleId: $("#articleId").val(),
				name: $("#comment-name").val(),
				email: $("#comment-email").val(),
				body: $("#comment-body").val() },
			CreateCommentSuccess,
			"json"
		);
	}

	// don't allow submit because this is an ajax request	
	return false;
});