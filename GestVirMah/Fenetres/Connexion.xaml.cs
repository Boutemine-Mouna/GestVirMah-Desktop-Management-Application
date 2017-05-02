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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data.SqlClient;
using System.Windows.Threading;
using GestVirMah.Classes;

//retenperiodique = float.parse
namespace GestVirMah
{
    public partial class Connexion : MetroWindow
    {


        private static SqlConnection connexionSql = new SqlConnection("Data source=localhost;Initial Catalog=OeuvresSociales;Integrated Security=SSPI");
        private DispatcherTimer t = new DispatcherTimer();
        private Utilisateur user;
       
        public Connexion()
        {
            
            InitializeComponent();
        }

        public void time()
        {

            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        void t_Tick(Object sender, EventArgs e)
        {

            t.Stop();
            progressRing1.IsActive = false;
            FenetrePrincipale fen = new FenetrePrincipale(connexionSql, user);
            if (user.Droits == 'S')
            {
                fen.settingsButton.Visibility = Visibility.Hidden;
            }
            fen.Show();
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                button1.IsEnabled = false;
                textBox1.IsReadOnly = true;
                passwordBox1.IsEnabled = false;
                connexionSql.Open();
                SqlCommand cmdUser = new SqlCommand("select * from utilisateur where Login='" + textBox1.Text + "' and Supprime ='N' ");
                cmdUser.Connection = connexionSql;
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
               
                if (ReadUser.Read())
                {
                    if (ReadUser["MotPasse"].Equals(passwordBox1.Password))
                    {
                        progressRing1.IsActive = true;
                        time();
                        label2.Content = "";
                       user = new Utilisateur(ReadUser.GetInt32(0), ReadUser["NomUser"].ToString(), ReadUser["PrenUser"].ToString(), ReadUser["Login"].ToString(), ReadUser["droit"].ToString()[0]);
                    }
                    else
                    {
                        label2.Content = "* Mot de passe incorrecte";
                        passwordBox1.Clear();
                        button1.IsEnabled = true;
                        textBox1.IsReadOnly = false;
                        passwordBox1.IsEnabled = true;  
                    }
                }
                else
                {
                    label2.Content = "* Utilisateur introuvable!";
                    passwordBox1.Clear();
                    button1.IsEnabled = true;
                    textBox1.IsReadOnly = false;
                    passwordBox1.IsEnabled = true;  
                }                  
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
            }
            finally
            {                            
                connexionSql.Close();
            }           
        }
    }
}
