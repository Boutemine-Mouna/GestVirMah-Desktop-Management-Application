using System;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using GestVirMah.Classes;
using GestVirMah.Fenetres;
using CrystalDecisions.Shared;
using System.IO;
using System.Windows.Media.Animation;


namespace GestVirMah
{

    public partial class ftrAjouterVirement : MetroWindow
    {
        public int codePv;
        public string codeVir;
        private Utilisateur user;
        SqlConnection conn;
       // SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        public ftrAjouterVirement(SqlConnection con)
        {
            this.conn = con;
            InitializeComponent();
        }

        public ftrAjouterVirement(Utilisateur user,SqlConnection con)
        {
            this.conn = con;
            InitializeComponent();
            this.user = user;
        }
        private void annuler_Click(object sender, RoutedEventArgs e)
        {
            ftrAjouterVirement ftr = new ftrAjouterVirement(user,conn);

            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select CodePV,DateCreatPV from PV where IsVir='N'", conn);
            DataTable dt = new DataTable("PV");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            pvGrid.ItemsSource = dt.DefaultView;
        }
        public List<string> getParametres()
        {
            List<string> l = new List<string>();

            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("select Ministere,Organisme,CompteSocEsi,CompteEsiTresor from Parametres", conn);
                SqlDataReader reader = cmdUser.ExecuteReader();

                reader.Read();

                string Ministere = reader["Ministere"].ToString(); l.Add(Ministere);
                string Organisme = reader["Organisme"].ToString(); l.Add(Organisme);
                string CompteSocEsi = reader["CompteSocEsi"].ToString(); l.Add(CompteSocEsi);
                string CompteEsiTresor = reader["CompteEsiTresor"].ToString(); l.Add(CompteEsiTresor);

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

        private void ajouter_Click(object sender, RoutedEventArgs e)
        {
            String dateVir = (dateBox.Text.Substring(6, 4) + "/" + dateBox.Text.Substring(3, 3) + dateBox.Text.Substring(0, 2)).ToString();
            List<string> l = getParametres();
            codeVir = numDem(dateVir).ToString();
            try
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("insert into Virement values(" + codeVir + ",GETDATE()," + codePv.ToString() + ",'" + dateVir + "'," + user.Code + ",@ministète,@organisme," + cheqvirBox.Text + ",GETDATE(),'" + l[2] + "','" + l[3] + "','" + benifBox.Text + "','" + observBox.Text + "')", conn);
                cmd1.Parameters.AddWithValue("@ministète", l[0]);
                cmd1.Parameters.AddWithValue("@organisme", l[1]);
                SqlDataReader r1 = cmd1.ExecuteReader();
                r1.Read();
                SqlCommand cmd2 = new SqlCommand("update PV set IsVir='O' where CodePV=" + codePv + " ", conn);
                r1.Close();
                SqlDataReader r2 = cmd2.ExecuteReader();
                r2.Read();
                r2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source " + ex.ToString());

            }
            finally
            {
                conn.Close();
            }

            detailVir detail = new detailVir(conn);
            double i = detail.calculeSommeVir(codePv);
            Demande dem = new Demande(conn);
            EtatVirement rapport = new EtatVirement();
            rapport.SetParameterValue("ministère", l[0]);
            rapport.SetParameterValue("organisme", l[1]);
            rapport.SetParameterValue("compte", l[2]);
            rapport.SetParameterValue("compte2", l[3]);
            rapport.SetParameterValue("nVir", codeVir);
            rapport.SetParameterValue("date", dateVir);
            rapport.SetParameterValue("montant ", i.ToString());
            rapport.SetParameterValue("sommeLettre", dem.converti(int.Parse(i.ToString())));
            rapport.SetParameterValue("cheque", cheqvirBox.Text);
            rapport.SetParameterValue("observation", observBox.Text);
            rapport.SetParameterValue("beneficier", benifBox.Text);

            Decision fen = new Decision();
            fen.afficher(rapport);
            fen.Show();
            getInfoCcp(codePv);
            this.Close();
        }


        private void pvGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView dataRow = (DataRowView)pvGrid.SelectedItem;
            codePv = int.Parse(dataRow.Row.ItemArray[0].ToString());

        }

        public String maxNumDem(String date)
        {
            String cmd = "select MAX(CodeVir) as CodeVir from Virement where DateCreatVir like '" + date + "%'";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();

                if (ReadUser["CodeVir"] != DBNull.Value)
                {
                    return ReadUser["CodeVir"].ToString();
                }

                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source  " + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }

        }
        /**************************************************************************/
        /***********************************************/
        public int numDem(String d)
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
                    maxNum = (int.Parse(maxNumDem(date2)) % 100) + 1;
                }

            }
            int i = ((date * 100) + maxNum);
            return i;
        }

        private void cheqvirBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void getParametres(string Ministere, string Organisme, string CompteSocEsi, string CompteEsiTresor)
        {
            conn.Open();
            SqlCommand cmdUser = new SqlCommand("select Ministere,Organisme,CompteSocEsi,CompteEsiTresor from Parametres", conn);

            SqlDataReader reader = cmdUser.ExecuteReader();
            try
            {
                reader.Read();
                if (reader.Read())
                {
                    Ministere = reader.GetString(reader.GetOrdinal("Ministere"));
                    Organisme = reader.GetString(reader.GetOrdinal("Organisme"));
                    CompteSocEsi = reader.GetString(reader.GetOrdinal("CompteSocEsi"));
                    CompteEsiTresor = reader.GetString(reader.GetOrdinal("CompteEsiTresor"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());

            }
            finally
            {
                reader.Close();

            }
            conn.Close();

        }

        public void getInfoCcp(int codePv)
        {
            try
            {
                AvisEtOrdre rapoprt = new AvisEtOrdre();
                rapoprt.SetParameterValue("codePv", codePv);
                //rapoprt.SetParameterValue("codePvv", codePv);
                Decision fen = new Decision();
                fen.afficher(rapoprt);
                fen.setToAvisLayout();
                fen.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
