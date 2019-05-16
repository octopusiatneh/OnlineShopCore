var ContactController = function () {
    this.initialize = function () {
        registerEvent();
    }
    function registerEvent() {
        initMap();
    }

    function initMap() {
        var uluru = { lat: 10.7758883, lng: 106.6673327 };
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 17,
            center: uluru
        });

        var contentString = 'Coza Store';

        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });

        var marker = new google.maps.Marker({
            position: uluru,
            map: map,
            title: $('#hidName').val()
        });
        infowindow.open(map, marker);

      

      

    }
}