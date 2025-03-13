using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackSoftware.Classes.Dtos
{
    public class FormularDto
    {
        public FormularDto() { }

        public int FormularId { get; set; }
        public int Schluessel {  get; set; }
        public string Data { get; set; }

    }
}
