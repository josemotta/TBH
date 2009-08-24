function ValidateUserName() {
    return VerifyRequiredField("#UserName", "required");
}

function ValidatePassword() {
    return VerifyRequiredField("#Password", "required");
}

function ValidateConfirmPassword() {
    return VerifyRequiredField("#ConfirmPassword", "required");
}

function ValidateEmail() {
    return VerifyRequiredField("#Email", "required");
}

function ValidateSecretQuestion() {
    return VerifyRequiredField("#SecretQuestion", "required");
}

function ValidateSecretAnswer() {
    return VerifyRequiredField("#SecretAnswer", "required");
}

function ValidateRegistration() {
    return ValidateUserName()
		&& ValidatePassword()
		&& ValidateConfirmPassword()
		&& ValidateEmail()
		&& ValidateSecretQuestion()
		&& ValidateSecretAnswer();
}

$("form.user-registration").validate(ValidateRegistration);