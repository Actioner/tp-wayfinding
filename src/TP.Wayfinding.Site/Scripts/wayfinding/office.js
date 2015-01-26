function OfficeManager() {
    var self = this;
    self.form = $('#placeTmpl');

    self.isValid = function () {
        return self.form.valid();
    };

    self.save = function (office, errors, selected) {
        if (!self.form.valid()) {
            return;
        }
        var isUpdate = office.id() > 0;
        var url = '/api/office/' + (isUpdate ? office.id() : '');
        var method = isUpdate > 0 ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            data: ko.toJSON(office),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (response) {
            ko.mapping.fromJS(response, {}, office);
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

    self.delete = function (office) {
        var deleteUrl = '/api/office/' + office.id();

        $.ajax({
            url: deleteUrl,
            type: 'DELETE'
        }).done(function () {
            window.notifyDelete();
        });
    };

};

function OfficePoint(office, officeMap) {
    var self = this;
    self.office = office;
    self.officeMap = officeMap
    self.isSelected = ko.observable(false);
    self.isNew = ko.computed(function () {
        return !self.office.id() || self.office.id() == 0;
    });
    self.infoWindow_ = new window.google.maps.InfoWindow({
        content: '<div id="infoPlaceholder" style="width:289px; height:277px"></div>'
    });

    self.tmpl_ = $("#placeTmpl");

    self.marker_ = new google.maps.Marker({
        title: self.office.displayName(),
        draggable: false
    });

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
            self.marker_.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-waypoint-blue.png');
            self.infoWindow_.open(self.officeMap.getMap(), self.marker_);
        }
        else {
            self.marker_.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-poi.png');
            self.closeInfoReady();
            self.infoWindow_.close();
        }

        self.marker_.setDraggable(sel);
    });


    google.maps.event.addListener(self.marker_, 'click', function () {
        officeMap.toggle(self);
    });

    google.maps.event.addListener(self.marker_, 'dragend', function () {
        var pos = self.marker_.getPosition();
        self.office.latitude(pos.lat());
        self.office.longitude(pos.lng());

        officeMap.officeSave(self.office);
    });


    google.maps.event.addListener(self.marker_, 'rightclick', function () {
        self.clearMarker();

        officeMap.officeDelete(self);
    });

    self.init = function () {
        var position = new google.maps.LatLng(self.office.latitude(), self.office.longitude());
        self.marker_.setPosition(position);
        self.marker_.setMap(self.officeMap.getMap())
    };
}


function OfficeMap() {
    var self = this;
    self.floorMapId = ko.observable(0);
    self.errors = new Office();
    self.officeManager = new OfficeManager();
    self.officePoints = ko.observableArray([]);

    self.createOffice = function () {
        var newOffice = new Office();
        newOffice.floorMapId(self.floorMapId());

        return newOffice;
    };

    self.getMap = function () {
        return self.map_;
    };

    self.selectedPoint = ko.computed(function () {
        var selectedOfficePoint;
        for (var i = 0; i < self.officePoints().length; i++) {
            selectedOfficePoint = self.officePoints()[i];
            if (selectedOfficePoint.isSelected()) {
                return selectedOfficePoint;
            }
        };

        return new OfficePoint(self.createOffice(), self);
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

    self.toggle = function (officePoint) {
        if (officePoint.office.id() != self.selectedPoint().office.id())
            self.selectedPoint().isSelected(false);
        officePoint.isSelected(!officePoint.isSelected());
    };

    self.officeCancel = function () {
        var officePoint = self.selectedPoint();
        if (officePoint.isNew()) {
            self.officePoints.pop();
            officePoint.isSelected(false);
            officePoint.clearMarker();
            self.attachCreateListener();
        }
        else {
            officePoint.isSelected(false);
        }
    };

    self.officeDelete = function (officePoint) {
        self.officeManager.delete(officePoint.office);
    };

    self.officeSave = function (office) {
        self.officeManager.save(office, self.errors);
    };

    self.officeCreate = function (office) {
        if (!self.officeManager.isValid()) {
            return;
        }

        self.officeManager.save(office, self.errors);
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

        self.overlayCreateListener_ = google.maps.event.addListener(self.overlay_, 'click', self.preOfficeCreate);
    };


    self.preOfficeCreate = function (event) {
        var newOfficePoint = new OfficePoint(self.createOffice(), self);
        newOfficePoint.office.latitude(event.latLng.lat());
        newOfficePoint.office.longitude(event.latLng.lng());
        newOfficePoint.init();
        self.officePoints.push(newOfficePoint);

        self.toggle(newOfficePoint);
        self.detachCreateListener();
    };

    self.clearMap = function () {
        for (var i = 0; i < self.officePoints().length; i++) {
            self.officePoints()[i].clearMarker();
        }
        self.officePoints([]);
        self.detachCreateListener();

        if (self.overlay_ != null)
            self.overlay_.setMap(null);
    };

    self.addOfficePoint = function (office) {
        var officePoint = new OfficePoint(office, self);
        officePoint.init();
        self.officePoints.push(officePoint);

        return officePoint;
    };

    self.render = function (building, floor, offices) {
        var ne = new google.maps.LatLng(building.nwLatitude(), building.nwLongitude());
        var sw = new google.maps.LatLng(building.seLatitude(), building.seLongitude());

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

        for (var i = 0; i < offices.length; i++) {
            var office = new Office();
            ko.mapping.fromJS(offices[i], {}, office);

            self.addOfficePoint(office);
        }
    };
}

function SearchModel() {
    var self = this;

    self.selectedFloorId = ko.observable(0);
    self.selectedBuildingId = ko.observable(0);
    self.floors = ko.observableArray([]);
    self.buildings = ko.observableArray([]);
    self.offices = ko.observableArray([]);

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
            $.getJSON('/api/floor?buildingid=' + buildingId, {
                returnformat: 'json'
            }, function (data) {
                ko.mapping.fromJS(data, {}, self.floors);
            });
        }
    }.bind(self));

    self.selectedFloorId.subscribe(function (floorId) {
        this.offices([]);

        if (floorId > 0) {
            $.getJSON('/api/office?floorMapId=' + floorId, {
                returnformat: 'json'
            }, function (data) {
                ko.mapping.fromJS(data, {}, self.offices);
            });
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

    self.officeMap = new OfficeMap();
    self.searchModel = new SearchModel();
    self.officeManager = new OfficeManager();

    self.officeTypes = ko.observableArray([]);

    self.searchModel.offices.subscribe(function (offices) {
        this.officeMap.clearMap();

        if (!this.searchModel.selectedFloorId() || this.searchModel.selectedFloorId() === 0)
            return;

        //this.officeMap.selected().floorMapId(self.searchModel.selectedFloorId());
        this.officeMap.floorMapId(self.searchModel.selectedFloorId());
        var building = self.searchModel.selectedBuilding();
        var floor = self.searchModel.selectedFloor();

        if (building == null || floor == null)
            return;

        this.officeMap.render(building, floor, offices);
    }.bind(self));

    self.getOfficeTypes = function () {
        $.getJSON('/api/officetype', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.officeTypes);
        });
    };

    self.init = function () {
        self.searchModel.getBuildings();
        self.getOfficeTypes();
    };
}

$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    vm.init();
});