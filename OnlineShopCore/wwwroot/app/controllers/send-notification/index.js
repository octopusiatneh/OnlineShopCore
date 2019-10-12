var sendNotificationController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtContentM: { required: true }
            }
        });
        //todo: binding events to controls  
        

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
              
                var content = $('#txtContentM').val();      
                $.ajax({
                    type: "POST",
                    url: "/Admin/SendNotification/Send",
                    data: {
                       Content: content
                    },
                    dataType: "json",
                    beforeSend: function () {
                        onlineshop.startLoading();
                    },
                    success: function (response) {
                        onlineshop.notify('Gửi thông báo thành công', 'success');
                        resetFormMaintainance();

                        onlineshop.stopLoading();
                    },
                    error: function () {
                        onlineshop.notify('Gửi thông báo thành công', 'success');
                        onlineshop.stopLoading();

                        resetFormMaintainance();
                    }
                });
                return false;
            }

        });
    }


    function resetFormMaintainance() {     
        $('#txtContentM').val('');
    }

}