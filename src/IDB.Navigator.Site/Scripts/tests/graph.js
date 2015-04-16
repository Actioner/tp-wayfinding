var Graph = function (map) {
    this._numOfEdges = 0;
    this._adjacencyLists = {};
    this._vertexList = [];
    this._map = map;
};

var Edge = function (poly) {
    this.poly = poly;
    this.start = null;
    this.end = null;
};

var Vertex = function (marker) {
    this.marker = marker;
    this.edges = [];
};

var AdjacencyList = function () {
    this._vertexList = [];
};

AdjacencyList.prototype.add = function (vertex) {
    this._vertexList.push(vertex);
};

AdjacencyList.prototype.find = function (vertex) {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].marker.equals(vertex.marker)) {
            return this._vertexList[i];
        }
    };
    return null;
};

Graph.prototype.addVertex = function (vertex) {
    this._vertexList.push(vertex);
};

Graph.prototype.removeVertex = function (vertex) {
    //remove adjancency list
    delete this._adjacencyLists[vertex.marker.id];
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
        while (adjacentList._vertexList[j] != null && !adjacentList._vertexList[j].marker.equals(vertex.marker)) {
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
    while (this._vertexList[i] != null && !this._vertexList[i].marker.equals(vertex.marker)) {
        i++;
    }
    if (i < this._vertexList.length) {
        this._vertexList.splice(i, 1);
    }
    vertex.marker.setMap(null);
};

Graph.prototype.findVertex = function (vertex) {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].marker.equals(vertex.marker)) {
            return this._vertexList[i];
        }
    };
    return null;
};

Graph.prototype.addEdge = function (v, w) {
    this._adjacencyLists[v.marker.id] = this._adjacencyLists[v.marker.id] ||
      new AdjacencyList();
    this._adjacencyLists[w.marker.id] = this._adjacencyLists[w.marker.id] ||
      new AdjacencyList();

    var start = this.findVertex(v);
    if (!start) {
        this.addVertex(v);
        start = v;
    }
    var end = this.findVertex(w);
    if (!end) {
        this.addVertex(w);
        end = w;
    }

    var added = false;
    if (!this._adjacencyLists[v.marker.id].find(w)) {
        this._adjacencyLists[v.marker.id].add(w);
        added = true;
    }
    if (!this._adjacencyLists[w.marker.id].find(v)) {
        this._adjacencyLists[w.marker.id].add(v);
        added = true;
    }
    if (added) {
        this._numOfEdges++;

        var poly = new google.maps.Polyline(this._polyOptions);
        poly.setMap(this._map);
        var path = poly.getPath();
        path.push(v.marker.getPosition());
        path.push(w.marker.getPosition());

        var edge = new Edge(poly);
        edge.start = start;
        edge.end = end;

        start.edges.push(edge);
        end.edges.push(edge);
    }
};

Graph.prototype.getVerticesKeys = function () {
    return Object.keys(this._adjacencyLists);
};

Graph.prototype.getSelected = function () {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].marker.selected) {
            return this._vertexList[i];
        }
    };
    return null;
};

Graph.prototype.findVertexByMarker = function (marker) {
    for (var i = 0; i < this._vertexList.length; i++) {
        if (this._vertexList[i].marker.equals(marker)) {
            return this._vertexList[i];
        }
    };
    return null;
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
            adjString += " " + currentNode.marker.id + "(" + currentNode.marker.selected.toString() + ")";
        }
        console.log(adjString);
        adjString = '';
    }

    adjString = "v:";
    for (var i = 0; i < this._vertexList.length; i++) {
        adjString += " " + this._vertexList[i].marker.id;
    };
    console.log(adjString);
    adjString = '';
};
