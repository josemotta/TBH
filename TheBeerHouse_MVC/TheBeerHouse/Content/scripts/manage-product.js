/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
* Product
***********************************************************************************************************/

$("#title").focus(function() { ShowMessage(this, "Enter your product name here."); });
$("#sku").focus(function() { ShowMessage(this, "Enter your SKU number here."); });
$("#unitPrice").focus(function() { ShowMessage(this, "Enter the unit price of this product."); });
$("#discountPercentage").focus(function() { ShowMessage(this, "Enter the percent off you want to give for this product."); });
$("#unitsInStock").focus(function() { ShowMessage(this, "Total number of units of this product you have in stock."); });
$("#smallImageURL").focus(function() { ShowMessage(this, "The url to the thumbnail for this product."); });
$("#fullImageURL").focus(function() { ShowMessage(this, "The url to full sized image of this product."); });

function ValidateTitle() {
    return VerifyRequiredField("#title", "required");
}

function ValidateDescription() {
    return VerifyRequiredField("#description", "required");
}

function ValidateProduct() {
    return ValidateTitle()
		&& ValidateDescription();
}

$("form.product-create").validate(ValidateProduct);

/************************************************************************************************************
* Rich Text Editor
***********************************************************************************************************/

var bodyEditor;

$(document).ready(function() {
    bodyEditor = new tinymce.Editor("description", __editorConfig);
    bodyEditor.onChange.add(function(ed) { bodyEditor.save(); });
    bodyEditor.onClick.add(function(ed) { ShowMessage("#description", "Enter the description of your product here."); });
    bodyEditor.render();
});

// clears the message from the description when another input gets focus
$(":input")
	.focus(function() { HideMessage("#description"); })
	.blur(function() { HideMessage("#description"); });