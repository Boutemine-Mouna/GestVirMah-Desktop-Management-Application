using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestVirMah.ClassePret;
using System.Data.SqlClient;
using System.Windows;
using System.Data;
using GestVirMah.Classes;
using System.Windows.Controls;

namespace GestVirMah.ClassePret
{
    class RembAnticip
    {
        private SqlConnection conn;
       // public static SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        public RembAnticip(SqlConnection con)
        {
            this.conn = con;
        }
        public  DataTable fonctInfo(String nom, String prenom)
        {

            String cmd = "select SitFamFonct,TelFonct,Matricule,CompteFonct from Fonctionnaire where NomFonct= @nom and PrenFonct= @prenom";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                cmdUser.Parameters.AddWithValue("@nom", nom);
                cmdUser.Parameters.AddWithValue("@prenom", prenom);
                SqlDataAdapter adapter = new SqlDataAdapter(cmdUser);
                DataTable t = new DataTable("resultat");
                adapter.Fill(t);
                return t;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }

        }

        public List<LigneDemandePret> fonctDemandes(String nom, String prenom)
        {

            List<LigneDemandePret> l = new List<LigneDemandePret>();
            String matricule = fonctInfo(nom, prenom).Rows[0]["Matricule"].ToString();
            String cmd = "select NumDemPret,MontantVoulu,CodePret from DemandePret where Matricule=' " + matricule + "'";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader reader = cmdUser.ExecuteReader();
                while (reader.Read())
                {
                    LigneDemandePret ligne = new LigneDemandePret();
                    ligne.NumDem = reader[0].ToString();
                    ligne.MontVoulu = int.Parse(reader[1].ToString());
                    ligne.CodePret = int.Parse(reader[2].ToString());
                    l.Add(ligne);
                }

                return l;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }
        }

        public  void creerDemande(String nom, String prenom, String num, String mont, String per, String RemSur, DatePicker DatAnt, DatePicker DatPrem)
        {

            String dateAnt = (DatAnt.Text.Substring(6, 4) + "/" + DatAnt.Text.Substring(3, 3) + DatAnt.Text.Substring(0, 2)).ToString();
            String datePrem = (DatPrem.Text.Substring(6, 4) + "/" + DatPrem.Text.Substring(3, 3) + DatPrem.Text.Substring(0, 2)).ToString();

            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("INSERT INTO RemboursementAnticipe (DateAnt,RetenuPerAnt,PeriodiciteAnt,NumDemPret,PremiereEcheanceAnt,RemSur) VALUES ('" + dateAnt + "', '" + mont + "', '" + per + "', " + num + ",'" + datePrem + "','" + RemSur + "')", conn);
                /*cmdUser.Parameters.Add("@dateAnt", SqlDbType.Date).Value = DatAnt.SelectedDate.Value.Date.ToString();
                cmdUser.Parameters.Add("@datePrem", SqlDbType.Date).Value = DatPrem.SelectedDate.Value.Date.ToString();*/
                SqlDataReader reader = cmdUser.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public  List<LigneNomPren> fonctionaire(String Nom)
        {
            List<LigneNomPren> l = new List<LigneNomPren>();
            String cmd = "select NomFonct,PrenFonct,CompteFonct from Fonctionnaire where NomFonct +' '+ PrenFonct like '" + Nom + "%' or PrenFonct +' '+ NomFonct like '" + Nom + "%'";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader reader = cmdUser.ExecuteReader();
                while (reader.Read())
                {
                    LigneNomPren ligne = new LigneNomPren();
                    ligne.nom = reader[0].ToString();
                    ligne.prenom = reader[1].ToString();
                    ligne.compte = reader[2].ToString();
                    l.Add(ligne);
                }

                return l;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }


        }

        public  string converti(int chiffre)
        {
            int centaine, dizaine, unite, reste, y;
            bool dix = false;
            string lettre = "";
            //strcpy(lettre, "");

            reste = chiffre;

            for (int i = 1000000000; i >= 1; i /= 1000)
            {
                Console.WriteLine(i);
                y = reste / i;
                if (y != 0)
                {
                    centaine = y / 100;
                    dizaine = (y - centaine * 100) / 10;
                    unite = y - (centaine * 100) - (dizaine * 10);
                    switch (centaine)
                    {
                        case 0:
                            break;
                        case 1:
                            lettre += "cent ";
                            break;
                        case 2:
                            if ((dizaine == 0) && (unite == 0)) lettre += "deux cents ";
                            else lettre += "deux cent ";
                            break;
                        case 3:
                            if ((dizaine == 0) && (unite == 0)) lettre += "trois cents ";
                            else lettre += "trois cent ";
                            break;
                        case 4:
                            if ((dizaine == 0) && (unite == 0)) lettre += "quatre cents ";
                            else lettre += "quatre cent ";
                            break;
                        case 5:
                            if ((dizaine == 0) && (unite == 0)) lettre += "cinq cents ";
                            else lettre += "cinq cent ";
                            break;
                        case 6:
                            if ((dizaine == 0) && (unite == 0)) lettre += "six cents ";
                            else lettre += "six cent ";
                            break;
                        case 7:
                            if ((dizaine == 0) && (unite == 0)) lettre += "sept cents ";
                            else lettre += "sept cent ";
                            break;
                        case 8:
                            if ((dizaine == 0) && (unite == 0)) lettre += "huit cents ";
                            else lettre += "huit cent ";
                            break;
                        case 9:
                            if ((dizaine == 0) && (unite == 0)) lettre += "neuf cents ";
                            else lettre += "neuf cent ";
                            break;
                    }// endSwitch(centaine)

                    switch (dizaine)
                    {
                        case 0:
                            break;
                        case 1:
                            dix = true;
                            break;
                        case 2:
                            lettre += "vingt ";
                            break;
                        case 3:
                            lettre += "trente ";
                            break;
                        case 4:
                            lettre += "quarante ";
                            break;
                        case 5:
                            lettre += "cinquante ";
                            break;
                        case 6:
                            lettre += "soixante ";
                            break;
                        case 7:
                            dix = true;
                            lettre += "soixante ";
                            break;
                        case 8:
                            lettre += "quatre-vingt ";
                            break;
                        case 9:
                            dix = true;
                            lettre += "quatre-vingt ";
                            break;
                    } // endSwitch(dizaine)

                    switch (unite)
                    {
                        case 0:
                            if (dix) lettre += "dix ";
                            break;
                        case 1:
                            if (dix) lettre += "onze ";
                            else lettre += "un ";
                            break;
                        case 2:
                            if (dix) lettre += "douze ";
                            else lettre += "deux ";
                            break;
                        case 3:
                            if (dix) lettre += "treize ";
                            else lettre += "trois ";
                            break;
                        case 4:
                            if (dix) lettre += "quatorze ";
                            else lettre += "quatre ";
                            break;
                        case 5:
                            if (dix) lettre += "quinze ";
                            else lettre += "cinq ";
                            break;
                        case 6:
                            if (dix) lettre += "seize ";
                            else lettre += "six ";
                            break;
                        case 7:
                            if (dix) lettre += "dix-sept ";
                            else lettre += "sept ";
                            break;
                        case 8:
                            if (dix) lettre += "dix-huit ";
                            else lettre += "huit ";
                            break;
                        case 9:
                            if (dix) lettre += "dix-neuf ";
                            else lettre += "neuf ";
                            break;
                    } // endSwitch(unite)

                    switch (i)
                    {
                        case 1000000000:
                            if (y > 1) lettre += "milliards ";
                            else lettre += "milliard ";
                            break;
                        case 1000000:
                            if (y > 1) lettre += "millions ";
                            else lettre += "million ";
                            break;
                        case 1000:
                            if (y > 1) lettre += "milles ";
                            else lettre = "mille ";
                            break;
                    }
                } // end if(y!=0)
                reste -= y * i;
                dix = false;
            } // end for
            if (lettre.Length == 0) lettre += "zero";

            return lettre;

        }
    }
}
