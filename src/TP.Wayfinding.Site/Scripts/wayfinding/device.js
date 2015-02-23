function DeviceManager() {
    var self = this;
    self.form = $('#deviceTmpl');

    self.isValid = function () {
        return self.form.valid();
    };

    self.save = function (device, errors, callback) {
        if (!self.form.valid()) {
            return;
        }
        var isUpdate = device.id() > 0;
        var url = '/api/device/' + (isUpdate ? device.id() : '');
        var method = isUpdate > 0 ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            data: ko.toJSON(device),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (response) {
            ko.mapping.fromJS(response, {}, device);
            console.log(device.id());
            if (window.isFunction(callback)) {
                callback(device);
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

    self.delete = function (device) {
        var deleteUrl = '/api/device/' + device.id();

        $.ajax({
            url: deleteUrl,
            type: 'DELETE'
        }).done(function () {
            window.notifyDelete();
        });
    };
};

function DevicePoint(device, deviceMap) {
    var self = this;
    self.device = device;
    self.deviceMap = deviceMap
    self.isSelected = ko.observable(false);
    self.isNew = ko.computed(function () {
        return !self.device.id() || self.device.id() == 0;
    });
    self.infoWindow_ = new window.google.maps.InfoWindow({
        content: '<div id="infoPlaceholder" style="width:289px; height:129px"></div>'
    });

    self.tmpl_ = $("#deviceTmpl");

    self.marker_ = new google.maps.Marker({
        title: self.device.name(),
        draggable: false
    });

    self.marker_.setIcon('/Content/mapIcons/red-device.png');

    self.openInfoReady = function () {
        var placeholder = $("#infoPlaceholder");
        self.tmpl_.appendTo(placeholder);
    };

    self.closeInfoReady = function () {
        self.tmpl_.appendTo("#deviceTmplContainer");
    };

    //Add a listener
    google.maps.event.addListener(self.infoWindow_, 'domready', self.openInfoReady);
    google.maps.event.addListener(self.infoWindow_, 'closeclick', self.closeInfoReady);

    self.clearMarker = function () {
        self.marker_.setMap(null);
    };

    self.isSelected.subscribe(function (sel) {
        if (sel) {
            self.marker_.setIcon('/Content/mapIcons/blu-device.png');
            self.infoWindow_.open(self.deviceMap.getMap(), self.marker_);
        }
        else {
            self.marker_.setIcon('/Content/mapIcons/red-device.png');
            self.closeInfoReady();
            self.infoWindow_.close();
        }

        self.marker_.setDraggable(sel);
    });


    google.maps.event.addListener(self.marker_, 'click', function () {
        deviceMap.toggle(self);
    });

    google.maps.event.addListener(self.marker_, 'dragend', function () {
        var pos = self.marker_.getPosition();
        self.device.latitude(pos.lat());
        self.device.longitude(pos.lng());

        deviceMap.deviceSave(self.device);
    });

    google.maps.event.addListener(self.marker_, 'rightclick', function () {
        self.clearMarker();
        deviceMap.deviceDelete(self);
    });

    self.init = function () {
        var position = new google.maps.LatLng(self.device.latitude(), self.device.longitude());
        self.marker_.setPosition(position);
        self.marker_.setMap(self.deviceMap.getMap())
    };
}

function DeviceMap() {
    var self = this;
    self.floorMapId = ko.observable(0);
    self.errors = new Device();
    self.deviceManager = new DeviceManager();
    self.devicePoints = ko.observableArray([]);

    self.createDevice = function () {
        var newDevice = new Device();
        newDevice.floorMapId(self.floorMapId());

        return newDevice;
    };

    self.getMap = function () {
        return self.map_;
    };

    self.selectedPoint = ko.computed(function () {
        var selectedDevicePoint;
        for (var i = 0; i < self.devicePoints().length; i++) {
            selectedDevicePoint = self.devicePoints()[i];
            if (selectedDevicePoint.isSelected()) {
                return selectedDevicePoint;
            }
        };

        return new DevicePoint(self.createDevice(), self);
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

    self.toggle = function (devicePoint) {
        if (devicePoint.device.id() != self.selectedPoint().device.id())
            self.selectedPoint().isSelected(false);
        devicePoint.isSelected(!devicePoint.isSelected());
    };

    self.deviceCancel = function () {
        var devicePoint = self.selectedPoint();
        if (devicePoint.isNew()) {
            self.devicePoints.pop();
            devicePoint.isSelected(false);
            devicePoint.clearMarker();
            self.attachCreateListener();
        }
        else {
            devicePoint.isSelected(false);
        }
    };

    self.deviceDelete = function (devicePoint) {
        self.deviceManager.delete(devicePoint.device);
    };

    self.deviceSave = function (device) {
        self.deviceManager.save(device, self.errors);
    };

    self.deviceCreate = function (device) {
        if (!self.deviceManager.isValid()) {
            return;
        }

        self.deviceManager.save(device, self.errors);
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

        self.overlayCreateListener_ = google.maps.event.addListener(self.overlay_, 'click', self.preDeviceCreate);
    };


    self.preDeviceCreate = function (event) {
        var newDevicePoint = new DevicePoint(self.createDevice(), self);
        newDevicePoint.device.latitude(event.latLng.lat());
        newDevicePoint.device.longitude(event.latLng.lng());
        newDevicePoint.init();
        self.devicePoints.push(newDevicePoint);

        self.toggle(newDevicePoint);
        self.detachCreateListener();
    };

    self.clearMap = function () {
        for (var i = 0; i < self.devicePoints().length; i++) {
            self.devicePoints()[i].clearMarker();
        }
        self.devicePoints([]);
        self.detachCreateListener();

        if (self.overlay_ != null)
            self.overlay_.setMap(null);
    };

    self.addDevicePoint = function (device) {
        var devicePoint = new DevicePoint(device, self);
        devicePoint.init();
        self.devicePoints.push(devicePoint);

        return devicePoint;
    };

    self.render = function (building, floor, devices) {
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

        for (var i = 0; i < devices().length; i++) {
            var device = new Device();
            ko.mapping.fromJS(devices()[i], {}, device);

            self.addDevicePoint(device);
        }
    };
}

function SearchModel() {
    var self = this;

    self.selectedFloorId = ko.observable(0);
    self.selectedBuildingId = ko.observable(0);
    self.floors = ko.observableArray([]);
    self.buildings = ko.observableArray([]);
    self.devices = ko.observableArray([]);
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
        this.devices([]);
        if (floorId > 0) {
            $.getJSON('/api/device?floorMapId=' + floorId, {
                returnformat: 'json'
            }).done(function (data) {
                ko.mapping.fromJS(data, {}, self.devices);
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

    self.deviceMap = new DeviceMap();
    self.searchModel = new SearchModel();

    self.searchModel.floorChanged.subscribe(function () {
        this.deviceMap.clearMap();

        if (!this.searchModel.selectedFloorId()
            || this.searchModel.selectedFloorId() === 0)
            return;

        this.deviceMap.floorMapId(self.searchModel.selectedFloorId());
        var building = self.searchModel.selectedBuilding();
        var floor = self.searchModel.selectedFloor();

        if (building == null || floor == null)
            return;

        this.deviceMap.render(building, floor, self.searchModel.devices);
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