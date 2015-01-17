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
