var ProductDetailController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#btnAddToCart').on('click', function (e) {

            e.preventDefault();
            if (parseInt($('#txtQuantity').val()) >= 1) {
                var id = parseInt($(this).data('id'));
                var nameProduct = $(this).parent().parent().parent().parent().find('.js-name-detail').html();
                $.ajax({
                    url: '/Cart/AddToCart',
                    type: 'post',
                    dataType: 'json',
                    data: {
                        productId: id,
                        quantity: parseInt($('#txtQuantity').val()),
                    },
                    success: function () {
                        swal(nameProduct, resources["AddCartOK"], "success");
                        loadHeaderCart();
                        loadHeader();
                        loadHeaderMobile();
                    }
                });
            }
            else {
                swal(nameProduct, resources["AddCartFail"], "error");
            }

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
