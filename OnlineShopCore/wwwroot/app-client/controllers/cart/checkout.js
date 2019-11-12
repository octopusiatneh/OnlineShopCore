var toDistrictID = 0;
var subTotalAmount = 0;
var total = 0;
getCartSubTotal();

$('#comboProvince').on("change", function () {
    var provinceName = $("#comboProvince option:selected").val();
    getcbDistrict(provinceName);
    toDistrictID = $("#comboDistrict option:selected").attr('value');
    getcbWard();
    calculateDeliveryDate();
});

$('#comboDistrict').on("change", function () {
    toDistrictID = $("#comboDistrict option:selected").attr('value');
    getcbWard();
    calculateDeliveryDate();
    calculateDeliveryDate();
});

$('#comboWard').on('change', function () {
    calculateShippingFee();
});

$('#comboShippingMethod').on('change', function () {
    if (toDistrictID != 0) {
        console.log('serviceID selected:' + $("#comboShippingMethod option:selected").attr('value'))
        calculateShippingFee();
        calculateDeliveryDate();
    }     
});

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
        url: "/Manage/GetWards",
        data: {
            districtID: toDistrictID
        },
        success: function (response) {
            var cbWard = "";
            for (let i in response) {
                cbWard += '<option value=' + response[i].WardCode + ">" + response[i].WardName + '</option>';
            }
            document.getElementById('comboWard').innerHTML = cbWard;
        }
    })

    calculateShippingFee();
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
        }
    })
}

function calculateDeliveryDate() {
    $.ajax({
        url: "/Manage/CalculateDeliveryDate",
        data: {
            toDistrictID: toDistrictID,
        },
        success: function (response) {
            var s = $("#comboShippingMethod option:selected").attr('value');
            response.forEach(function (element) {
                console.log(element.ServiceID)
                if (element.ServiceID == s) {
                    var formattedDate = moment(element.ExpectedDeliveryTime).format("DD/MM/YYYY HH:MM");
                    console.log('Yup its inside the if')
                    console.log(formattedDate)
                    $('#lblArriveTime').text(formattedDate);
                }
            })
        }
    })
}