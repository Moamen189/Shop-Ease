var dtble;
$(document).ready(function () {
    loadData();
});

function loadData() {
    dtble = $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData"
        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Product/Edit/${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                                    Edit
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Product/Delete/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:70px;'>
                                    Delete
                                </a>
                            </div>`;
                }
            }


        ]
    });

}