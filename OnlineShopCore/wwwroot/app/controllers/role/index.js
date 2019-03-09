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

        $('body').on('click', '.btn-grant', function () {
            $('#hidRoleId').val($(this).data('id'));
            $.when(loadFunctionList())
                .done(fillPermission($('#hidRoleId').val()));
            $('#modal-grantpermission').modal('show');
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

        $("#btnSavePermission").off('click').on('click', function () {
            var listPermmission = [];
            $.each($('#tblFunction tbody tr'), function (i, item) {
                listPermmission.push({
                    RoleId: $('#hidRoleId').val(),
                    FunctionId: $(item).data('id'),
                    CanRead: $(item).find('.ckView').first().prop('checked'),
                    CanCreate: $(item).find('.ckAdd').first().prop('checked'),
                    CanUpdate: $(item).find('.ckEdit').first().prop('checked'),
                    CanDelete: $(item).find('.ckDelete').first().prop('checked'),
                });
            });
            $.ajax({
                type: "POST",
                url: "/admin/role/SavePermission",
                data: {
                    listPermmission: listPermmission,
                    roleId: $('#hidRoleId').val()
                },
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    onlineshop.notify('Save permission successful', 'success');
                    $('#modal-grantpermission').modal('hide');
                    onlineshop.stopLoading();
                },
                error: function () {
                    onlineshop.notify('Has an error in save permission progress', 'error');
                    onlineshop.stopLoading();
                }
            });
        });
    };

    function loadFunctionList(callback) {
        var strUrl = "/admin/Function/GetAll";
        return $.ajax({
            type: "GET",
            url: strUrl,
            dataType: "json",
            beforeSend: function () {
                onlineshop.startLoading();
            },
            success: function (response) {
                var template = $('#result-data-function').html();
                var render = "";
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Name: item.Name,
                        treegridparent: item.ParentId != null ? "treegrid-parent-" + item.ParentId : "",
                        Id: item.Id,
                        AllowCreate: item.AllowCreate ? "checked" : "",
                        AllowEdit: item.AllowEdit ? "checked" : "",
                        AllowView: item.AllowView ? "checked" : "",
                        AllowDelete: item.AllowDelete ? "checked" : "",
                        Status: onlineshop.getStatus(item.Status),
                    });
                });
                if (render != undefined) {
                    $('#lst-data-function').html(render);
                }
                $('.tree').treegrid();

                $('#ckCheckAllView').on('click', function () {
                    $('.ckView').prop('checked', $(this).prop('checked'));
                });

                $('#ckCheckAllCreate').on('click', function () {
                    $('.ckAdd').prop('checked', $(this).prop('checked'));
                });
                $('#ckCheckAllEdit').on('click', function () {
                    $('.ckEdit').prop('checked', $(this).prop('checked'));
                });
                $('#ckCheckAllDelete').on('click', function () {
                    $('.ckDelete').prop('checked', $(this).prop('checked'));
                });

                $('.ckView').on('click', function () {
                    if ($('.ckView:checked').length == response.length) {
                        $('#ckCheckAllView').prop('checked', true);
                    } else {
                        $('#ckCheckAllView').prop('checked', false);
                    }
                });
                $('.ckAdd').on('click', function () {
                    if ($('.ckAdd:checked').length == response.length) {
                        $('#ckCheckAllCreate').prop('checked', true);
                    } else {
                        $('#ckCheckAllCreate').prop('checked', false);
                    }
                });
                $('.ckEdit').on('click', function () {
                    if ($('.ckEdit:checked').length == response.length) {
                        $('#ckCheckAllEdit').prop('checked', true);
                    } else {
                        $('#ckCheckAllEdit').prop('checked', false);
                    }
                });
                $('.ckDelete').on('click', function () {
                    if ($('.ckDelete:checked').length == response.length) {
                        $('#ckCheckAllDelete').prop('checked', true);
                    } else {
                        $('#ckCheckAllDelete').prop('checked', false);
                    }
                });
                if (callback != undefined) {
                    callback();
                }
                onlineshop.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }

    function fillPermission(roleId) {
        var strUrl = "/Admin/Role/ListAllFunction";
        return $.ajax({
            type: "POST",
            url: strUrl,
            data: {
                roleId: roleId
            },
            dataType: "json",
            beforeSend: function () {
                onlineshop.stopLoading();
            },
            success: function (response) {
                var litsPermission = response;
                $.each($('#tblFunction tbody tr'), function (i, item) {
                    $.each(litsPermission, function (j, jitem) {
                        if (jitem.FunctionId == $(item).data('id')) {
                            $(item).find('.ckView').first().prop('checked', jitem.CanRead);
                            $(item).find('.ckAdd').first().prop('checked', jitem.CanCreate);
                            $(item).find('.ckEdit').first().prop('checked', jitem.CanUpdate);
                            $(item).find('.ckDelete').first().prop('checked', jitem.CanDelete);
                        }
                    });
                });

                if ($('.ckView:checked').length == $('#tblFunction tbody tr .ckView').length) {
                    $('#ckCheckAllView').prop('checked', true);
                } else {
                    $('#ckCheckAllView').prop('checked', false);
                }
                if ($('.ckAdd:checked').length == $('#tblFunction tbody tr .ckAdd').length) {
                    $('#ckCheckAllCreate').prop('checked', true);
                } else {
                    $('#ckCheckAllCreate').prop('checked', false);
                }
                if ($('.ckEdit:checked').length == $('#tblFunction tbody tr .ckEdit').length) {
                    $('#ckCheckAllEdit').prop('checked', true);
                } else {
                    $('#ckCheckAllEdit').prop('checked', false);
                }
                if ($('.ckDelete:checked').length == $('#tblFunction tbody tr .ckDelete').length) {
                    $('#ckCheckAllDelete').prop('checked', true);
                } else {
                    $('#ckCheckAllDelete').prop('checked', false);
                }
                onlineshop.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    }

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
                targets: [0,1,2],
                autowidth: false
            }],
            columnDefs: [{
                targets: [0, 1, 2],
                sortable: false
            }],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return '<button data-id="' + data + '" class="btn btn-primary btn-grant"><i class="fas fa-allergies"></i></button> <button data-id="' + data + '" class="btn btn-danger btn-delete"><i class="fas fa-trash"></i></button> <button data-id="' + data + '" class="btn btn-success btn-edit"><i class="fas fa-pencil-alt"></i></button>';
                    }, width:'15%'
                },
                { data: "Name" },
                { data: "Description" }
            ]
        });
    }
};


