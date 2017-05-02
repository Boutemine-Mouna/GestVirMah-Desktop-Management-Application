using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestVirMah.Classes
{
    public class LigneHistorique
    {
        public string Fonctionnaire { get; set; }
		public DateTime DateDem { get; set; }
        public string TypePrime { get; set; }
        //public int NumDem { get; set; }       
        //public float Montant { get; set; }
        //public string CompteFonct { get; set; }
        public string EtatDem { get; set; }
        public string Motif { get; set; }
    }
}
