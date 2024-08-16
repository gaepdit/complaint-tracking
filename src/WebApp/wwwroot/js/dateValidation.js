document.getElementById("SearchButton").addEventListener("click", function (e) {
  const datePairs = [
    //Index.cshtml
    {
      fromID: "Spec_DateFrom",
      toID: "Spec_DateTo",
      from: "Received FROM Date ",
      to: "Received THROUGH Date ",
    },
    //Staff/Complaints/Index.cshtml
    {
      fromID: "Spec_ReceivedFrom",
      toID: "Spec_ReceiveTo",
      from: "Received FROM Date ",
      to: "Received THROUGH Date ",
    },
    {
      fromID: "Spec_ClosedFrom",
      toID: "Spec_ClosedTo",
      from: "Closed FROM Date ",
      to: "Closed THROUGH Date ",
    },
    //Staff/ComplaintActions/Index.cshtml
    {
      fromID: "Spec_EnteredFrom",
      toID: "Spec_EnteredTo",
      from: "Entered FROM Date ",
      to: "Entered THROUGH Date ",
    },
  ];

  var today = new Date().toISOString().slice(0, 10);

  for (const pair of datePairs) {
    var dateFrom = document.getElementById(pair.fromID).value;
    var dateTo = document.getElementById(pair.toID).value;

    if (dateTo && dateTo > today) {
      alert(
        pair.to + "(" + dateTo + ") cannot exceed current date (" + today + ")"
      );
      e.preventDefault();
      break;
    } else if (dateFrom && dateFrom > today) {
      alert(
        pair.from +
          "(" +
          dateFrom +
          ") cannot exceed current date (" +
          today +
          ")"
      );
      e.preventDefault();
      break;
    } else if (dateTo && dateFrom && dateFrom > dateTo) {
      alert(
        pair.from +
          "(" +
          dateFrom +
          ") cannot exceed " +
          pair.to +
          "(" +
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
