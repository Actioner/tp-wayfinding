using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfSharp;
using System.IO;
using System.Drawing;

namespace IDBMaps.Controllers
{
    public class PDFController : Controller
    {
        //
        // GET: /PDF/

        public ActionResult Index()
        {
            return View();
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowByOffice(string Company, string Location, string OfficeNumber, int? CoorX, int? CoorY)
        {
            string filename = Server.MapPath(@"~/Content/pdf/1300_Floorplans -Floor2.pdf");
            string filenameafter = Server.MapPath(@"~/Content/pdf/Mark1300_Floorplans -Floor2-a.pdf");
            string avatar = Server.MapPath(@"~/Images/Marker.png");

            PdfDocument document = CompatiblePdfReader.Open(filename, PdfDocumentOpenMode.Modify);
            PdfPage page = document.Pages[0];
            page.Size = PageSize.Size17x11;
           
            XGraphics gfx = XGraphics.FromPdfPage(page,XGraphicsUnit.Point);
            XImage image = XImage.FromFile(avatar);

           
             PDFTextExtractor ext =  new PDFTextExtractor();
             PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(0).Stream;
             Dictionary<string, Point> offices = ext.ExtractTextAndPosition(stream.Value);

             Point coor = offices["(SE0239)"];

             double x = (ext.ConvertToPointX(coor.X));
             double y = (ext.ConvertToPointY(coor.Y) - (25) / 2);

             gfx.DrawImage(image, x , y,25,25);
            document.Save(filenameafter);

            /*String outputText = "";

            try
            {
                PdfDocument inputDocument = PdfReader.Open(filenameafter, PdfDocumentOpenMode.ReadOnly);

                foreach (PdfPage page in inputDocument.Pages)
                {
                    for (int index = 0; index < page.Contents.Elements.Count; index++)
                    {
                        PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(index).Stream;
                        outputText = new PDFTextExtractor().ExtractTextFromPDFBytes(stream.Value);

                        streamWriter.WriteLine(outputText);
                    }
                }

            }
            catch (Exception e)
            {

            }
            streamWriter.Close();*/
            /*
            
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);//, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.DocumentRenderer = new DocumentRenderer(new MigraDoc.DocumentObjectModel.Document());;
            renderer.PdfDocument = document;
            renderer.Document = new Document();
            renderer.RenderDocument();

            List<DocumentObject> docObjects = renderer.DocumentRenderer.GetDocumentObjectsFromPage(0).ToList();
            */
            /*
            

            Map map = Map.GetMap(Company, Location, OfficeNumber, avatar);

            if (map != null)
            {
                map.ImageFolder = Server.MapPath(@"~/Images/Maps/");
                return map.generateImage(CoorX, CoorY);
            }
            */

            return View();
        }
    }



    public class PDFTextExtractor
    {
        /// BT = Beginning of a text object operator 
        /// ET = End of a text object operator
        /// Td move to the start of next line
        ///  5 Ts = superscript
        /// -5 Ts = subscript

        #region Fields

        #region _numberOfCharsToKeep
        /// <summary>
        /// The number of characters to keep, when extracting text.
        /// </summary>
        private static int _numberOfCharsToKeep = 100;
        #endregion

        #endregion



        #region ExtractTextFromPDFBytes

        public Dictionary<String, Point> ExtractTextAndPosition(byte[] input)
        {
            String text = System.Text.Encoding.UTF8.GetString(input);
            StringReader reader = new StringReader(text);
            Dictionary<String, Point> result = new Dictionary<string, Point>();
            string st = "";
            Point p = new Point();
            bool inText = false;
            while (true)
            {
                
                string line = reader.ReadLine();
                if (line != null)
                {
                    // process string
                    if (line == "ET")
                    {
                        result.Add(st, p);
                        inText = false;
                    }

                    if (inText)
                    {
                        String[] words = line.Split(' ');

                        if (words.Length > 6)
                        {
                            if (words[6] == "Tm")
                            {
                                p.X = int.Parse(words[4]);
                                p.Y = int.Parse(words[5]);
                            }
                        }
                        if (words.Length > 1)
                        {
                            if (words[1] == "Tj")
                            {
                                st = words[0];
                            }
                        }


                    }

                    if (line == "BT")
                    {
                        st = "";
                        p = new Point();
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

        #endregion

        #region CheckToken
        /// <summary>
        /// Check if a certain 2 character token just came along (e.g. BT)
        /// </summary>
        /// <param name="search">the searched token</param>
        /// <param name="recent">the recent character array</param>
        /// <returns></returns>
        private bool CheckToken(string[] tokens, char[] recent)
        {
            foreach (string token in tokens)
            {
                if (token.Length > 1)
                {
                    if ((recent[_numberOfCharsToKeep - 3] == token[0]) &&
                        (recent[_numberOfCharsToKeep - 2] == token[1]) &&
                        ((recent[_numberOfCharsToKeep - 1] == ' ') ||
                        (recent[_numberOfCharsToKeep - 1] == 0x0d) ||
                        (recent[_numberOfCharsToKeep - 1] == 0x0a)) &&
                        ((recent[_numberOfCharsToKeep - 4] == ' ') ||
                        (recent[_numberOfCharsToKeep - 4] == 0x0d) ||
                        (recent[_numberOfCharsToKeep - 4] == 0x0a))
                        )
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }
            return false;
        }

        public float ConvertToPointX(float pointX)
        {
            return pointX * 792 / 6550;
        }

        public float ConvertToPointY(float pointY)
        {
            return 1224 - (pointY * 1224 / 10200);
        }
        #endregion
    }
}
