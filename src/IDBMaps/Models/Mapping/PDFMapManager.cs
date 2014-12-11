using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp;
using System.IO;
using PdfSharp.Pdf.Advanced;

namespace IDBMaps.Models.Mapping
{
    public class PDFMapManager
    {

        public static Coordinate GetOfficeCoordinates(Building Building, string OfficeNumber)
        {
            //select PDF base in Company and location
            string path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/pdf/") + Building.Name + ".pdf";
            PdfDocument document = PdfReader.Open(path, PdfDocumentOpenMode.Modify);
            Coordinate result= new Coordinate();

            //get Page base in the OfficeNumber
            int i=0;
            foreach (PdfPage page in document.Pages)
            {
                PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(0).Stream;
                Dictionary<String, Coordinate> tempdic = ExtractTextAndPosition(stream.Value, i);
               
                //look for the office coordinates
                if (tempdic.ContainsKey(OfficeNumber))
                {
                    result = tempdic[OfficeNumber];               
                    break;
                }
                i++;

            }

            result.FloorMap = FloorMap.GetFloorMap(Building.BuildingId, i);

            //Convert to Coordinates
            Coordinate NW = new Coordinate(3680, 9207, 38.899411,-77.031286);
            Coordinate SE =  new Coordinate(1615, 1927 , 38.899099,-77.029831);

            result.ConvertToCoord(NW,SE);
            result.OfficeNumber = OfficeNumber.ToUpper();
            return result;
        }

        public static Coordinate GetFromCoordinates(Building Building, int Floor, double Latitude, double Longitude)
        {
            //select PDF base in Company and location
            string path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/pdf/") + Building.Name + ".pdf";
            PdfDocument document = PdfReader.Open(path, PdfDocumentOpenMode.Modify);
            Coordinate result = null;

            //get Page base in the OfficeNumber
            int pageIndex = Floor;
            PdfPage page = document.Pages[pageIndex];
            
            PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(0).Stream;
            Dictionary<String, Coordinate> tempdic = ExtractTextAndPosition(stream.Value, pageIndex);

            Coordinate NW = new Coordinate(3680, 9207, 38.899411, -77.031286);
            Coordinate SE = new Coordinate(1615, 1927, 38.899099, -77.029831);

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
            
            //Convert to Coordinates
            result.FloorMap = FloorMap.GetFloorMap(Building.BuildingId, Floor);

            result.ConvertToCoord(NW, SE);
            return result;
        }

        public static Dictionary<String, Coordinate> ExtractTextAndPosition(byte[] input, int page)
        {

         //   File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/pdf/") + page + ".txt", input);

            String text = System.Text.Encoding.UTF8.GetString(input);
            StringReader reader = new StringReader(text);
            Dictionary<String, Coordinate> result = new Dictionary<string, Coordinate>();
            string st = "";
            Coordinate p = new Coordinate();
            bool inText = false;
            while (true)
            {

                string line = reader.ReadLine();
                if (line != null)
                {
                    // process string
                    if (line == "ET")
                    {
                        if (!result.ContainsKey(st))
                        {
                            result.Add(st, p);
                        }
                        inText = false;
                    }

                    if (inText)
                    {
                        String[] words = line.Split(' ');

                        if (words.Length > 6)
                        {
                            if (words[6] == "Tm")
                            {
                                p.X = double.Parse(words[4]);
                                p.Y = double.Parse(words[5]);
                            }
                        }
                        if (words.Length > 1)
                        {
                            if (words[1] == "Tj")
                            {
                                st = words[0].Replace("(","").Replace(")","");
                            }
                        }


                    }

                    if (line == "BT")
                    {
                        st = "";
                        p = new Coordinate();
                        inText = true;
                    }

                }
                else
                {
                    break;
                }
            }

            return result;
        }
    }
}