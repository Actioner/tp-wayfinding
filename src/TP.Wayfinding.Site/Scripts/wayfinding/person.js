


function ViewModel() {
    var self = this;
    self.buildingId = ko.observable(0);
    self.buildings = ko.observableArray([]);
    self.data = new Person();
    self.errors = new Person();
    self.id = parseInt($("#Id").val());
    self.isUpdate = self.id > 0;
    self.form = $('form.form-validate');

    function minOffice(data) {
        var self = this;
        self.id = data.id;
        self.text = data.displayName;
        return self;
    }

    self.save = function () {
        if (!self.form.valid()) {
            return;
        }

        var url = '/api/person/' + (self.isUpdate ? self.data.id() : '');
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

    self.getBuildings = function () {
        loading.show();
        $.getJSON('/api/building', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.buildings);
            loading.hide();
        });
    };

    self.getOffices = function () {

        $("#OfficeId").select2({
            placeholder: "Chose an office...",
            minimumInputLength: 2,
            quietMillis: 100,
            ajax: {
                url: "/api/office",
                dataType: 'json',
                data: function (term, page) {
                    return {
                        buildingId:  self.buildingId(),
                        displayNameTerm: term,
                        officeTypeId: 5 //oficinas
                    };
                },
                results: function (data, page) {
                    return { results: $.map(data, function (item) { return new minOffice(item) }) };
                }
            },
            initSelection: function (element, callback) {
                var id = self.data.officeId();
                if (id > 0) {
                    $.getJSON('/api/office/' + id, {
                        returnformat: 'json'
                    }).done(function (data) {
                        callback(new minOffice(data));
                    });
                }
                //callback(data);
            },
            formatResult: function (office) {
                return office.text;
            },
            formatSelection: function (office) {
                self.data.officeId(office.id);
                return office.text;
            },  
            escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
        });
    };

    self.getPerson = function (id) {
        loading.show();
        $.getJSON('/api/person/' + id, {
            returnformat: 'json'
        }, function (data) {
            loading.hide();
            ko.mapping.fromJS(data, {}, self.data);
            self.getOffices();
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
        vm.getOffices();
    }

    vm.getBuildings();
});