using Excel;
using GestVirMah.Classes;
using GestVirMah.Fenetres;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestVirMah
{
	/// <summary>
	/// Interaction logic for UpdateFonctionrList.xaml
	/// </summary>
	public partial class UpdateFonctionrList : MetroWindow
    {
        private SqlConnection conn;
       // private SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True;MultipleActiveResultSets=True");

		public UpdateFonctionrList(SqlConnection con)
        {
            this.conn = con;
			this.InitializeComponent();
            
		}

        private void Parc_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.ShowDialog();
            ChemainFile.Text = dlg.FileName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ChemainFile.Text.Length == 0) MessageBox.Show("Vous n'avez inserer aucun lien !!");
            else
            {
                try
                {
                    FileStream stream = File.Open(ChemainFile.Text, FileMode.Open, FileAccess.Read);
                    IExcelDataReader read = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    read.IsFirstRowAsColumnNames = true;
                    DataSet result = read.AsDataSet();
                    List<LigneFonctionR> tab_ligne = new List<LigneFonctionR>();
                    foreach (DataRow dr in result.Tables[0].Rows)
                    {

                        LigneFonctionR Ligne = new LigneFonctionR();
                        Ligne.Matricule = dr["Matricule"].ToString();
                        Ligne.Nom = dr["NomFonct"].ToString();
                        Ligne.Prenom = dr["PrenFonct"].ToString();
                        DateTime dt = Convert.ToDateTime(dr["DateRecrut"].ToString());                        
                        Ligne.Date= dt.Year.ToString() + "/";

                        ////*************J'ai fait celui là pour toute les date *********

                        if (dt.Month.ToString().Length == 1)
                        {
                            Ligne.Date += "0";
                        }
                        Ligne.Date += dt.Month.ToString() + "/" + dt.Day.ToString();
                        //*************************************************************
                        Ligne.Telephone = dr["TelFonct"].ToString();
                        Ligne.Email += "/" + dr["EmailFonct"].ToString();
                        Ligne.CompteFonct = dr["CompteFonct"].ToString();
                        Ligne.CodeBanque = dr["CodeBanque"].ToString();
                        Ligne.SitFam = dr["SitFamFonct"].ToString();
                        Ligne.NomMme = dr["NomJFilleFonct"].ToString();
                        Ligne.AdrFonct = dr["AdresseFonct"].ToString();
                        Ligne.FonctionFonct = dr["FonctionFonct"].ToString();
                        Ligne.GradeFonct = dr["GradeFonct"].ToString();

                        //*************************************************************
                        if (dr["DateDepartDefi"].ToString().Length > 0)
                        {
                            DateTime d1 = Convert.ToDateTime(dr["DateDepartDefi"].ToString());
                            Ligne.DateDD = d1.Year.ToString() + "/";
                            if (d1.Month.ToString().Length == 1)
                            {
                                Ligne.DateDD += "0";
                            }
                            Ligne.DateDD += d1.Month.ToString() + "/" + d1.Day.ToString();
                        }
                        else Ligne.DateDD = dr["DateDepartDefi"].ToString();
                        //*************************************************************

                        Ligne.MotifDD = dr["MotifDepartDefi"].ToString();

                        //*************************************************************
                        if (dr["DateDepartTmp"].ToString().Length > 0)
                        {
                            DateTime d2 = Convert.ToDateTime(dr["DateDepartTmp"].ToString());
                            Ligne.DateDT = d2.Year.ToString() + "/";
                            if (d2.Month.ToString().Length == 1)
                            {
                                Ligne.DateDT += "0";
                            }
                            Ligne.DateDT += d2.Month.ToString() + "/" + d2.Day.ToString();
                        }
                        else Ligne.DateDT = dr["DateDepartTmp"].ToString();
                        //*************************************************************

                        Ligne.MotifDT = dr["MotifDepartTmp"].ToString();

                        //*************************************************************
                        if (dr["DateRetrTmp"].ToString().Length > 0)
                        {
                            DateTime d3 = Convert.ToDateTime(dr["DateRetrTmp"].ToString());
                            Ligne.DateRT = d3.Year.ToString() + "/";
                            if (d3.Month.ToString().Length == 1)
                            {
                                Ligne.DateRT += "0";
                            }
                            Ligne.DateRT += d3.Month.ToString() + "/" + d3.Day.ToString();
                        }
                        else Ligne.DateRT = dr["DateRetrTmp"].ToString();
                        //*************************************************************
                        tab_ligne.Add(Ligne);
                    }
                    tabFonct2.ItemsSource = tab_ligne;
                    read.Close();
                }
                catch (System.IO.FileNotFoundException)
                {
                    MessageBox.Show("Le lien que vous avez entrer est incorrecte");

                }
                
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Le fichier excel est ouvert. Veuillez le fermer avant l'importer");
                }
                catch (System.IndexOutOfRangeException)
                {
                    MessageBox.Show("Le fichier Excel que vous avez séléctionner est vide");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat = MessageBox.Show("Voulez vous l'enregistrement des fonctionnaires sélectionées ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultat == MessageBoxResult.Yes)
            {

                //conn.Open();
                for (int i = 0; i < tabFonct2.Items.Count; i++)
                {
                    LigneFonctionR LigneSel = new LigneFonctionR();
                    conn.Open();
                    LigneSel = (LigneFonctionR)tabFonct2.Items[i];
                    SqlCommand sel = new SqlCommand("select Matricule from Fonctionnaire where Matricule = " + LigneSel.Matricule, conn);
                    SqlDataReader rd = sel.ExecuteReader();
                    if (rd.Read())
                    {
                        conn.Close();
                        conn.Open();
                        string o = "update Fonctionnaire set TelFonct =" + LigneSel.Telephone + ", EmailFonct ='" + LigneSel.Email + "', CompteFonct = '" + LigneSel.CompteFonct + "', CodeBanque =" + LigneSel.CodeBanque + " , SitFamFonct='" + LigneSel.SitFam + "', NomJFilleFonct='" + LigneSel.NomMme + "', DateDepartDefi='" + LigneSel.DateDD + "', MotifDepartDefi='" + LigneSel.MotifDD + "', DateDepartTmp='" + LigneSel.DateDT + "', MotifDepartTmp='" + LigneSel.MotifDT + "', DateRetrTmp='" + LigneSel.DateRT + "', AdresseFonct = '" + LigneSel.AdrFonct + "', FonctionFonct = '" + LigneSel.FonctionFonct + "', GradeFonct = '" + LigneSel.GradeFonct + "' where Matricule =" + LigneSel.Matricule;
                        SqlCommand cmd = new SqlCommand(o, conn); 
                        cmd.ExecuteReader();
                        conn.Close();
                    }
                    else
                    {
                        conn.Close();                    
                        String StrSQL, dt = LigneSel.Date;
                        if (LigneSel.SitFam == "Mme") StrSQL = "insert into Fonctionnaire values (" + LigneSel.Matricule + ",'" + LigneSel.Nom + "','" + LigneSel.Prenom + "','" + dt + "'," + LigneSel.Telephone + ",'" + LigneSel.Email + "','" + LigneSel.CompteFonct + "'," + LigneSel.CodeBanque + ",'" + LigneSel.SitFam + "','" + LigneSel.NomMme + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)";
                        else StrSQL = "insert into Fonctionnaire values (" + LigneSel.Matricule + ",'" + LigneSel.Nom + "','" + LigneSel.Prenom + "','" + dt + "'," + LigneSel.Telephone + ",'" + LigneSel.Email + "','" + LigneSel.CompteFonct + "'," + LigneSel.CodeBanque + ",'" + LigneSel.SitFam + "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)";
                        conn.Open();
                        SqlCommand commandeSQL = new SqlCommand(StrSQL, conn);
                        commandeSQL.ExecuteReader();
                        conn.Close();
                    }
                    //rd.Close();
                }
                MessageBox.Show("Enregistrement de données avec succèes ");
                //conn.Close();               
            }
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) 
        {
            Modifier m = new Modifier(conn);
            Methodes.ShowWindow(this, m);
        }

        private void tabFonct2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tabFonct2.Items.Count != 0)
            {
                var grid = sender as DataGrid;
                LigneFonctionR lig = new LigneFonctionR();
                lig = (LigneFonctionR)grid.SelectedValue;
                Window2 wind = new Window2();
                if (lig.DateDD.Length != 0) wind.ddd.Content = lig.DateDD.Substring(0, 10); else wind.ddd.Content = "";
                if (lig.DateDT.Length != 0) wind.ddt.Content = lig.DateDT.Substring(0, 10); else wind.ddt.Content = "";
                if (lig.SitFam.Substring(0, 2) != "Mr" && lig.SitFam != "Mlle")
                {
                    wind.NAM.Visibility = System.Windows.Visibility.Visible;
                    wind.nm.Content = lig.NomMme;
                }
                else wind.NAM.Visibility = System.Windows.Visibility.Hidden;
                wind.np.Content = lig.Nom + " " + lig.Prenom;
                if (lig.DateRT.Length != 0) wind.drt.Content = lig.DateRT.Substring(0, 10); else wind.drt.Content = "";
                wind.mdd.Content = lig.MotifDD;
                wind.mdt.Content = lig.MotifDT;
                Methodes.ShowWindow(this, wind);
            }

            else MessageBox.Show("La table est vide !!!");

        }                             
	}
}