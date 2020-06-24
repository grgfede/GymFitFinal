using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GymFitFinal
{
    public class User
    {


        public static string Email { get; set; }
        public bool flagGym { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        public string Uid { get; set; }
        public string profilePic { get; set; }
      
    }
}
