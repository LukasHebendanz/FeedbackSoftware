﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackSoftware.Classes.Dtos
{
    public class FeedbackDto
    {
        public int Schluessel { get; set; }
        public int KlasseId { get; set; }
        public string Name { get; set; }
        public string FeedbackArt { get; set; }
    }
}
