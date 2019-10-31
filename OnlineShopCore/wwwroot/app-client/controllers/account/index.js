var AccountController = function () {
    var cachedObj = {
        billStatuses: []
    }
    this.initialize = function () {
        loadBillStatus();
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
                    else {
                        isDuplicate = true;
                    }
                    console.log(item);

                    if (!isDuplicate) {
                        render += Mustache.render(template,
                            {
                                ProductName: item.ProductName,
                                BillStatus: getBillStatusName(item.Billstatus),
                                Image: item.Image,
                                BillId: item.BillId,
                                Price: onlineshop.formatNumber(item.Price, 0),
                                Quantity: item.Quantity
                                //Url: '/' + item.Product.SeoAlias + "-p" + item.Product.Id
                            });
                    }
                    else {
                        console.log('DUPLICATED');
                        render += Mustache.render(template2,
                            {
                                ProductName: item.ProductName,
                                BillStatus: getBillStatusName(item.Billstatus),
                                Image: item.Image,
                                Price: onlineshop.formatNumber(item.Price, 0),
                                Quantity: item.Quantity
                                //Url: '/' + item.Product.SeoAlias + "-p" + item.Product.Id
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

    function getBillStatusName(i) {
        if (i >= 0)
            return cachedObj.billStatuses[i].Name;
        else
            return '';
    }

    function loadBillStatus() {
        return $.ajax({
            type: "GET",
            url: "/Manage/GetBillStatus",
            dataType: "json",
            success: function (response) {
                cachedObj.billStatuses = response;
            }
        });
    }
}