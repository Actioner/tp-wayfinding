var map = new google.maps.Map(document.getElementById("map"), {
    zoom: 7,
    center: new google.maps.LatLng(44.331216, 23.927536),
    mapTypeId: google.maps.MapTypeId.ROADMAP
});

var polyOptions = {
    strokeColor: '#000000',
    strokeOpacity: 1.0,
    strokeWeight: 3
};

var currentMarkerId = 0;
var graph = new Graph(map);

google.maps.Marker.prototype.toggle = function(){
    this.selected = !this.selected;
    if (this.selected)
        this.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-waypoint-blue.png')
    else
        this.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-poi.png')
};
google.maps.Marker.prototype.equals = function (other) {
    return this.id === other.id;
};

var addMarker = function (event) {
    var marker = new google.maps.Marker({
        id: ++currentMarkerId,
        selected: false,
        position: event.latLng,
        title: '#' + currentMarkerId,
        draggable: true,
        map: map
    });
    var vertex = new Vertex(marker);
    graph.addVertex(vertex);

    var selectedVertex = graph.getSelected();
    if (selectedVertex) {
        graph.addEdge(selectedVertex, vertex);
    }

    google.maps.event.addListener(marker, 'click', function () {
        var selectedVertex = graph.getSelected();
        if (!selectedVertex) {
            marker.toggle();
            return;
        }
        if (selectedVertex.marker.equals(marker)) {
            marker.toggle();
        } else {
            var ver = graph.findVertexByMarker(marker);
            graph.addEdge(selectedVertex, ver);
            ver.toggle();
        }
        graph.toString();
    });

    google.maps.event.addListener(marker, 'rightclick', function () {
        var ver = graph.findVertexByMarker(marker);
        graph.removeVertex(ver);
        graph.toString();
    });

    google.maps.event.addListener(marker, 'drag', function (evt) {
        var ver = graph.findVertexByMarker(marker);
        for (var i = 0; i < ver.edges.length; i++) {
            var edge = ver.edges[i];
            if (edge.start.marker.equals(marker)) {
                edge.poly.getPath().setAt(0, marker.getPosition());
            }
            else {
                edge.poly.getPath().setAt(1, marker.getPosition());
            }
        }
    });

    graph.toString();
}

// Add a listener for the click event
google.maps.event.addListener(map, 'click', addMarker);