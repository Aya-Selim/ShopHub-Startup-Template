var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#mytable").DataTable({
        ajax: {
            url: "/Product/GetData",
            type: "GET",
            dataSrc: "data"
        },
        columns: [
            { data: "name" },
            { data: "description" },
            { data: "price" },
            { data: "categoryName" },
            {
                data: "id",
                render: function (id) {
                    return `
                        <a href="/Product/Edit/${id}" class="btn btn-success btn-sm">
                            <i class="fa-solid fa-pen"></i> Edit
                        </a>

                        <a onclick="DeleteItem('/Product/Delete/${id}')" class="btn btn-danger btn-sm text-white" style="cursor:pointer">
                            <i class="fa-solid fa-trash"></i> Delete
                        </a>
                    `;
                }
            }
        ],
        autoWidth: false,
        scrollX: true
    });
}

function DeleteItem(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}