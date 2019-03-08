var UserController = function () {
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
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
            }
        });

        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            initRoleList();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/User/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtFullName').val(data.FullName);
                    $('#txtUserName').val(data.UserName);
                    $('#txtEmail').val(data.Email);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#ckStatus').prop('checked', data.Status === 1);

                    initRoleList(data.Roles);

                    disableFieldEdit(true);
                    $('#modal-add-edit').modal('show');
                    onlineshop.stopLoading();

                },
                error: function () {
                    onlineshop.notify('Có lỗi xảy ra', 'error');
                    onlineshop.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();

                var id = $('#hidId').val();
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                var email = $('#txtEmail').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var roles = [];
                $.each($('input[name="ckRoles"]'), function (i, item) {
                    if ($(item).prop('checked') === true)
                        roles.push($(item).prop('value'));
                });
                var status = $('#ckStatus').prop('checked') === true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/User/SaveEntity",
                    data: {
                        Id: id,
                        FullName: fullName,
                        UserName: userName,
                        Password: password,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Status: status,
                        Roles: roles
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function () {
                        onlineshop.notify('Save user succesful', 'success');
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
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            onlineshop.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/User/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function () {
                        onlineshop.notify('Delete successful', 'success');
                        onlineshop.stopLoading();
                        dataTable.ajax.reload();
                    },
                    error: function () {
                        onlineshop.notify('Has an error', 'error');
                        onlineshop.stopLoading();
                    }
                });
            });
        });

    };


    function disableFieldEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);

    }
    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidId').val('');
        initRoleList();
        $('#txtFullName').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('input[name="ckRoles"]').removeAttr('checked');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#ckStatus').prop('checked', true);

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "/Admin/Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render += Mustache.render(template,
                        {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            }
        });
    }

    function loadData() {
        dataTable = $('#zero_config_1').DataTable({
            // the indexs of the column that want to have the dropdown filter
            //initComplete: function () {
            //    this.api().columns([2]).every(function () {
            //        var column = this;
            //        var select = $('<select><option value="">--Category filter--</option></select>')
            //            .appendTo($(column.header()).empty())
            //            .on('change', function () {
            //                var val = $.fn.dataTable.util.escapeRegex(
            //                    $(this).val()
            //                );

            //                column
            //                    .search(val ? '^' + val + '$' : '', true, false)
            //                    .draw();
            //            });

            //        column.data().unique().sort().each(function (d, j) {
            //            select.append('<option value="' + d + '">' + d + '</option>')
            //        });
            //    });
            //},
            processing: true, // for show progress bar
            serverSide: false, // for process server side
            //order: [[4, "desc"]], // for setting the default sort column
            ajax: {
                type: 'GET',
                url: '/admin/user/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [{
                targets: [0, 1, 2, 3, 4, 5],
                autoWidth: true
            }],
            columnDefs: [{
                targets: [0, 2, 4],
                sortable: false
            }],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return '<button data-id="' + data + '" class="btn btn-danger btn-delete"><i class="fas fa-trash"></i></button> <button data-id="' + data + '" class="btn btn-success btn-edit"><i class="fas fa-pencil-alt"></i></button>';
                    }
                },
                { data: "UserName" },
                { data: "FullName" },             
                {
                    data: "Avatar", render: function (data, type, row) {
                        return data == null ? '<img src="~/admin-side/assets/images/No-image-found.jpg" width=25' : '<img src="' + data + '" width=25 />'
                    }
                },
                {
                    data: "DateCreated", render: function (data, type, row) {
                        return data = onlineshop.dateTimeFormatJson(data)
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