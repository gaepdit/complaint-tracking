function datePairs(fromID, toID, fromString, toString) {
    document
        .getElementById("SearchButton")
        .addEventListener("click", function (e) {
            const today = new Date().toISOString().slice(0, 10);
            const dateFrom = document.getElementById(fromID).value;
            const dateTo = document.getElementById(toID).value;

            if (dateTo && dateTo > today) {
                alert(`${toString} (${dateTo}) cannot exceed current date (${today}).`);
                e.stopImmediatePropagation();
                e.preventDefault();
            } else if (dateFrom && dateFrom > today) {
                alert(`${fromString} (${dateFrom}) cannot exceed current date (${today}).`);
                e.stopImmediatePropagation();
                e.preventDefault();
            } else if (dateTo && dateFrom && dateFrom > dateTo) {
                alert(`${fromString} (${dateFrom}) cannot exceed ${toString} (${dateTo}).`);
                e.stopImmediatePropagation();
                e.preventDefault();
            }
        });
}
