$(document).ready(function () {
    $('body').on('click', '.btnAddToIndex', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }

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
        })
    })
})