var BillController = function () {
    var cachedObj = {
        products: [],
        colors: [],
        sizes: [],
        paymentMethods: [],
        billStatuses: []
    }
    this.initialize = function () {
        $.when(loadBillStatus(),
            loadPaymentMethod(),
            loadColors(),
            loadSizes(),
            loadProducts())
            .done(function () {
                loadData();
            });

        registerEvents();
    }

   

    function registerEvents() {
        

        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtCustomerName: { required: true },
                txtCustomerAddress: { required: true },
                txtCustomerMobile: { required: true },
                txtCustomerMessage: { required: true },
                ddlBillStatus: { required: true }
            }
        });



     


        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-detail').modal('show');
        });
        $("#ddl-show-page").on('change', function () {
            onlineshop.configs.pageSize = $(this).val();
            onlineshop.configs.pageIndex = 1;
            loadData(true);
        });

        $('body').on('click', '.btn-view', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Bill/GetById",
                data: { id: that },
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#hidDateCreated').val(data.DateCreated);
                    $('#txtCustomerName').val(data.CustomerName);    
                    $('#txtCustomerAddress').val(data.CustomerAddress);
                    $('#txtCustomerMobile').val(data.CustomerMobile);
                    $('#txtCustomerMessage').val(data.CustomerMessage);
                    $('#ddlPaymentMethod').val(data.PaymentMethod);
                    $('#ddlCustomerId').val(data.CustomerId);
                    $('#ddlBillStatus').val(data.BillStatus);

                    var billDetails = data.BillDetails;
                    if (data.BillDetails != null && data.BillDetails.length > 0) {
                        var render = '';
                        var templateDetails = $('#template-table-bill-details').html();

                        $.each(billDetails, function (i, item) {
                            var products = getProductOptions(item.ProductId);
                            var colors = getColorOptions(item.ColorId);
                            var sizes = getSizeOptions(item.SizeId);

                            render += Mustache.render(templateDetails,
                                {
                                    Id: item.Id,
                                    Products: products,
                                    Colors: colors,
                                    Sizes: sizes,
                                    Quantity: item.Quantity
                                });
                        });
                        $('#tbl-bill-details').html(render);
                    }
                    $('#modal-detail').modal('show');
                    onlineshop.stopLoading();

                },
                error: function (e) {
                    onlineshop.notify('Has an error in progress', 'error');
                    onlineshop.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var dateCreated = $('#hidDateCreated').val();
                var customerName = $('#txtCustomerName').val();
                var customerAddress = $('#txtCustomerAddress').val();
                var customerId = $('#ddlCustomerId').val();
                var customerMobile = $('#txtCustomerMobile').val();
                var customerMessage = $('#txtCustomerMessage').val();
                var paymentMethod = $('#ddlPaymentMethod').val();
                var billStatus = $('#ddlBillStatus').val();
                //bill detail

                var billDetails = [];
                $.each($('#tbl-bill-details tr'), function (i, item) {
                    billDetails.push({
                        Id: $(item).data('id'),                       
                        ProductId: $(item).find('select.ddlProductId').first().val(),
                        Quantity: $(item).find('input.txtQuantity').first().val(),
                        ColorId: $(item).find('select.ddlColorId').first().val(),
                        SizeId: $(item).find('select.ddlSizeId').first().val(),
                        BillId: id
                    });
                });

                $.ajax({
                    type: "POST",
                    url: "/Admin/Bill/SaveEntity",
                    data: {
                        Id: id,
                        BillStatus: billStatus,
                        DateCreated: dateCreated,
                        CustomerAddress: customerAddress,   
                        CustomerId: customerId,
                        CustomerMessage: customerMessage,
                        CustomerMobile: customerMobile,
                        CustomerName: customerName,
                        PaymentMethod: paymentMethod,
                        Status: 1,
                        BillDetails: billDetails
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Save order successful', 'success');
                        $('#modal-detail').modal('hide');
                        resetFormMaintainance();

                        onlineshop.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        onlineshop.notify('Has an error in progress', 'error');
                        onlineshop.stopLoading();
                    }
                });
                return false;
            }

        });

        $('#btnAddDetail').on('click', function () {
            var template = $('#template-table-bill-details').html();
            var products = getProductOptions(null);
            var colors = getColorOptions(null);
            var sizes = getSizeOptions(null);
            var render = Mustache.render(template,
                {
                    Id: 0,
                    Products: products,
                   
                    Colors: colors,
                    Sizes: sizes,
                    Quantity: 0,
                    Total: 0
                });
            $('#tbl-bill-details').append(render);
        });

        $('body').on('click', '.btn-delete-detail', function () {
            $(this).parent().parent().remove();
        });

        $("#btnExport").on('click', function () {
            var that = $('#hidId').val();
            $.ajax({
                type: "POST",
                url: "/Admin/Bill/ExportExcel",
                data: { billId: that },
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    window.location.href = response;

                    onlineshop.stopLoading();

                }
            });
        });
    };

    function loadBillStatus() {
        return $.ajax({
            type: "GET",
            url: "/admin/bill/GetBillStatus",
            dataType: "json",
            success: function (response) {
                cachedObj.billStatuses = response;
                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddlBillStatus').html(render);
            }
        });
    }

    function loadPaymentMethod() {
        return $.ajax({
            type: "GET",
            url: "/admin/bill/GetPaymentMethod",
            dataType: "json",
            success: function (response) {
                cachedObj.paymentMethods = response;
                var render = "";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                });
                $('#ddlPaymentMethod').html(render);
            }
        });
    }

    function loadProducts() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAll",
            dataType: "json",
            success: function (response) {
                cachedObj.products = response;
            },
            error: function () {
                onlineshop.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadColors() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetColors",
            dataType: "json",
            success: function (response) {
                cachedObj.colors = response;
            },
            error: function () {
                onlineshop.notify('Has an error in progress', 'error');
            }
        });
    }

    function loadSizes() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Bill/GetSizes",
            dataType: "json",
            success: function (response) {
                cachedObj.sizes = response;
            },
            error: function () {
                onlineshop.notify('Has an error in progress', 'error');
            }
        });
    }

    function getProductOptions(selectedId) {
        var products = "<select class='form-control ddlProductId'>";
        $.each(cachedObj.products, function (i, product) {
            if (selectedId === product.Id)
                products += '<option value="' + product.Id + '" selected="select">' + product.Name + '</option>';
            else
                products += '<option value="' + product.Id + '">' + product.Name + '</option>';
        });
        products += "</select>";
        return products;
    }

    function getPaymentMethodName(paymentMethod) {
        var method = $.grep(cachedObj.paymentMethods, function (element, index) {
            return element.Value == paymentMethod;
        });
        if (method.length > 0)
            return method[0].Name;
        else return '';
    }

    function getBillStatusName(status) {
        var status = $.grep(cachedObj.billStatuses, function (element, index) {
            return element.Value == status;
        });
        if (status.length > 0)
            return status[0].Name;
        else return '';
    }

    function getColorOptions(selectedId) {
        var colors = "<select class='form-control ddlColorId'>";
        $.each(cachedObj.colors, function (i, color) {
            if (selectedId === color.Id)
                colors += '<option value="' + color.Id + '" selected="select">' + color.Name + '</option>';
            else
                colors += '<option value="' + color.Id + '">' + color.Name + '</option>';
        });
        colors += "</select>";
        return colors;
    }

    function getSizeOptions(selectedId) {
        var sizes = "<select class='form-control ddlSizeId'>";
        $.each(cachedObj.sizes, function (i, size) {
            if (selectedId === size.Id)
                sizes += '<option value="' + size.Id + '" selected="select">' + size.Name + '</option>';
            else
                sizes += '<option value="' + size.Id + '">' + size.Name + '</option>';
        });
        sizes += "</select>";
        return sizes;
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtCustomerName').val('');

        $('#txtCustomerAddress').val('');
        $('#txtCustomerMobile').val('');
        $('#txtCustomerMessage').val('');
        $('#ddlPaymentMethod').val('');
        $('#ddlCustomerId').val('');
        $('#ddlBillStatus').val('');
        $('#tbl-bill-details').html('');
    }

    //function loadData(isPageChanged) {
    //    $.ajax({
    //        type: "GET",
    //        url: "/admin/bill/GetAllPaging",
    //        data: {
    //            startDate: $('#txtFromDate').val(),
    //            endDate: $('#txtToDate').val(),
    //            keyword: $('#txtSearchKeyword').val(),
    //            page: onlineshop.configs.pageIndex,
    //            pageSize: onlineshop.configs.pageSize
    //        },
    //        dataType: "json",
    //        beforeSend: function () {
    //            onlineshop.startLoading();
    //        },
    //        success: function (response) {
    //            var template = $('#table-template').html();
    //            var render = "";
    //            if (response.RowCount > 0) {
    //                $.each(response.Results, function (i, item) {
    //                    render += Mustache.render(template, {
    //                        CustomerName: item.CustomerName,
    //                        Id: item.Id,
    //                        PaymentMethod: getPaymentMethodName(item.PaymentMethod),
    //                        DateCreated: onlineshop.dateTimeFormatJson(item.DateCreated),
    //                        BillStatus: getBillStatusName(item.BillStatus)
    //                    });
    //                });
    //                $("#lbl-total-records").text(response.RowCount);
    //                if (render != undefined) {
    //                    $('#tbl-content').html(render);

    //                }
    //                wrapPaging(response.RowCount, function () {
    //                    loadData();
    //                }, isPageChanged);


    //            }
    //            else {
    //                $("#lbl-total-records").text('0');
    //                $('#tbl-content').html('');
    //            }
    //            onlineshop.stopLoading();
    //        },
    //        error: function (status) {
    //            console.log(status);
    //        }
    //    });
    //};

    function loadData() {

        $(document).ready(function () {
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var min = $('#fromDate').datepicker("getDate");
                    var max = $('#toDate').datepicker("getDate");
                    var startDate = new Date(data[3]);
                    if (min == null && max == null) { return true; }
                    if (min == null && startDate <= max) { return true; }
                    if (max == null && startDate >= min) { return true; }
                    if (startDate <= max && startDate >= min) { return true; }
                    return false;
                }
            );


            $("#fromDate").datepicker({ onSelect: function () { table.draw(); }, changeMonth: true, changeYear: true });
            $("#toDate").datepicker({ onSelect: function () { table.draw(); }, changeMonth: true, changeYear: true });
            var table = $('#zero_config').DataTable();

            // Event listener to the two range filtering inputs to redraw on input
            $('#fromDate, #toDate').change(function () {
                table.draw();
            });

        });





        $.fn.dataTable.moment('DD/MM/YYYY');
        db = $('#zero_config').dataTable({
            // the indexs of the column that want to have the dropdown filter
            initComplete: function () {
                this.api().columns([2]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">--Payment method filter--</option></select>')
                        .appendTo($(column.header()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });

                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + getPaymentMethodName(d) + '</option>')
                    });
                });
            },
            processing: true, // for show progress bar
            serverSide: false, // for process server side
            destroy: true,
            order: [[3, "desc"]],
            ajax: {
                type: 'GET',
                url: '/admin/bill/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [{
                targets: [0, 1, 2, 3, 4],
                autoWidth: true
            }],
            columnDefs: [{
                targets: [0, 2, 4],
                sortable: false
            }],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return ' <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Edit" data-original-title="Edit" data-id="' + data + '" class="btn btn-success btn-view"><i class="fas fa-pencil-alt"></i></button> ';
                    }
                },
                { data: "CustomerName" },
                {
                    data: "PaymentMethod", render: function (data, type, row) {
                        return data = getPaymentMethodName(data)
                    }
                },
              
                {
                    data: "DateCreated", render: function (data, type, row) {
                        return data = moment(data).format('DD/MM/YYYY')
                    }
                },
                {
                    data: "BillStatus", render: function (data, type, row) {
                        return data = getBillStatusName(data)
                    }
                }
            ]
        });
    }
}