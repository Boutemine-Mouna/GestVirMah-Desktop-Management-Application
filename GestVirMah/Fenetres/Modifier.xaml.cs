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
using System.Data.SqlClient;
using System.Data.Sql;
using MahApps.Metro.Controls;

namespace GestVirMah.Fenetres
{
    /// <summary>
    /// Interaction logic for Modifier.xaml
    /// </summary>
    public partial class Modifier : MetroWindow
    {
        private SqlConnection conn;

    //    SqlConnection conn = new System.Data.SqlClient.SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True;MultipleActiveResultSets=True");
        public class Ligne
        {
            public string Matricule { get; set; }
            public string Nom { get; set; }
            public string Prenom { get; set; }
            public string Date { get; set; }
            public string Telephone { get; set; }
            public string Email { get; set; }
            public string CompteFonct { get; set; }
            public string CodeBanque { get; set; }
            public string SitFam { get; set; }
            public string NomMme { get; set; }
            public string DateDD { get; set; }
            public string MotifDD { get; set; }
            public string DateDT { get; set; }
            public string MotifDT { get; set; }
            public string DateRT { get; set; }
        }
        public Modifier(SqlConnection con)
        {
            this.conn = con;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GPer.Visibility = System.Windows.Visibility.Visible;
            GCompte.Visibility = System.Windows.Visibility.Hidden;
            GAutres.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GPer.Visibility = System.Windows.Visibility.Hidden;
            GCompte.Visibility = System.Windows.Visibility.Visible;
            GAutres.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GPer.Visibility = System.Windows.Visibility.Hidden;
            GCompte.Visibility = System.Windows.Visibility.Hidden;
            GAutres.Visibility = System.Windows.Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            conn.Open();
            MessageBoxResult resultat = MessageBox.Show("Voulez vous l'enregistrement des fonctionnaires sélectionées ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultat == MessageBoxResult.Yes)
            {
                if (mat1.Text.Length != 0)
                {
                   
                    SqlCommand sel = new SqlCommand("select Matricule from Fonctionnaire where Matricule = " + mat1.Text, conn);
                    SqlDataReader rd = sel.ExecuteReader();
                    if (rd.Read())
                    {
                        Boolean v = false;
                        string cmd = "update Fonctionnaire set ";
                        if (Tel1.Text.Length != 0) { cmd += "TelFonct=" + Tel1.Text; v = true; }
                        if (Email1.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "EmailFonct ='" + Email1.Text + "'"; }
                        if (SitFam1.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "SitFamFonct='" + SitFam1.Text + "'"; }
                        if (NomMlle1.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "NomJFilleFonct='" + NomMlle1.Text + "'"; }
                        if (Cpt1.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "CompteFonct = '" + Cpt1.Text + "'"; }
                        if (Bque1.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "CodeBanque = " + Bque1.Text; }
                        if (Adresse.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "AdresseFonct = '" + Adresse.Text + "'"; }
                        if (FF.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "FonctionFonct = '" + FF.Text + "'"; }
                        if (GF.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "GradeFonct = '" + GF.Text + "'"; }
                        if (DDD.Text.Length != 0)
                        {
                            if (v) cmd += ", ";
                            v = true;
                            DateTime dt = Convert.ToDateTime(DDD.Text);
                            cmd += "DateDepartDefi='" + dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString() + "'";
                        }
                        if (MDD.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "MotifDepartDefi='" + MDD.Text + "'"; }
                        if (DDT.Text.Length != 0)
                        {
                            if (v) cmd += ", ";
                            v = true;
                            DateTime dt = Convert.ToDateTime(DDD.Text);
                            cmd += "DateDepartTmp='" + dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString() + "'";
                        }
                        if (MDT.Text.Length != 0) { if (v) cmd += ", "; v = true; cmd += "MotifDepartTmp='" + MDT.Text + "'"; }
                        if (DRT.Text.Length != 0)
                        {
                            if (v) cmd += ", ";
                            v = true;
                            DateTime dt = Convert.ToDateTime(DDD.Text);
                            cmd += "DateRetrTmp='" + dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString() + "'";
                        }
                        
                        if (v)
                        {
                            
                            
                            cmd += "where Matricule=" + mat1.Text;
                            try
                            {
                               
                                SqlCommand s1 = new SqlCommand("select Matricule from Fonctionnaire where Matricule = " + mat1.Text, conn);
                                SqlDataReader red = s1.ExecuteReader();
                                if (red.Read())
                                {
                                    SqlCommand cd = new SqlCommand(cmd, conn);
                                    cd.ExecuteReader();
                                    MessageBox.Show("Enregistrer avec succès");
                                    red.Close();
                                    this.Close();
                                }


                                else
                                {
                                    MessageBox.Show("Echec de mise à jour: Le fonctionnaire n'existe pas");
                                }
                                
                            }
                            catch (System.Data.SqlClient.SqlException)
                            {
                                MessageBox.Show("Echec de mise à jour: Numéro de téléphone doit ne pas contenir des caractères !!");
                            }
                            catch (Exception es)
                            {
                                MessageBox.Show(es.Message);
                            }
                           
                        }
                        else MessageBox.Show("Vous n'avez remplit aucun champs !!");
                    }
                    else MessageBox.Show("Le fonctionnaire n'existe pas !!");
                }
                else MessageBox.Show("Vous n'avez inséré aucun matricule !!");
            }
            conn.Close();
        }

        private void Bque1_Loaded_1(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT CodeBanque FROM Banque", conn);
            SqlDataReader read = cmd.ExecuteReader();
            List<Ligne> tab_ligne = new List<Ligne>();
            while (read.Read())
            {
                Bque1.Items.Add(Convert.ToString(read["CodeBanque"]));
            }
            read.Close();
            conn.Close();
        }

        private void SitFam1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SitFam1.SelectedIndex == 1) NomMlle1.IsEnabled = false;
            if (SitFam1.SelectedIndex == 2) NomMlle1.IsEnabled = true;
            if (SitFam1.SelectedIndex == 3) NomMlle1.IsEnabled = false;
        }
    }
}
