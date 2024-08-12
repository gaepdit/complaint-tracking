document.getElementById("SearchButton").addEventListener("click", function (e) {
  var dateFrom = document.getElementById("Spec_DateFrom").value;
  var dateTo = document.getElementById("Spec_DateTo").value;
  var today = new Date().toISOString().slice(0, 10);
  //alert(dateFrom + " " + dateTo + " " + today);

  if (dateTo > today) {
    alert("not valid date");
    e.preventDefault();
  }
});
