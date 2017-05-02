using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestVirMah.Classes
{
    public class Utilisateur
    {
        private int code;
        private String nom;
        private String identifiant;
        private char droits;
        private String prenom;

        public int Code
        {
            get { return code; }
            set { code = value; }
        }
        
        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public String Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public String Identifiant
        {
            get { return identifiant; }
            set { identifiant = value; }
        }

        public char Droits
        {
            get { return droits; }
            set { droits = value; }
        }

        public Utilisateur(int code, String nom, String prenom, String identifiant, char droits)
        {
            this.code = code;
            this.nom = nom;
            this.prenom = prenom;
            this.identifiant = identifiant;
            this.droits = droits;
        }
    }
}
