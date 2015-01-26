function Building() {
    var self = this;
    self.id = ko.observable(0);
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
    self.id = ko.observable(0);
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
    self.id = ko.observable(0);
    self.displayName = ko.observable();
    self.officeNumber = ko.observable();
    self.officeType = ko.observable();
    self.latitude = ko.observable();
    self.longitude = ko.observable();
    self.floorMapId = ko.observable();
    self.manual = ko.observable();
}

function OfficeType() {
    var self = this;
    self.id = ko.observable(0);
    self.description = ko.observable();
    self.code = ko.observable();
    self.icon = ko.observable();
    self.static = ko.observable();
}

function Node() {
    var self = this;
    self.id = ko.observable(0);
    self.floorMapId = ko.observable();
    self.identifier = ko.observable();
    self.latitude = ko.observable();
    self.longitude = ko.observable();
    self.floorConnector = ko.observable();
}

function Connection() {
    var self = this;
    self.id = ko.observable(0);
    self.nodeAId = ko.observable();
    self.nodeBId = ko.observable();
    self.show = ko.observable();
    self.floorConnection = ko.observable();
}

function Device() {
    var self = this;
    self.id = ko.observable(0);
    self.floorMapId = ko.observable();
    self.name = ko.observable();
    self.latitude = ko.observable();
    self.longitude = ko.observable();
}

function Person() {
    var self = this;
    self.id = ko.observable(0);
    self.officeId = ko.observable();
    self.accountName = ko.observable();
    self.firstName = ko.observable();
    self.lastName = ko.observable();
    self.phoneNumber = ko.observable();
    self.department = ko.observable();
    self.division = ko.observable();
}

function PersonList() {
    var self = this;
    self.id = ko.observable(0);
    self.accountName = ko.observable();
    self.firstName = ko.observable();
    self.lastName = ko.observable();
    self.building = ko.observable();
    self.floor = ko.observable();
    self.office = ko.observable();
}


