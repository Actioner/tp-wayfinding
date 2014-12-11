using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Simple.Data;

namespace IDBMaps.Models.Mapping
{
    public class DxfMapExtractor
    {

        public static void ImportOfficeLocations()
        {
            //select PDF base in Company and location
            Coordinate result = new Coordinate();
            //Coordinate NW = new Coordinate(-1689.760336242332, 202.2125829684866, 38.899411, -77.031286);
           // Coordinate SE = new Coordinate(-2990.955017650648, 4782.541538991917, 38.899099, -77.029831);

            //2
            //Coordinate NW = new Coordinate(-2378.98571036, 189.36051794, 38.899411, -77.031286);
            //Coordinate SE = new Coordinate(-3700.38150051, 4848.17647373, 38.899099, -77.029831);

            //3
            // for IDB
            Coordinate NW = new Coordinate(1740.61864691, 160.04212071, 38.899411, -77.031286);
            Coordinate SE = new Coordinate(445.52390402, 4822.58882054, 38.899099, -77.029831);

            int BuildingId = 2;
            for (int i = 11; i < 13; i++)
            {
                var db = Database.Open();
                FloorMap floorMap = db.FloorMap.Find(db.FloorMap.BuildingId == BuildingId && db.FloorMap.Floor == i);

                string path = String.Format(@"D:\\DATA.IDB\\Documents\\Projects\\IDBMaps\\IDBMaps\\IDBMaps.Import\\dxf\\{0}-{1}.dxf", "IDB", i);// +pdffilename;

                using (StreamReader sr = new StreamReader(path))
                {
                    Dictionary<String, Coordinate> tempdic = ExtractTextAndPosition(sr);

                    foreach (KeyValuePair<String, Coordinate> item in tempdic)
                    {
                        item.Value.ConvertToCoord(GetNWCoordinate(i), GetSECoordinate(i));
                        Office office = new Office();
                        office.OfficeNumber = item.Key.ToUpper();
                        office.Latitude = item.Value.Latitude;
                        office.Longitude = item.Value.Longitude;
                        office.FloorMapId = floorMap.FloorMapId;

                        db.Office.Insert(office); 
                    }
                }
                //Convert to Coordinates
            }
           
            
        }

        public static Coordinate GetNWCoordinate(int floor)
        {
            switch(floor)
            {
                case 0:
                    return new Coordinate(1568.0, 6879.19443848, 38.899444, -77.030980);
                case 1:
                    //return new Coordinate(1571.19367872, 223.24384077, 38.899417, -77.031227);
                    return new Coordinate(1571.19367872, 223.24384077, 38.899447, -77.031178);
                case 2:
                    //return new Coordinate(-3700.38150051, 4848.17647373, 38.899099, -77.029831);
                    return new Coordinate(-2378.98571036, 189.36051794, 38.899444, -77.031185);
                case 3:
                    //return new Coordinate(445.52390402, 4822.58882054, 38.899099, -77.029831);
                    return new Coordinate(1740.61864691, 160.04212071, 38.899448, -77.031195);
                case 4:
                    //return new Coordinate(-1683.76033624, 234.5745152, 38.899413, -77.031288);//REVIW
                    return new Coordinate(-1683.76033624, 234.5745152, 38.899440, -77.031187);
                case 5:
                    //return new Coordinate(1541.7100123, 200.55495972, 38.899411, -77.031286);.
                    return new Coordinate(1541.7100123, 200.55495972, 38.899449, -77.031184);
                case 6:
                    //return new Coordinate(1548.86966477, 200.55495642, 38.899411, -77.031286);
                    return new Coordinate(1548.86966477, 200.55495642, 38.899448, -77.031182);
                case 7:
                    //return new Coordinate(2378.3032212, 2356.63902812, 38.899620, -77.030606);
                    return new Coordinate(2378.3032212, 2356.63902812, 38.899640, -77.030559);
                case 8:
                    //return new Coordinate(1476.8646555, 208.3133285, 38.899413, -77.031284);
                    return new Coordinate(1476.8646555, 208.3133285, 38.899444, -77.031184);
                case 9:
                   // return new Coordinate(1485.86297034, 142.41729594, 38.899404, -77.031298);
                    return new Coordinate(1485.86297034, 142.41729594, 38.899436, -77.031198);
                case 10:
                    //return new Coordinate(1590.22139942, 262.47535492, 38.899428, -77.031269);//REVIW
                    return new Coordinate(1590.22139942, 262.47535492, 38.899456, -77.031163);//REVIW
                case 11:
                    //return new Coordinate(1510.57453529, 438.15000054, 38.899410, -77.031282);
                    return new Coordinate(1510.57453529, 438.15000054, 38.899438, -77.031183);
                case 12:
                    //return new Coordinate(1525.61833727, 230.50033771, 38.899414, -77.031277);
                    return new Coordinate(1525.61833727, 230.50033771, 38.899444, -77.031179);

            }
            return null;
        }

