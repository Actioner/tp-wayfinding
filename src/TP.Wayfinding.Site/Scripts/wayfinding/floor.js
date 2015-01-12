function Floor() {
    var self = this;
    self.id = ko.observable();
    self.buildingId = ko.observable();
    self.floor = ko.observable();
    self.description = ko.observable();
    self.imagePath = ko.observable();
}

function ViewModel() {
    var self = this;
    self.id = parseInt($("#Id").val());
    self.buildingId = parseInt($("#BuildingId").val());
    self.isUpdate = self.id > 0;
    self.data = new Floor();
    self.errors = new Floor();
    self.form = $('form.form-validate');

    if (!self.isUpdate)
        self.data.buildingId(self.buildingId);

    self.save = function () {
        if (!self.form.valid()) {
            return;
        }

        var url = '/api/floor/' + (self.isUpdate ? self.id : '');
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
                window.location = '/floor/edit/' + response.id + '?create=true'
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
        $.getJSON('/api/floor/' + id, {
            returnformat: 'json'
        }, function (data) {
            loading.hide();
            ko.mapping.fromJS(data, {}, self.data);
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
        vm.getData(vm.id);
    }
});