using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Exchange.WebServices.Data;
using System.Configuration;
using System.Net;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Helpers.Mock
{
    public class CalendarManager
    {
        private static Random rng = new Random();

        public static List<Marker> GetAppointments(List<Marker> marker)
        {
           
           
            foreach (Marker office in marker)
            {
                if (office.Type.Code == "EXECUTIVE_CONF")
                {
                    office.Status = "BUSY";
                    office.Detail = "Busy";
                }
                else
                {

                    
                    bool randomBool = rng.Next(1, 4) > 2;

                    if (!randomBool)
                    {
                        office.Status = "FREE";
                        office.Detail = "Free";
                    }
                    else
                    {
                        office.Status = "BUSY";
                        office.Detail = "Busy";
                    }


                }
            }

            return marker;
        }
    }
}