using Android.Text.Format;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymFitFinal
{
    public class Gym
    {
        public override string ToString()
        {
            return Nome;
        }

        public static string Email { get; set; }

        public string Nome { get; set; }
        public string Citta { get; set; }

        public string Indirizzo { get; set; }
        public string Telefono{ get; set; }
        public string Uid { get; set; }
        public string profilePic { get; set; }
        public static string citta { get; set; }
        public static string indirizzo { get; set; }
        public static string uid{ get; set; }

        public string DataIMattina { get; set; }
        public string DataFMattina { get; set; }
        public string DataIPomeriggio { get; set; }
        public string DataFPomeriggio { get; set; }
    }
       
}

