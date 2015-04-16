
function ViewModel() {
    var self = this;
    self.id = parseInt($("#Id").val());
    self.data = new Person();
    self.officeNumber = ko.observable();

    self.gerMarker = function (officeId) {
        if (officeId && officeId > 0) {
            $.getJSON('/api/admin/marker/' + officeId, {
                returnformat: 'json'
            }).done(function (data) {
                self.officeNumber(data.displayName);
            });
        }
    };

    self.getPerson = function (id) {
        loading.show();
        $.getJSON('/api/admin/person/' + id, {
            returnformat: 'json'
        }, function (data) {
            loading.hide();
            ko.mapping.fromJS(data, {}, self.data);

            self.gerMarker(data.markerId);
        });
    };
}


$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    vm.getPerson(vm.id);
});