using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple.Data;

namespace IDBMaps.Models.Routing
{
    public class RouteEngine
    {

        List<Connection> _connections;
        List<Node> _nodes;

        public List<Node> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }
        public List<Connection> Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        public RouteEngine()
        {
            _connections = new List<Connection>();
            _nodes = new List<Node>();
        }



        /// <summary>
        /// Calculates the shortest route to all the other locations
        /// </summary>
        /// <param name="_startLocation"></param>
        /// <returns>List of all locations and their shortest route</returns>
        public Dictionary<String, Route> CalculateMinCost(Node _startLocation)
        {
            //Initialise a new empty route list
            Dictionary<String, Route> _shortestPaths = new Dictionary<String, Route>();
            //Initialise a new empty handled locations list
            List<String> _handledLocations = new List<String>();

            //Initialise the new routes. the constructor will set the route weight to in.max
            foreach (Node location in _nodes)
            {
                _shortestPaths.Add(location.NodeId.ToString(), new Route(location.NodeId.ToString()));
            }

            //The startPosition has a weight 0. 
            _shortestPaths[_startLocation.NodeId.ToString()].Cost = 0;


            //If all locations are handled, stop the engine and return the result
            while (_handledLocations.Count != _nodes.Count)
            {
                //Order the locations
                List<String> _shortestLocations = (List<String>)(from s in _shortestPaths
                                                                 orderby s.Value.Cost
                                                                 select s.Key).ToList();


                String _locationToProcess = null;

                //Search for the nearest location that isn't handled
                foreach (String _location in _shortestLocations)
                {
                    if (!_handledLocations.Contains(_location))
                    {
                        //If the cost equals int.max, there are no more possible connections to the remaining locations
                        if (_shortestPaths[_location].Cost == int.MaxValue)
                            return _shortestPaths;
                        _locationToProcess = _location;
                        break;
                    }
                }

                //Select all connections where the startposition is the location to Process
                var _selectedConnections = from c in _connections
                                           where c.NodeA.NodeId.ToString() == _locationToProcess || c.NodeB.NodeId.ToString() == _locationToProcess
                                           select c;

                //Iterate through all connections and search for a connection which is shorter
                foreach (Connection conn in _selectedConnections)
                {
                    try
                    {
                        if (_shortestPaths[conn.NodeB.NodeId.ToString()].Cost > conn.Weight + _shortestPaths[conn.NodeA.NodeId.ToString()].Cost)
                        {
                            _shortestPaths[conn.NodeB.NodeId.ToString()].Connections = _shortestPaths[conn.NodeA.NodeId.ToString()].Connections.ToList();
                            _shortestPaths[conn.NodeB.NodeId.ToString()].Connections.Add(conn);
                            _shortestPaths[conn.NodeB.NodeId.ToString()].Cost = conn.Weight + _shortestPaths[conn.NodeA.NodeId.ToString()].Cost;
                        }
                        if (_shortestPaths[conn.NodeA.NodeId.ToString()].Cost > conn.Weight + _shortestPaths[conn.NodeB.NodeId.ToString()].Cost)
                        {
                            _shortestPaths[conn.NodeA.NodeId.ToString()].Connections = _shortestPaths[conn.NodeB.NodeId.ToString()].Connections.ToList();
                            _shortestPaths[conn.NodeA.NodeId.ToString()].Connections.Add(conn.Invert());
                            _shortestPaths[conn.NodeA.NodeId.ToString()].Cost = conn.Weight + _shortestPaths[conn.NodeB.NodeId.ToString()].Cost;
                        }
                    }
                    catch
                    {

                    }
                }
                //Add the location to the list of processed locations
                _handledLocations.Add(_locationToProcess);
            }


            return _shortestPaths;
        }




