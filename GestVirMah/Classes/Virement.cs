using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace GestVirMah
{
    class Virement
    {
        public String dateVir;
        public int pv_codePV;
        public int codeVir;
        private SqlConnection con;
        public Virement(SqlConnection conn)
        {
            this.con = conn;
        }
        public DataTable getListeViremnt(SqlConnection conn)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select *from dbo.Virement", conn);
            DataTable dt = new DataTable("Virement");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            return dt;
        }

       //*public void Add_DemandePrime(SqlConnection conn, DateTime date, int matri, int code, float montant, string compte, DateTime DateEv, string etatdem, string Motif, int codepv, DateTime Datecrea, int codeUse)
       // {

       //     try
       //     {
       //         conn.Open();
       //         String requete = "INSERT INTO  DemandePrime (NumDem, DateDem, Matricule, CodePrime, MontsantDem, CompteDem,DateEven,EtatDem,MotifEtat,pv_codepv,DateCreatDem,CodeUser) VALUES('" + date + "','" + matri + "','" + code + "','" + montant + "','" + compte + "','" + DateEv + "','" + etatdem + "','" + Motif + "','" + codepv + "','" + Datecrea + "','" + codeUse + "')";
       //         SqlCommand cmd = new SqlCommand(requete, conn);
       //         cmd.ExecuteNonQuery();
       //         MessageBox.Show(" réussite de l ' addition ");
       //     }
       //     catch (SqlException ex)
       //     {
       //         MessageBox.Show("!Ce fonctionnaire existe deja ! \n" + ex.Message);
       //     }
       //     finally
       //     {
       //         conn.Close();
       //     }
       // }
        
       
        public SqlDataAdapter batch_accepte(SqlConnection conn)
        {


           // SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True");
            conn.Open();

            //on recupère les parametre (la date debut de l'année et la durrée de cotisation
            SqlCommand cmd = new SqlCommand("SELECT * FROM Parametres ", con);
            SqlDataReader read = cmd.ExecuteReader();
            read.Read();
            int durCot = Convert.ToInt32(read[2]);
            int jourDeb = Convert.ToInt32(read[3]);
            int moiDeb = Convert.ToInt32(read[4]);
            int year = Convert.ToInt32(DateTime.Now.Year);
            int month = Convert.ToInt32(DateTime.Now.Month);
            conn.Close();
            if (month >= moiDeb && month <= 12) { } //toujour un batch par raport à l'année précédente
            else { year--; }
            conn.Open();
             //La requete qui selectionne tout les fonctioonaires qui sont acceptes dans la prime generale
             SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Fonctionnaire WHERE (DateDepartTmp IS NULL AND DateDepartDefi IS NULL) OR(   DateDepartDefi IS  NOT NULL AND  DATEDIFF(MONTH,'" + year + "-" + moiDeb + "-" + jourDeb + "',DateDepartDefi)>=" + durCot + ")OR( DateDepartTmp IS NOT NULL AND DateRetrTmp IS NULL AND DATEDIFF(MONTH,'" + year + "-" + moiDeb + "-" + jourDeb + "',DateDepartTmp)>=" + durCot + ")OR(NOT DateDepartTmp IS NULL  AND    DATEADD(MM, DATEDIFF(MONTH,'" + year + "-" + moiDeb + "-" + jourDeb + "',DateDepartTmp),DATEDIFF(MONTH, DateRetrTmp,GETDATE()))>=" + durCot + ")", conn);
             conn.Close();
            return da;
            }
            //public void ajouterDemandeBatch(SqlConnection conn, String desingPrime, int codeUsr)
            //  {
            //    int codePrime,codeUsr;
            //    float MontantPrime;
            //    ////////////////////les info sur la prime
            //    SqlCommand cmd = new SqlCommand("select *from TypePrime where DésignationPrime =" + desingPrime + " ",conn);
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    codePrime = int.Parse(reader["codePrime"].ToString());
            //    MontantPrime = float.Parse(reader["MontantPrime"].ToString());
            //    codeUsr = int.Parse(reader["codePrime"].ToString());
            //    reader.Close();

            //    ////////////////////

            //    String nom1, prenom1, email1, SitFamFonct1, NomJFilleFonct1;
            //    int Matricule1, TelFonct1;

            //    /////////////////ajouter les demande des fonctionnaire présents pendant la date de virement

            //    DataTable dt1 = new DataTable("Fonctionnaire1");
            //    SqlDataAdapter da1 = batch_accepte();
            //    da1.Fill(dt1);
            //    foreach (DataRow row in dt1.Rows)
            //    {
            //        Matricule1 = int.Parse(reader["Matricule"].ToString());
            //        nom1 = row["NomFonct"].ToString();
            //        prenom1 = row["PrenFonct"].ToString();
            //        email1 = row["EmailFonct"].ToString();
            //        SitFamFonct1 = row["SitFamFonct"].ToString();
            //        NomJFilleFonct1 = row["NomJFilleFonct"].ToString();
            //        TelFonct1 = int.Parse(reader["TelFonct"].ToString());
            //        ajouterDemande();
            //    }

            //    ///////////////
            //    String  nom2, prenom2, email2, SitFamFonct2, NomJFilleFonct2;
            //    int Matricule2, TelFonct2;
            //    DateTime? DateRecrut, DateDepartDefi, DateDepartTmp, DateRetrTmp;
            //    /////////////vérification des bénificiere du prime de l'année précédente
            //    SqlCommand cmd2 = new SqlCommand("select *from Fonctionnaire where DateDepartDefi=null or DateDepartTmp=null or DateRetrTmp<GETDATE() ", conn);
            //    DataTable dt2 = new DataTable("Fonctionnaire2");
            //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //    da1.Fill(dt2);
            //    foreach (DataRow row in dt1.Rows)
            //    {

            //        Matricule2 = int.Parse(reader["Matricule"].ToString());
            //        nom2 = row["NomFonct"].ToString();
            //        prenom2 = row["PrenFonct"].ToString();
            //        email2 = row["EmailFonct"].ToString();
            //        SitFamFonct2 = row["SitFamFonct"].ToString();
            //        NomJFilleFonct2 = row["NomJFilleFonct"].ToString();
            //        TelFonct2 = int.Parse(reader["TelFonct"].ToString());
            //        DateDepartDefi = DateTime.Parse(row["DateDepartDefi"].ToString());
            //        DateDepartTmp = DateTime.Parse(row["DateDepartTmp"].ToString());
            //        DateRetrTmp = DateTime.Parse(row["DateRetrTmp"].ToString());
            //        on verifie si le fonctionnaire erite la prime
            //        if (!DateDepartDefi.HasValue   )//si le fonctionnaire est présent(date départ = null ou date retour < date d'aujourd'hui)
            //        {
            //            ajouterDemande();
            //        }
            //        else
            //        {
            //            if 
            //        }
            //    }

            //    /////////////
            //}

        }
}
