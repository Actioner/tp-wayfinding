var PARAMS = parseParams();
var SRCFILE = unescape(PARAMS.overlay);

window.onload = function () {
    var img = new Image();
    //img.src = SRCFILE;

    var sourceCanvas = document.getElementById('src');
    var sourceContext = sourceCanvas.getContext('2d');

    // To enable drag and drop
    sourceCanvas.addEventListener("dragover", function (evt) {
        evt.preventDefault();
    }, false);

    // Handle dropped image file - only Firefox and Google Chrome
    sourceCanvas.addEventListener("drop", function (evt) {
        var files = evt.dataTransfer.files;
        if (files.length > 0) {
            var file = files[0];
            if (typeof FileReader !== "undefined" && file.type.indexOf("image") != -1) {
                var reader = new FileReader();
                // Note: addEventListener doesn't work in Google Chrome for this event
                reader.onload = function (evt) {
                    img.src = evt.target.result;
                };
                reader.readAsDataURL(file);
            }
        }
        evt.preventDefault();
    }, false);

    img.onload = function () {
        sourceContext.clearRect(0, 0, sourceCanvas.width, sourceCanvas.height);
        sourceCanvas.height = img.height;
        sourceCanvas.width = img.width;
        sourceContext.drawImage(img, 0, 0);

        updateHermite(this);
        //updateResized(this);
    };
};

function parseParams() {
    var result = {};
    var params = window.location.search.substring(1).split('&');
    for (var i = 0, param; param = params[i]; i++) {
        var parts = param.split('=');
        result[parts[0]] = parts[1];
    }
    return result;
}

var updateHermite = function (source) {
    var img = source;

    var hermite = new resample.Hermite(img);

    var width = 800;
    var height = img.height * width / img.width;

    var destinationCanvas = document.getElementById('dst');
    destinationCanvas.height = height;
    destinationCanvas.width = width;
    hermite.resize(width, height);

    destinationCanvas.getContext("2d").putImageData(hermite.getResult(), 0, 0);

    var transformedCanvas = document.getElementById('trns');
    transformedCanvas.height = destinationCanvas.height * 1.4;
    transformedCanvas.width = destinationCanvas.width * 1.2;
    var transformedContext = transformedCanvas.getContext('2d');

    transformedContext.setTransform(1.1, 0.1, 0.1, 1.1, 0.1, 0.1);
    transformedContext.drawImage(destinationCanvas, 0, 0);
};
