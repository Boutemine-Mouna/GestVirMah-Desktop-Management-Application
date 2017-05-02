using MahApps.Metro.Controls;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace GestVirMah
{
    /// <summary>
    /// Interaction logic for detailVir.xaml
    /// </summary>
    public partial class detailVir : MetroWindow
    {
        LigneVirement ligne;
        // private int codePv;
        private int codeVr;
        private SqlConnection conn;
       // private SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        //public void setCodePv(int codePv)
        //{
        //    this.codePv = codePv;
        //}

        public void setCodeVr(int codeVr)
        {
            this.codeVr = codeVr;
        }
        public detailVir(SqlConnection con)
        {
            this.conn = con;
            InitializeComponent();
          
        }
        public detailVir(LigneVirement l,SqlConnection con)
        {
            this.conn = con;
            string nomUser = "";
            string prenUser = "";
            InitializeComponent();
            

            try
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("select NomUser, PrenUser from utilisateur where CodeUser = " + l.CodeUser + "", conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    nomUser = reader1[0].ToString();
                    prenUser = reader1[1].ToString();
                    reader1.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed to connect to data source" + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            this.label16.Content = l.CodeVir.ToString();
            this.label29.Content = l.pv_codepv.ToString();
            //this.label8.Content = l.CompteEsiVir.ToString();
            //this.label21.Content = l.CompteSocVir.ToString();
            this.label26.Content = l.MinistereVir.ToString();
            this.label27.Content = l.DateCreatVir.ToString();
            this.label23.Content = l.ChequeVir.ToString();
            this.label17.Content = l.BenefVir.ToString(); ;
            this.label18.Content = calculeSommeVir(int.Parse(l.pv_codepv)).ToString(); ;
            this.label19.Content = l.ObserVir.ToString(); ;
            this.label28.Content = l.DateVir.ToString(); ;
            this.label25.Content = nomUser + " " + prenUser;
            this.label24.Content = l.OrganismeVir.ToString(); ;
            this.label22.Content = l.DateChequeVir.ToString(); ;
        }

        public detailVir(int codePv, SqlConnection con)
        {
            this.conn = con;
            InitializeComponent();
           
            string nomUser = "";
            string prenUser = "";
            int codeVir = 0;
            int CodeUser = 0;
            DateTime DateVir = DateTime.Now;
            DateTime DateCreatVir = DateTime.Now;
            string MinistereVir = "";
            string OrganismeVir = "";
            double ChequeVir = 0;
            DateTime DateChequeVir = DateTime.Now;
            string CompteSocVir = "";
            string CompteEsiVir = "";
            string BenefVir = "";
            string ObserVir = "";


            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Virement where pv_codepv = " + codePv + "", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    codeVir = int.Parse(reader[0].ToString());
                    CodeUser = int.Parse(reader[4].ToString());
                    DateVir = DateTime.Parse((reader[1].ToString()));
                    DateCreatVir = DateTime.Parse((reader[3].ToString()));
                    MinistereVir = reader[5].ToString();
                    OrganismeVir = reader[6].ToString();
                    ChequeVir = double.Parse(reader[7].ToString());
                    DateChequeVir = DateTime.Parse((reader[8].ToString()));
                    CompteSocVir = reader[9].ToString();
                    CompteEsiVir = reader[10].ToString();
                    BenefVir = reader[11].ToString();
                    ObserVir = reader[12].ToString();
                }
                reader.Close();
                conn.Close();
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("select NomUser, PrenUser from utilisateur where CodeUser = " + CodeUser + "", conn);
               
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    nomUser = reader1[0].ToString();
                    prenUser = reader1[1].ToString();
                }
                reader1.Close();

                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed to connect to data source" + ex.ToString());
            }
            finally
            {
                //conn.Close();

            }
            this.label16.Content = codeVir;
            this.label29.Content = codePv;
            //this.label8.Content = CompteEsiVir;
            //this.label21.Content = CompteSocVir;
            this.label26.Content = MinistereVir;
            this.label27.Content = DateCreatVir;
            this.label23.Content = ChequeVir;
            this.label17.Content = BenefVir;
            this.label18.Content = calculeSommeVir(codePv);
            this.label19.Content = ObserVir;
            this.label28.Content = DateVir.ToString().Substring(0, 8);
            this.label25.Content = nomUser + " " + prenUser;
            this.label19.Content = OrganismeVir;
            this.label22.Content = DateChequeVir;

        }

        public double calculeSommeVir(int cp)  ///C làààààààààààààààààààààààààààààààààààààààààààààààààààààààà
        {
            double som = 0;
            string cmd = "select MontsantDem from DemandePrime where pv_codepv = " + cp + " and EtatDem ='A' ";
            string cmd1 = "select MontantAcc from DemandePret where pv_codepv = " + cp + " and Etat ='A' ";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader reader = cmdUser.ExecuteReader();
                while (reader.Read())
                {
                    som += double.Parse(reader[0].ToString());
                }
                reader.Close();
                conn.Close();
                conn.Open();
                SqlCommand cmdUser1 = new SqlCommand(cmd1, conn);
                SqlDataReader reader1 = cmdUser1.ExecuteReader();
                while (reader1.Read())
                {
                    som += double.Parse(reader1[0].ToString());
                }
                reader1.Close();
                conn.Close(); 
                return som;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source11" + ex.ToString());
                return som;
            }
            finally
            {
                conn.Close();

            }

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /****************************************************************/






        /***************************************************************/

        private void fermer_Click(object sender, RoutedEventArgs e)
        {
            //detailVir fen1 = new detailVir();
            this.Close();
        }
    }

}
