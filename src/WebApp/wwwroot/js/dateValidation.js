document.getElementById("SearchButton").addEventListener("click", function (e) {
  var dateFrom = document.getElementById("Spec_DateFrom").value;
  var dateTo = document.getElementById("Spec_DateTo").value;
  var today = new Date().toISOString().slice(0, 10);
  //alert(dateFrom + " " + dateTo + " " + today);

  if (dateTo > today) {
    alert(
      "Through date (" + dateTo + ") cannot exceed current date (" + today + ")"
    );
    e.preventDefault();
  } else if (dateFrom > today) {
    alert(
      "From date (" + dateFrom + ") cannot exceed current date (" + today + ")"
    );
    e.preventDefault();
  } else if (dateFrom > dateTo) {
    alert(
      "From date (" + dateFrom + ") cannot exceed through date (" + dateTo + ")"
    );
    e.preventDefault();
  } else {
  }
});
