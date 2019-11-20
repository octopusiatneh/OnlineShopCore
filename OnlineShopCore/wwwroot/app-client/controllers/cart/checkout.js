var toDistrictID = 0;
var subTotalAmount = 0;
var total = 0;
var serviceValue = 0;
var provinceValue = 0;
getCartSubTotal();

$('#comboProvince').on("change", function () {
    provinceValue = $("#comboProvince option:selected").val();
    if (provinceValue == 0) {
        $('#comboDistrict').empty().trigger('change');
        $("#comboDistrict").append("<option value='0' selected>Chọn quận/huyện...</option>").trigger('change');
        $('#comboWard').empty().trigger('change');
        $("#comboWard").append("<option value='0' selected>Chọn phường/xã...</option>").trigger('change');
    }
    if (provinceValue != 0) {
        var provinceName = $("#comboProvince option:selected").val();
        getcbDistrict(provinceName);
        toDistrictID = $("#comboDistrict option:selected").attr('value');
        getcbWard();
        if (serviceValue != 0) {
            calculateShippingFee();
            calculateDeliveryDate();
        }   
    }
});

$('#comboDistrict').on("change", function () {
    toDistrictID = $("#comboDistrict option:selected").attr('value');
    if (serviceValue != 0) {
        getcbWard();
        calculateShippingFee();
        calculateDeliveryDate();
    }
});

$('#comboShippingMethod').on('change', function () {
    serviceValue = $("#comboShippingMethod option:selected").val();
    if (toDistrictID != 0 && serviceValue != 0) {
        calculateShippingFee();
        calculateDeliveryDate();
    }
    else {
        $('#shipping-fee-container').text('Không có phương thức vận chuyển có sẵn. Vui lòng kiểm tra lại địa chỉ của bạn hoặc liên hệ với chúng tôi nếu bạn cần bất kỳ sự giúp đỡ nào.');
        $('#lblArriveTime').text('...');
        $('#total-container').text('...');
    }
});

$('#btn-proceed').click(function () {
    document.getElementById('form-container').style.display = 'block';
})

function getcbDistrict(provinceName) {
    var arrDistrictName = [];
    for (let i in data) {
        if (data[i].ProvinceName === provinceName) {
            arrDistrictName.push({ DistrictID: data[i].DistrictID, DistrictName: data[i].DistrictName })
        }

        var cbDistrict = "";
        for (let i in arrDistrictName) {
            cbDistrict += '<option value=' + arrDistrictName[i].DistrictID + ">" + arrDistrictName[i].DistrictName + '</option>';
        }
        document.getElementById('comboDistrict').innerHTML = cbDistrict;
    }
}

function getcbWard() {
    $.ajax({
        type: "POST",
        url: "/Manage/GetWards",
        data: {
            districtID: toDistrictID
        },
        beforeSend: function () {
            // setting a timeout  
            document.getElementById('form-container').style.display = 'block';
        },
        success: function (response) {
            var cbWard = "";
            for (let i in response) {
                cbWard += '<option value=' + response[i].WardCode + ">" + response[i].WardName + '</option>';
            }
            document.getElementById('comboWard').innerHTML = cbWard;
            document.getElementById('form-container').style.display = 'none';
        }
    })
}

function getCartSubTotal() {
    $.ajax({
        url: '/Cart/GetCart',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $.each(response, function (i, item) {
                subTotalAmount += item.Price * item.Quantity;
            });
            $('#lblSubTotalAmount').text(onlineshop.formatNumber(subTotalAmount, 0) + " VNĐ");
        }
    });
    return false;
}

function calculateShippingFee() {
    $.ajax({
        type: "POST",
        url: "/Manage/CalculateFee",
        data: {
            weight: 1000,
            toDistrictID: toDistrictID,
            serviceID: $("#comboShippingMethod option:selected").attr('value')
        },
        success: function (response) {
            document.getElementById('shipping-fee-container').innerHTML = (onlineshop.formatNumber(response, 0)) + " VNĐ";
            total = 0;
            total += subTotalAmount + response;
            document.getElementById('total-container').innerHTML = (onlineshop.formatNumber(total, 0)) + " VNĐ";
            $('#hid-total').val(total);
        },
        error: function () {
            document.getElementById('form-container').style.display = 'none';
            $('#shipping-fee-container').text('Không có phương thức vận chuyển có sẵn. Vui lòng kiểm tra lại địa chỉ của bạn hoặc liên hệ với chúng tôi nếu bạn cần bất kỳ sự giúp đỡ nào.');
            $('#lblArriveTime').text('...');
            $('#total-container').text('...');
            swal("Lỗi!", "Hình thức giao hàng hiện tại không hợp lệ, vui lòng chọn hình thức giao hàng khác.", "error");
            $('#comboShippingMethod').val('0').trigger('change');
            
        }
    })
}

function calculateDeliveryDate() {
    $.ajax({
        type: "POST",
        url: "/Manage/CalculateDeliveryDate",
        data: {
            toDistrictID: toDistrictID,
        },
        beforeSend: function () {
            // setting a timeout  
            document.getElementById('form-container').style.display = 'block';
        },
        success: function (response) {
            var s = $("#comboShippingMethod option:selected").attr('value');
            response.forEach(function (element) {

                if (element.ServiceID == s) {
                    var formattedDate = moment(element.ExpectedDeliveryTime).format("DD/MM/YYYY HH:MM");
                    $('#lblArriveTime').text(formattedDate);
                }
            })
            document.getElementById('form-container').style.display = 'none';
        }
    })
}