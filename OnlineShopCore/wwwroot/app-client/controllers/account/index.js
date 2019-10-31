var AccountController = function() {
    var cachedObj = {
        billStatuses: []
    }
    this.initialize = function() {
        loadBillStatus();
        loadData();
        registerEvent();
    }



    function registerEvent() {
        $('body').on('click', '#btn-cancel-order', function(e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                url: '/Manage/CancelOrder',
                type: 'PUT',
                data: { billId: that },
                success: function() {
                    var html = "<h5>Đơn hàng số:" + that + " | Đơn hàng bị hủy";
                    html += "</h5>"
                    document.getElementsByClassName('tr-display-bill-id-' + that)[0].childNodes[1].innerHTML = html;
                },
                error: function() {
                    console.log('cancel bill fail');
                }
            })
        });
    }

    function loadData() {
        $.ajax({
            url: '/Manage/GetOrderHistory',
            type: 'GET',
            dataType: 'json',
            success: function(response) {
                var template = $('#template-order-history').html();
                var template2 = $('#template-order-history-2').html();
                var template3 = $('#template-order-history-with-canceled-status').html();

                var temp;
                var isDuplicate;
                var render = "";
                $.each(response, function(i, item) {

                    var temp2 = item.BillId;
                    if (temp != temp2) {
                        isDuplicate = false;
                        temp = temp2;
                    }
                    else {
                        isDuplicate = true;
                    }

                    if (!isDuplicate) {
  
                        if (item.Billstatus !== 3) {
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
                            render += Mustache.render(template3,
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
                    }
                    else {
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
            success: function(response) {
                cachedObj.billStatuses = response;
            }
        });
    }
}