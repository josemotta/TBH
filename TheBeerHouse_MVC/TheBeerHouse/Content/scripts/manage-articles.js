/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
 * Articles
 ***********************************************************************************************************/

$("#title").focus(function () { ShowMessage(this, "Enter the title for your article."); });
$("#summary").focus(function () { ShowMessage(this, "(optional) Enter a summary for your article to be displayed instead of body."); });
$("#body").focus(function () { ShowMessage(this, "Enter the body of your article."); });
$("#country").focus(function () { ShowMessage(this, "(optional) Enter the country that is associated with this article."); });
$("#state").focus(function () { ShowMessage(this, "(optional) Enter the state that is associated with this article."); });
$("#city").focus(function () { ShowMessage(this, "(optional) Enter the city that is associated with this article."); });
$("#releaseDate").focus(function () { ShowMessage(this, "(optional) This is the date that you want this article to be first show on the site.  If left blank todays day is used."); });
$("#expireDate").focus(function () { ShowMessage(this, "(optional) This is the date that you want this article to stop showing on the site."); });

function ValidateTitle () {
	return VerifyRequiredField("#title", "required");
}

function ValidateBody () {
	return VerifyRequiredField("#body", "required");
}

function ValidateArticle () {
	return ValidateTitle()
		&& ValidateBody();
}

$("form.article-create").validate(ValidateArticle);

/************************************************************************************************************
 * Rich Text Editor
 ***********************************************************************************************************/

var bodyEditor;

$(document).ready(function () {
	bodyEditor = new tinymce.Editor("body", __editorConfig);
	bodyEditor.onChange.add(function (ed) { bodyEditor.save(); });
	bodyEditor.onClick.add(function (ed) { ShowMessage("#body", "Enter the body of your article."); });
	bodyEditor.render();
});

// clears the message from the description when another input gets focus
$(":input")
	.focus(function () { HideMessage("#body"); })
	.blur(function () { HideMessage("#body"); });