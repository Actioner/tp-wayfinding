function OfficeManager() {
    var self = this;
    self.form = $('#officeForm');

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
            console.log(office.id());
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

function OfficePoint(office, map) {
    var self = this;
    self.office = office;
    self.map_ = map;
    self.selected = false;

    self.marker_ = new google.maps.Marker({
        position: new google.maps.LatLng(office.latitude(), office.longitude()),
        title: office.displayName(),
        map: self.map_,
        draggable: false
    });

    self.clearMarker = function () {
        self.marker_.setMap(null);
    };

    self.postClickEvent = null;
    self.postDragendEvent = null;
    self.postRightClickEvent = null;

    self.toggle = function () {
        self.selected = !self.selected;
        if (self.selected){
            self.marker_.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-waypoint-blue.png');
        }
        else {
            self.marker_.setIcon('https://mts.googleapis.com/vt/icon/name=icons/spotlight/spotlight-poi.png');
        }
        self.marker_.setDraggable(self.selected);

    };

    google.maps.event.addListener(self.marker_, 'click', function () {
        self.toggle();

        if (isFunction(self.postClickEvent)) {
            self.postClickEvent(self);
        }
    });

    google.maps.event.addListener(self.marker_, 'dragend', function () {
        var pos = self.marker_.getPosition();
        self.office.latitude(pos.lat());
        self.office.longitude(pos.lng());

        if (isFunction(self.postDragendEvent)) {
            self.postDragendEvent(self);
        }
    }.bind(this));

 
    google.maps.event.addListener(self.marker_, 'rightclick', function () {
        self.clearMarker();

        if (isFunction(self.postRightClickEvent)) {
            self.postRightClickEvent(self);
        }
    });
}


function OfficeMap() {
    var self = this;
    self.floorMapId = 0;
    self.selected = new Office();
    self.errors = new Office();
    self.officeManager = new OfficeManager();
    self.officePoints = ko.observableArray([]);
    self.overlay_ = null;
    self.map_ = new google.maps.Map(document.getElementById('map'), {
        mapTypeId: 'roadmap',
        center: new google.maps.LatLng(0, 0),
        zoom: 3
    });

    self.officePointClick = function (officePoint) {
        self.selected.clear();
        self.selected.floorMapId(self.floorMapId);

        self.errors.clear();
        if (officePoint.selected) {
            self.clearPreviousSelectedOfficePoint(officePoint);
            self.selected.copyFrom(officePoint.office);
        }
    };

    self.officePointDragEnd = function (officePoint) {
        self.selected.latitude(officePoint.office.latitude());
        self.selected.longitude(officePoint.office.longitude());
        officePoint.office.copyFrom(self.selected);
    };

    self.officeRightClick = function (officePoint) {
        self.officeDelete(officePoint.office);
    };

    self.officeDelete = function (office) {
        self.officeManager.delete(office);

        self.selected.clear();
        self.selected.floorMapId(self.floorMapId);
        self.errors.clear();
    };

    self.officeSave = function () {
        self.officeManager.save(self.selected, self.errors);

        self.selected.clear();
        self.selected.floorMapId(self.floorMapId);
        self.errors.clear();
    };


    self.officeCreate = function () {
        if (!self.officeManager.isValid()) {
            return;
        }

        var newOffice = new Office();
        var pos = self.overlay_.getBounds().getCenter();
        self.selected.latitude(pos.lat());
        self.selected.longitude(pos.lng());
        newOffice.copyFrom(self.selected);
        self.officeManager.save(newOffice, self.errors, self.selected);

        var officePoint = self.addOfficePoint(newOffice);
        officePoint.toggle();
    };


    self.clearPreviousSelectedOfficePoint = function (current) {
        var selectedOfficePoint;
        for (var i = 0; i < self.officePoints().length; i++) {
            selectedOfficePoint = self.officePoints()[i];
            if (selectedOfficePoint.selected && selectedOfficePoint.office.id() != current.office.id()) {
                selectedOfficePoint.toggle();
            }
        };
    };

    self.clearMap = function () {
        for (var i = 0; i < self.officePoints().length; i++) {
            self.officePoints()[i].clearMarker();
        }
        self.selected.clear();
        self.selected.floorMapId(self.floorMapId);
        self.errors.clear();
        self.officePoints([]);

        if (self.overlay_ != null)
            self.overlay_.setMap(null);
    };

    self.addOfficePoint = function (office) {
        var officePoint = new OfficePoint(office, self.map_);

        officePoint.postClickEvent = self.officePointClick;
        officePoint.postDragendEvent = self.officePointDragEnd;
        officePoint.postRightClickEvent = self.officeRightClick;
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

        if (offices.length > 0) {
            for (var i = 0; i < offices.length; i++) {
                var office = new Office();
                ko.mapping.fromJS(offices[i], {}, office);

                self.addOfficePoint(office);
            }
        }
    };


    self.init = function () {
        
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

        if (this.searchModel.selectedFloorId() === 0)
            return;

        this.officeMap.selected.floorMapId(self.searchModel.selectedFloorId());
        this.officeMap.floorMapId = self.searchModel.selectedFloorId();
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
        self.officeMap.init();
    };
}

$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    vm.init();
});