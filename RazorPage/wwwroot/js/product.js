var dataTable;




$(document).ready(function () {
    $("#dialog").dialog({
        autoOpen: false,
        show: "fade",
        hide: "fade",
        modal: "true",
        height: '500',
        width: '1000',
        resizable: true,
        title: 'Quản lí ảnh sản phẩm',
        close: function () {
            window.location.reload();
        }
    });

    $('body').on("click", "#imgproduct", function () {
        var proid = $(this).attr("data-id");
        $("#dialog #myIframe").attr("src", "/Admin/ProductImages/Index?id=" + proid);
        $('#dialog').dialog('open');
        return false;
    });
});


//function Delete(url) {
//    Swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: 'DELETE',
//                success: function (data) {
//                    if (data.success) {
//                        dataTable.ajax.reload();
//                        toastr.success("Xóa danh mục thành công", "Thông báo");
//                    } else {
//                        toastr.error("Xóa danh mục thất bại", "Thông báo");
//                    }
//                }
//            })
//        }
//    })
//}