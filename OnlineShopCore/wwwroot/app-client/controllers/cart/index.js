var CartController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    registerEvents = () => {
        $('body').on('click', '.btn-delete', e => {
            e.preventDefault();
            const id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success() {
                    swal({
                        text: resources["RemoveCartOK"],
                        icon: "success",
                    });
                    reload()
                }
            });
        });

        $('body').on('click', '.btn-num-product-down', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            var numProduct = Number($(this).next().val());
            if (numProduct > 1) $(this).next().val(numProduct - 1);
            var quantity = Number($(this).next().val());
            $.ajax({
                url: '/Cart/UpdateCart',
                data: {
                    productId: that,
                    quantity: quantity
                }
            })
            var price = Number($(`#hidden-price-${that}`).data('price'));
            $(`#amount-${that}`).text(onlineshop.formatNumber((price * quantity), 0) + "₫")
        });

        $('body').on('click', '.btn-num-product-up', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            var numProduct = Number($(this).prev().val());
            $(this).prev().val(numProduct + 1);
            var quantity = Number($(this).prev().val());
            var price = Number($(`#hidden-price-${that}`).data('price'));
            $.ajax({
                url: '/Cart/UpdateCart',
                data: {
                    productId: that,
                    quantity: quantity
                }
            })

            $(`#amount-${that}`).text(onlineshop.formatNumber((price * quantity), 0) + "₫")
        });

        $('body').on('change', '.num-product', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            var quantity = $(this).val();
            $.ajax({
                url: '/Cart/UpdateCart',
                data: {
                    productId: that,
                    quantity: quantity
                },
            })
            var price = Number($(`#hidden-price-${that}`).data('price'));
            $(`#amount-${that}`).text(onlineshop.formatNumber((price * quantity), 0) + "₫")
        });

        $('#btnClearAll').on('click', e => {
            e.preventDefault();
            $.ajax({
                url: '/Cart/ClearCart',
                type: 'post',
                success() {
                    swal({
                        text: resources["ClearCart"],
                        icon: "success",
                    });
                    reload()
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

    const reload = () => {
        loadHeaderCart();
        loadHeader();
        loadHeaderMobile();
        loadData();
    }

    const loadData = () => {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: response => {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, (i, item) => {
                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: onlineshop.formatNumber(item.Price, 0),
                            Quantity: item.Quantity,
                            Amount: onlineshop.formatNumber(item.Price * item.Quantity, 0),
                            Url: '/' + item.Product.SeoAlias + "-p" + item.Product.Id,
                            PriceR: item.Price
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