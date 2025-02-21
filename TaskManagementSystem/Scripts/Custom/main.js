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


    // --------------------------- Sort Table ---------------------------
    const currentTitlePage = $("title").text();
    if (currentTitlePage === "Your Task List - Task Management System") {
        enableSorting("sortTable", "sortTableHeader1", 0);
        enableSorting("sortTable", "sortTableHeader2", 1);
        enableSorting("sortTable", "sortTableHeader3", 2);
        enableSorting("sortTable", "sortTableHeader4", 3);
        enableSorting("sortTable", "sortTableHeader5", 4);
        enableSorting("sortTable", "sortTableHeader6", 5);
        enableSorting("sortTable", "sortTableHeader7", 6);
    }
    else if (currentTitlePage === "All Task List - Task Management System") {
        enableSorting("sortTable", "sortTableHeader1", 0);
        enableSorting("sortTable", "sortTableHeader2", 1);
        enableSorting("sortTable", "sortTableHeader3", 2);
        enableSorting("sortTable", "sortTableHeader4", 3);
        enableSorting("sortTable", "sortTableHeader5", 4);
        enableSorting("sortTable", "sortTableHeader6", 5);
        enableSorting("sortTable", "sortTableHeader7", 6);
        enableSorting("sortTable", "sortTableHeader8", 7);
    }
    // --------------------------- Sort Table ---------------------------
});
