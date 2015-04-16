

function ViewModel() {
    var self = this;
    self.id = parseInt($("#Id").val());
    self.building = new Building();
    self.isUpdate = self.id > 0;
    self.data = new Floor();
    self.errors = new Floor();
    self.overlay = null;
    self.form = $('form.form-validate');

    self.save = function () {
        self.overlay.refreshImageData();

        if (!self.form.valid()) {
            return;
        }

        var url = '/api/admin/floor/' + (self.isUpdate ? self.id : '');
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
                window.location = '/floor/view/' + response.id + '?create=true';
            }
        }).fail(function (jqXHR) {
            var errors = extractErrors(jqXHR);
            for (var error in errors) {
                self.errors[error](errors[error]);
                self.errors[error].valueHasMutated();
            }
        });
    };

    self.getData = function (id) {
        loading.show();
        $.getJSON('/api/admin/floor/' + id, {
            returnformat: 'json'
        }, function (data) {
            loading.hide();
            ko.mapping.fromJS(data, {}, self.data);

            self.getBuilding();
        });
    };

    var dropZone = document.getElementById('map');
    var map = new google.maps.Map(dropZone, {
        mapTypeId: 'roadmap'
    });



    self.getBuilding = function () {
        $.getJSON('/api/admin/building/' + self.data.buildingId(), {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.building);

            var ne = new google.maps.LatLng(self.building.nwLatitude(), self.building.seLongitude());
            var sw = new google.maps.LatLng(self.building.seLatitude(), self.building.nwLongitude());

            if (self.isUpdate) {
                if (self.data.neLatitude() != 0 && self.data.neLongitude() != 0
                    && self.data.swLongitude() != 0 && self.data.swLongitude() != 0) {
                    ne = new google.maps.LatLng(self.data.neLatitude(), self.data.neLongitude());
                    sw = new google.maps.LatLng(self.data.swLatitude(), self.data.swLongitude());
                }

                var imageBounds = new google.maps.LatLngBounds(sw, ne);
                var groundOverlay = new google.maps.GroundOverlay(self.data.imagePath(), imageBounds);
                groundOverlay.setMap(map);
            }

            var bounds = new google.maps.LatLngBounds(sw, ne);
            map.fitBounds(bounds);
        });
    };

    function setupOverlay(img, map) {
        // sometimes the image hasn't actually loaded
        if (!img.height) {
            setTimeout(setupOverlay.bind(this, img, map), 50);
            return;
        }

        self.overlay = new overlaytiler.AffineOverlay(img, self.data);
        self.overlay.setMap(map);

        var opacity = new overlaytiler.OpacityControl(self.overlay);
        map.controls[google.maps.ControlPosition.TOP_LEFT]
            .push(opacity.getElement());
    }

    self.initializeCreate = function () {
        var img;
        img = new Image();
        img.onload = setupOverlay.bind(this, img, map);
        var startUpload = function (files) {
            if (files.length > 0) {
                var file = files[0];
                if (typeof FileReader !== "undefined" && file.type.indexOf("image") != -1) {
                    var reader = new FileReader();
                    // Note: addEventListener doesn't work in Google Chrome for this event
                    reader.onload = function (evt) {
                        img.src = evt.target.result;
                    };
                    reader.readAsDataURL(file);
                }
            }
        }

        dropZone.ondrop = function (evt) {
            evt.preventDefault();
            //if (hasOverlay)
            //    overlay.clear();

            startUpload(evt.dataTransfer.files);
        }

        dropZone.ondragover = function () {
            return false;
        }

        dropZone.ondragleave = function () {
            return false;
        }

        self.data.buildingId($.QueryString['buildingId']);

        self.getBuilding();
    };
}


$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    if ($.QueryString['create']) {
        window.notifyCreate();
    }

    if (vm.isUpdate) {
        vm.getData(vm.id);
    }
    else {
        vm.initializeCreate();
    }

});