        public List<FloorConnection> Calculate(int toFloorMapId, int fromFloorMapId, double latitueS, double longitudeS, double latitueE, double longitudeE)
        {
            //Get Nodes
            Coordinate CoordStart = new Coordinate(latitueS, longitudeS);
            Coordinate CoordEnd = new Coordinate(latitueE, longitudeE);

            var db = Database.Open();

            this.Nodes = db.Node.FindAllByFloorMapId(toFloorMapId).Where(db.Node.FloorConnector == false);
            if(fromFloorMapId != toFloorMapId)
                this.Nodes.AddRange((List<Node>)db.Node.FindAllByFloorMapId(fromFloorMapId).Where(db.Node.FloorConnector == false));
            this.Nodes.AddRange((List<Node>)db.Node.All().Where(db.Node.FloorConnector == true));

            if (this.Nodes.Count == 0)
                return null;

            //get closer nodes      
            this.Connections = db.Connection.FindAllByFloorMapId(toFloorMapId).Where(db.Connection.FloorConnection == false);//.With(db.Connection.NodeA.As("NodeA")).With(db.Connection.NodeB.As("NodeB"));
            if (fromFloorMapId != toFloorMapId)
                this.Connections.AddRange((List<Connection>)(db.Connection.FindAllByFloorMapId(fromFloorMapId).Where(db.Connection.FloorConnection == false)));
            this.Connections.AddRange((List<Connection>)(db.Connection.All().Where(db.Connection.FloorConnection == true)));
            this.Connections.ForEach(c => { c.NodeA = this.Nodes.Where(n => n.NodeId == c.NodeAId).FirstOrDefault(); c.NodeB = this.Nodes.Where(n => n.NodeId == c.NodeBId).FirstOrDefault(); });


            Node startNode = SplitConnection(CoordStart, "START", fromFloorMapId);
            Node endNode = SplitConnection(CoordEnd, "END", toFloorMapId);

            if (!this.Nodes.Exists(c => c.NodeId == startNode.NodeId))
                this.Nodes.Add(startNode);
            if (!this.Nodes.Exists(c => c.NodeId == endNode.NodeId))
                this.Nodes.Add(endNode);

            Dictionary<String, Route> _shortestPaths = this.CalculateMinCost(startNode);

            List<String> _shortestLocations = (List<String>)(from s in _shortestPaths
                                                             orderby s.Value.Cost
                                                             select s.Key).ToList();

            List<Connection> allConnection = _shortestPaths[endNode.NodeId.ToString()].Connections;
            List<FloorConnection> result = new List<FloorConnection>();

            //merge connection between floors
            for (int i = 0; i < allConnection.Count; i++)
            {
                if (allConnection[i].FloorConnection && allConnection[i + 1].FloorConnection)
                {
                    allConnection[i].NodeB = allConnection[i + 1].NodeB;
                    allConnection.RemoveAt(i + 1);
                    i--;
                }
            }

            //Get the Floor Connecion
            FloorConnection floorconnection = null;
            foreach (Connection con in allConnection)
            {
                if (floorconnection == null || floorconnection.FloorMap.FloorMapId != con.NodeA.FloorMapId)
                {

                    floorconnection = new FloorConnection();
                    floorconnection.FloorMap = db.FloorMap.Get(con.NodeA.FloorMapId);
                    floorconnection.Route.Add(con);
                    result.Add(floorconnection);
                }
                else
                {
                    floorconnection.Route.Add(con);
                }
            }

            return result;
        }

        private Node SplitConnection(Coordinate Coord, string identifier, int floorMapId)
        {
            double minDistance = double.MaxValue;
            Connection minConnection = null;
            foreach (Connection con in this.Connections.Where(c => c.FloorMapId == floorMapId).ToList())
            {
                double distance = con.DistanceToPoint(Coord);
                if (distance < minDistance)
                {
                    minConnection = con;
                    minDistance = distance;
                }
            }

            List<Connection> split = minConnection.Split(Coord, identifier);
            int tempNodeId = -1;
            if (identifier == "START")
                tempNodeId = -2;

            split[0].NodeA.FloorMapId = floorMapId;
            split[0].NodeB.FloorMapId = floorMapId;
            split[0].NodeB.NodeId = tempNodeId;
            split[1].NodeA.FloorMapId = floorMapId;
            split[1].NodeA.NodeId = tempNodeId;
            split[1].NodeB.FloorMapId = floorMapId;

            this.Connections.AddRange(split);
            this.Connections.Remove(minConnection);

            return split[0].NodeB;

        }
    }
}