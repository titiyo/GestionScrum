﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionScrumV3.Entity
{
    public class MeetingType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}