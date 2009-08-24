/// <reference path="global.js" />
/// <reference path="jquery.intellisense.js" />

/************************************************************************************************************
 * Newsletter
 ***********************************************************************************************************/


$("#subject").focus(function () { ShowMessage(this, "Enter the subject for your newsletter."); });

function ValidateTitle () {
	return VerifyRequiredField("#subject", "required");
}

function ValidateBody () {
	return VerifyRequiredField("#body", "required");
}

function ValidateNewsletter () {
	return ValidateTitle()
		&& ValidateBody();
}

$("form.newsletter-create").validate(ValidateNewsletter);

/************************************************************************************************************
 * Rich Text Editor
 ***********************************************************************************************************/

var bodyEditor;

$(document).ready(function () {
	bodyEditor = new tinymce.Editor("body", __editorConfig);
	bodyEditor.onChange.add(function (ed) { bodyEditor.save(); });
	bodyEditor.onClick.add(function (ed) { ShowMessage("#body", "Enter the body of your newsletter."); });
	bodyEditor.render();
});

// clears the message from the description when another input gets focus
$(":input")
	.focus(function () { HideMessage("#body"); })
	.blur(function () { HideMessage("#body"); });
	
/************************************************************************************************************
 * Update Status
 ***********************************************************************************************************/	
	
function UpdateStatus() {
    $.ajax({
       type: "GET",
       url: "/Newsletter/UpdateStatus",
       dataType: "html",
       sucess: function(result) {
            var domElement = $(result);
            $("#Newsletter_Status_Table").replaceWith(domElement);
       }
    });
}