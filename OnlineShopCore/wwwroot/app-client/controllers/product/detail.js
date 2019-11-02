var ProductDetailController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#btnAddToCart').on('click', function (e) {
           
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            $.ajax({
                url: '/Cart/AddToCart',
                type: 'post',
                dataType: 'json',
                data: {
                    productId: id,
                    quantity: parseInt($('#txtQuantity').val()),
                },
                success: function () {
                    loadHeaderCart();
                    loadHeader();
                    loadHeaderMobile();
                }
            });
        });
    }
    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }

    function loadHeader() {
        $("#cartButton").load("/AjaxContent/Header");
    }

    function loadHeaderMobile() {
        $("#cartButtonMobile").load("/AjaxContent/HeaderMobile");
    }
}