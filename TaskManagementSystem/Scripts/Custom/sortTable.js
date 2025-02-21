function getCellValue(row, index) {
    return row.children[index].innerText || row.children[index].textContent;
}

function comparer(index, asc) {
    return function (a, b) {
        const v1 = getCellValue(asc ? a : b, index);
        const v2 = getCellValue(asc ? b : a, index);
        return v1 !== '' && v2 !== '' && !isNaN(v1) && !isNaN(v2) ? v1 - v2 : v1.toString().localeCompare(v2);
    };
}


function enableSorting(tableId, headerId, columnIndex) {
    const header = document.getElementById(headerId);
    if (!header) return; // Prevents running if header doesn't exist

    header.addEventListener("click", function () {
        const table = document.getElementById(tableId);
        if (!table) return;
        const tbody = table.querySelector("tbody");
        const rows = Array.from(tbody.querySelectorAll("tr"));
        const asc = this.getAttribute("data-sort") !== "asc";
        this.setAttribute("data-sort", asc ? "asc" : "desc");

        rows.sort(comparer(columnIndex, asc)).forEach(row => tbody.appendChild(row));
    });
}