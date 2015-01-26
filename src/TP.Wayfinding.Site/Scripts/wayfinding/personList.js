
function SearchModel() {
    var self = this;
    var throttleLimit = 200;

    self.selectedBuildingId = ko.observable(0);
    self.accountName = ko.observable('').extend({ rateLimit: throttleLimit });
}

function ViewModel() {
    var self = this;
    var deleteConfirm = new DeleteConfirm('Delete Building');

    self.buildings = ko.observableArray([]);
    self.currentData = ko.observableArray([]);
    self.searchModel = new SearchModel();

    self.getBuildings = function () {
        loading.show();
        $.getJSON('/api/building', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.buildings);
            loading.hide();
        });
    };

    self.getPeople = function () {
        $.getJSON('/api/person', {
            returnformat: 'json',
            buildingId: self.searchModel.selectedBuildingId(),
            accountName: self.searchModel.accountName()
        }).done(function (data) {
            ko.mapping.fromJS(data, {}, self.currentData);
        });
    };

    self.searchModel.selectedBuildingId.subscribe(self.getPeople);
    self.searchModel.accountName.subscribe(self.getPeople);
    deleteConfirm.onConfirm = self.getPeople;

    self.init = function () {
        self.getBuildings();
    };
}

$(function () {
    var vm = new ViewModel();
    ko.applyBindings(vm);

    vm.init();
});