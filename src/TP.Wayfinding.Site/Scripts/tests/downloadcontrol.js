overlaytiler.DownloadControl = function (affineOverlay) {
    var el = this.el_ = document.createElement('a');
    el.text = 'download tile';
    el.download = 'sosgroso.png';
    el.onclick = this.onClick_.bind(this);

    this.affineOverlay = affineOverlay;
};

overlaytiler.DownloadControl.prototype.onClick_ = function () {
    var Point = function (x, y) {
        var self = this;
        self.x = x;
        self.y = y;

        self.toString = function () {
            return (self.x + ", " + self.y);
        };

        self.add = function (Point) {
            self.x += Point.x;
            self.y += Point.y;
        };
    };

    var img = this.affineOverlay.getOrigImg();
    var resultMatrix = this.affineOverlay.ctx.transform.getMatrix();
    console.log("matrix: ", JSON.stringify(resultMatrix));

    var tmpCanvas = document.createElement('canvas');
    var tmpContext = tmpCanvas.getContext('2d');

    var scHor = resultMatrix[0];
    var skHor = resultMatrix[1];
    var skVer = resultMatrix[2];
    var scVer = resultMatrix[3];
    var moHor = resultMatrix[4];
    var moVer = resultMatrix[5];

    var topLeft = new Point(moHor, moVer);
    var topRight = new Point(scHor * img.width + moHor, skHor * img.width + moVer);
    var bottomLeft = new Point(skVer * img.height + moHor, scVer * img.height + moVer);
    var bottomRight = new Point((scHor * img.width) + (skVer * img.height) + moHor, (skHor * img.width) + (scVer * img.height) + moVer);
    var resultTopLeft = new Point(Math.min(topLeft.x, topRight.x, bottomLeft.x, bottomRight.x), Math.min(bottomLeft.y, bottomRight.y, topLeft.y, topRight.y));
    var resultBottomRight = new Point(Math.max(topLeft.x, topRight.x, bottomLeft.x, bottomRight.x), Math.max(bottomLeft.y, bottomRight.y, topLeft.y, topRight.y));
    var offset = new Point(resultTopLeft.x < 0 ? Math.abs(resultTopLeft.x) : 0, resultTopLeft.y < 0 ? Math.abs(resultTopLeft.y) : 0);
    topLeft.add(offset);
    topRight.add(offset);
    bottomLeft.add(offset);
    bottomRight.add(offset);
    resultTopLeft.add(offset);
    resultBottomRight.add(offset);

    console.log("resultTopLeft: ", resultTopLeft.toString());
    console.log("resultBottomRight: ", resultBottomRight.toString());

    var mapTopLeft = this.affineOverlay.getTopLeftPoint_();
    var mapBottomRight = this.affineOverlay.getBottomRightPoint_();
    console.log("mapTopLeft", ": ", mapTopLeft.x, ", ", mapTopLeft.y);
    console.log("mapBottomRight", ": ", mapBottomRight.x, ", ", mapBottomRight.y);

    var proj = this.affineOverlay.getProjection();
    console.log(proj.fromDivPixelToLatLng(new google.maps.Point(mapTopLeft.x, mapTopLeft.y)));
    console.log(proj.fromDivPixelToLatLng(new google.maps.Point(mapBottomRight.x, mapBottomRight.y)));

    return false;
    tmpCanvas.width = resultBottomRight.x;
    tmpCanvas.height = resultBottomRight.y;
    tmpContext.setTransform(scHor, skHor, skVer, scVer, offset.x + moHor, offset.y + moVer);
    tmpContext.drawImage(img, 0, 0, img.width, img.height);

    var resultCanvas = document.createElement('canvas');
    resultCanvas.width = resultBottomRight.x - resultTopLeft.x;
    resultCanvas.height = resultBottomRight.y - resultTopLeft.y;
    var resultContext = resultCanvas.getContext('2d');
    resultContext.drawImage(tmpCanvas, resultTopLeft.x, resultTopLeft.y, resultCanvas.width, resultCanvas.height, 0, 0, resultCanvas.width, resultCanvas.height);

    this.el_.href = resultCanvas.toDataURL('image/png');
};

overlaytiler.DownloadControl.prototype.getElement = function() {
  return this.el_;
};
