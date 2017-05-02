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
    /// Logique d'interaction pour ModifProduit.xaml
    /// </summary>
    public partial class ModifProduit : MetroWindow
    {
        private SqlConnection conn;
        //SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        public ModifProduit(SqlConnection con)
        {
            this.conn = con;
            InitializeComponent();
            fillProd();
        }
        void fillProd()
        {
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT Designation FROM Produit ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ComboProd.Items.Add(dr["Designation"].ToString());

            }
            conn.Close();

        }

        private void APPLIQUE_Click(object sender, RoutedEventArgs e)
        {
            if (ComboProd.SelectedIndex != -1) 
            {
                if (Nomm.Text != "" || PrixHT.Text != "" || PrixTTC.Text != "")
                {
                    MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder ces modifications ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultat == MessageBoxResult.Yes)
                    {
                        String prod = ComboProd.SelectedItem.ToString();
                        String info; int info2, info3;
                        if (Nomm.Text != "")
                        {
                            info = Nomm.Text.ToString();
                            String cmd = "UPDATE Produit SET Designation= '" + info + "' WHERE Designation='" + prod + "'";
                            conn.Open();
                            SqlCommand cmdUser = new SqlCommand(cmd, conn);
                            SqlDataReader reader = cmdUser.ExecuteReader();
                            conn.Close();
                        }
                        if (PrixHT.Text != "")
                        {
                            info2 = int.Parse(PrixHT.Text.ToString());
                            String cmd1 = "UPDATE Produit SET PrixUnitHT= '" + info2 + "' WHERE Designation='" + prod + "'";
                            conn.Open();
                            SqlCommand cmdUser1 = new SqlCommand(cmd1, conn);
                            SqlDataReader reader1 = cmdUser1.ExecuteReader();
                            conn.Close();
                        }
                        if (PrixTTC.Text != "")
                        {
                            info3 = int.Parse(PrixTTC.Text.ToString());
                            String cmd2 = "UPDATE Produit SET PrixUnitTTC= '" + info3 + "' WHERE Designation='" + prod + "'";
                            conn.Open();
                            SqlCommand cmdUser2 = new SqlCommand(cmd2, conn);
                            SqlDataReader reader2 = cmdUser2.ExecuteReader();
                            conn.Close();
                        }
                        MessageBox.Show("Les modifications sont effectuées !");

                    }
                    
                }
                else MessageBox.Show("Veuillez mettre à jour au moins une des informations");
            }
            else MessageBox.Show("Veuillez Selectionner un Produit");
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrixHT_TextChanged(object sender, TextChangedEventArgs e)
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
