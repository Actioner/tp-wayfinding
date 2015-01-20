function Building() {
    var self = this;
    self.id = ko.observable();
    self.name = ko.observable();
    self.location = ko.observable();
    self.company = ko.observable();
    self.address = ko.observable();
    self.nwLatitude = ko.observable();
    self.nwLongitude = ko.observable();
    self.seLatitude = ko.observable();
    self.seLongitude = ko.observable();
}

function Floor() {
    var self = this;
    self.id = ko.observable();
    self.buildingId = ko.observable();
    self.floor = ko.observable();
    self.description = ko.observable();
    self.image = ko.observable();
    self.imagePath = ko.observable();
    self.neLatitude = ko.observable();
    self.neLongitude = ko.observable();
    self.swLatitude = ko.observable();
    self.swLongitude = ko.observable();
}

function Office() {
    var self = this;
    self.id = ko.observable();
    self.displayName = ko.observable();
    self.officeNumber = ko.observable();
    self.officeType = ko.observable();
    self.latitude = ko.observable();
    self.longitude = ko.observable();
    self.floorMapId = ko.observable();
    self.manual = ko.observable();

    self.clear = function () {
        self.id(0);
        self.displayName(undefined);
        self.officeNumber(undefined);
        self.officeType(undefined);
        self.floorMapId(0);
        self.latitude(undefined);
        self.longitude(undefined);
        self.manual(undefined);
    };
}

function OfficeType() {
    var self = this;
    self.id = ko.observable();
    self.description = ko.observable();
    self.code = ko.observable();
    self.icon = ko.observable();
    self.static = ko.observable();

}

