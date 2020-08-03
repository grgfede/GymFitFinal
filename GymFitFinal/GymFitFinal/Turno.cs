using System;
using System.Collections.Generic;
using System.Text;

namespace GymFitFinal
{
    public class Turno
    {
        public string uidTurno { get; set; }
        public string PalestraIscrizione { get; set; }
        public bool LunMat { get; set; }
        public bool LunPom { get; set; }
        public bool MarMat { get; set; }
        public bool MarPom { get; set; }
        public bool MerMat { get; set; }
        public bool MerPom { get; set; }
        public bool GioMat { get; set; }
        public bool GioPom { get; set; }
        public bool VenMat { get; set; }
        public bool VenPom { get; set; }
        public bool SabMat { get; set; }
        public bool SabPom { get; set; }
        public bool DomMat { get; set; }
        public bool DomPom { get; set; }
        public string Capienza { get; set; }
    }
}
