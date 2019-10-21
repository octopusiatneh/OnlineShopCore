var HomeController = function () {
    this.initialize = function () {
        loadData();
    }

    function loadData(from, to) {

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
                initChart(response);

                onlineshop.stopLoading();

            },
            error: function (status) {
                onlineshop.notify('Có lỗi xảy ra', 'error');
                onlineshop.stopLoading();
            }
        });
    }
    function initChart(data) {
        var arrRevenue = [];

        $.each(data, function (i, item) {
            arrRevenue.push([new Date(item.Date).getTime(), item.Revenue]);
        });
        console.log(arrRevenue);
        var chart_plot_02_settings = {
            grid: {
                show: true,
                aboveData: true,
                color: "#3f3f3f",
                labelMargin: 10,
                axisMargin: 0,
                borderWidth: 0,
                borderColor: null,
                minBorderMargin: 5,
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
                    radius: 4.5,
                    symbol: "circle",
                    lineWidth: 3.0
                }
            },
            legend: {
                position: "ne",
                margin: [0, -25],
                noColumns: 0,
                labelBoxBorderColor: null,
                labelFormatter: function (label, series) {
                    return label + '&nbsp;&nbsp;';
                },
                width: 40,
                height: 1
            },
            colors: ['#96CA59', '#3F97EB', '#72c380', '#6f7a8a', '#f7cb38', '#5a8022', '#2c7282'],
            shadowSize: 0,
            tooltip: true,
            tooltipOpts: {
                content: '%s: %y.0',
                xDateFormat: '%d/%m',
                shifts: {
                    x: -30,
                    y: -50
                },
                defaultTheme: false
            },
            yaxis: {
                min: 0
            },
            xaxis: {
                mode: 'time',
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
                console.log(date);
                $("#tooltip").html(item.series.label + " ngày " + date + " = " + y)
                    .css({ top: item.pageY + 5, left: item.pageX + 5 })
                    .fadeIn(200);
            } else {
                $("#tooltip").hide();
            }
        });

        if ($("#chart_plot_02").length) {
            console.log('Plot2');

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
    }
}