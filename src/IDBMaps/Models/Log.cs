using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple.Data;

namespace IDBMaps.Models
{
    public class Log
    {

        public static void CreateLog(String DeviceName, String Description)
        {
            var db = Database.Open();
            db.Log.Insert(DeviceName: DeviceName, Description: Description,EntryTime: DateTime.Now);    
        }
    }
}