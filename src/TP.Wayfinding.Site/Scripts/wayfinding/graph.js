var Graph = function (map) {
    this._numOfEdges = 0;
    this._adjacencyLists = {};
    this._vertexList = [];
    this._map = map;
};

var Vertex = function (node, marker) {
    var self = this;

    this.selected = false;
    this.node = node;
    this.marker = marker;
    this.edges = [];

    this.onClick = null;
    this.onRightClick = null;
    this.onDrag = null;
    this.onDragEnd = null;

    marker.setIcon('/Content/mapIcons/red-circle-lv.png');


    google.maps.event.addListener(marker, 'click', function (evt) {
        if (window.isFunction(self.onClick)) {
            self.onClick(self, evt);
        }
    });
    google.maps.event.addListener(marker, 'rightclick', function (evt) {
        if (window.isFunction(self.onRightClick)) {
            self.onRightClick(self, evt);
        }
    });
    google.maps.event.addListener(marker, 'drag', function (evt) {
        if (window.isFunction(self.onDrag)) {
            self.onDrag(self, evt);
        }
    });
    google.maps.event.addListener(marker, 'dragend', function (evt) {
        if (window.isFunction(self.onDragEnd)) {
            self.onDragEnd(self, evt);
        }
    });
};


var Edge = function (connection, poly) {
    var self = this;

    this.connection = connection;
    this.poly = poly;
    this.start = null;
    this.end = null;
    this.onClick = null;
    this.onRightClick = null;
    this.polyOptions = function (show) {
        return {
            strokeColor: show ? '#000000' : '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 3
        };
    };

    this.connection.show.subscribe(function (show) {
        self.poly.setOptions(self.polyOptions(show));
    });

    google.maps.event.addListener(poly, 'click', function (evt) {
        if (window.isFunction(self.onClick)) {
            self.onClick(self, evt);
        }
    });
    google.maps.event.addListener(poly, 'rightclick', function (evt) {
        if (window.isFunction(self.onRightClick)) {
            self.onRightClick(self, evt);
        }
    });
};

var AdjacencyList = function () {
    this._vertexList = [];
};

AdjacencyList.prototype.add = function (vertex) {
    this._vertexList.push(vertex);
};

AdjacencyList.prototype.find = function (vertex) {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].equals(vertex)) {
            return this._vertexList[i];
        }
    };
    return null;
};

Vertex.prototype.equals = function (other) {
    return this.node.equals(other.node);
};

Vertex.prototype.toggle = function () {
    this.selected = !this.selected;
    if (this.selected)
        this.marker.setIcon('/Content/mapIcons/blu-circle-lv.png');
    else
        this.marker.setIcon('/Content/mapIcons/red-circle-lv.png');
};

Graph.prototype.addVertex = function (vertex) {
    this._vertexList.push(vertex);

    return true;
};

Graph.prototype.removeVertex = function (vertex) {
    //remove adjancency list
    delete this._adjacencyLists[vertex.node.id()];
    //remove edges
    for (var i = 0; i < vertex.edges.length; i++) {
        var edge = vertex.edges[i];
        edge.poly.setMap(null);
    }
    //remove vertexs
    var vertices = this.getVerticesKeys();
    for (var i = 0; i < vertices.length; i++) {
        var adjacentList = this._adjacencyLists[vertices[i]];
        var j = 0;
        while (adjacentList._vertexList[j] != null && !adjacentList._vertexList[j].equals(vertex)) {
            j++;
        }
        if (j < adjacentList._vertexList.length) {
            this._numOfEdges--;

            adjacentList._vertexList.splice(j, 1);
            if (adjacentList._vertexList.length === 0) {
                delete this._adjacencyLists[vertices[i]];
            }
        }
    }
    var i = 0;
    while (this._vertexList[i] != null && !this._vertexList[i].equals(vertex)) {
        i++;
    }
    if (i < this._vertexList.length) {
        this._vertexList.splice(i, 1);
    }
    vertex.marker.setMap(null);
};


