using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDBMaps.Models
{
    [DataContract]
    public class OfficeType
    {

        private int officeTypeId;

        [DataMember]
        public int OfficeTypeId
        {
            get { return officeTypeId; }
            set { officeTypeId = value; }
        }

        private string description;

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string code;

        [DataMember]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private string icon;

        [DataMember]
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        private bool? _static;

        [DataMember]
        public bool? Static
        {
            get { return _static; }
            set { _static = value; }
        }

    }
}