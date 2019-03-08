var dataTable;
var RoleController = function () {
    var self = this;

    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtName: { required: true }
            }
        });

        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Role/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtName').val(data.Name);
                    $('#txtDescription').val(data.Description);
                    $('#modal-add-edit').modal('show');
                    onlineshop.stopLoading();

                },
                error: function (status) {
                    onlineshop.notify('Có lỗi xảy ra', 'error');
                    onlineshop.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidId').val();
                var name = $('#txtName').val();
                var description = $('#txtDescription').val();

                $.ajax({
                    type: "POST",
                    url: "/Admin/Role/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Update role successful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        onlineshop.stopLoading();
                        dataTable.ajax.reload();
                    },
                    error: function () {
                        onlineshop.notify('Has an error', 'error');
                        onlineshop.stopLoading();
                    }
                });
                return false;
            }

        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            onlineshop.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Role/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Delete successful', 'success');
                        onlineshop.stopLoading();
                        dataTable.ajax.reload();
                    },
                    error: function (status) {
                        onlineshop.notify('Has an error in deleting progress', 'error');
                        onlineshop.stopLoading();
                    }
                });
            });
        });


    };

    function resetFormMaintainance() {
        $('#hidId').val('');
        $('#txtName').val('');
        $('#txtDescription').val('');
    }

    function loadData() {
        dataTable = $('#zero_config').DataTable({
            processing: true, // for show progress bar
            serverSide: false, // for process server side    
            ajax: {
                type: 'GET',
                url: '/admin/role/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [{
                targets: [0, 1, 2],
                autoWidth: true
            }],
            columnDefs: [{
                targets: [0, 1, 2],
                sortable: false
            }],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return '<button data-id="' + data + '" class="btn btn-danger btn-delete"><i class="fas fa-trash"></i></button> <button data-id="' + data + '" class="btn btn-success btn-edit"><i class="fas fa-pencil-alt"></i></button>';
                    }
                },
                { data: "Name" },
                { data: "Description" }
            ]
        });
    }
} 