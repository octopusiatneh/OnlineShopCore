var CartController = function () {
    this.initialize = function () {
        loadData();

        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    swal({
                        text: resources["RemoveCartOK"],
                        icon: "success",
                    });
                    loadHeaderCart();
                    loadHeader();
                    loadHeaderMobile();
                    loadData();
                }
            });
        });
        $('body').on('keyup', '.txtQuantity', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var q = $(this).val();
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q
                    },
                    success: function () {
                        onlineshop.notify('Update quantity is successful', 'success');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                onlineshop.notify('Your quantity is invalid', 'error');
            }

        });

        $('#btnClearAll').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'post',
                success: function () {
                    swal({
                        text: resources["ClearCart"],
                        icon: "success",
                    });
                    loadHeaderCart();
                    loadHeader();
                    loadHeaderMobile();
                    loadData();
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
                            Url: '/' + item.Product.SeoAlias + "-p" + item.Product.Id
                        });
                    totalAmount += item.Price * item.Quantity;
                });
                $('#lblTotalAmount').text(onlineshop.formatNumber(totalAmount, 0));
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#cuocDoiNayKhoQuaMa').html(resources["YourCartIsEmpty"]);
            }
        });
        return false;
    }
} 