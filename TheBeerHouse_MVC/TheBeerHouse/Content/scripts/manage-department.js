/// <reference path="global.js" />
/// <reference path="jquery-1.3.2-vsdoc.js" />

/************************************************************************************************************
* Departments
***********************************************************************************************************/

$("#title").focus(function() { ShowMessage(this, "Enter the title for your department."); });
$("#importance").focus(function() { ShowMessage(this, "(optional) Enter the order of importance that you want the departments shown in."); });
$("#imageUrl").focus(function() { ShowMessage(this, "The relative web path of an image you want to be shown with products in this department."); });
$("#description").focus(function() { ShowMessage(this, "Enter a short description of the department to display to your users."); });

function ValidateTitle() {
    return VerifyRequiredField("#title", "required");
}

function ValidateImageUrl() {
    return VerifyRequiredField("#imageUrl", "required");
}

function ValidateDescription() {
    return VerifyRequiredField("#description", "required");
}

function ValidateDepartment() {
    var validTitle = ValidateTitle();
    var validImage = ValidateImageUrl();
    var validDescription = ValidateDescription();

    return validTitle && validImage && validDescription;
}

$("form.department-create").validate(ValidateDepartment);