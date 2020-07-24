using System;
using System.Collections.Generic;
using System.Text;

namespace GymFitFinal
{
    public class Abbomamento
    {
        public string uid { get; set; }
        public string TipoAbbonamento { get; set; }
        public double Costo { get; set; }
        public string DataInizio { get; set; }
        public string DataFine { get; set; }

        public static implicit operator List<object>(Abbomamento v)
        {
            throw new NotImplementedException();
        }
    }
}
