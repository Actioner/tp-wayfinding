function MarkerManager() {
    var self = this;
    self.form = $('#placeTmpl');

    self.isValid = function () {
        return self.form.valid();
    };

    self.save = function (marker, errors, selected) {
        if (!self.form.valid()) {
            return;
        }
        var isUpdate = marker.id() > 0;
        var url = '/api/admin/marker/' + (isUpdate ? marker.id() : '');
        var method = isUpdate > 0 ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            data: ko.toJSON(marker),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (response) {
            ko.mapping.fromJS(response, {}, marker);
            if (isUpdate) {
                window.notifyUpdate();
            } else {
                ko.mapping.fromJS(response, {}, selected);
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

    self.delete = function (marker) {
        var deleteUrl = '/api/admin/marker/' + marker.id();

        $.ajax({
            url: deleteUrl,
            type: 'DELETE'
        }).done(function () {
            window.notifyDelete();
        });
    };

};

function MarkerPoint(marker, markerMap) {
    var self = this;
    self.marker = marker;
    self.markerMap = markerMap;
    self.isSelected = ko.observable(false);
    self.isNew = ko.computed(function () {
        return !self.marker.id() || self.marker.id() == 0;
    });
    self.infoWindow_ = new window.google.maps.InfoWindow({
        content: '<div id="infoPlaceholder" style="width:289px; height:277px"></div>'
    });

    self.tmpl_ = $("#placeTmpl");

    self.marker_ = new google.maps.Marker({
        title: self.marker.displayName(),
        draggable: false
    });

    self.setIcon_ = function (markerTypeId, selected) {
        var prefix = selected ? 'blu' : 'red';
        if (typeof markerTypeId != "undefined") {
            var code = self.markerMap.getMarkerCode(markerTypeId);
            if (code === null) {
                return markerTypeId;
            }

            self.marker_.setIcon('/Content/mapIcons/' + prefix + '-' + code + '.png');
        }
    };

    self.setIcon_(self.marker.markerTypeId(), false);

    self.openInfoReady = function () {
        var placeholder = $("#infoPlaceholder");
        self.tmpl_.appendTo(placeholder);
    };


    self.closeInfoReady = function () {
        self.tmpl_.appendTo("#tmplContainer");

    };

    //Add a listener
    google.maps.event.addListener(self.infoWindow_, 'domready', self.openInfoReady);
    google.maps.event.addListener(self.infoWindow_, "closeclick", self.closeInfoReady);

    self.clearMarker = function () {
        self.marker_.setMap(null);
    };

    self.isSelected.subscribe(function (sel) {
        if (sel) {
            //self.marker_.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-waypoint-blue.png');
            self.infoWindow_.open(self.markerMap.getMap(), self.marker_);
        }
        else {
            //self.marker_.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-poi.png');
            self.closeInfoReady();
            self.infoWindow_.close();
        }

        self.setIcon_(self.marker.markerTypeId(), sel);
        self.marker_.setDraggable(sel);
    });

    self.marker.markerTypeId.subscribe(function (ofType) {
        self.setIcon_(ofType, self.isSelected);
    });


    google.maps.event.addListener(self.marker_, 'click', function () {
        markerMap.toggle(self);
    });

    google.maps.event.addListener(self.marker_, 'dragend', function () {
        var pos = self.marker_.getPosition();
        self.marker.latitude(pos.lat());
        self.marker.longitude(pos.lng());

        markerMap.markerSave(self.marker);
    });


    google.maps.event.addListener(self.marker_, 'rightclick', function () {
        self.clearMarker();

        markerMap.markerDelete(self);
    });

    self.init = function () {
        var position = new google.maps.LatLng(self.marker.latitude(), self.marker.longitude());
        self.marker_.setPosition(position);
        self.marker_.setMap(self.markerMap.getMap())
    };
};


function MarkerMap() {
    var self = this;
    self.floorMapId = ko.observable(0);
    self.errors = new Marker();
    self.markerManager = new MarkerManager();
    self.markerPoints = ko.observableArray([]);
    self.markerTypes = {};

    self.setMarkerTypes = function (types) {
        for (var i = 0; i < types.length; i++) {
            self.markerTypes[types[i].id()] = types[i].code();
        }
    };

    self.getMarkerCode = function (markerTypeId) {
        if (typeof markerTypeId != "undefined") {
            return self.markerTypes[markerTypeId];
        }
        return null;
    };

    self.createMarker = function () {
        var newMarker = new Marker();
        newMarker.floorMapId(self.floorMapId());

        return newMarker;
    };

    self.getMap = function () {
        return self.map_;
    };

    self.selectedPoint = ko.computed(function () {
        var selectedMarkerPoint;
        for (var i = 0; i < self.markerPoints().length; i++) {
            selectedMarkerPoint = self.markerPoints()[i];
            if (selectedMarkerPoint.isSelected()) {
                return selectedMarkerPoint;
            }
        };

        return new MarkerPoint(self.createMarker(), self);
    });

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

    self.toggle = function (markerPoint) {
        if (markerPoint.marker.id() != self.selectedPoint().marker.id())
            self.selectedPoint().isSelected(false);
        markerPoint.isSelected(!markerPoint.isSelected());
    };

    self.markerCancel = function () {
        var markerPoint = self.selectedPoint();
        if (markerPoint.isNew()) {
            self.markerPoints.pop();
            markerPoint.isSelected(false);
            markerPoint.clearMarker();
            self.attachCreateListener();
        }
        else {
            markerPoint.isSelected(false);
        }
    };

    self.markerDelete = function (markerPoint) {
        self.markerManager.delete(markerPoint.marker);
    };

    self.markerSave = function (marker) {
        self.markerManager.save(marker, self.errors);
    };

    self.markerCreate = function (marker) {
        if (!self.markerManager.isValid()) {
            return;
        }

        self.markerManager.save(marker, self.errors);
        self.attachCreateListener();
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

        self.overlayCreateListener_ = google.maps.event.addListener(self.overlay_, 'click', self.preMarkerCreate);
    };


    self.preMarkerCreate = function (event) {
        var newMarkerPoint = new MarkerPoint(self.createMarker(), self);
        newMarkerPoint.marker.latitude(event.latLng.lat());
        newMarkerPoint.marker.longitude(event.latLng.lng());
        newMarkerPoint.init();
        self.markerPoints.push(newMarkerPoint);

        self.toggle(newMarkerPoint);
        self.detachCreateListener();
    };

    self.clearMap = function () {
        for (var i = 0; i < self.markerPoints().length; i++) {
            self.markerPoints()[i].clearMarker();
        }
        self.markerPoints([]);
        self.detachCreateListener();

        if (self.overlay_ != null)
            self.overlay_.setMap(null);
    };

    self.addMarkerPoint = function (marker) {
        var markerPoint = new MarkerPoint(marker, self);
        markerPoint.init();
        self.markerPoints.push(markerPoint);

        return markerPoint;
    };

    self.render = function (building, floor, markers) {
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

        for (var i = 0; i < markers.length; i++) {
            var marker = new Marker();
            ko.mapping.fromJS(markers[i], {}, marker);

            self.addMarkerPoint(marker);
        }
    };
}

function SearchModel() {
    var self = this;

    self.selectedFloorId = ko.observable(0);
    self.selectedBuildingId = ko.observable(0);
    self.floors = ko.observableArray([]);
    self.buildings = ko.observableArray([]);
    self.markers = ko.observableArray([]);

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
        this.selectedFloorId(0);
        this.floors([]);
        if (buildingId > 0) {
            $.getJSON('/api/admin/floor?buildingid=' + buildingId, {
                returnformat: 'json'
            }, function (data) {
                ko.mapping.fromJS(data, {}, self.floors);

                //TODO: remove hack
                self.selectedFloorId(15);
            });
        }
    }.bind(self));

    self.selectedFloorId.subscribe(function (floorId) {
        this.markers([]);

        if (floorId > 0) {
            $.getJSON('/api/admin/marker?floorMapId=' + floorId, {
                returnformat: 'json'
            }, function (data) {
                ko.mapping.fromJS(data, {}, self.markers);
            });
        }
    }.bind(self));

    self.getBuildings = function () {
        $.getJSON('/api/admin/building', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.buildings);

            //TODO: remove hack
            self.selectedBuildingId(2);
        });
    };
}

function ViewModel() {
    var self = this;

    self.markerMap = new MarkerMap();
    self.searchModel = new SearchModel();
    self.markerManager = new MarkerManager();
    self.markerTypes = ko.observableArray([]);

    self.searchModel.markers.subscribe(function (markers) {
        this.markerMap.clearMap();

        if (!this.searchModel.selectedFloorId() || this.searchModel.selectedFloorId() === 0)
            return;

        //this.markerMap.selected().floorMapId(self.searchModel.selectedFloorId());
        this.markerMap.floorMapId(self.searchModel.selectedFloorId());
        var building = self.searchModel.selectedBuilding();
        var floor = self.searchModel.selectedFloor();

        if (building == null || floor == null)
            return;

        this.markerMap.render(building, floor, markers);
    }.bind(self));

    self.getMarkerTypes = function () {
        $.getJSON('/api/admin/markertype', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.markerTypes);
            self.markerMap.setMarkerTypes(self.markerTypes());
        });
    };

    self.init = function () {
        self.searchModel.getBuildings();
        self.getMarkerTypes();
    };
}

$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    vm.init();
});