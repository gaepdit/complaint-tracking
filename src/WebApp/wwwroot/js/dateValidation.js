document.getElementById("SearchButton").addEventListener("click", function (e) {
  const datePairs = [
    //Index.cshtml
    {
      fromID: "Spec_DateFrom",
      toID: "Spec_DateTo",
      from: "RECEIVED From Date",
      to: "RECEIVED To Date",
    },
    //Staff/Complaints/Index.cshtml
    {
      fromID: "Spec_ReceivedFrom",
      toID: "Spec_ReceiveTo",
      from: "RECIEVED From Date",
      to: "RECIEVED To Date",
    },
    {
      fromID: "Spec_ClosedFrom",
      toID: "Spec_ClosedTo",
      from: "CLOSED From Date",
      to: "CLOSED To Date",
    },
    //Staff/ComplaintActions/Index.cshtml
    {
      fromID: "Spec_EnteredFrom",
      toID: "Spec_EnteredTo",
      from: "ENTERED From Date",
      to: "ENTERED To Date",
    },
  ];

  var today = new Date().toISOString().slice(0, 10);

  for (const pair of datePairs) {
    var dateFrom = document.getElementById(pair.fromID).value;
    var dateTo = document.getElementById(pair.toID).value;

    if (dateFrom != null || dateTo != null) {
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
