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
    /// Logique d'interaction pour AjouterProduit.xaml
    /// </summary>
    public partial class AjouterProduit : MetroWindow
    {
        private SqlConnection con;
      //  public static SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        public AjouterProduit(SqlConnection conn)
        {
            this.con = conn;
            InitializeComponent();
            fillfournisseur();
        }
        public void fillfournisseur()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT NomFournisseur FROM Fournisseur";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ComboFournis.Items.Add(dr["NomFournisseur"].ToString());

            }
            con.Close();
        }
        private void Valider__Click(object sender, RoutedEventArgs e)
        {
            Fournisseur fr = new Fournisseur(con);
            if (ComboFournis.SelectedIndex != -1)
            {
                if (NomProd.Text != "")
                {
                    if (RefProd.Text != "")
                    {
                        if (PrixHt.Text != "")
                        {
                            if (PrixTTC.Text != "")
                            {
                                String fournis = ComboFournis.SelectedItem.ToString();
                                int refFournis = int.Parse(fr.infoFournisseur(fournis).Rows[0]["RefFournisseur"].ToString());
                                String nom = NomProd.Text.ToString();
                                String Ref = RefProd.Text.ToString();
                                int prixht = int.Parse(PrixHt.Text.ToString());
                                int prixTTC = int.Parse(PrixTTC.Text.ToString());
                                MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder ces informations ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (resultat == MessageBoxResult.Yes)
                                {
                                    fr.ajouterProduit(Ref, nom, prixht, prixTTC, refFournis);
                                    MessageBox.Show("L'ajout de ce produit est effectué!");
                                }
                            }
                            else MessageBox.Show("Veuillez entrer le PrixTTC du Produit ");
                        }
                        else MessageBox.Show("Veuillez entrer le PrixHT du Produit ");
                    }
                    else MessageBox.Show("Veuillez entrer la reférence du Produit ");
                }
                else MessageBox.Show("Veuillez entrer le nom du produit");
            }
            else MessageBox.Show("Veuillez selectionner un fournisseur pour ce produit ");

            foreach (var item in Prod.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).Clear();
                if (item is ComboBox)
                    ((ComboBox)item).SelectedIndex = -1;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrixHt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void PrixTTC_TextChanged(object sender, TextChangedEventArgs e)
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
