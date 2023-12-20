function formatCurrency(number) {
    return number.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
}

$('#keyword').keyup(function () {

    var searchField = $('#keyword').val();
    var expression = RegExp(searchField, "i");
   /* $('.psearch-results').empty(); // Sử dụng .empty() để xóa nội dung hiện tại*/

    $('.psearch-results').remove();
    $.ajax({
        type: "GET",
        url: "/SearchHeader/Search",
        success: function (response) {
            var data = JSON.parse(response);
            console.log(data);

            var matchingProducts = [];

            if (searchField != "") {
                $.each(data, function (key, item) {
                    if (item.Title.search(expression) != -1) {
                        matchingProducts.push(item);
                    }
                });
            }
            if (searchField === "") {
                // Ẩn kết quả tìm kiếm hoặc làm gì đó tương tự để xử lý trường hợp này
                $('.psearch-results').remove(); // Ẩn kết quả tìm kiếm bằng cách xóa nội dung
                return; // Kết thúc hàm tại đây
            }

            var productResult = $('#productResult');
            productResult.text(matchingProducts.length + " Kết quả tìm thấy");


            if (searchField != "") {
                var html_Body = `<div class="psearch-results">
                
            </div>`;
            } $('.card-body').append(html_Body);
            $.each(data, function (key, item) {
              
                if (item.Title.search(expression) != -1 && searchField != "") {
                    var html_Search = ` <div class="axil-product-list">
                    <div class="thumbnail">
                        <a href="/chi-tiet/${item.Alias}-p${item.Id}">
                            <img src="${item.ProductImage}" alt="Yantiti Leather Bags" width="120" height="120">
                        </a>
                    </div>
                    <div class="product-content">
                        <div class="product-rating">
                            <span class="rating-icon">
                                <i class="fas fa-star"></i>
                                <i class="fas fa-star"></i>
                                <i class="fas fa-star"></i>
                                <i class="fas fa-star"></i>
                                <i class="fal fa-star"></i>
                            </span>
                            <span class="rating-number"><span>${item.ViewCount}+</span> Số lượng xem</span>
                        </div>
                        <h6 class="product-title"><a href="/chi-tiet/${item.Alias}-p${item.Id}">${item.Title} </a></h6>
                        <div class="product-price-variant">
                            <span class="price current-price">${formatCurrency(item.Price)}</span>
                            <span class="price old-price">${formatCurrency(item.PriceSale)}</span>
                        </div>
                        <div class="product-cart">
                            <a href="cart.html" class="cart-btn"><i class="fal fa-shopping-cart"></i></a>
                            <a href="wishlist.html" class="cart-btn"><i class="fal fa-heart"></i></a>
                        </div>
                    </div>
                </div>`;
                    $('.psearch-results').append(html_Search);
                }
            })
        }
    })
})





