var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/booksview/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "author", "width": "25%" },
            { "data": "isbn", "width": "25%" },
            { "data": "available", "width": "25%"},
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}