        public static Coordinate GetSECoordinate(int floor)
        {
            switch (floor)
            {
                case 0:
                    return new Coordinate(437.5, 10502.05371305, 38.899194, -77.029887);
                case 1:
                    // return new Coordinate(382.08041164, 4739.61855618, 38.899133, -77.029858);
                    return new Coordinate(382.08041164, 4739.61855618, 38.899178, -77.029860);
                case 2:
                    //return new Coordinate(-2378.98571036, 189.36051794, 38.899411, -77.031286);
                    return new Coordinate(-3700.38150051, 4848.17647373 , 38.899145, -77.029830);
                case 3:
                    //return new Coordinate(1740.61864691, 160.04212071, 38.899411, -77.031286);
                    return new Coordinate(445.52390402, 4822.58882054,  38.899162, -77.029837);
                case 4:
                    //return new Coordinate(-2990.95501765, 4782.54153899, 38.899102, -77.029843);
                    return new Coordinate(-2990.95501765, 4782.54153899, 38.899149, -77.029854);
                case 5:
                    //return new Coordinate(215.28816272, 4771.94678335, 38.899098, -77.029845);
                    return new Coordinate(215.28816272, 4771.94678335, 38.899148, -77.029859);
                case 6:
                    //return new Coordinate(222.4478152, 4771.94678004, 38.899098, -77.029845);
                    return new Coordinate(222.4478152, 4771.94678004, 38.899147, -77.029852);
                case 7:
                    //return new Coordinate(221.0185179, 4726.40280853, 38.899100, -77.029859);
                    return new Coordinate(221.0185179, 4726.40280853, 38.899153, -77.029867);
                case 8:
                    //return new Coordinate(150.248616, 4775.5728078, 38.899094, -77.029845);
                    return new Coordinate(150.248616, 4775.5728078, 38.899142, -77.029852);
                case 9:
                    //return new Coordinate(229.33823778, 4812.67021518, 38.899100, -77.029832);
                    return new Coordinate(229.33823778, 4812.67021518, 38.899149, -77.029843);
                case 10:
                   //return new Coordinate(240.95919711, 4809.41721694, 38.899104, -77.029832);
                    return new Coordinate(240.95919711, 4809.41721694, 38.899153, -77.029840);
                case 11:
                    //return new Coordinate(240.95919711, 4809.41721694, 38.899104, -77.029832);
                    return new Coordinate(240.95919711, 4809.41721694, 38.899149, -77.029857);
                case 12:
                    //return new Coordinate(215.68059256, 4836.88517461, 38.899095, -77.029825);
                    return new Coordinate(215.68059256, 4836.88517461, 38.899147, -77.029830);

            }
            return null;
        }

        public static bool isOffice(string text)
        {
            return text.StartsWith("NW") || text.StartsWith("NE") || text.StartsWith("SW") || text.StartsWith("SE");
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

                        if( isOffice(officenumber))
                            result.Add(officenumber, new Coordinate(x, y, 0, 0));
                    }
                    catch { }
                }

            }

            return result;
        }

    }
    //update office set Latitude = Latitude - 0.0000846449225 , Longitude = Longitude - 0.0000236943905 where FloorMapId=11 
}