function GraphData() {
    var self = this;

    self.nodes = ko.observableArray([]);
    self.connections = ko.observableArray([]);
}

Node.prototype.equals = function (other) {
    return this.id() === other.id();
};

Connection.prototype.equals = function (other) {
    return this.id() === other.id();
};

function NodeManager() {
    var self = this;
    self.form = $('#nodeTmpl');

    self.isValid = function () {
        return self.form.valid();
    };

    self.save = function (node, errors, callback) {
        if (!self.form.valid()) {
            return;
        }
        var isUpdate = node.id() > 0;
        var url = '/api/node/' + (isUpdate ? node.id() : '');
        var method = isUpdate > 0 ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            data: ko.toJSON(node),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (response) {
            ko.mapping.fromJS(response, {}, node);
            console.log(node.id());
            if (window.isFunction(callback)) {
                callback(node);
            }

            if (isUpdate) {
                window.notifyUpdate();
            } else {
                window.notifyCreate();
            }
        }).fail(function (jqXHR) {
            var serializedErrors = extractErrors(jqXHR);
            for (var error in serializedErrors) {
                errors[error](serializedErrors[error]);
                errors[error].valueHasMutated();
            }
        });
    };

    self.delete = function (node) {
        var deleteUrl = '/api/node/' + node.id();

        $.ajax({
            url: deleteUrl,
            type: 'DELETE'
        }).done(function () {
            window.notifyDelete();
        });
    };
};

function ConnectionManager() {
    var self = this;
    self.form = $('#connectionTmpl');

    self.isValid = function () {
        return self.form.valid();
    };

    self.save = function (connection, errors) {
        if (!self.form.valid()) {
            return;
        }
        var isUpdate = connection.id() > 0;
        var url = '/api/connection/' + (isUpdate ? connection.id() : '');
        var method = isUpdate > 0 ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            data: ko.toJSON(connection),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (response) {
            ko.mapping.fromJS(response, {}, connection);
            console.log(connection.id());
            if (isUpdate) {
                window.notifyUpdate();
            } else {
                ko.mapping.fromJS(response, {}, connection);
                window.notifyCreate();
            }
        }).fail(function (jqXHR) {
            var serializedErrors = extractErrors(jqXHR);
            for (var error in serializedErrors) {
                errors[error](serializedErrors[error]);
                errors[error].valueHasMutated();
            }
        });
    };

    self.delete = function (connection) {
        var deleteUrl = '/api/connection/' + connection.id();

        $.ajax({
            url: deleteUrl,
            type: 'DELETE'
        }).done(function () {
            window.notifyDelete();
        });
    };
};

