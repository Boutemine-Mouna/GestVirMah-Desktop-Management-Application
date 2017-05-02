using GestVirMah.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using MahApps.Metro.Controls;

namespace GestVirMah
{
    /// <summary>
    /// Interaction logic for infoPrime.xaml
    /// </summary>
    public partial class infoPrime : MetroWindow
    {
        SqlConnection connexionSql;
        Utilisateur user;
        int codePrime;
        public infoPrime()
        {
            InitializeComponent();
        }
        private void Confirm_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connexionSql.Open();
                SqlCommand cmd = new SqlCommand("Update TypePrime SET MontantPrime =" + int.Parse(monBox.Text) + " where CodePrime = " + codePrime, connexionSql);
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Close();
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { connexionSql.Close(); }
        }

        private void annulebtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public infoPrime(SqlConnection connexionSql, Utilisateur user,int codePrime)
        {
            this.InitializeComponent();
            this.connexionSql = connexionSql;
            this.user = user;
            this.codePrime = codePrime;
            string DésignationPrime = "";
            double MontantPrime = 0;
            int codeUser = 0;
            string DateCreatType = "";
            string nomUser = "";
            string prenUser = "";


            try
            {
                connexionSql.Open();
                SqlCommand cmd = new SqlCommand("select *from TypePrime where CodePrime = " + codePrime + "", connexionSql);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    DésignationPrime = reader[1].ToString();
                    MontantPrime = double.Parse(reader[2].ToString());
                    DateCreatType = reader[3].ToString();
                    codeUser = int.Parse(reader[4].ToString());

                }
                reader.Close();
                connexionSql.Close();
                connexionSql.Open();
                
                SqlCommand cmd1 = new SqlCommand("select NomUser, PrenUser from utilisateur where CodeUser = " + codeUser + "", connexionSql);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (reader1.Read())
                {
                    nomUser = reader1[0].ToString();
                    prenUser = reader1[1].ToString();
                }
                reader1.Close();
                connexionSql.Close();

                label5.Content = codePrime;
                label6.Content = DésignationPrime;
                monBox.Text = MontantPrime.ToString();
                label8.Content = DateCreatType;
                label10.Content = nomUser + " " + prenUser;

            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed to connect to data source" + ex.ToString());
            }
            finally
            {
                //connexionSql.Close();

            }

        }
    }
    
}
