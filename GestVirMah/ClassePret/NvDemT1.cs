﻿using System;
using System.Collections.Generic;
using GestVirMah.ClassePret;
using GestVirMah.Classes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Data;
using System.Windows.Controls;


namespace GestVirMah.ClassePret
{
    class NvDemT1
    {
        private SqlConnection conn;
       // public static SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
     public NvDemT1(SqlConnection con)
        {
            this.conn = con;
        }
        public  List<String> typePret()
        {

            List<String> l = new List<string>();
            String cmd = "select DesignationPret from TypePret where TypePret = '1' ";
            try
            {
                conn.Open();

                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();
                while (ReadUser.Read())
                {
                    l.Add(ReadUser["DesignationPret"].ToString());
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

        public  DataTable fonctInfo(String nom, String prenom)
        {

            String cmd = "select SitFamFonct,TelFonct,Matricule,CompteFonct,GradeFonct,FonctionFonct,AdresseFonct from Fonctionnaire where NomFonct= @nom and PrenFonct= @prenom";
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

        public  void creerDemande(String nom, String prenom, String montant, String Periodicite, String typePret, DatePicker DatDem , String rt)
        {

            String matricule = fonctInfo(nom, prenom).Rows[0]["Matricule"].ToString();
            String codePret = getCodePret(typePret);
            String dateDem = (DatDem.Text.Substring(6, 4) + "/" + DatDem.Text.Substring(3, 3) + DatDem.Text.Substring(0, 2)).ToString();
            //MessageBox.Show(dateDem);
            String num = numDem(dateDem).ToString();
            //MessageBox.Show(num);
           // String etat = "E";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("INSERT INTO DemandePret (NumDemPret, DateDemPret,MontantVoulu,Periodicite,Matricule,CodePret,RetenuPeriodique) VALUES (" + num + ", @dateDem," + montant + "," + Periodicite + "," + matricule + "," + codePret + ","+rt+")", conn);
                cmdUser.Parameters.Add("@dateDem", SqlDbType.Date).Value = DatDem.SelectedDate.Value.Date.ToString();
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

        public  String getCodePret(String typepret)
        {
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("select CodePret from TypePret where DesignationPret ='" + typepret + "'");
                cmdUser.Connection = conn;
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();
                return ReadUser["CodePret"].ToString();
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

        public  String maxNumDem(String date)
        {
            String cmd = "select MAX(NumDemPret) as NumDem from DemandePret where DateDemPret like '" + date + "%'";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();

                if (ReadUser["NumDem"] != DBNull.Value)
                {
                    return ReadUser["NumDem"].ToString();
                }

                else return null;
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

        public  int numDem(String d)
        {
            int date = int.Parse(d.Substring(2, 2));
            String date2 = d.Substring(0, 4);
            int maxNum = 0;
            if (maxNumDem(date2) == null)
            {
                maxNum = 0;
            }
            else
            {
                if (int.Parse(maxNumDem(date2).Substring(0, 2)) < date)
                {
                    maxNum = 0;
                }
                else
                {
                    maxNum = (int.Parse(maxNumDem(date2)) % 1000) + 1;
                }
            }
            int i = ((date * 1000) + maxNum);
            return i;
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