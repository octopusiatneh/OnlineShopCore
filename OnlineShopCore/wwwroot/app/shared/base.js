var BaseController = function () {

    this.initialize = function () {
        loadAnnouncement();
    }
};

function loadAnnouncement() {
    $.ajax({
        type: "GET",
        url: "/admin/announcement/GettAllPaging",
        data: {
            page: onlineshop.configs.pageIndex,
            pageSize: onlineshop.configs.pageSize
        },
        dataType: "json",
        beforeSend: function () {
            onlineshop.startLoading();
        },
        success: function (response) {
            var template = $('#announcement-template').html();
            var render = "";
            if (response.RowCount > 0) {
                $("#totalAnnouncement").text(response.RowCount);
                $.each(response.Results, function (i, item) {
                    render += Mustache.render(template, {
                        Content: item.Content,
                        DateCreated: moment(item.DateCreated).fromNow(),
                    });
                });           
                if (render != undefined) {
                    $('#announcementList').html(render);
                }
            }
            else {
                $('#announcementArea').hide();
                $('#annoncementList').html('');
            }
            onlineshop.stopLoading();
        },
    });
};