var db;
var productCategoryController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true }
            }
        });

        $('#btnCreate').off('click').on('click', function () {
            $('#modal-add-edit').modal('show');
        });  

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //loadDetails(that);
            $.ajax({
                type: "GET",
                url: "/Admin/Category/GetById",
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
                    $('#txtSeoAliasM').val(data.SeoAlias);      
                    $('#ckStatusM').prop('checked', data.Status == 1);       
                    $('#modal-add-edit').modal('show');
                    onlineshop.stopLoading();
                },
                error: function (status) {
                    onlineshop.notify('Có lỗi xảy ra', 'error');
                    onlineshop.stopLoading();
                }
            });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            //deleteProduct(that);
            onlineshop.confirm('Bạn có muốn xóa danh mục này', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Category/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Xóa danh mục thành công', 'success');
                        onlineshop.stopLoading();
                        $('#zero_config').DataTable().ajax.reload()

                    },
                    error: function (status) {
                        onlineshop.notify('Xóa không thành công', 'error');
                        onlineshop.stopLoading();
                    }
                });
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val());
                var name = $('#txtNameM').val();    
                var seoAlias = $('#txtSeoAliasM').val();
                var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;
                $.ajax({
                    type: "POST",
                    url: "/Admin/Category/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,                   
                        Status: status,           
                        SeoAlias: seoAlias,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Thành công !', 'success');
                        $('#modal-add-edit').modal('hide');

                        resetFormMaintainance();

                        onlineshop.stopLoading();
                        $('#zero_config').DataTable().ajax.reload()
                    },
                    error: function () {
                        onlineshop.notify('Lỗi!', 'error');
                        onlineshop.stopLoading();
                    }
                });
            }
            return false;

        });
    }
    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtSeoAliasM').val('');

        $('#ckStatusM').prop('checked', true);
    }
  
    function loadData() {       
        db = $('#zero_config').dataTable({
            processing: true, // for show progress bar
            serverSide: false, // for process server side
            order: [[2, "desc"]], // sort db by date created
            ajax: {
                type: 'GET',
                url: '/admin/category/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [{
                targets: [0, 1, 2, 3],
                autoWidth: true
            }],
            columnDefs: [{
                targets: [0, 3],
                sortable: false
            }],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return '<button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Xóa" data-original-title="Delete" data-id="' + data + '" class="btn btn-danger btn-delete"><i class="fas fa-trash"></i></button> <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Sửa" data-original-title="Edit" data-id="' + data + '" class="btn btn-success btn-edit"><i class="fas fa-pencil-alt"></i></button>' /*<button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Quantity managment" data-original-title="QUantity management" data-id="' + data + '" class="btn btn-info btn-quantity"><i class="fas fa-hashtag"></i></button> <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Wholesale price management" data-original-title="Wholesale price management" data-id="' + data + '" class="btn btn-warning btn-whole-price"><i class="fas fa-money-bill-alt"></i></button>*/;
                    }
                },
                {
                    data: "Name"
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
        $.fn.dataTable.moment('DD/MM/YYYY');
    }
}