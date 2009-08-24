/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
 * Set Cookies
 ***********************************************************************************************************/

function SetCookie(cookieName, cookieValue, nDays) {
	var today = new Date(),
		expire = new Date();
		
	if (nDays == null || nDays == 0) 
		nDays = 1;
	
	expire.setTime(today.getTime() + 3600000 * 24 * nDays);
	document.cookie = cookieName + "=" + escape(cookieValue) + ";expires=" + expire.toGMTString();
}

/************************************************************************************************************
 * jQuery extentions
 ***********************************************************************************************************/

jQuery.fn.extend({
	validate: function(fn) {
		///<summary>Validate the element.</summary>
		///<param name="fn" optional="true">The validation function.</param>
		///<returns type="bool" />

		// make sure the calling object exists
		if (this[0] == undefined)
			return true;

		if (fn) {
			return this.data("validate", fn);
		} else {
			fn = this.data("validate");
			var ret = true;

			// call the validate function if found
			if (fn)
				ret = fn();

			return ret;
		}
	}
});

/************************************************************************************************************
 * Extention Methods
 ***********************************************************************************************************/

String.prototype.trim = function () {
	return this.replace(/^\s*/, "").replace(/\s*$/, "");
}

/************************************************************************************************************
 * YUI Rich Text Editor Configuration
 ***********************************************************************************************************/
 
var __editorConfig = {
	mode: "textareas",
	theme: "advanced",
	plugins: "advhr,advimage,advlink,contextmenu,inlinepopups,media,paste,safari,spellchecker,xhtmlxtras",
	
	theme_advanced_toolbar_location: "top",
	theme_advanced_toolbar_align: "center",
	theme_advanced_statusbar_location: "bottom",
	theme_advanced_resizing_use_cookie: false,
	theme_advanced_resize_horizontal: false,
	theme_advanced_resizing: true,
	theme_advanced_resizing_min_height: 200,
	
	convert_urls: false,
	
	gecko_spellcheck: true,
	dialog_type: "modal",
	
	paste_auto_cleanup_on_paste: true,
	paste_convert_headers_to_strong: true,
	paste_strip_class_attributes: "all"
};

/************************************************************************************************************
 * Global Form Policy
 ***********************************************************************************************************/

// set gloal blur for all fields to hide message when not in focus anymore
$("form .field :input").blur(function () { HideMessage(this); });

// set gloal submit for forms to call validation event
$("form").submit(function () {
	var valid = $(this).validate();
	
	// if the form didn't validate then focus the input on the first error
	if (!valid) 
		$(this).find(":input[error]:first").focus();
		
	return valid;
});

/************************************************************************************************************
 * Form Validation
 ***********************************************************************************************************/
 
function DisplayMessage (display, input, css, text) {
	var message = $(input).parent().children("span.input-message");
	
	// clear all old css
	message.removeClass("input-info input-error");
	
	if (display) {
		if (css == "input-error") {
			message.data("error", text);
			$(input).attr("error", "true");
		}
	
		message.text(text);
		message.addClass(css);
	} else {
		// remove the attribute the error isn't 
		// being displayed any more
		if (css == "input-error") {
			message.removeData("error");
			$(input).removeAttr("error");
		}
		
		// if the error attribute is still present then display
		// the error message else clear out the text
		if (message.data("error") != undefined && message.data("error") != "") {
			message.text(message.data("error"));
			message.addClass("input-error");
		} else {
			message.text("");
		}
	}
}

function DisplayError (display, input, message) {
	DisplayMessage(display, input, "input-error", message);
}

function ShowMessage (input, message) {
	DisplayMessage(true, input, "input-info", message);
}

function HideMessage (input) {
	DisplayMessage(false, input, "input-info", "");
}

function VerifyRequiredField (input, message, originalValue) {
	if (originalValue == undefined)
		originalValue = "";
		
	var valid = jQuery.trim($(input).val()) != jQuery.trim(originalValue);
	
	DisplayError(!valid, input, message);
	
	return valid;
}

function VerifyEmail (input, message) {
	return VerifyRegularExpression("([\\w-]+(?:\\.[\\w-]+)*@(?:[\\w-]+\\.)+[a-zA-Z]{2,7})", input, message);
}

function VerifyInternetAddress (input, message) {
	return VerifyRegularExpression("((?:http|https)(?::\\/{2}[\\w]+)(?:[\\/|\\.]?)(?:[^\\s\"]*))", input, message);
}

function VerifyRegularExpression (exp, input, message) {
	var process = new RegExp(exp, "i");
	var match = process.exec($(input).val());
	var valid = match != undefined && match.length > 0;
	
	DisplayError(!valid, input, message);
	
	return valid;
}

function VerifyMatch (input1, input2, message) {
	var valid = $(input1).val() == $(input2).val();
	
	DisplayError(!valid, input2, message);
	
	return valid;
}

function VerifyLength (input, rangeStart, rangeEnd, message) {
	var valid = $(input).val().length >= rangeStart && $(input).val().length <= rangeEnd;
	
	DisplayError(!valid, input, message);
	
	return valid;
}