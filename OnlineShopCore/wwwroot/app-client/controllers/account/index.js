var AccountController = function () {
    this.initialize = function () {
        loadData();
    }


    function loadData() {
        $.ajax({
            url: '/Manage/GetOrderHistory',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-order-history').html();
                var template2 = $('#template-order-history-2').html();
                var temp;
                var isDuplicate;
                var render = "";
                $.each(response, function (i, item) {

                    var temp2 = item.BillId;
                    if (temp != temp2) {
                        isDuplicate = false;
                        temp = temp2;
                    }
                    else
                        isDuplicate = true;
                    if (!isDuplicate) {
                        render += Mustache.render(template,
                            {
                                ProductName: item.ProductName,
                                Image: item.Image,
                                BillId: item.BillId,
                                Price: onlineshop.formatNumber(item.Price, 0),
                                Quantity: item.Quantity,
                                //Url: '/' + item.Product.SeoAlias + "-st-" + item.Product.Id
                            });
                    }
                    else {
                        console.log('DUPLICATED');
                        render += Mustache.render(template2,
                            {
                                ProductName: item.ProductName,
                                Image: item.Image,
                                Price: onlineshop.formatNumber(item.Price, 0),
                                Quantity: item.Quantity,
                                //Url: '/' + item.Product.SeoAlias + "-st-" + item.Product.Id
                            });
                    }
                    
                });
                
                if (render !== "") {
                    $('#table-order-history-content').html(render);
                }
                else
                    $('#order-history').html(resources["YourOrderHistoryEmpty"]);
            }
        });
        return false;
    }
}