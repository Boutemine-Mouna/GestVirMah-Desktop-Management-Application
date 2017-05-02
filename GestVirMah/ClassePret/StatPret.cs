using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace GestVirMah.ClassePret
{
    class StatPret
    {
        //  private SqlConnection conn;
        public class SeriesVir
        {
            public string Année { get; set; }
            public double Montant { get; set; }
        }
        private static double calculeSommeVir(int cp, SqlConnection conn)
        {
          //  SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
            double som = 0;



            DataTable TabCode = new DataTable();
            conn.Open(); 
            SqlCommand cmd0 = new SqlCommand("select CodePret from TypePret where TypePret='" + "1" + "'", conn);
            cmd0.Connection = conn;
            SqlDataAdapter add = new SqlDataAdapter(cmd0);
            add.Fill(TabCode);
            conn.Close();

            DataTable TabCode1 = new DataTable();
            conn.Open();
            SqlCommand cmd11 = new SqlCommand("select MontantAcc,CodePret from DemandePret where pv_codepv = " + cp + " and Etat ='A'", conn);
            cmd11.Connection = conn;
           
            SqlDataAdapter add1 = new SqlDataAdapter(cmd11);
            add1.Fill(TabCode1);

            DataTable TabVir = new DataTable();
            TabVir.Columns.Add("MontantAcc", typeof(Int32));
            int code1, code2, montant = 0;
            for (int j = 0; j < TabCode.Rows.Count; j++)
            {
                code1 = Convert.ToInt32(TabCode.Rows[j]["CodePret"]);
                for (int t = 0; t < TabCode1.Rows.Count; t++)
                {
                    code2 = Convert.ToInt32(TabCode1.Rows[t]["CodePret"]);
                    montant = Convert.ToInt32(TabCode1.Rows[t]["MontantAcc"]);
                    if (code1 == code2)
                    {
                        TabVir.Rows.Add(montant);
                    }

                }
            }
            int somme = 0;
            int inter = 0;

            for (int i1 = 0; i1 < TabVir.Rows.Count; i1++)
            {
                inter = Convert.ToInt32(TabVir.Rows[i1]["MontantAcc"]);
                somme += inter;
            }
            conn.Close();
            return somme;


        }
       
        private static double montantAnnee(int annee, SqlConnection conn)
        {
            DataTable tab = new DataTable();
           // SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
            conn.Open();
            int codePv = 0;
            double som = 0;
            SqlCommand cmdUser = new SqlCommand("select DateCreatVir,pv_codepv from Virement where CodeVir like '" + annee.ToString().Substring(2, 2) + "%'", conn);
            SqlDataAdapter add1 = new SqlDataAdapter(cmdUser);
            add1.Fill(tab);
            conn.Close();
            foreach(DataRow row in tab.Rows)
            {
                codePv = int.Parse(row["pv_codepv"].ToString());
                som += calculeSommeVir(codePv,conn);
            }
            return som;
        }

        public static List<SeriesVir> montantIntervalAnnees(int annee1, int annee2, SqlConnection conn)
        {
            List<SeriesVir> virs = new List<SeriesVir>();
            SeriesVir item = null;
            int diff = annee2 - annee1 + 1;

            int i = 0; String annee = ""; double mnt = 0;
            while (i < diff)
            {
                annee = (annee1 + i).ToString();
                mnt = montantAnnee(annee1 + i, conn);
                item = new SeriesVir { Année = annee, Montant = mnt };
                virs.Add(item);
                i++;
            }
            return virs;
        }

    }
}

