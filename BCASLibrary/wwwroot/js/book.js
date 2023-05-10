﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": { url: '/book/tolist' },
        "columns": [
            { data: 'title', 'width': '15%' },
            { data: 'author', 'width': '15%' },
/*            { data: 'description', 'width': '25%' },*/
            { data: 'isbn', 'width': '10%' },
            { data: 'genre.genreName', 'width': '15%' },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-25 btn-group" role="group">
                            <a href="/book/upsert?id=${data}" class="btn btn-outline-warning mx-2"><i class="bi bi-pencil-square"></i></a>
                            <a onClick=Delete('/book/delete/${data}') class="btn btn-outline-danger mx-2"><i class="bi bi-trash-fill"></i></a>

                            <div/>`
                },
                'width': '20%'
            },

            //{ data: 'imageUrl', 'width': '25%' }

        ]
    });
}

function Delete(url)
{
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
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
            
        }
    })
}
