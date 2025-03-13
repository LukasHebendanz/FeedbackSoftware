using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackSoftware.Classes.Dtos
{
    public class KlasseDto
    {
        public int KlasseId { get; set; }
        public string Name { get; set; }
        public string Jahrgangsstufe { get; set; }
        public string Schuljahr { get; set; }
        public string Abteilung { get; set; }
        public string Fach { get; set; }
    }
}
