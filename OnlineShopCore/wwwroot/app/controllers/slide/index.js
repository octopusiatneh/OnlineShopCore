var db;
var slideController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();   
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true }
            }
        });
        //todo: binding events to controls  
        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
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
                    onlineshop.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    onlineshop.notify('There was error uploading files!', 'error');
                }
            });
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Slide/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    onlineshop.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    $('#txtImage').val(data.Image);
                    $('#txtDescM').val(data.Description);
                    $('#txtContentM').val(data.Content);                
                    $('#ckStatusM').prop('checked', data.Status == 1);
                    $('#modal-add-edit').modal('show');
                    onlineshop.stopLoading();

                },
                error: function (status) {
                    onlineshop.notify('Error while processing', 'error');
                    onlineshop.stopLoading();
                }
            });
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            onlineshop.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Slide/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Delete successfully', 'success');
                        onlineshop.stopLoading();
                        loadData();
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
                var description = $('#txtDescM').val();          
                var image = $('#txtImage').val();

                var content = $('#txtContentM').val();
                var status = $('#ckStatusM').prop('checked') == true ? 1 : 0;          
                $.ajax({
                    type: "POST",
                    url: "/Admin/Slide/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,              
                        Image: image,              
                        Description: description,
                        Content: content,
                        Status: status
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Update slide successfully', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        onlineshop.stopLoading();
                        db.ajax.reload();
                    },
                    error: function () {
                        onlineshop.notify('Has an error in saving slide progress', 'error');
                        onlineshop.stopLoading();
                    }
                });
                return false;
            }

        });
    }

    function registerControls() {
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

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');  

        $('#txtDescM').val('');

        $('#txtImage').val('');

        $('#txtContentM').val('');
        $('#ckStatusM').prop('checked', true);
    }

    function loadData() {
        db = $('#zero_config').DataTable({
            processing: true, // for show progress bar
            serverSide: false, // for process server side
            destroy: true,        
            ajax: {
                type: 'GET',
                url: '/admin/slide/GetAll',
                dataSrc: '',
                dataType: 'json'
            },
            columnDefs: [{
                targets: [0, 1, 2],
                autoWidth: true
            }],
            columnDefs: [{
                targets: [0,3,2],
                sortable: false
            }],
            columns: [
                {
                    data: "Id", render: function (data, type, row) {
                        return '<button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Delete" data-original-title="Delete" data-id="' + data + '" class="btn btn-danger btn-delete"><i class="fas fa-trash"></i></button> <button style="width: 40px" data-toggle="tooltip" data-placement="top" title="Edit" data-original-title="Edit" data-id="' + data + '" class="btn btn-success btn-edit"><i class="fas fa-pencil-alt"></i></button>';
                    }
                },
                { data: "Name" },         
                {
                    data: "Image", render: function (data, type, row) {
                        return data == null ? '<img src="~/admin-side/assets/images/No-image-found.jpg" width=35' : '<img src="' + data + '" width=35 />'
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