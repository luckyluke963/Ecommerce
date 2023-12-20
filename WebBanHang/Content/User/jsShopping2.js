$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCartasd', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quantity = parseInt(tQuantity);
        }

        $.ajax({
            url: '/ShoppingCart/AddToCart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);

                    // Sử dụng SweetAlert2 thay vì alert
                    Swal.fire({
                        icon: 'success',
                        title: rs.msg,
                        showConfirmButton: false,
                        timer: 1500
                    });

                }
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