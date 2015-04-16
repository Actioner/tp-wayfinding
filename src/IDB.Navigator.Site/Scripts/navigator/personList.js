
function SearchModel() {
    var self = this;
    var throttleLimit = 200;

    self.selectedBuildingId = ko.observable(0);
    self.accountName = ko.observable('').extend({ rateLimit: throttleLimit });
}

function ViewModel() {
    var self = this;
    var deleteConfirm = new DeleteConfirm('Delete Building');

    var throttleLimit = 150;
    var defaultPageSize = 10;
    self.pageSize = ko.observable(defaultPageSize);
    self.currentPageIndex = ko.observable(0);
    self.count = ko.observable(0);

    self.buildings = ko.observableArray([]);
    self.currentData = ko.observableArray([]);
    self.searchModel = new SearchModel();

    self.getBuildings = function () {
        loading.show();
        $.getJSON('/api/admin/building', {
            returnformat: 'json'
        }, function (data) {
            ko.mapping.fromJS(data, {}, self.buildings);

            //TODO: remove hack
            self.searchModel.selectedBuildingId(2);

            loading.hide();
        });
    };

    self.showDuplicatesFilter = ko.observable(false).extend({ rateLimit: throttleLimit });
    self.lastPageIndex = ko.computed(function () {
        return Math.ceil(self.count() / self.pageSize()) - 1;
    });

    self.currentPageText = ko.computed(function () {
        var current = self.currentPageIndex() + 1;
        var last = self.lastPageIndex() + 1;

        return current + " of " + last;
    });

    self.previousPage = function () {
        if (self.currentPageIndex() > 0) {
            loading.show();
            self.currentPageIndex(self.currentPageIndex() - 1);
            self.getPeople();
        }
    };

    self.nextPage = function () {
        if (self.currentPageIndex() < self.lastPageIndex()) {
            loading.show();
            self.currentPageIndex(self.currentPageIndex() + 1);
            self.getPeople();
        }
    };
    
    self.getPeople = function () {
        if (typeof self.searchModel.selectedBuildingId() == "undefined") {
            self.currentData([]);
            self.currentPageIndex(0);
            self.count(0);
            return;
        }

        $.getJSON('/api/admin/person', {
            returnformat: 'json',
            buildingId: self.searchModel.selectedBuildingId(),
            accountName: self.searchModel.accountName(),
            currentPageIndex: self.currentPageIndex(),
            pageSize: self.pageSize()
        }).done(function (result) {
            ko.mapping.fromJS(result.data, {}, self.currentData);
            self.count(result.count);
            loading.hide();
        });
    };

    self.searchModel.selectedBuildingId.subscribe(function () {
        loading.show();
        self.currentPageIndex(0);
        self.getPeople();
    });
    self.searchModel.accountName.subscribe(function () {
        //loading.show();
        self.currentPageIndex(0);
        self.getPeople();
    });

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