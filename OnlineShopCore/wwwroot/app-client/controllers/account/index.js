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

            swal({
                title: "Are you sure?",
                text: "Once canceled, you will not be able to recover this order!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        swal("Okay! Your order has been canceled!", {
                            icon: "success",
                        });

                        var that = $(this).data('id');
                        $.ajax({
                            url: '/Manage/CancelOrder',
                            type: 'PUT',
                            data: { billId: that },
                            success: function () {
                                var html = "<h5>Đơn hàng số:" + that + " | Đơn hàng bị hủy";
                                html += "</h5>"
                                document.getElementsByClassName('tr-display-bill-id-' + that)[0].childNodes[1].innerHTML = html;
                            },
                            error: function () {
                                console.log('cancel bill fail');
                            }
                        })

                    } else {
                        swal("Your order is safe! :D");
                    }
                });
        
        });
    }

    function loadData() {
        $.ajax({
            url: '/Manage/GetOrderHistory',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log(response);
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

                    console.log(isDuplicate)
                    if (!isDuplicate) {
                        if (item.Billstatus !== 4 && item.Billstatus !== 3 && item.Billstatus !== 1) {
                            console.log('render template 1')
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
                            console.log('render template 3');
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
                        console.log('render template 2')
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