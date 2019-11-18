var db;
var promotionController = function () {
    var cachedObj = {
        products: []
    }
    this.initialize = function () {
        loadProducts();
        loadData();
        registerEvents();
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
                    $('#date-expired').val(moment(data.DateExpired).format("DD/MM/YYYY HH:MM"));

                    var promotionDetails = data.PromotionDetails;
                    if (data.PromotionDetails != null && data.PromotionDetails.length > 0) {
                        var render = '';
                        var templateDetails = $('#template-table-bill-details').html();

                        $.each(promotionDetails, function (i, item) {
                            var products = getProductOptions(item.ProductId);

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
                var dateExpired = $('#date-expired').val();
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
                        DateExpired: dateExpired,
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

    function loadProducts() {
        return $.ajax({
            type: "GET",
            url: "/Admin/Product/GetAllWithNoPromotionPrice",
            dataType: "json",
            success: function (response) {
                console.log(response)
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

    function resetFormMaintainance() {
        $('#hidId').val(0);
        $('#txtPromotionName').val('');
        $('#tbl-bill-details').html('');
        $('#date-expired').val('');
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
                    targets: [1],
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
                    data: "DateExpired", render: function (data, type, row) {
                        return data = moment(data).format('DD/MM/YYYY HH:mm:ss')
                    }
                }
            ]
        });

    }
}
