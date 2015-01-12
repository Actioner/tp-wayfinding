﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Models.Floor
{
    public class FloorListModel
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int Floor { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}