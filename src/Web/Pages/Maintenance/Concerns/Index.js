$(function () {
    var l = abp.localization.getResource("ComplaintTracking");

    var dataTable = $("#ConcernsTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            lengthMenu: [[10, 25, 1000], [10, 25, "All"]],
            order: [[0, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(complaintTracking.concerns.concern.getList),
            columnDefs: [
                {
                    title: l("Name"),
                    data: "name",
                },
                {
                    title: l("Active"),
                    data: "active",
                    render: function (data) { return data ? "Yes" : "No" },
                },
            ]
        })
    );
});