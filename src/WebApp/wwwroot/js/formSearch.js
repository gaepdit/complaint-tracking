// (Add this script to search forms that use GET for submit. It keeps clutter out of the resulting query string.)
$(document).ready(function () {
    function disableEmptyInput(n, el) {
        const $input = $(el);
        if ($input.val() === "") $input.attr("disabled", "disabled");
    }

    $("#SearchButton").click(function DisableEmptyInputs() {
        $("input").each(disableEmptyInput);
        $("select").each(disableEmptyInput);
        return true;
    });
});
