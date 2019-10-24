var db;
var productController = function () {
    var imageManagement = new ImageManagement();

    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
        imageManagement.initialize();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                ddlCategoryIdM: { required: true },
                ddlAuthorIdM: { required: true },
                ddlPublisherIdM: { required: true },
                txtPriceM: {
                    required: true,
                    number: true
                }
            }
        });
        //todo: binding events to controls  
        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            initTreeDropDownCategory();
            initTreeDropDownAuthor();
            initTreeDropDownPublisher();
            $('#modal-add-edit').modal('show');

        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImage').val(path);
                    onlineshop.notify('Tải ảnh lên thành công', 'success');

                },
                error: function () {
                    onlineshop.notify('Tải ảnh lên thất bại!', 'error');
                }
            });
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //loadDetails(that);
            $.ajax({
                type: "GET",
                url: "/Admin/Product/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#hidDateCreated').val(data.DateCreated)
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.CategoryId);
                    initTreeDropDownAuthor(data.AuthorId);
                    initTreeDropDownPublisher(data.PublisherId);

                    $('#txtDescM').val(data.Description);
                    $('#txtPriceM').val(data.Price);
                    $('#txtPromotionPriceM').val(data.PromotionPrice);
                    $('#txtImage').val(data.Image);          
                    $('#txtSeoAliasM').val(data.SeoAlias);

                    CKEDITOR.instances.txtContent.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status == 1);
                    $('#ckHotM').prop('checked', data.HotFlag);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);

                    $('#modal-add-edit').modal('show');
                    onlineshop.stopLoading();

                },
                error: function (status) {
                    onlineshop.notify('Có lỗi xảy ra!', 'error');
                    onlineshop.stopLoading();
                }
            });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //deleteProduct(that);
            onlineshop.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Delete successful', 'success');
                        onlineshop.stopLoading();
                        $('#zero_config').DataTable().ajax.reload()

                    },
                    error: function (status) {
                        onlineshop.notify('Has an error in delete progress', 'error');
                        onlineshop.stopLoading();
                    }
                });
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var dateCreated = $('#hidDateCreated').val();
                var categoryId = $('#ddlCategoryIdM').combotree('getValue');
                var authorId = $('#ddlAuthorIdM').combotree('getValue');
                var publisherId = $('#ddlPublisherIdM').combotree('getValue');
                var description = $('#txtDescM').val();
                var viewCount = $('#hidViewCount').val();
                var price = $('#txtPriceM').val();
                var promotionPrice = $('#txtPromotionPriceM').val();
                var image = $('#txtImage').val();
                var seoAlias = $('#txtSeoAliasM').val();
                var content = CKEDITOR.instances.txtContent.getData();
                var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;
                var hot = $('#ckHotM').prop('checked');
                var showHome = $('#ckShowHomeM').prop('checked');

                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        DateCreated: dateCreated,
                        CategoryId: categoryId,
                        AuthorId: authorId,
                        PublisherId: publisherId,
                        Image: image,
                        Price: price,
                        PromotionPrice: promotionPrice,
                        Description: description,
                        ViewCount: viewCount,
                        Content: content,
                        HomeFlag: showHome,
                        HotFlag: hot,
                        Status: status,
                        SeoAlias: seoAlias
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Lưu thành công!', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        onlineshop.stopLoading();
                        $('#zero_config').DataTable().ajax.reload()
                    },
                    error: function () {
                        onlineshop.notify('Có lỗi xảy ra!', 'error');
                        onlineshop.stopLoading();
                    }
                });
                return false;
            }

        });

        $('#btn-import').on('click', function () {
            initTreeDropDownCategory();
            initTreeDropDownAuthor();
            initTreeDropDownPublisher();
            $('#modal-import-excel').modal('show');
        });

        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            fileData.append('authorId', $('#ddlAuthorIdImportExcel').combotree('getValue'));
            fileData.append('categoryId', $('#ddlPublisherIdImportExcel').combotree('getValue'));
            $.ajax({
                url: '/Admin/Product/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#modal-import-excel').modal('hide');
                    loadData();
                }
            });
            return false;
        });

        $('#btn-export').on('click', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Product/ExportExcel",
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    onlineshop.stopLoading();
                },
                error: function () {
                    onlineshop.notify('Has an error in progress', 'error');
                    onlineshop.stopLoading();
                }
            });
        });
    }

    function registerControls() {
        CKEDITOR.replace('txtContent', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };


    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/ProductCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                console.log("category")
                console.log(data);
                var arr = onlineshop.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    editable: true,
                    autocomplete: true,
                    data: arr
                });

                $('#ddlCategoryIdImportExcel').combotree({
                    editable: true,
                    autocomplete: true,
                    data: arr
                });

                if (selectedId != undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }

    function initTreeDropDownAuthor(selectedId) {
        $.ajax({
            url: "/Admin/Author/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.AuthorName,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                console.log("author")
                console.log(data);
                var arr = onlineshop.unflattern(data);
                $('#ddlAuthorIdM').combotree({
                    editable: true,
                    autocomplete: true,
                    data: arr
                });

                $('#ddlAuthorIdImportExcel').combotree({
                    editable: true,
                    autocomplete: true,
                    data: arr
                });

                if (selectedId != undefined) {
                    $('#ddlAuthorIdM').combotree('setValue', selectedId);
                }
            }
        });
    }

    function initTreeDropDownPublisher(selectedId) {
        $.ajax({
            url: "/Admin/Publisher/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.NamePublisher,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                console.log("publisher")
                console.log(data);
                var arr = onlineshop.unflattern(data);
                $('#ddlPublisherIdM').combotree({
                    editable: true,
                    autocomplete: true,
                    data: arr
                });

                $('#ddlPublisherIdImportExcel').combotree({
                    editable: true,
                    autocomplete: true,
                    data: arr
                });

                if (selectedId != undefined) {
                    $('#ddlPublisherIdM').combotree('setValue', selectedId);
                }
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory();
        initTreeDropDownAuthor();
        initTreeDropDownPublisher();

        $('#txtDescM').val('');

        $('#txtPriceM').val('');
        $('#txtPromotionPriceM').val('');

        $('#txtImage').val('');

        $('#txtSeoAliasM').val('');

        CKEDITOR.instances.txtContent.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);

    }

    function loadData() {
        $.fn.dataTable.moment('DD/MM/YYYY');
        db = $('#zero_config').dataTable({
            // the indexs of the column that want to have the dropdown filter
            initComplete: function () {
                this.api().columns([2]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">--Category filter--</option></select>')
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
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            },
            processing: true, // for show progress bar
            serverSide: false, // for process server side
            order: [[5, "desc"]],
            ajax: {
                type: 'GET',
                url: '/admin/product/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [{
                targets: [0, 1, 2, 3, 4, 5, 6],
                autoWidth: true
            }],
            columnDefs: [{
                targets: [0, 2, 4, 6],
                sortable: false
            }],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return '<button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Xóa" data-original-title="Delete" data-id="' + data + '" class="btn btn-danger btn-delete"><i class="fas fa-trash"></i></button> <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Sửa" data-original-title="Edit" data-id="' + data + '" class="btn btn-success btn-edit"><i class="fas fa-pencil-alt"></i></button> <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Quản lý hình ảnh" data-original-title="Image management" data-id="' + data + '" class="btn btn-primary btn-images"><i class="fas fa-file-image"></i></button>' /*<button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Quantity managment" data-original-title="QUantity management" data-id="' + data + '" class="btn btn-info btn-quantity"><i class="fas fa-hashtag"></i></button> <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Wholesale price management" data-original-title="Wholesale price management" data-id="' + data + '" class="btn btn-warning btn-whole-price"><i class="fas fa-money-bill-alt"></i></button>*/;
                    }
                },
                { data: "Name" },
                { data: "ProductCategory.Name" },
                {
                    data: "Price", render: function (data, type, row) {
                        return data = onlineshop.formatNumber(data, 0)
                    }
                },
                {
                    data: "Image", render: function (data, type, row) {
                        return data == null ? '<img src="/admin-side/assets/images/No-image-found.jpg" width=35' : '<img src="' + data + '" width=35 />'
                    }
                },
                {
                    data: "DateCreated", render: function (data, type, row) {
                        return data = moment(data).format('DD/MM/YYYY HH:mm:ss')
                    }
                },
                {
                    data: "Status", render: function (data, type, row) {
                        return data = onlineshop.getStatus(data)
                    }
                }
            ]
        });
    }

}