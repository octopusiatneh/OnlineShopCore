$('#btnClearAll').on('click', function (e) {
    e.preventDefault();
    $.ajax({
        url: '/Cart/ClearCart',
        type: 'post',
        success: function () {       
            loadHeaderCart();
            loadHeader();
            loadData();
        }
    });
});

function loadHeaderCart() {
    $("#headerCart").load("/AjaxContent/HeaderCart");
}

function loadHeader() {
    $("#cartButton").load("/AjaxContent/Header");
}

function loadData() {
    $.ajax({
        url: '/Cart/GetCart',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var template = $('#template-cart').html();
            var render = "";
            var totalAmount = 0;
            $.each(response, function (i, item) {
                render += Mustache.render(template,
                    {
                        ProductId: item.Product.Id,
                        ProductName: item.Product.Name,
                        Image: item.Product.Image,
                        Price: onlineshop.formatNumber(item.Price, 0),
                        Quantity: item.Quantity,
                        Amount: onlineshop.formatNumber(item.Price * item.Quantity, 0),
                        Url: '/' + item.Product.SeoAlias + "-st-" + item.Product.Id
                    });
                totalAmount += item.Price * item.Quantity;
            });
            $('#lblTotalAmount').text(onlineshop.formatNumber(totalAmount, 0));
            if (render !== "")
                $('#table-cart-content').html(render);
            else
                $('#cuocDoiNayKhoQuaMa').html('Your cart is empty');
        }
    });
    return false;
}