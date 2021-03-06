﻿function Gmap() { }
var geocoder = new google.maps.Geocoder();
var map;
var markersArray = [];
var tipo_comercio = '';
var bounds = new google.maps.LatLngBounds();


Gmap.LoadMap = function (latitude, longitude, onMapLoaded) {

    var latlng = new google.maps.LatLng(20.10106, -98.759131);
    if (latitude != null && longitude != null) {
        latlng = new google.maps.LatLng(latitude, longitude);
        
    }
    var myOptions = {
        zoom: 8,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("map_canvas"),
        myOptions);
}

Gmap.FindAddressOnMap = function (where) {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }

    geocoder.geocode({ 'address': where }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            map.setZoom(15);
                $("#Latitude").val(results[0].geometry.location.lat());
                $("#Longitude").val(results[0].geometry.location.lng());
            var marker = new google.maps.Marker({
                map: map,
                draggable: true,
                animation: google.maps.Animation.DROP,
                position: results[0].geometry.location
            });
            markersArray.push(marker);
            google.maps.event.addListener(marker, 'dragend', function (event) {
                $("#Latitude").val(marker.position.lat().toFixed(7));
                $("#Longitude").val(marker.position.lng().toFixed(7));
            });

        } else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
}

Gmap.FindCPonMap = function (where) {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }

    geocoder.geocode({ 'address': where }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            map.setZoom(10);
            //$("#Latitud").val(Globalize.format(results[0].geometry.location.lat(), "n14"));
            //$("#Longitud").val(Globalize.format(results[0].geometry.location.lng(), "n14"));
            var marker = new google.maps.Marker({
                map: map,
                draggable: false,
                animation: google.maps.Animation.DROP,
                position: results[0].geometry.location
            });
            markersArray.push(marker);
//            google.maps.event.addListener(marker, 'dragend', function (event) {
//                $("#Latitud").val(Globalize.format(this.getPosition().lat(), "n14"))
//                $("#Longitud").val(Globalize.format(this.getPosition().lng(), "n14"))
//            });

        } else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
}

Gmap.ClearMap = function () {
    if (markersArray) {
        alert('limpia');
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
    }
}


Gmap.LoadPin = function (latlong, drag) {
    map.setZoom(15);
    var marker = new google.maps.Marker({
        map: map,
        draggable: drag,
        animation: google.maps.Animation.DROP,
        position: latlong
    });
    markersArray.push(marker);
    google.maps.event.addListener(marker, 'dragend', function (event) {
        $("#Latitude").val(marker.position.lat().toFixed(7));
        $("#Longitude").val(marker.position.lng().toFixed(7));
    });
}

Gmap.FindAddressGivenLocation = function (where, tipo) {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
        }
        markersArray.length = 0;
        bounds = new google.maps.LatLngBounds();
    }
    geocoder.geocode({ 'address': where }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            map.setZoom(15);
            var lat = results[0].geometry.location.lat();
            var long = results[0].geometry.location.lng();

            tipo_comercio = tipo;
            if (tipo_comercio == '' || tipo_comercio == null)
                tipo_comercio = 0;

            Gmap._callbackUpdateMapAddress(lat, long, tipo_comercio)
        } else {
            alert("Geocode was not successful for the following reason: " + status);
            var lat = '0';
            var long = '0';
        }
    });
}


Gmap._callbackUpdateMapAddress = function (lat, long, tipo) {
    var infoWindow = new google.maps.InfoWindow({ content: "Cargando..." ,
        maxWidth: 350
    });
    var urlb = "/UserInformation/SearchByLocation/" + lat + "/" + long + "/" + tipo;
    var callbacks = 0;
    var numAllCallbacks = 0;

    $.ajax({
        type: "POST",
        url: urlb,
        datatype: "json",
        success: function (data) {
            numAllCallbacks = data.length;
            for (var i = 0; i < data.length; i++) {
                callbacks++;
                
                var point = new google.maps.LatLng(data[i].latitud, data[i].longitud);

                bounds.extend(point);

                var contentString = '<div id="content">' +
                '<h2 id="firstHeading" class="firstHeading">' + data[i].nombre + '</h2>' +
                '<div id="bodyContent">' +
                '<p>' + data[i].direccion + '</p>' +
                '<p>Pagina Web:<a href="/Comercio/Details/' + data[i].url + '">' +
                 data[i].nombre + '</a>' +
                '</div>' +
                '</div>';

                var marker = new google.maps.Marker({
                    position: point,
                    map: map,
                    html: contentString
                });

                markersArray.push(marker);

                google.maps.event.addListener(marker, "click", function () {
                    infoWindow.setContent(this.html);
                    infoWindow.open(map, this);
                });

                if (callbacks == numAllCallbacks) {
                    Gmap.Callbounds();
                }
            }
        }
    });
}


Gmap.Callbounds = function () {
    map.fitBounds(bounds);
}


Gmap.getCurrentLocationByLatLong = function (latitude, longitude) {
    var lug = "";
    var latlng = new google.maps.LatLng(-34.397, 150.644);
    if (latitude != null && longitude != null) {
        latlng = new google.maps.LatLng(latitude, longitude);
    }

    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                for (var j = 0; j < results[1].address_components.length; j++) {
                    for (var k = 0; k < results[1].address_components[j].types.length; k++) {
                        if (results[1].address_components[j].types[k] == "country") {
                            lug = lug + " " + results[1].address_components[j].long_name;
                        }
                        if (results[1].address_components[j].types[k] == "locality") {
                            lug = lug + " " + results[1].address_components[j].long_name;
                        }
                        if (results[1].address_components[j].types[k] == "postal_code") {
                            lug = lug + " " + results[1].address_components[j].long_name;
                        }
                    }
                    $('#Lugar').val(lug);
                }

            }
        }
    });
}

    Gmap.getCurrentLocationByIpAddress = function () {
        var requestUrl = "http://api.ipinfodb.com/v3/ip-city/?format=json&callback=?&key=9742e62afd81cdbc5d814f735aa5e437ddb6b0b7f34d6c6f424be6e4320106f8";

        $.getJSON(requestUrl,
           function (data) {
               if (data.RegionName != '') {
                   $('#Location').val(data.regionName + ', ' + data.countryName);
                   var country = data.countryName;
                   // alert(country);
               }
           });
    }