function NavigationMap() {
    var self = this;
    self.floorMapId = ko.observable(0);
    self.selectedConnection = ko.observable(new Connection());
    self.nodeErrors = new Node();
    self.connectionErrors = new Connection();
    self.nodeManager = new NodeManager();
    self.connectionManager = new ConnectionManager();

    self.polyOptions = function (show) {
        return {
            strokeColor: show ? '#000000' : '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 3
        };
    };

    self.connectionTmpl_ = $("#connectionTmpl");
    self.overlay_ = null;
    self.overlayCreateListener_ = null;

    self.map_ = new google.maps.Map(document.getElementById('map'), {
        mapTypeId: 'roadmap',
        center: new google.maps.LatLng(0, 0),
        zoom: 3,
        panControl: false,
        zoomControl: true,
        scaleControl: false,
        streetViewControl: false,
        overviewMapControl: false,
        mapTypeControl: true
    });
    self.connectionInfoWindow_ = new google.maps.InfoWindow({
        content: '<div id="connectionPlaceholder" style="width:279px; height:136px"></div>'
    });
   
    self.openConnectionInfoReady = function () {
        var placeholder = $("#connectionPlaceholder");
        self.connectionTmpl_.appendTo(placeholder);
    };

    self.closeConnectionInfoReady = function () {
        self.connectionTmpl_.appendTo("#connectionTmplContainer");
    };

    google.maps.event.addListener(self.connectionInfoWindow_, 'domready', self.openConnectionInfoReady);
    google.maps.event.addListener(self.connectionInfoWindow_, 'closeclick', self.closeConnectionInfoReady);

    self.graph = new Graph(self.map_);

    self.saveConnection = function (connection) {
        self.connectionManager.save(connection, self.connectionErrors);

        self.closeConnectionInfoReady();
        self.connectionInfoWindow_.close();
    };

    self.cancelConnection = function (connection) {
        self.closeConnectionInfoReady();
        self.connectionInfoWindow_.close();

        self.selectedConnection(new Connection());
    };

    self.nodeOnClick_ = function (vertex) {
        var selectedVertex = self.graph.getSelected();
        if (selectedVertex && !selectedVertex.equals(vertex)) {
            var poly = new google.maps.Polyline(self.polyOptions(true));
            var connection = new Connection();
            connection.id(0);
            connection.nodeAId(selectedVertex.node.id());
            connection.nodeBId(vertex.node.id());
            connection.show(true);
            connection.floorConnection(false);

            var added = self.addConnection(connection);
            if (added) {
                self.saveConnection(connection);
            }
        }
        self.graph.toggleVertex(vertex);
    };

    self.nodeOnRightClick_ = function (vertex) {
        self.graph.removeVertex(vertex);
        self.nodeManager.delete(vertex.node);
    };

    self.connectionOnClick_ = function (edge) {
        self.selectedConnection(edge.connection);
        
     

        var inBetween = google.maps.geometry.spherical.interpolate(edge.start.marker.getPosition(), edge.end.marker.getPosition(), 0.5);
        self.connectionInfoWindow_.setPosition(inBetween);
        self.connectionInfoWindow_.open(self.map_);
    };

    self.connectionOnRightClick_ = function (edge) {
        self.graph.removeEdge(edge);
        self.saveConnection(edge.connection);
    };
    
    self.nodeOnDrag_ = function (vertex) {
        var newPosition = vertex.marker.getPosition();
        vertex.node.latitude(newPosition.lat());
        vertex.node.longitude(newPosition.lng());

        for (var i = 0; i < vertex.edges.length; i++) {
            var edge = vertex.edges[i];
            if (edge.start.equals(vertex)) {
                edge.poly.getPath().setAt(0, newPosition);
            }
            else {
                edge.poly.getPath().setAt(1, newPosition);
            }
        }
    };

    self.nodeOnDragEnd_ = function (vertex) {
        self.nodeManager.save(vertex.node);
    };

    self.addNode = function (node) {
        var marker = new google.maps.Marker({
            //id: node.id(),
            //selected: false,
            position: new google.maps.LatLng(node.latitude(), node.longitude()),
            title: '#' + node.id(),
            draggable: true,
            map: self.map_
        });
        var vertex = new Vertex(node, marker);
        vertex.onClick = self.nodeOnClick_;
        vertex.onRightClick = self.nodeOnRightClick_;
        vertex.onDrag = self.nodeOnDrag_;
        vertex.onDragEnd = self.nodeOnDragEnd_;

        self.graph.addVertex(vertex);
    };

    self.createNode = function (event) {
        var context = this;
        this.marker = new google.maps.Marker({
            //id: 0,
            //selected: false,
            position: event.latLng,
            title: '#NEW',
            draggable: true,
            map: self.map_
        });
        var node = new Node();
        node.id(0);
        node.identifier('NEW');
        node.floorMapId(self.floorMapId());
        node.latitude(event.latLng.lat());
        node.longitude(event.latLng.lng());
        node.floorConnector(false);

        var vertex = new Vertex(node, this.marker);
        vertex.onClick = self.nodeOnClick_;
        vertex.onRightClick = self.nodeOnRightClick_;
        vertex.onDrag = self.nodeOnDrag_;
        vertex.onDragEnd = self.nodeOnDragEnd_;

        self.graph.addVertex(vertex);

        var saveNodeCallback = function (node) {
            context.marker.setTitle('#' + node.id());

            var selectedVertex = self.graph.getSelected();
            if (selectedVertex) {

                var connection = new Connection();
                connection.id(0);
                connection.nodeAId(selectedVertex.node.id());
                connection.nodeBId(node.id());
                connection.show(true);
                connection.floorConnection(false);

                var added = self.addConnection(connection);
                if (added) {
                    self.saveConnection(connection);
                }
            }
            self.graph.toggleVertex(vertex);
        };
        self.nodeManager.save(node, self.nodeErrors, saveNodeCallback);
    };

    self.addConnection = function (connection) {
        var vertexA = self.graph.findVertexById(connection.nodeAId());
        var vertexB = self.graph.findVertexById(connection.nodeBId());

        if (vertexA == null || vertexB == null)
            return;

        var poly = new google.maps.Polyline(self.polyOptions(connection.show()));
        var edge = new Edge(connection, poly);
        edge.onClick = self.connectionOnClick_;
        edge.onRightClick = self.connectionOnRightClick_;

        edge.start = vertexA;
        edge.end = vertexB;

        return self.graph.addEdge(edge);
    };

    self.clearMap = function () {
        self.graph.clear();
        self.detachCreateListener();

        if (self.overlay_ != null)
            self.overlay_.setMap(null);
    };

    self.detachCreateListener = function () {
        if (self.overlayCreateListener_ == null)
            return;

        google.maps.event.removeListener(self.overlayCreateListener_);
        self.overlayCreateListener_ = null;
    };

    self.attachCreateListener = function () {
        if (self.overlayCreateListener_ != null)
            return;

        self.overlayCreateListener_ = google.maps.event.addListener(self.overlay_, 'click', self.createNode);
    };

    self.render = function (building, floor, graphData) {
        var ne = new google.maps.LatLng(building.nwLatitude(), building.seLongitude());
        var sw = new google.maps.LatLng(building.seLatitude(), building.nwLongitude());

        if (floor.neLatitude() != 0 && floor.neLongitude() != 0
            && floor.swLongitude() != 0 && floor.swLongitude() != 0) {
            ne = new google.maps.LatLng(floor.neLatitude(), floor.neLongitude());
            sw = new google.maps.LatLng(floor.swLatitude(), floor.swLongitude());
        }

        var bounds = new google.maps.LatLngBounds(sw, ne);
        self.overlay_ = new google.maps.GroundOverlay(floor.imagePath(), bounds);
        self.overlay_.setMap(self.map_);
        self.map_.fitBounds(bounds);

        self.attachCreateListener();

        if (graphData.nodes().length > 0) {
            for (var i = 0; i < graphData.nodes().length; i++) {
                self.addNode(graphData.nodes()[i]);
            }
        }

        if (graphData.connections().length > 0) {
            for (var i = 0; i < graphData.connections().length; i++) {
                self.addConnection(graphData.connections()[i]);
            }
        }
    };
}

