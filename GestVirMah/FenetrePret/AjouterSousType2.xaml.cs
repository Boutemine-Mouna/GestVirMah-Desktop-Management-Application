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
using GestVirMah.Classes;
using MahApps.Metro.Controls;
using GestVirMah.ClassePret;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;

namespace GestVirMah.FenetrePret
{
    /// <summary>
    /// Logique d'interaction pour AjouterSousType2.xaml
    /// </summary>
    public partial class AjouterSousType2 : MetroWindow
    {
        private SqlConnection con;
       // public static SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        public AjouterSousType2(SqlConnection conn)
        {
            this.con = conn; 
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (TextType.Text != "")
            {
                MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder ce type ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultat == MessageBoxResult.Yes)
                {

                    String type = TextType.Text.ToString();
                    String t = "2";
                    String cmd = "Insert into TypePret(DesignationPret,TypePret)values('" + type + "','" + t + "')";
                    try
                    {
                        con.Open();
                        SqlCommand cmdUser = new SqlCommand(cmd, con);
                        SqlDataReader reader = cmdUser.ExecuteReader();
                        MessageBox.Show("L'ajout de ce type est effectué !");

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
            else MessageBox.Show("Veuillez saisir le nom du nouveau type à ajouter"); 
        }


    }
}
