var cartController = function () {
    this.initialize = function () {
        loadData();
    }
    function loadData() {
        const culture = $('#').val();
        $.ajax({
            type: "GET",
            url: "/ShoppingCart/GetListItems",
            success: function (res) {
                console.log(res)
            }
        });
    }
}