function SearchModel() {
    var self = this;

    self.selectedFloorId = ko.observable(0);
    self.selectedBuildingId = ko.observable(0);
    self.floors = ko.observableArray([]);
    self.buildings = ko.observableArray([]);
    self.graphData = new GraphData();
    self.floorChanged = ko.observable();

    self.selectedBuilding = ko.computed(function () {
        return ko.utils.arrayFirst(self.buildings(), function (item) {
            return item.id() === self.selectedBuildingId();
        });
    });

    self.selectedFloor = ko.computed(function () {
        return ko.utils.arrayFirst(self.floors(), function (item) {
            return item.id() === self.selectedFloorId();
        });
    });

    self.selectedBuildingId.subscribe(function (buildingId) {
        if (buildingId > 0) {
            $.getJSON('/api/floor?buildingid=' + buildingId, {
                returnformat: 'json'
            }).done(function (data) {
                ko.mapping.fromJS(data, {}, self.floors);
            });
        }
        else {
            this.selectedFloorId(0);
            this.floors([]);
        }
    }.bind(self));

    self.selectedFloorId.subscribe(function (floorId) {
        this.graphData.nodes([]);
        this.graphData.connections([]);
        if (floorId > 0) {
            $.getJSON('/api/navigation?floorMapId=' + floorId, {
                returnformat: 'json'
            }).done(function (data) {
                for (var i = 0; i < data.nodes.length; i++) {
                    var node = new Node();
                    ko.mapping.fromJS(data.nodes[i], {}, node);

                    self.graphData.nodes().push(node);
                }
                for (i = 0; i < data.connections.length; i++) {
                    var connection = new Connection();
                    ko.mapping.fromJS(data.connections[i], {}, connection);

                    self.graphData.connections().push(connection);
                }
                self.floorChanged.valueHasMutated();
            });
        }
        else {
            this.floorChanged.valueHasMutated();
        }
    }.bind(self));

    self.getBuildings = function () {
        $.getJSON('/api/building', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.buildings);
        });
    };
}

function ViewModel() {
    var self = this;

    self.navigationMap = new NavigationMap();
    self.searchModel = new SearchModel();

    self.searchModel.floorChanged.subscribe(function () {
        this.navigationMap.clearMap();

        if (!this.searchModel.selectedFloorId()
            || this.searchModel.selectedFloorId() === 0)
            return;

        this.navigationMap.floorMapId(self.searchModel.selectedFloorId());
        var building = self.searchModel.selectedBuilding();
        var floor = self.searchModel.selectedFloor();

        if (building == null || floor == null)
            return;

        this.navigationMap.render(building, floor, self.searchModel.graphData);
    }.bind(self));

    self.init = function () {
        self.searchModel.getBuildings();
    };
}

$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    vm.init();
});