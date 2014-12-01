var map = new google.maps.Map(d3.select("#map").node(), {
    zoom: 7,
    center: new google.maps.LatLng(44.331216, 23.927536),
    mapTypeId: google.maps.MapTypeId.ROADMAP
});
// Load the station data. When the data comes back, create an overlay.
d3.json("/Scripts/json/graph.js", function (json) {
    var overlay = new google.maps.OverlayView();

    // Add the container when the overlay is added to the map.
    overlay.onAdd = function () {

        var layer = d3.select(this.getPanes().overlayLayer)
            .append("div")
            .attr("height", "100%")
            .attr("width", "100%")
            .attr("class", "stations");
        overlay.draw = function () {
            var radius = 5;
            var projection = this.getProjection(),
                padding = 10;


            var node_coord = {};

            var marker = layer.selectAll("svg")
              .data(json.nodes)
              .each(transform) // update existing markers
            .enter().append("svg:svg")
              .each(transform)
              .attr("class", "marker");
            marker.append("svg:circle")
              .attr("r", radius)
              .attr("cx", padding)
              .attr("cy", padding);

            // Add a label.
            marker.append("svg:text")
                    .attr("x", padding + 7)
                    .attr("y", padding)
                    .attr("dy", ".37em")
                    .text(function (d) { return d.id; });


            var markerLink = layer.selectAll(".links")
              .data(json.links)
              .each(pathTransform) // update existing markers       
            .enter().append("svg:svg")
             .attr("class", "links")
              .each(pathTransform);
            function pathTransform(d) {
                var t, b, l, r, w, h, currentSvg;
                $(this).empty(); // get rid of the old lines (cannot use d3 .remove() because i cannot use selectors after ... )

                var dsrc = new google.maps.LatLng(node_coord[d.source - 1 + "," + 1], node_coord[d.source - 1 + "," + 0]);
                var dtrg = new google.maps.LatLng(node_coord[d.target - 1 + "," + 1], node_coord[d.target - 1 + "," + 0]);
                var d1 = projection.fromLatLngToDivPixel(dsrc);
                var d2 = projection.fromLatLngToDivPixel(dtrg);
                if (d1.y < d2.y) {
                    t = d1.y;
                    b = d2.y;
                } else {
                    t = d2.y;
                    b = d1.y;
                }
                if (d1.x < d2.x) {
                    l = d1.x;
                    r = d2.x;
                } else {
                    l = d2.x;
                    r = d1.x;
                }
                currentSvg = d3.select(this)
                    .style("left", (l) + "px")
                    .style("top", (t) + "px")
                    .style("width", (r - l) + "px")
                    .style("height", (b - t) + "px");
                // drawing the diagonal lines inside the svg elements. We could use 2 cases instead of for but maybe you will need to orient your graph (so you can use some arrows)
                if ((d1.y < d2.y) && (d1.x < d2.x)) {
                    currentSvg.append("svg:line")
                        .style("stroke-width", 1)
                        .style("stroke", "black")
                        .attr("y1", 0)
                        .attr("x1", 0)
                        .attr("x2", r - l)
                        .attr("y2", b - t);
                } else if ((d1.x > d2.x) && (d1.y > d2.y)) {
                    currentSvg.append("svg:line")
                        .style("stroke-width", 1)
                        .style("stroke", "black")
                        .attr("y1", 0)
                        .attr("x1", 0)
                        .attr("x2", r - l)
                        .attr("y2", b - t);
                } else if ((d1.y < d2.y) && (d1.x > d2.x)) {
                    currentSvg.append("svg:line")
                        .style("stroke-width", 1)
                        .style("stroke", "black")
                        .attr("y1", 0)
                        .attr("x2", 0)
                        .attr("x1", r - l)
                        .attr("y2", b - t);
                } else if ((d1.x < d2.x) && (d1.y > d2.y)) {
                    currentSvg.append("svg:line")
                        .style("stroke-width", 1)
                        .style("stroke", "black")
                        .attr("y1", 0)
                        .attr("x2", 0)
                        .attr("x1", r - l)
                        .attr("y2", b - t);
                } else {
                    console.log("something is wrong!!!");
                }


                return currentSvg;
            }

            function transform(d, i) {
                console.log(i);
                node_coord[i + "," + 0] = d.lng;
                node_coord[i + "," + 1] = d.lat;
                d = new google.maps.LatLng(d.lat, d.lng);
                d = projection.fromLatLngToDivPixel(d);
                return d3.select(this)
                    .style("left", (d.x - padding) + "px")
                    .style("top", (d.y - padding) + "px");
            }

            layer.append("div")
                .attr("class", "stations.line");


        };
    };


    // Bind our overlay to the map…
    overlay.setMap(map);
});