Graph.prototype.removeEdge = function (edge) {
    var startAdjacentList = this._adjacencyLists[edge.start.node.id()];
    var endAdjacentList = this._adjacencyLists[edge.end.node.id()];
    var removed = false;

    //remove edge from start adjacent list
    var j = 0;
    while (startAdjacentList._vertexList[j] != null && !startAdjacentList._vertexList[j].equals(edge.end)) {
        j++;
    }
    if (j < startAdjacentList._vertexList.length) {
        removed = true;

        startAdjacentList._vertexList.splice(j, 1);
        if (startAdjacentList._vertexList.length === 0) {
            delete this._adjacencyLists[edge.start.node.id()];
        }
    }

    //remove edge from end adjacent list
    j = 0;
    while (endAdjacentList._vertexList[j] != null && !endAdjacentList._vertexList[j].equals(edge.start)) {
        j++;
    }
    if (j < endAdjacentList._vertexList.length) {
        removed = true;

        endAdjacentList._vertexList.splice(j, 1);
        if (endAdjacentList._vertexList.length === 0) {
            delete this._adjacencyLists[edge.end.node.id()];
        }
    }

    //discount number of edges
    if (removed)
        this._numOfEdges--;

    //hide edge from map
    edge.poly.setMap(null);
};

Graph.prototype.findVertex = function (vertex) {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].equals(vertex)) {
            return this._vertexList[i];
        }
    };
    return null;
};

Graph.prototype.findVertexById = function (vertexId) {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].node.id() === vertexId) {
            return this._vertexList[i];
        }
    };
    return null;
};

Graph.prototype.addEdge = function (edge) {
    this._adjacencyLists[edge.start.node.id()] = this._adjacencyLists[edge.start.node.id()] ||
      new AdjacencyList();
    this._adjacencyLists[edge.end.node.id()] = this._adjacencyLists[edge.end.node.id()] ||
      new AdjacencyList();

    var start = this.findVertex(edge.start);
    if (!start) {
        this.addVertex(edge.start);
        start = edge.start;
    }
    var end = this.findVertex(edge.end);
    if (!end) {
        this.addVertex(edge.end);
        end = edge.end;
    }

    var added = false;
    if (!this._adjacencyLists[edge.start.node.id()].find(edge.end)) {
        this._adjacencyLists[edge.start.node.id()].add(edge.end);
        added = true;
    }
    if (!this._adjacencyLists[edge.end.node.id()].find(edge.start)) {
        this._adjacencyLists[edge.end.node.id()].add(edge.start);
        added = true;
    }
    if (added) {
        this._numOfEdges++;

        edge.poly.setMap(this._map);
        var path = edge.poly.getPath();
        path.push(edge.start.marker.getPosition());
        path.push(edge.end.marker.getPosition());

        edge.start.edges.push(edge);
        edge.end.edges.push(edge);
    }

    return added;
};

Graph.prototype.getVerticesKeys = function () {
    return Object.keys(this._adjacencyLists);
};

Graph.prototype.getSelected = function () {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].selected) {
            return this._vertexList[i];
        }
    };
    return null;
};

Graph.prototype.toggleVertex = function (vertex) {
    var currentlySelected = this.getSelected();

    if (currentlySelected && !currentlySelected.equals(vertex))
        currentlySelected.toggle();

    vertex.toggle();
};

//Graph.prototype.findVertexByMarker = function (marker) {
//    for (var i = 0; i < this._vertexList.length; i++) {
//        if (this._vertexList[i].marker.equals(marker)) {
//            return this._vertexList[i];
//        }
//    };
//    return null;
//};

Graph.prototype.clear = function () {
    while (this._vertexList.length > 0) {
        this.removeVertex(this._vertexList[0]);
    }
};

Graph.prototype.toString = function () {
    var adjString = '';
    var currentNode = null;
    var vertices = this.getVerticesKeys();
    console.log(this._vertexList.length + " vertices, " + this._numOfEdges + " edges");
    for (var i = 0; i < vertices.length; i++) {
        adjString = vertices[i] + ":";
        var vertexList = this._adjacencyLists[vertices[i]]._vertexList;
        for (var j = 0; j < vertexList.length; j++) {
            currentNode = vertexList[j];
            adjString += " " + currentNode.node.id() + "(" + currentNode.selected.toString() + ")";
        }
        console.log(adjString);
        adjString = '';
    }

    adjString = "v:";
    for (var i = 0; i < this._vertexList.length; i++) {
        adjString += " " + this._vertexList[i].node.id();
    };
    console.log(adjString);
    adjString = '';
};
