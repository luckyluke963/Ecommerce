$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('#quantity_value').val();  // Use .val() instead of .text()

        // Ensure that tQuantity is a positive integer
        if (tQuantity !== '' && !isNaN(tQuantity) && parseInt(tQuantity) > 0) {
            quatity = parseInt(tQuantity);
        }

        //alert(id + " " + quatity);
        $.ajax({
            url: '/ShoppingCart/AddToCart',
            type: 'POST',
            data: { id: id, quantity: quatity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);
                    Swal.fire({
                        icon: "success",
                        title: rs.msg,
                        showConfirmButton: false,
                        timer: 1500
                        ,
                        customClass: {
                            popup: 'custom-size-modal', // Define the custom class in your CSS
                        },
                    });
                }
            }
        });
    });



    $('#btnUpdateAll').off('click').on('click', function () {
        var listProduct = $('.txtQuantity');
        var cartList = [];

        $.each(listProduct, function (i, item) {
            var productId = $(item).data('product-id'); // Lấy productId từ data attribute
            cartList.push({
                Quantity: $(item).val(),
                ProductId: productId, // Sử dụng productId thay vì giá trị input
            });
        });

        $.ajax({
            url: '/ShoppingCart/UpdateCartAll',
            data: { cartModel: JSON.stringify(cartList) },
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/gio-hang";
                }
            }
        });
    });


    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quatity = $('#Quantity_' + id).val();
        Update(id, quatity);


    });



    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();

        Swal.fire({
            title: "Bạn có chắc muốn xóa hết sản phẩm trong giỏ hàng?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Có, Xóa hết!",

        }).then((result) => {
            if (result.isConfirmed) {
                DeleteAll();
            }
        });
    });




    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?');
        if (conf == true) {
            $.ajax({
                url: '/ShoppingCart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                        LoadCart();
                        //location.reload();
                    }
                }
            });
        }


    });
});

function ShowCount() {
    $.ajax({
        url: '/ShoppingCart/ShowCount',
        type: 'GET',

        success: function (rs) {
            $('#checkout_items').html(rs.Count);
        }
    });
}

function DeleteAll() {
    $.ajax({
        url: '/shoppingcart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.Success) {
                LoadCart()
            }
        }
    })
}



function Update(id, quatity) {
    $.ajax({
        url: '/ShoppingCart/Update',
        type: 'POST',
        data: { id: id, quatity: quatity },
        success: function (rs) {
            if (rs.Success) {
                LoadCart()
            }
        }
    })
}

function LoadCart() {
    $.ajax({
        url: '/ShoppingCart/Index',
        type: 'GET',

        success: function (rs) {
            $('#load_data').html(rs); //load_data ở trang _layout.cshtml
        }
    });
}