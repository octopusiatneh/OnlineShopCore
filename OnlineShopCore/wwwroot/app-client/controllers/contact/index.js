var ContactController = function () {
    this.initialize = function () {
        registerEvent();
    }
    function registerEvent() {
        initMap();
    }

    function initMap() {
        //var uluru = { lat: parseFloat($('#hidLat').val()), lng: parseFloat($('#hidLng').val()) };
        //var map = new google.maps.Map(document.getElementById('map'), {
        //    zoom: 17,
        //    center: uluru
        //});

        //var contentString = $('#hidAddress').val();

        //var infowindow = new google.maps.InfoWindow({
        //    content: contentString
        //});

        //var marker = new google.maps.Marker({
        //    position: uluru,
        //    map: map,
        //    title: $('#hidName').val()
        //});
        //infowindow.open(map, marker);

      
        map = new OpenLayers.Map("mapdiv");
        
        map.addLayer(new OpenLayers.Layer.OSM());

        var lonLat = new OpenLayers.LonLat(parseFloat($('#hidLng').val()), parseFloat($('#hidLat').val()))
            .transform(
                new OpenLayers.Projection("EPSG:4326"), // transform from WGS 1984
                map.getProjectionObject() // to Spherical Mercator Projection
            );

        var zoom = 18;

        var markers = new OpenLayers.Layer.Markers("Markers");
        map.addLayer(markers);

        markers.addMarker(new OpenLayers.Marker(lonLat));
        map.addControl(new OpenLayers.Control.Navigation({ 'disableZoomWheel': true }));
        map.setCenter(lonLat, zoom);

      

    }
}