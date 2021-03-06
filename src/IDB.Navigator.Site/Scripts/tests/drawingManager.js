﻿var map = new google.maps.Map(document.getElementById("map"), {
    zoom: 7,
    center: new google.maps.LatLng(44.331216, 23.927536),
    mapTypeId: google.maps.MapTypeId.ROADMAP
});


var drawingManager = new google.maps.drawing.DrawingManager({
    drawingMode: google.maps.drawing.OverlayType.MARKER,
    drawingControl: true,
    drawingControlOptions: {
        position: google.maps.ControlPosition.TOP_CENTER,
        drawingModes: [
            google.maps.drawing.OverlayType.MARKER,
            google.maps.drawing.OverlayType.CIRCLE,
            google.maps.drawing.OverlayType.POLYGON,
            google.maps.drawing.OverlayType.POLYLINE,
            google.maps.drawing.OverlayType.RECTANGLE
        ]
    },
    markerOptions: {
        icon: 'images/beachflag.png'
    },
    circleOptions: {
        fillColor: '#ffff00',
        fillOpacity: 1,
        strokeWeight: 5,
        clickable: false,
        editable: true,
        zIndex: 1
    }
});
drawingManager.setMap(map);