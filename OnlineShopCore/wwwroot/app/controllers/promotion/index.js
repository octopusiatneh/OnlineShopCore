var db;
var promotionController = function () {
    var cachedObj = {
        products: []
    }

    var cachedObjFull = {
        products: []
    }
    this.initialize = function () {
        loadProductsFull();
        loadData();
        registerEvents();
    }

    this.loadProducts = function () {
        loadProducts();
    }

    this.resetTableDetail = function () {
        resetTableDetail();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtPromotionName: { required: true },
            }
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });


        $('body').on('click', '.btn-view', function (e) {
            resetFormMaintainance();
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Promotion/GetById",
                data: { id: that },
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtPromotionName').val(data.PromotionName);
                    $('#hidDateCreated').val(data.DateCreated);
                    $('#date-start').val(moment(data.DateStart).format("DD/MM/YYYY HH:MM"));
                    $('#date-end').val(moment(data.DateEnd).format("DD/MM/YYYY HH:MM"));

                    var promotionDetails = data.PromotionDetails;
                    if (data.PromotionDetails != null && data.PromotionDetails.length > 0) {
                        var render = '';
                        var templateDetails = $('#template-table-bill-details').html();

                        $.each(promotionDetails, function (i, item) {
                            var products = getProductOptionsFull(item.ProductId);

                            render += Mustache.render(templateDetails,
                                {
                                    Id: item.Id,
                                    Products: products,
                                    PromotionPercent: item.PromotionPercent
                                });
                        });
                        $('#tbl-bill-details').html(render);
                    }
                    $('#modal-add-edit').modal('show');
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
                var promotionName = $('#txtPromotionName').val();
                var dateStart = $('#date-start').val();
                var dateEnd = $('#date-end').val();
                //promotion detail
                var promotionDetails = [];
                $.each($('#tbl-bill-details tr'), function (i, item) {
                    promotionDetails.push({
                        Id: $(item).data('id'),
                        ProductId: $(item).find('select.ddlProductId').first().val(),
                        PromotionPercent: $(item).find('input.txtPromotionPercent').first().val(),
                        PromotionId: id
                    });
                });

                $.ajax({
                    type: "POST",
                    url: "/Admin/Promotion/SaveEntity",
                    data: {
                        Id: id,
                        PromotionName: promotionName,
                        DateStart: dateStart,
                        DateEnd: dateEnd,
                        PromotionDetails: promotionDetails,
                        Status: 1
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Lưu thành công!', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
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
            var render = Mustache.render(template,
                {
                    Id: 0,
                    Products: products,
                });
            $('#tbl-bill-details').append(render);
        });

        $('body').on('click', '.btn-delete-detail', function () {
            $(this).parent().parent().remove();
        });

    };

    //get available product for dropdown list select product
    function loadProducts() {
        var dateStart = $('#date-start').val();
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAvailableProductForPromotion",
            data: {
                dateStart: dateStart
            },
            success: function (response) {
                cachedObj.products = response;
                console.log(cachedObj)
            },
            error: function () {
                onlineshop.notify('Has an error in getting available product progress', 'error');
            }
        });
    }

    //get cache product for btn-view click => get all product to display
    function loadProductsFull() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAll",
            success: function (response) {
                cachedObjFull.products = response;
            },
            error: function () {
                onlineshop.notify('Has an error in getting available product progress', 'error');
            }
        });
    }

    function getProductOptions(selectedId) {
        products = "<select class='form-control ddlProductId'>";
        $.each(cachedObj.products, function (i, product) {
            if (selectedId === product.Id)
                products += '<option value="' + product.Id + '" selected="select">' + product.Name + '</option>';
            else
                products += '<option value="' + product.Id + '">' + product.Name + '</option>';
        });
        products += "</select>";
        return products;
    }

    function getProductOptionsFull(selectedId) {
        products = "<select class='form-control ddlProductId'>";
        $.each(cachedObjFull.products, function (i, product) {
            if (selectedId === product.Id)
                products += '<option value="' + product.Id + '" selected="select">' + product.Name + '</option>';
            else
                products += '<option value="' + product.Id + '">' + product.Name + '</option>';
        });
        products += "</select>";
        return products;
    }

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtPromotionName').val('');
        $('#tbl-bill-details').html('');
        $('#date-start').val('');
        $('#date-end').val('');
    }

    function resetTableDetail() {
        $('#tbl-bill-details').html('');
    }

    function loadData() {
        //init dataTables 
        db = $('#zero_config').dataTable({
            processing: true, // for show progress bar
            serverSide: false, // for process server side
            destroy: true,
            order: [[2, "asc"]],
            ajax: {
                type: 'GET',
                url: '/admin/promotion/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [
                {
                    targets: [2, 3, 4],
                    searchable: false
                },
                {
                    targets: [0],
                    sortable: false
                }
            ],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return ' <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Edit" data-original-title="Edit" data-id="' + data + '" class="btn btn-success btn-view"><i class="fas fa-pencil-alt"></i></button> ';
                    }
                },
                {
                    data: "PromotionName"
                },
                {
                    data: "DateCreated", render: function (data, type, row) {
                        return data = moment(data).format('DD/MM/YYYY HH:mm:ss')
                    }
                },
                {
                    data: "DateStart", render: function (data, type, row) {
                        return data = moment(data).format('DD/MM/YYYY HH:mm:ss')
                    }
                },
                {
                    data: "DateEnd", render: function (data, type, row) {
                        return data = moment(data).format('DD/MM/YYYY HH:mm:ss')
                    }
                }
            ]
        });

    }
}
