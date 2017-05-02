using System;
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
using GestVirMah.Classes;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;

namespace GestVirMah
{
    /// <summary>
    /// Interaction logic for AjouterPrime.xaml
    /// </summary>
    public partial class AjouterPrime : MetroWindow
    {
        SqlConnection connexionSql;
        Utilisateur user;
        public string ba = "N";

        public AjouterPrime(SqlConnection connexionSql)
        {
            connexionSql.Close();
            this.connexionSql = connexionSql;
            InitializeComponent();
        }

        public AjouterPrime(Utilisateur user, SqlConnection connexionSql)
        {
            InitializeComponent();
            this.user = user;
            this.connexionSql = connexionSql;
        }

        private void Confirm_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connexionSql.Open();
                String dPrime = (datePrime.Text.Substring(6, 4) + "/" + datePrime.Text.Substring(3, 3) + datePrime.Text.Substring(0, 2)).ToString();
                int codeUser = user.Code;
                SqlCommand cmd = new SqlCommand("Insert into TypePrime (CodePrime,DésignationPrime,MontantPrime, CodeUser, DateCreatType,IsBatch) Values (" + numDem(dPrime).ToString() + " ,'" + typeBox.Text + "'," + MontatntBox.Text + "," + codeUser + ",'" + dPrime + "','"+ ba + "')", connexionSql);
                SqlDataReader reader = cmd.ExecuteReader();
                MessageBox.Show("Prime " + typeBox.Text + " avec un montant de " + MontatntBox.Text + " Insérée", "Insertion terminée", MessageBoxButton.OK, MessageBoxImage.Information);
                reader.Close();
                connexionSql.Close(); 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally {  }
        }

        public String maxNumDem(String date)
        {
            String cmd = "select MAX(CodePrime) as CodePrime from TypePrime where DateCreatType like '" + date + "%'";
            try
            {
                connexionSql.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, connexionSql);
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                
                ReadUser.Read();

                if (ReadUser["CodePrime"] != DBNull.Value)
                {

                    String s = ReadUser["CodePrime"].ToString();
                    ReadUser.Close();
                    connexionSql.Close(); 
                    return s;

                }

                else
                {
                    ReadUser.Close();
                    connexionSql.Close(); 
                    return null;
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                connexionSql.Close(); 
                return null;

            }
            finally {  }

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


        private void annulebtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void batch_Checked(object sender, RoutedEventArgs e)
        {
            ba = "O";
        }

        private void MontatntBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }
    }
}
