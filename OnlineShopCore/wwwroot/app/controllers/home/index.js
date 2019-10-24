var HomeController = function () {
    this.initialize = function () {
        loadRevenueData();
        loadUserData();
        loadOrderData();
        loadTopVisitProductData();
    }

    function loadRevenueData(from, to) {
        $.ajax({
            type: "GET",
            url: "/Admin/Home/GetRevenue",
            data: {
                fromDate: from,
                toDate: to
            },
            dataType: "json",
            beforeSend: function () {
                onlineshop.startLoading();
            },
            success: function (response) {
                initRevenueChart(response);

                onlineshop.stopLoading();

            },
            error: function (status) {
                onlineshop.notify('Có lỗi xảy ra khi tải dữ liệu doanh thu', 'error');
                onlineshop.stopLoading();
            }
        });
    }

    function loadUserData(from, to) {
        $.ajax({
            type: "GET",
            url: "/Admin/Home/GetNewUser",
            data: {
                fromDate: from,
                toDate: to
            },
            dataType: "json",
            beforeSend: function () {
                onlineshop.startLoading();
            },
            success: function (response) {
                initNewUserChart(response);

                onlineshop.stopLoading();

            },
            error: function (status) {
                onlineshop.notify('Có lỗi xảy ra khi tải dữ liệu người dùng', 'error');
                onlineshop.stopLoading();
            }
        });
    }

    function loadOrderData(from, to) {
        $.ajax({
            type: "GET",
            url: "/Admin/Home/GetNewOrder",
            data: {
                fromDate: from,
                toDate: to
            },
            dataType: "json",
            beforeSend: function () {
                onlineshop.startLoading();
            },
            success: function (response) {
                initNewOrderChart(response);

                onlineshop.stopLoading();

            },
            error: function (status) {
                onlineshop.notify('Có lỗi xảy ra khi tải dữ liệu đơn hàng', 'error');
                onlineshop.stopLoading();
            }
        });
    }

    function loadTopVisitProductData() {
        $.ajax({
            type: "GET",
            url: "/Admin/Home/GetTopVisitProduct",
            dataType: "json",
            beforeSend: function () {
                onlineshop.startLoading();
            },
            success: function (response) {
                initTopVisitProductChart(response);
                onlineshop.stopLoading();

            },
            error: function (status) {
                onlineshop.notify('Có lỗi xảy ra khi tải dữ liệu', 'error');
                onlineshop.stopLoading();
            }
        });
    }

    function initRevenueChart(data) {
        var arrRevenue = [];

        $.each(data, function (i, item) {
            arrRevenue.push([new Date(item.Date).getTime(), item.Revenue]);
        });
        var chart_plot_02_settings = {
            grid: {
                show: true,
                aboveData: true,
                color: "#3f3f3f",
                //labelMargin: 10,
                axisMargin: 0,
                borderWidth: 0,
                borderColor: null,
                //minBorderMargin: 5,
                clickable: true,
                hoverable: true,
                autoHighlight: true
            },
            series: {
                lines: {
                    show: true,
                    fill: true,
                    lineWidth: 2,
                    steps: false
                },
                points: {
                    show: true,
                    radius: 2.5,
                    symbol: "circle",
                    lineWidth: 3.0
                }
            },
            shadowSize: 0,
            tooltip: true,
            yaxis: {
                min: 0
            },
            xaxis: {
                mode: 'time',
                tickDecimals: 0,
                minTickSize: [1, 'day'],
                timeformat: '%d/%m/%y',
            }
        };

        $("<div id='tooltip'></div>").css({
            position: "absolute",
            display: "none",
            border: "1px solid #fdd",
            padding: "2px",
            "background-color": "#fee",
            opacity: 0.80
        }).appendTo("body");

        $("#chart_plot_02").bind("plothover", function (event, pos, item) {

            if (!pos.x || !pos.y) {
                return;
            }

            if (item) {
                var x = item.datapoint[0],
                    y = item.datapoint[1];
                var date = moment(x).format('DD/MM');
                $("#tooltip").html(item.series.label + " ngày " + date + ": " + y)
                    .css({ top: item.pageY + 5, left: item.pageX + 5 })
                    .fadeIn(200);
            } else {
                $("#tooltip").hide();
            }
        });

        if ($("#chart_plot_02").length > 0) {

            if (arrRevenue.length > 0) {
                $.plot($("#chart_plot_02"),
                    [{
                        label: "Doanh thu",
                        data: arrRevenue,
                        lines: {
                            fillColor: "rgba(150, 202, 89, 0.12)"
                        },
                        points: {
                            fillColor: "#fff"
                        }
                    }], chart_plot_02_settings);
            }

            else {
                document.getElementById("chart_plot_02").innerHTML = "Không có dữ liệu hóa đơn!!";
            }
        }
    }

    function initNewUserChart(data) {
        var arrNewUser = [];

        $.each(data, function (i, item) {
            arrNewUser.push([new Date(item.Date).getTime(), item.TotalNewUser]);
        });
        var chart_plot_02_settings = {
            grid: {
                show: true,
                aboveData: true,
                color: "#3f3f3f",
                //labelMargin: 10,
                axisMargin: 0,
                borderWidth: 0,
                borderColor: null,
                //minBorderMargin: 5,
                clickable: true,
                hoverable: true,
                autoHighlight: true
            },
            series: {
                lines: {
                    show: true,
                    fill: true,
                    lineWidth: 2,
                    steps: false
                },
                points: {
                    show: true,
                    radius: 2.5,
                    symbol: "circle",
                    lineWidth: 3.0
                }
            },
            shadowSize: 0,
            tooltip: true,
            yaxis: {
                min: 0
            },
            xaxis: {
                mode: 'time',
                tickDecimals: 0,
                minTickSize: [1, 'day'],
                timeformat: '%d/%m/%y',
            }
        };

        $("<div id='tooltipUser'></div>").css({
            position: "absolute",
            display: "none",
            border: "1px solid #fdd",
            padding: "2px",
            "background-color": "#fee",
            opacity: 0.80
        }).appendTo("body");

        $("#chart_user").bind("plothover", function (event, pos, item) {

            if (!pos.x || !pos.y) {
                return;
            }

            if (item) {
                var x = item.datapoint[0],
                    y = item.datapoint[1];
                var date = moment(x).format('DD/MM');
                $("#tooltipUser").html(item.series.label + " ngày " + date + ": " + y)
                    .css({ top: item.pageY + 5, left: item.pageX + 5 })
                    .fadeIn(200);
            } else {
                $("#tooltipUser").hide();
            }
        });

        if ($("#chart_user").length > 0) {

            if (arrNewUser.length > 0) {
                $.plot($("#chart_user"),
                    [{
                        label: "Người dùng mới",
                        data: arrNewUser,
                        lines: {
                            fillColor: "rgba(150, 202, 89, 0.12)"
                        },
                        points: {
                            fillColor: "#fff"
                        }
                    }], chart_plot_02_settings);

            }
            else {
                document.getElementById("chart_user").innerHTML = "Không có người dùng mới!!";
            }
        }
    }

    function initNewOrderChart(data) {
        var arrNewOrder = [];

        $.each(data, function (i, item) {
            arrNewOrder.push([new Date(item.Date).getTime(), item.TotalNewOrder]);
        });
        var chart_plot_02_settings = {
            grid: {
                show: true,
                aboveData: true,
                color: "#3f3f3f",
                //labelMargin: 10,
                axisMargin: 0,
                borderWidth: 0,
                borderColor: null,
                //minBorderMargin: 5,
                clickable: true,
                hoverable: true,
                autoHighlight: true
            },
            series: {
                lines: {
                    show: true,
                    fill: true,
                    lineWidth: 2,
                    steps: false
                },
                points: {
                    show: true,
                    radius: 2.5,
                    symbol: "circle",
                    lineWidth: 3.0
                }
            },
            shadowSize: 0,
            tooltip: true,
            yaxis: {
                min: 0
            },
            xaxis: {
                mode: 'time',
                tickDecimals: 0,
                minTickSize: [1, 'day'],
                timeformat: '%d/%m/%y',
            }
        };

        $("<div id='tooltipOrder'></div>").css({
            position: "absolute",
            display: "none",
            border: "1px solid #fdd",
            padding: "2px",
            "background-color": "#fee",
            opacity: 0.80
        }).appendTo("body");

        $("#chart_order").bind("plothover", function (event, pos, item) {

            if (!pos.x || !pos.y) {
                return;
            }

            if (item) {
                var x = item.datapoint[0],
                    y = item.datapoint[1];
                var date = moment(x).format('DD/MM');
                $("#tooltipOrder").html("SL đơn hàng ngày " + date + ": " + y)
                    .css({ top: item.pageY + 5, left: item.pageX + 5 })
                    .fadeIn(200);
            } else {
                $("#tooltipOrder").hide();
            }
        });

        if ($("#chart_order").length > 0) {

            if (arrNewOrder.length > 0) {
                $.plot($("#chart_order"),
                    [{
                        label: "Đơn hàng mới",
                        data: arrNewOrder,
                        lines: {
                            fillColor: "rgba(150, 202, 89, 0.12)"
                        },
                        points: {
                            fillColor: "#fff"
                        }
                    }], chart_plot_02_settings);

            }
            else {
                document.getElementById("chart_order").innerHTML = "Không có đơn hàng mới!!";
            }
        }
    }

    function initTopVisitProductChart(dataList) {
        var arrTopProduct = [];
        $.each(dataList, function (i, item) {
            arrTopProduct.push({
                'label': item.label,
                'data': item.data
            });
        }); 

        var options = {
            series: {
                pie: {
                    show: true,
                    radius: 3 / 4,
                    label: {
                        show: true,
                        radius: 3 / 4,
                        formatter: function (label, series) {
                            return '<div style="font-size:8pt;text-align:center;padding:2px;color:white;">' + label + '<br/>' + Math.round(series.percent) + '%</div>';
                        },
                        background: {
                            opacity: 0.5,
                            color: '#000'
                        }
                    },
                    innerRadius: 0.2
                },
                legend: {
                    show: false
                }
            },
        };

        if ($("#chart_top_product").length > 0) {
            if (arrTopProduct.length > 0) {
                $.plot($("#chart_top_product"), arrTopProduct, options);
            }
            else {
                document.getElementById("chart_top_product").innerHTML = "Không có dữ liệu!!";
            }
        }
    }
}