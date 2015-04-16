


function ViewModel() {
    var self = this;
    self.buildingId = ko.observable(0);
    self.buildings = ko.observableArray([]);
    self.data = new Person();
    self.errors = new Person();
    self.id = parseInt($("#Id").val());
    self.isUpdate = self.id > 0;
    self.form = $('form.form-validate');

    function minMarker(data) {
        var self = this;
        self.id = data.id;
        self.text = data.displayName;
        return self;
    }

    self.save = function () {
        if (!self.form.valid()) {
            return;
        }

        var url = '/api/admin/person/' + (self.isUpdate ? self.data.id() : '');
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
                window.location = '/person/edit/' + response.id + '?create=true'
            }
        }).fail(function (jqXHR) {
            var errors = extractErrors(jqXHR);
            for (var error in errors) {
                self.errors[error](errors[error]);
                self.errors[error].valueHasMutated();
            }
        });
    };

    self.getBuilding = function () {
        $.getJSON('/api/admin/building?markerId=' + self.data.markerId(), {
            returnformat: 'json'
        }, function (data) {
            self.buildingId(data[0].id);
        });
    };

    self.getBuildings = function () {
        loading.show();
        $.getJSON('/api/admin/building', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.buildings);
            loading.hide();
        });
    };

    self.getMarkers = function () {

        $("#MarkerId").select2({
            placeholder: "Chose a marker...",
            minimumInputLength: 2,
            quietMillis: 100,
            ajax: {
                url: "/api/admin/marker",
                dataType: 'json',
                data: function (term, page) {
                    return {
                        buildingId:  self.buildingId(),
                        displayNameTerm: term,
                        markerTypeId: 5 //oficinas
                    };
                },
                results: function (data, page) {
                    return { results: $.map(data, function (item) { return new minMarker(item) }) };
                }
            },
            initSelection: function (element, callback) {
                var id = self.data.markerId();
                if (id > 0) {
                    $.getJSON('/api/admin/marker/' + id, {
                        returnformat: 'json'
                    }).done(function (data) {
                        callback(new minMarker(data));
                    });
                }
            },
            formatResult: function (marker) {
                return marker.text;
            },
            formatSelection: function (marker) {
                self.data.markerId(marker.id);
                return marker.text;
            },  
            escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
        });
    };

    self.getPerson = function (id) {
        loading.show();
        $.getJSON('/api/admin/person/' + id, {
            returnformat: 'json'
        }, function (data) {
            loading.hide();
            ko.mapping.fromJS(data, {}, self.data);
            self.getBuilding();
            self.getMarkers();
        });
    };
}


$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    if ($.QueryString['create']) {
        window.notifyCreate();
    }

    if (vm.isUpdate) {
        vm.getPerson(vm.id);
    }
    else {
        vm.getMarkers();
    }

    vm.getBuildings();
});