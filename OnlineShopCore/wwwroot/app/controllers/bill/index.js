var BillController = function () {
    var cachedObj = {
        products: [],
        paymentMethods: [],
        billStatuses: []
    }
    this.initialize = function () {
        $.when(loadBillStatus(),
            loadPaymentMethod(),
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
                txtCustomerMessage: { required: false },
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
            resetFormMaintainance();
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
                    console.log(data)
                    $('#hidId').val(data.Id);
                    $('#hidProvince').val(data.Province);
                    $('#hidToDistrictID').val(data.DistrictID);
                    $('#hidToWardCode').val(data.WardCode);
                    $('#hidDateCreated').val(data.DateCreated);
                    $('#txtCustomerName').val(data.CustomerName);
                    $('#txtCustomerName').prop('disabled', true);
                    $('#txtCODAmount').val(onlineshop.formatNumber(data.CODAmount, 0) + " VNĐ");
                    $('#hidCODAmount').val(data.CODAmount);
                    $('#txtCODAmount').prop('disabled', true);
                    $('#txtCustomerAddress').val(data.CustomerAddress);
                    $('#txtCustomerAddress').prop('disabled', true);
                    $('#txtCustomerMobile').val(data.CustomerMobile);
                    $('#txtCustomerMobile').prop('disabled', true);
                    $('#txtCustomerMessage').val(data.CustomerMessage);
                    $('#txtCustomerMessage').prop('disabled', true);
                    $('#ddlPaymentMethod').val(data.PaymentMethod);
                    $('#ddlPaymentMethod').prop('disabled', true);
                    $('#hidCustomerId').val(data.CustomerId);
                    $('#ddlBillStatus').val(data.BillStatus);
                    if (data.BillStatus == '4' || data.BillStatus == '3') {
                        $('#ddlBillStatus').prop('disabled', true);

                        var billDetails = data.BillDetails;
                        if (data.BillDetails != null && data.BillDetails.length > 0) {
                            var render = '';
                            var templateDetails = $('#template-table-bill-details').html();

                            $.each(billDetails, function (i, item) {
                                var products = getProductOptions(item.ProductId);

                                render += Mustache.render(templateDetails,
                                    {
                                        Id: item.Id,
                                        Products: products,
                                        Quantity: item.Quantity
                                    });
                            });
                            $('#tbl-bill-details').html(render);
                        }
                        $('tbl-bill-details').prop('disable', true);
                        $('#modal-detail').modal('show');
                        $("#tbl-bill-details input").prop('disabled', true);
                        $("#tbl-bill-details select").prop('disabled', true);
                        $("#tbl-bill-details button").prop('disabled', true);
                        $("#btnAddDetail").prop('disabled', true);
                        onlineshop.stopLoading();
                    }

                    else {
                        var billDetails = data.BillDetails;
                        if (data.BillDetails != null && data.BillDetails.length > 0) {
                            var render = '';
                            var templateDetails = $('#template-table-bill-details').html();

                            $.each(billDetails, function (i, item) {
                                var products = getProductOptions(item.ProductId);

                                render += Mustache.render(templateDetails,
                                    {
                                        Id: item.Id,
                                        Products: products,
                                        Quantity: item.Quantity
                                    });
                            });
                            $('#tbl-bill-details').html(render);
                        }
                        $('tbl-bill-details').prop('disable', true);
                        $('#modal-detail').modal('show');
                        onlineshop.stopLoading();
                    }



                },
                error: function (e) {
                    onlineshop.notify('Has an error in progress', 'error');
                    onlineshop.stopLoading();
                }
            });
        });

        $('#btnCreateOrderGHN').on('click', function () {
            //e.preventDefault();

            var toDistrictID = $('#hidToDistrictID').val();
            var toWardCode = $('#hidToWardCode').val();
            var customerName = $('#txtCustomerName').val();
            var customerPhone = $('#txtCustomerMobile').val();
            var customerAddress = $('#txtCustomerAddress').val();
            var customerMessage = $('#txtCustomerMessage').val();
            var codAmount = $('#hidCODAmount').val();      
            $.ajax({
                type: "POST",
                url: "/Admin/Bill/CreateOrderGHN",
                data: {
                    toDistrictID,
                    toWardCode,
                    customerName,
                    customerPhone,
                    customerAddress,
                    customerMessage,
                    codAmount
                },
                success: function () {
                    onlineshop.notify('Tạo đơn hàng GHN thành công!', 'success');
                    $('#modal-detail').modal('hide');
                    changeBillStatus();
                    resetFormMaintainance();               
                },
                error: function () {
                    onlineshop.notify('Có lỗi xảy ra!', 'error');
                }
            })
            
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var province = $('#hidProvince').val();
                var toDistrictID = $('#hidToDistrictID').val();
                var toWardCode = $('#hidToWardCode').val();
                var codAmount = $('#hidCODAmount').val(); 
                var dateCreated = $('#hidDateCreated').val();
                var customerName = $('#txtCustomerName').val();
                var customerAddress = $('#txtCustomerAddress').val();
                var customerId = $('#hidCustomerId').val();
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
                        BillId: id
                    });
                });

                $.ajax({
                    type: "POST",
                    url: "/Admin/Bill/SaveEntity",
                    data: {
                        Id: id,
                        Province: province,
                        DistrictID: toDistrictID,
                        WardCode: toWardCode,
                        CODAmount: codAmount,
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
                        onlineshop.notify('Lưu đơn hàng thành công!', 'success');
                        $('#modal-detail').modal('hide');
                        resetFormMaintainance();

                        onlineshop.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        onlineshop.notify('Có lỗi xảy ra!', 'error');
                        onlineshop.stopLoading();
                    }
                });
                return false;
            }
        });

        $('#btnAddDetail').on('click', function () {
            var template = $('#template-table-bill-details').html();
            var products = getProductOptions(null);
            //var colors = getColorOptions(null);
            //var sizes = getSizeOptions(null);
            var render = Mustache.render(template,
                {
                    Id: 0,
                    Products: products,
                    //Colors: colors,
                    //Sizes: sizes,
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
    function changeBillStatus() {
        if ($('#frmMaintainance').valid()) {
            var id = $('#hidId').val();
            var province = $('#hidProvince').val();
            var toDistrictID = $('#hidToDistrictID').val();
            var toWardCode = $('#hidToWardCode').val();
            var codAmount = $('#hidCODAmount').val();
            var dateCreated = $('#hidDateCreated').val();
            var customerName = $('#txtCustomerName').val();
            var customerAddress = $('#txtCustomerAddress').val();
            var customerId = $('#hidCustomerId').val();
            var customerMobile = $('#txtCustomerMobile').val();
            var customerMessage = $('#txtCustomerMessage').val();
            var paymentMethod = $('#ddlPaymentMethod').val();
            var billStatus = 1;
            //bill detail

            var billDetails = [];
            $.each($('#tbl-bill-details tr'), function (i, item) {
                billDetails.push({
                    Id: $(item).data('id'),
                    ProductId: $(item).find('select.ddlProductId').first().val(),
                    Quantity: $(item).find('input.txtQuantity').first().val(),
                    BillId: id
                });
            });

            $.ajax({
                type: "POST",
                url: "/Admin/Bill/SaveEntity",
                data: {
                    Id: id,
                    Province: province,
                    DistrictID: toDistrictID,
                    WardCode: toWardCode,
                    CODAmount: codAmount,
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
                success: function () {
                    loadData(true);
                }
            });
            return false;
        }

    }

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

        $('#txtCustomerName').prop('disabled', false);
        $('#txtCustomerAddress').prop('disabled', false);
        $('#txtCustomerMobile').prop('disabled', false);
        $('#txtCustomerMessage').prop('disabled', false);
        $('#txtCODAmount').prop('disabled', false);
        $('#ddlPaymentMethod').prop('disabled', false);
        $('#ddlBillStatus').prop('disabled', false);
        $("#btnAddDetail").prop('disabled', false);
    }

    function loadData() {
        $(document).ready(function () {
            //init date column format as dd/mm/yyyy
            $.fn.dataTable.moment('DD/MM/YYYY');
            //init date range filter
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    var min = $('#fromDate').datepicker("getDate");
                    var max = $('#toDate').datepicker("getDate");
                    var d = data[3].split("/");
                    var startDate = new Date(d[1] + "/" + d[0] + "/" + d[2]);
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

        $("#reset-date").click(function () {
            $('#fromDate').val("").datepicker("update");
        })

        $("#reset-date2").click(function () {
            $('#toDate').val("").datepicker("update");
        })

        //init dataTables 
        db = $('#zero_config').dataTable({
            // the indexs of the column that want to have the dropdown filter
            initComplete: function () {
                this.api().columns([2]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">--Phương thức thanh toán--</option></select>')
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
                        select.append('<option value="' + getPaymentMethodName(d) + '">' + getPaymentMethodName(d) + '</option>')
                    });
                });

                // the indexs of the column that want to have the dropdown filter
                this.api().columns([4]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">--Trạng thái--</option></select>')
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
                        select.append('<option value="' + getBillStatusName(d) + '">' + getBillStatusName(d) + '</option>')
                    });
                });
            },
            processing: true, // for show progress bar
            serverSide: false, // for process server side
            destroy: true,
            order: [[3, "asc"]],
            ajax: {
                type: 'GET',
                url: '/admin/bill/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [
                {
                    targets: [0, 1, 2, 3, 4],
                    searchable: false
                },
                {
                    targets: [0, 1, 2, 3, 4],
                    autoWidth: true
                },
                {
                    targets: [0, 2, 4],
                    sortable: false
                },
                {
                    targets: [5],
                    visible: false
                }
            ],
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
                        return data = moment(data).format('DD/MM/YYYY HH:mm:ss')
                    }
                },
                {
                    data: "BillStatus", render: function (data, type, row) {
                        return data = getBillStatusName(data)
                    }
                },
                {
                    data: "Id", render: function (data, type, row) {
                        return data = data
                    }
                }
            ]
        });
    }
}