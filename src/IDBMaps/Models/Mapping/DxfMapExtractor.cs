using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace IDBMaps.Models.Mapping
{
    public class DxfMapExtractor
    {

        public static Coordinate GetOfficeCoordinates(Building Building, string OfficeNumber)
        {
            //select PDF base in Company and location
            Coordinate result = new Coordinate();
            Coordinate NW = new Coordinate(-1689.760336242332, 202.2125829684866, 38.899411, -77.031286);
            Coordinate SE = new Coordinate(-2990.955017650648, 4782.541538991917, 38.899099, -77.029831);

            int floor=0;
            for (int i = 0; i < 14; i++)
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath(String.Format(@"~/Content/dxf/{0}-{1}.dxf", Building.Name, i));// +pdffilename;

                using (StreamReader sr = new StreamReader(path))
                {
                    Dictionary<String, Coordinate> tempdic = ExtractTextAndPosition(sr);

                    //look for the office coordinates
                    if (tempdic.ContainsKey(OfficeNumber))
                    {
                        result = tempdic[OfficeNumber];
                        floor = i;
                        break;
                    }
                }
                //Convert to Coordinates
            }
            result.FloorMap = FloorMap.GetFloorMap(Building.BuildingId, floor);
           
            result.ConvertToCoord(NW, SE);
            return result;
        }

        public static Coordinate GetFromCoordinates(Building Building, int Floor, double Latitude, double Longitude)
        {
            Coordinate result = null;
            Coordinate NW = new Coordinate(-1689.760336242332, 202.2125829684866, 38.899411, -77.031286);
            Coordinate SE = new Coordinate(-2990.955017650648, 4782.541538991917, 38.899099, -77.029831);


            string path = System.Web.Hosting.HostingEnvironment.MapPath(String.Format(@"~/Content/dxf/{0}-{1}.dxf", Building.Name, Floor));// +pdffilename;
            using (StreamReader sr = new StreamReader(path))
            {
                Dictionary<String, Coordinate> tempdic = ExtractTextAndPosition(sr);
               
                double minDistance = Double.MaxValue;

                foreach (KeyValuePair<String, Coordinate> coor in tempdic.ToList())
                {
                    coor.Value.ConvertToCoord(NW, SE);
                    if (coor.Value.distanceTo(Latitude, Longitude) < minDistance)
                    {
                        minDistance = coor.Value.distanceTo(Latitude, Longitude);
                        result = coor.Value;
                        result.OfficeNumber = coor.Key;
                    }

                }
            }

         
            //Convert to Coordinates
            result.FloorMap = FloorMap.GetFloorMap(Building.BuildingId, Floor);

            result.ConvertToCoord(NW, SE);
            return result;
        }

        public static Dictionary<String, Coordinate> ExtractTextAndPosition(StreamReader sr)
        {
            Dictionary<String, Coordinate> result = new Dictionary<string, Coordinate>();
            string st = "";
            Coordinate p = new Coordinate();

            while (sr.Peek() >= 0)
            {
                String line = sr.ReadLine();
                if (line == "AcDbText")
                {
                    try
                    {
                       
                        sr.ReadLine();//10
                        double y = double.Parse(sr.ReadLine());//X
                        sr.ReadLine();//20
                        double x = double.Parse(sr.ReadLine());//Y
                        sr.ReadLine();//30
                        sr.ReadLine();//0.0
                        sr.ReadLine();//40
                        sr.ReadLine();//12.0
                        sr.ReadLine();//11
                        String officenumber = sr.ReadLine();

                        result.Add(officenumber, new Coordinate(x, y, 0, 0));
                    }
                    catch { }
                }

            }

            return result;
        }

    }
}