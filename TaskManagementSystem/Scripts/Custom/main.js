$(document).ready(function () {
    // --------------------------- Toastr ---------------------------
    const successMessage = $('#successMessage').text().trim();
    if (successMessage !== '') toastr.success(successMessage);

    const errorMessage = $('#errorMessage').text().trim();
    if (errorMessage !== '') toastr.error(errorMessage);

    const infoMessage = $('#infoMessage').text().trim();
    if (infoMessage !== '') toastr.info(infoMessage);

    const warningMessage = $('#warningMessage').text().trim();
    if (warningMessage !== '') toastr.warning(warningMessage);
    // --------------------------- Toastr ---------------------------


    // --------------------------- Sort Table ---------------------------
    const currentTitlePage = $("title").text();
    const projectTitle = "Task Management System";
    const dash = "-";
    const space = " ";
    if (currentTitlePage === "Your Task List" + space + dash + space + projectTitle) {
        enableSorting("sortTable", "sortTableHeader1", 0);
        enableSorting("sortTable", "sortTableHeader2", 1);
        enableSorting("sortTable", "sortTableHeader3", 2);
        enableSorting("sortTable", "sortTableHeader4", 3);
        enableSorting("sortTable", "sortTableHeader5", 4);
        enableSorting("sortTable", "sortTableHeader6", 5);
        enableSorting("sortTable", "sortTableHeader7", 6);
    }
    else if (currentTitlePage === "All Task List" + space + dash + space + projectTitle) {
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
