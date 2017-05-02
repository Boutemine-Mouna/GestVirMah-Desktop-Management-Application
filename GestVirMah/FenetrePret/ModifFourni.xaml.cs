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
    /// Logique d'interaction pour ModifFourni.xaml
    /// </summary>
    public partial class ModifFourni : MetroWindow
    {   private SqlConnection con ;
        //SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        public ModifFourni(SqlConnection conn)
        {
            this.con = conn;
            InitializeComponent();
            FillFourniss();
        }
        

        public void FillFourniss()
        {
            DataTable dat;
            String cmd = "select NomFournisseur from Fournisseur";
            SqlCommand cmduser = new SqlCommand(cmd, con);
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmduser);
            dat = new DataTable();
            ad.Fill(dat);
            if (dat.Rows.Count != 0)
            {
                foreach (DataRow dr in dat.Rows)
                {
                    ComboFourniss.Items.Add(dr["NomFournisseur"].ToString());

                }
            }
            else MessageBox.Show("Aucun Fournisseur trouvé !");
            con.Close();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ComboFourniss.SelectedIndex != -1)
            {
                if (TextR.Text != "" || TextMat.Text != "" || TextNom.Text != "" || TextPer.Text != "" || TextRC.Text != "" || TextBNP.Text != "" || TextApp.Text != "")
                {
                    MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder ces modifications ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultat == MessageBoxResult.Yes)
                    {
                        String fournis = ComboFourniss.SelectedItem.ToString();
                        String info; int info2; float info3;
                        if (TextNom.Text != "")
                        {
                            info = TextNom.Text.ToString();
                            String cmd = "UPDATE Fournisseur SET NomFournisseur= '" + info + "' WHERE NomFournisseur='" + fournis + "'";
                            con.Open();
                            SqlCommand cmdUser = new SqlCommand(cmd, con);
                            SqlDataReader reader = cmdUser.ExecuteReader();
                            con.Close();
                        }
                        if (TextR.Text != "")
                        {
                            info = TextR.Text.ToString();
                            String cmd1 = "UPDATE Fournisseur SET RaisonSociale= '" + info + "' WHERE NomFournisseur='" + fournis + "'";
                            con.Open();
                            SqlCommand cmdUser1 = new SqlCommand(cmd1, con);
                            SqlDataReader reader1 = cmdUser1.ExecuteReader();
                            con.Close();
                        }
                        if (TextRC.Text != "")
                        {
                            info = TextRC.Text.ToString();
                            String cmd2 = "UPDATE Fournisseur SET CodeRC= '" + info + "' WHERE NomFournisseur='" + fournis + "'";
                            con.Open();
                            SqlCommand cmdUser2 = new SqlCommand(cmd2, con);
                            SqlDataReader reader2 = cmdUser2.ExecuteReader();
                            con.Close();
                        }

                        if (TextBNP.Text != "")
                        {
                            info2 = int.Parse(TextBNP.Text.ToString());
                            String cmd3 = "UPDATE Fournisseur SET CodeBNP= '" + info2 + "' WHERE NomFournisseur='" + fournis + "'";
                            con.Open();
                            SqlCommand cmdUser3 = new SqlCommand(cmd3, con);
                            SqlDataReader reader3 = cmdUser3.ExecuteReader();
                            con.Close();
                        }
                        if (TextMat.Text != "")
                        {
                            info2 = int.Parse(TextMat.Text.ToString());
                            String cmd4 = "UPDATE Fournisseur SET MatFiscal= '" + info2 + "' WHERE NomFournisseur='" + fournis + "'";
                            con.Open();
                            SqlCommand cmdUser4 = new SqlCommand(cmd4, con);
                            SqlDataReader reader4 = cmdUser4.ExecuteReader();
                            con.Close();
                        }
                        if (TextApp.Text != "")
                        {
                            info3 = float.Parse(TextApp.Text.ToString());
                            String cmd5 = "UPDATE Fournisseur SET AppFournisseur= '" + info3 + "' WHERE NomFournisseur='" + fournis + "'";
                            con.Open();
                            SqlCommand cmdUser5 = new SqlCommand(cmd5, con);
                            SqlDataReader reader5 = cmdUser5.ExecuteReader();
                            con.Close();
                        }
                        if (TextPer.Text != "")
                        {
                            info2 = int.Parse(TextPer.Text.ToString());
                            String cmd6 = "UPDATE Fournisseur SET PeriodeFournisseur= '" + info2 + "' WHERE NomFournisseur='" + fournis + "'";
                            con.Open();
                            SqlCommand cmdUser6 = new SqlCommand(cmd6, con);
                            SqlDataReader reader6 = cmdUser6.ExecuteReader();
                            con.Close();
                        }
                        MessageBox.Show("Les modifications sont effectuées !");
                    }
                }
                else MessageBox.Show("Veuillez modifier au moins une des informations");
            }
            else MessageBox.Show("Veuillez selectionner un fournisseur");
            
        }

        private void TextBNP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void TextMat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void TextApp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void TextPer_TextChanged(object sender, TextChangedEventArgs e)
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
