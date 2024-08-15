document.getElementById("SearchButton").addEventListener("click", function (e) {
  const datePairs = [{ from: "Spec_DateFrom", to: "Spec_DateTo" }];
  var today = new Date().toISOString().slice(0, 10);

  for (const pair of datePairs) {
    var dateFrom = document.getElementById(pair.from).value;
    var dateTo = document.getElementById(pair.to).value;

    if (dateTo > today) {
      alert(
        "Through date (" +
          dateTo +
          ") cannot exceed current date (" +
          today +
          ")"
      );
      e.preventDefault();
      break;
    } else if (dateFrom > today) {
      alert(
        "From date (" +
          dateFrom +
          ") cannot exceed current date (" +
          today +
          ")"
      );
      e.preventDefault();
      break;
    } else if (dateFrom > dateTo) {
      alert(
        "From date (" +
          dateFrom +
          ") cannot exceed through date (" +
          dateTo +
          ")"
      );
      e.preventDefault();
      break;
    }
  }
  if (!e.defaultPrevented) {
    //formSearch.js
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
  }
});
