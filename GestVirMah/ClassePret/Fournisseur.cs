using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Windows;


namespace GestVirMah.ClassePret
{
    class Fournisseur
    {
        private SqlConnection con;
      //  public static SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
       public Fournisseur(SqlConnection conn)
        {
            this.con = conn;
        }
        public  DataTable produitInfo(String nomProduit)
        {

            String cmd = "SELECT RefProduit,PrixUnitHT,PrixUnitTTC,Disponible,RefFournisseur FROM Produit WHERE Designation= @nomProduit";
            try
            {   
                con.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, con);
                cmdUser.Parameters.AddWithValue("@nomProduit", nomProduit);
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
                con.Close();

            }
        }
        public  DataTable infoFournisseur(String NomFournis)
        {

            String cmd = "SELECT * FROM Fournisseur WHERE NomFournisseur = @NomFounisseur";
            try
            {
                con.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, con);
                cmdUser.Parameters.AddWithValue("@NomFounisseur", NomFournis);
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
                con.Close();

            }
        }
        public  void ajouterFournisseur(String NomFournis,String Raison,String type,int matricule,int codeBNP,float apport, String CodeRc,int periode )
        {

            String cmd = "insert into Fournisseur(NomFournisseur,RaisonSociale,CodeRC,MatFiscal,CodeBNP,AppFournisseur,TypeFournisseur,PeriodeFournisseur)Values('" + NomFournis + "','" + Raison + "','" + CodeRc + "','" + matricule + "','" + codeBNP + "',@app,'" + type + "','" + periode + "')";
            try
            {
                con.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, con);
                cmdUser.Parameters.Add(new SqlParameter("@app", SqlDbType.Float));
                cmdUser.Parameters["@app"].Value = apport;
                SqlDataReader reader = cmdUser.ExecuteReader();
        
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
               
            }
            finally
            {
                con.Close();

            }
        }
        public void ajouterProduit(String Ref,String nom,int prixht,int prixTTC,int refFournis)
        {
            bool dis = true;
            String cmd = "insert into Produit(RefProduit,Designation,PrixUnitHT,PrixUnitTTC,Disponible,RefFournisseur)Values('" +Ref + "','" + nom + "','" + prixht + "','" + prixTTC + "','" + dis + "','"+refFournis+"')";
            try
            {
                con.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, con);
                SqlDataReader reader = cmdUser.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());

            }
            finally
            {
                con.Close();

            }
        }

    }
}
