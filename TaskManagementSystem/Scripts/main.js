$(document).ready(function () {
    // --------------------------- Toastr ---------------------------
    var successMessage = $('#successMessage').text();
    successMessage = successMessage.trim();
    if (successMessage !== '') {
        toastr.success(successMessage);
    }

    var errorMessage = $('#errorMessage').text();
    errorMessage = errorMessage.trim();
    if (errorMessage !== '') {
        toastr.error(errorMessage);
    }

    var infoMessage = $('#infoMessage').text();
    infoMessage = infoMessage.trim();
    if (infoMessage !== '') {
        toastr.info(infoMessage);
    }

    var warningMessage = $('#warningMessage').text();
    warningMessage = warningMessage.trim();
    if (warningMessage !== '') {
        toastr.warning(warningMessage);
    }
    // --------------------------- Toastr ---------------------------
});