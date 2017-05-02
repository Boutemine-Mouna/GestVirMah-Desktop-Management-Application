using GestVirMah.Classes;
using GestVirMah.ClassePret;
using GestVirMah.Fenetres;
using GestVirMah.FenetrePret;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Collections;
using System.Globalization;


namespace GestVirMah.FenetrePret
{
    /// <summary>
    /// Logique d'interaction pour AjoutFourni.xaml
    /// </summary>
    public partial class AjoutFourni : MetroWindow
    {  
       // private static SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        private SqlConnection con;
        public AjoutFourni(SqlConnection conn)
        {  this.con=conn;
            InitializeComponent();
            filltype();
            
        }

        private void  filltype()
        {
            con.Open();
            String t = "2";
            SqlCommand cmd = new SqlCommand();
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT DesignationPret FROM TypePret WHERE TypePret='" + t + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ComboType.Items.Add(dr["DesignationPret"].ToString());

            }
            con.Close();
        }

        private void ValiderFournis_Click(object sender, RoutedEventArgs e)
        {  Fournisseur fr=new Fournisseur(con);
            if (ComboType.SelectedIndex != -1)
            {
                if (TextNom.Text != "")
                {
                    if (codeRC.Text != "")
                    {
                        if (TextCodebnp.Text != "")
                        {
                            if (TextMF.Text != "")
                            {
                                if (TextApport.Text != "")
                                {
                                    if (TextRaison.Text != "")
                                    {
                                        if (TextPeriode.Text != "")
                                        {
                                            int per = int.Parse(TextPeriode.Text.ToString());
                                            String nom = TextNom.Text.ToString();
                                            String codeRc = codeRC.Text.ToString();
                                            String Raison = TextRaison.Text.ToString();
                                            int CodeBnp = int.Parse(TextCodebnp.Text.ToString());
                                            int matricule = int.Parse(TextMF.Text.ToString());
                                            String type = ComboType.SelectedItem.ToString();
                                            float app = float.Parse(TextApport.Text.ToString());
                                            MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder ces informations ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                            if (resultat == MessageBoxResult.Yes)
                                            {
                                                fr.ajouterFournisseur(nom, Raison, type, matricule, CodeBnp, app, codeRc,per);
                                                MessageBox.Show("L'ajout de ce fournisseur est effectué !");
                                            }
                                        }
                                        else MessageBox.Show("Veuillez entrer la periode de ce fournisseur");
                                    }
                                    else MessageBox.Show("Veuillez entrer la raison Sociale ");
                                }
                                else MessageBox.Show("Veuillez entrer l'apport ");
                            }
                            else MessageBox.Show("Veuillez entrer la matricule fiscal ");
                        }
                        else MessageBox.Show("Veuillez entrer le CodeBNP");
                    }
                    else MessageBox.Show("Veuillez entrer le CodeRc ");
                }
                else MessageBox.Show("Veuillez entrer le nom du fournisseur");
            }
            else MessageBox.Show("Veuillez Séléctionner le type de fournisseur");
            foreach (var item in Fournis.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).Clear();
                if (item is ComboBox)
                    ((ComboBox)item).SelectedIndex = -1;
            }

        }

        private void RetourFournis_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextCodebnp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void TextMF_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void TextApport_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void codeRC_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextPeriode_TextChanged(object sender, TextChangedEventArgs e)
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
