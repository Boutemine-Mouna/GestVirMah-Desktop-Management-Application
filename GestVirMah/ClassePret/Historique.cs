using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows;
using GestVirMah.Classes;


namespace GestVirMah.ClassePret
{
    class Historique
    {
        private SqlConnection con;
       // public static SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
            static SqlCommand cmd = new SqlCommand();
            static DataTable dt;
        public Historique(SqlConnection conn)
            {
                this.con = conn;
            }
            public  DataTable parPersonne(string nom,string prenom)
            {
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Matricule FROM Fonctionnaire WHERE NomFonct ='" +nom+ "' AND PrenFonct ='" +prenom+"'";
                cmd.ExecuteNonQuery();
                dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                try
                {
                    string matricule = dt.Rows[0][0].ToString();
                    DataTable dt1 = new DataTable();
                    cmd.CommandText = "SELECT NumDemPret, MontantVoulu AS [Montant voulu], MontantAcc as [Montant accordé], Etat, Motif, Matricule FROM DemandePret WHERE (Matricule ='" + matricule + "') AND (Etat = 'A' OR Etat = 'B' OR Etat = 'S') ";
                    cmd.ExecuteNonQuery();
                    sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt1);
                    return dt1;
                }
                catch (Exception) 
                { 
                    MessageBox.Show("Le nom et/ou le prénom est incorrecte");
                    dt = new DataTable();
                    return dt;
                }
                finally { con.Close(); };
            }

        public  DataTable parType(string t)
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CodePret FROM TypePret WHERE TypePret = '" + t + "'";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);

            DataTable dt1 = new DataTable();
            string ma_variable;
            foreach (DataRow row in dt.Rows)
            {
                ma_variable = row["CodePret"].ToString();
                cmd.CommandText = "SELECT NumDemPret, MontantVoulu AS [Montant voulu], MontantAcc as [Montant accordé], Etat, Motif, Matricule FROM DemandePret WHERE (CodePret ='" + ma_variable + "') AND (Etat = 'A' OR Etat = 'B' OR Etat = 'S') ";
                cmd.ExecuteNonQuery();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt1);
            };
            con.Close();
            return dt1;
        }

        public  DataTable parPériode(DatePicker datedeb, DatePicker datefin)  
        {
 
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT NumPrecompte,DatePrecompte,MontantPrecompte,MotifPrecpte, NumDemPret FROM Précompte WHERE DatePrecompte >= @datedebut and DatePrecompte <= @datefin";
            cmd.Parameters.Add("@datedebut",SqlDbType.Date).Value = datedeb.SelectedDate.Value.Date.ToString();
            cmd.Parameters.Add("@datefin",SqlDbType.Date).Value = datefin.SelectedDate.Value.Date.ToString();
            cmd.ExecuteNonQuery();

            dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

       public  DataTable parPrêt(string prêt)
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CodePret FROM TypePret WHERE DesignationPret = '" + prêt + "'";
            dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            string ma_variable = dt.Rows[0][0].ToString();
            DataTable dt1 = new DataTable();
            cmd.CommandText = "SELECT NumDemPret, MontantVoulu AS [Montant voulu], MontantAcc as [Montant accordé], Etat, Motif, Matricule FROM DemandePret WHERE (CodePret ='" + ma_variable + "') AND (Etat = 'A' OR Etat = 'B' OR Etat = 'S') ";
            cmd.ExecuteNonQuery();
            sda = new SqlDataAdapter(cmd);
            sda.Fill(dt1);
            con.Close();
            return dt1;
        }

        public  DataTable parNum (string num)
       {
           con.Open();
           cmd = con.CreateCommand();
           cmd.CommandType = CommandType.Text;
           cmd.CommandText = "SELECT NumPrecompte, DatePrecompte AS [Date du précompte], MontantPrecompte AS [Montant du précompte], MotifPrecpte AS [Motif], NumDemPret FROM Précompte WHERE NumDemPret = '" + num + "'";
           DataTable dt = new DataTable();
           SqlDataAdapter sda = new SqlDataAdapter(cmd);
           sda.Fill(dt);
           con.Close();
           return dt;
       }

        public  DataTable Remplir_ComboNum(DataGrid dg)
        {
            con.Open();
            DataTable dtSource = new DataTable();
            dtSource = ((DataView)dg.ItemsSource).ToTable();
            DataTable dt = new DataTable();
            dt.Columns.Add("Numéro");
            for (int i=0; i < dtSource.Rows.Count; i++)
            {
                dt.Rows.Add("1"); //Créer d'abord les lignes du tableau (valeur quelconque) puis les remplacer par les valeurs voulues
                dt.Rows[i][0] = dtSource.Rows[i][0];
            };
            con.Close();
            return dt;
        }


       public  List<LigneNomPren> fonctionaire(String Nom)
       {
           List<LigneNomPren> l = new List<LigneNomPren>();
           String cmd = "select NomFonct,PrenFonct from Fonctionnaire where NomFonct +' '+ PrenFonct like '" + Nom + "%' or PrenFonct +' '+ NomFonct like '" + Nom + "%'";
           try
           {
               con.Open();
               SqlCommand cmdUser = new SqlCommand(cmd, con);
               SqlDataReader reader = cmdUser.ExecuteReader();
               while (reader.Read())
               {
                   LigneNomPren ligne = new LigneNomPren();
                   ligne.nom = reader[0].ToString();
                   ligne.prenom = reader[1].ToString();
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
               con.Close();

           }


       }
    }
}
