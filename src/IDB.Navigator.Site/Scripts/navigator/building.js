

function AddressMap() {
    var self = this
    self.building = null;

    var map;
    var rectangle;
    var addressInput;
    var autoComplete;

    var showNewRect = function (event) {
        var ne = rectangle.getBounds().getNorthEast();
        var sw = rectangle.getBounds().getSouthWest();

        if (self.building != null) {
            self.building.nwLatitude(ne.lat());
            self.building.nwLongitude(sw.lng());
            self.building.seLatitude(sw.lat());
            self.building.seLongitude(ne.lng());
        }
    };

    self.initLocation = function () {
        if (self.building == null)
            return;

        var ne = new google.maps.LatLng(self.building.nwLatitude(), self.building.seLongitude());
        var sw = new google.maps.LatLng(self.building.seLatitude(), self.building.nwLongitude());
        var bounds = new google.maps.LatLngBounds(sw, ne);
        map.fitBounds(bounds);
        rectangle.setOptions({
            bounds: bounds
        });
    };

    self.initialize = function () {
        map = new google.maps.Map(document.getElementById("map"), {
            center: new google.maps.LatLng(36, -208),
            zoom: 1,
            mapTypeId: 'roadmap'
        });

        addressInput = $("#Address").get(0);
        autocomplete = new google.maps.places.Autocomplete(addressInput);
        autocomplete.bindTo('bounds', map);

        // Define the rectangle and set its editable property to true.
        rectangle = new google.maps.Rectangle({
            editable: true,
            draggable: true,
            map: map
        });

        // Add an event listener on the rectangle.
        google.maps.event.addListener(rectangle, 'bounds_changed', showNewRect);

        google.maps.event.addDomListener(addressInput, 'keydown', function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
            }
        });

        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            var place = autocomplete.getPlace();
            if (place.geometry.viewport) {
                map.fitBounds(place.geometry.viewport);
            } else {
                map.setCenter(place.geometry.location);
                map.setZoom(18);
            }

            if (self.building != null) {
                self.building.address(place.formatted_address);
            }

            var bounds = map.getBounds();
            map.fitBounds(bounds);
            rectangle.setOptions({
                bounds: bounds
            });
        });

    };
}

function ViewModel() {
    var self = this;
    self.id = parseInt($("#Id").val());
    self.isUpdate = self.id > 0;
    self.data = new Building();
    self.errors = new Building();
    self.form = $('form.form-validate');

    self.addressMap = new AddressMap(self.data);
    self.addressMap.building = self.data;

    self.save = function () {
        if (!self.form.valid()) {
            return;
        }

        var url = '/api/admin/building/' + (self.isUpdate ? self.id : '');
        var method = self.isUpdate ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            type: method,
            data: ko.toJSON(self.data),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (response) {
            if (self.isUpdate) {
                ko.mapping.fromJS(response, {}, self.data);
                window.notifyUpdate();
            } else {
                window.location = '/building/edit/' + response.id + '?create=true'
            }
        }).fail(function (jqXHR) {
            var errors = extractErrors(jqXHR);
            for (var error in errors) {
                self.errors[error](errors[error]);
                self.errors[error].valueHasMutated();
            }
        });
    };

    self.getBuilding = function (id) {
        loading.show();
        $.getJSON('/api/admin/building/' + id, {
            returnformat: 'json'
        }, function (data) {
            loading.hide();
            ko.mapping.fromJS(data, {}, self.data);
            self.addressMap.initLocation();
        });
    };

    self.addressMap.initialize();
}


$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    if ($.QueryString['create']) {
        window.notifyCreate();
    }

    if (vm.isUpdate) {
        vm.getBuilding(vm.id);
    }
});