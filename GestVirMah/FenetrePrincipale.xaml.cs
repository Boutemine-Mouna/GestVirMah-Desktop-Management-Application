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
using System.Threading.Tasks;

namespace GestVirMah
{
   
    public partial class FenetrePrincipale : MetroWindow
    {   
		private Button btnPressed;
        public static SqlConnection conn;
        private Utilisateur user;
        private int codeVr;
        private string nom;
        private string prenom;
        private TabItem tb = new TabItem();

    //   public static SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        public FenetrePrincipale(SqlConnection con)
        {
            InitializeComponent();
            conn = con;
        }

        #region initialisation

        public FenetrePrincipale( SqlConnection con, Utilisateur user)
        {
            InitializeComponent();
            conn = con;
 
            this.user = user;
            title.Transition = MahApps.Metro.Controls.TransitionType.Left;
            currentUser.Content = user.Nom;
            userLabel.Content = user.Nom;
            /***************************************************/
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT DesignationPret FROM TypePret where TypePret = '1' ";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ComboSousType.Items.Add(dr["DesignationPret"].ToString());
            }
            conn.Close();
            /***************************************************/

            fillDesignationtype2();
            fillDataProduit();
            dataProduit();
            datagrid_fill();
            dataPro_fill();
            dataTypePret_fill();
             fillfournisseur();
             filldesignationT1();
             filldesignationT2();
             fillRembAnticipé();

             /******************************************************/

            conn.Open();
            SqlCommand cmmd = new SqlCommand();
            cmmd = conn.CreateCommand();
            cmmd.CommandType = CommandType.Text;
            cmmd.CommandText = "SELECT CodePret FROM TypePret WHERE TypePret = '2'";
            cmmd.ExecuteNonQuery();
            SqlDataAdapter ad = new SqlDataAdapter(cmmd);
            DataTable dtt = new DataTable();
            ad.Fill(dtt);
            string ma_variable ;
            DataTable dt1 = new DataTable();
            foreach (DataRow row in dtt.Rows)
            {

                    ma_variable = row["CodePret"].ToString();
                    cmmd.CommandText = "SELECT NumDemPret,Matricule,bondecdeglobal_numbon,Motif FROM DemandePret WHERE  CodePret = '" + ma_variable + "' AND bondecdeglobal_numbon IS NOT NULL AND Etat != 'S' AND Etat !='B'";
                    cmmd.ExecuteNonQuery();
                    ad = new SqlDataAdapter(cmmd);
                    ad.Fill(dt1);
            }
            DataColumn cn = new DataColumn("Supprimer", typeof(Boolean));
            dt1.Columns.Add(cn);
            foreach (DataRow rw in dt1.Rows) rw["Supprimer"] = false;
            dataEf.DataContext = dt1;
            conn.Close();

            /********************************************************/
            conn.Open();
            SqlCommand cmd2 = new SqlCommand();
            cmd2= conn.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select NomFournisseur from Fournisseur";
            cmd2.ExecuteNonQuery();
            DataTable dt3 = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd2);
            sda.Fill(dt3);
            foreach (DataRow dr in dt3.Rows)
            {
                combobox2.Items.Add(dr["NomFournisseur"].ToString());
                ComboFournisseur.Items.Add(dr["NomFournisseur"].ToString());
                ComboFournisseur1.Items.Add(dr["NomFournisseur"].ToString());
            };
            conn.Close();

            /************************************************************/

            conn.Open();
            SqlCommand cmmmd1 = new SqlCommand();
            cmmmd1 = conn.CreateCommand();
            cmmmd1.CommandType = CommandType.Text;
            cmmmd1.CommandText = "SELECT DesignationPret FROM TypePret";
            cmmmd1.ExecuteNonQuery();
            DataTable dttt = new DataTable();
            SqlDataAdapter sda2 = new SqlDataAdapter(cmmmd1);
            sda2.Fill(dttt);
            foreach (DataRow dr in dttt.Rows)
            {
                comboPret.Items.Add(dr["DesignationPret"]);
            };
            conn.Close();

            /************************************************************/
            conn.Open();
            DataTable typeTable = new DataTable();
            SqlCommand cmd32 = new SqlCommand("SELECT CodePret FROM TypePret WHERE TypePret = '2'", conn);
            SqlDataAdapter ad32 = new SqlDataAdapter(cmd32);
            ad32.Fill(typeTable);
            string ma_variable32;
            DataTable datat32 = new DataTable();
            foreach (DataRow row in typeTable.Rows)
            {
                ma_variable32 = row["CodePret"].ToString();
                cmd32.CommandText = "SELECT NumDemPret,DateDemPret as [Date de demande],MontantVoulu as [Montant voulu],MontantAcc as [Montant accordé],PremiereEcheance as [Première Echeance],Periodicite as [Périodicité],RetenuPeriodique as [Retenue périodique],Etat,Motif,Matricule, bondecdeglobal_numbon as [Numéro du bon global]  FROM DemandePret WHERE CodePret = '" + ma_variable32 + "'";
                cmd32.ExecuteNonQuery();
                ad32 = new SqlDataAdapter(cmd32);
                ad32.Fill(datat32);
            }
            datagrid1.ItemsSource = datat32.DefaultView;
            conn.Close();

            /***********************************************************/
            conn.Open();
            SqlCommand cmdeff = new SqlCommand();
            cmdeff = conn.CreateCommand();
            cmdeff.CommandType = CommandType.Text;
            cmdeff.CommandText = "SELECT CodePret FROM TypePret WHERE TypePret = 1";
            cmdeff.ExecuteNonQuery();
            SqlDataAdapter adeff = new SqlDataAdapter(cmdeff);
            DataTable dteff = new DataTable();
            adeff.Fill(dteff);
            string ma_variable_eff;
            DataTable dt1eff = new DataTable();
            foreach (DataRow row in dteff.Rows)
            {

                ma_variable_eff = row["CodePret"].ToString();
                cmdeff.CommandText = "SELECT NumDemPret,Matricule,pv_codepv,Motif FROM DemandePret WHERE  CodePret = '" + ma_variable_eff + "' AND (Etat = 'A' OR Etat ='E' OR Etat IS NULL)";
                cmdeff.ExecuteNonQuery();
                adeff = new SqlDataAdapter(cmdeff);
                adeff.Fill(dt1eff);
            }
            DataColumn cn2 = new DataColumn("Supprimer", typeof(Boolean));
            dt1eff.Columns.Add(cn2);
            foreach (DataRow rw in dt1eff.Rows) rw["Supprimer"] = false;
            dataeff1.DataContext = dt1eff;
            conn.Close();
        }

        private void datagrid_fill()
        {
            DataTable dt;
            DataTable datat;
            SqlCommand cmd = new SqlCommand("SELECT NumDemPret,DateDemPret as [Date de demande],MontantVoulu as [Montant voulu],MontantAcc as [Montant accordé],PremiereEcheance as [Première Echeance],Periodicite as [Périodicité],RetenuPeriodique as [Retenue périodique],Etat,Motif,Matricule,pv_codepv as [Code du PV],bondecdeglobal_numbon as [Numéro du bon global] FROM DemandePret", conn);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            datat = new DataTable();
            ad.Fill(datat);
            datagrid1.ItemsSource = datat.DefaultView;
            conn.Close();
        }
        public void fillDataProduit()
        {
            DataTable dat; bool dis = true;
            String cmd2 = "select Designation from Produit where Disponible= @dis";
            SqlCommand cmduser = new SqlCommand(cmd2, conn);
            cmduser.Parameters.AddWithValue("@dis", dis);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmduser);
            dat = new DataTable();
            ad.Fill(dat);
            dat.Columns.Add("Quantité", typeof(int));
            dat.Columns.Add("selection", typeof(Boolean));
            foreach (DataRow row in dat.Rows)
            { row["Selection"] = false; }

            datagradProduit.ItemsSource = dat.DefaultView;
            conn.Close();
        }
        private void dataProduit()
        {
            DataTable datat;
            SqlCommand cmd = new SqlCommand("SELECT RefProduit,Designation,Disponible,PrixUnitHT,PrixUnitTTC FROM Produit", conn);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            datat = new DataTable();
            ad.Fill(datat);
            DataProduit.ItemsSource = datat.DefaultView;
            conn.Close();
        }

        private void dataPro_fill()
        {
            DataTable datat;
            SqlCommand cmd = new SqlCommand("SELECT RefProduit,Designation,PrixUnitHT,PrixUnitTTC,Disponible,RefFournisseur FROM Produit", conn);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            datat = new DataTable();
            ad.Fill(datat);
            dataPro.ItemsSource = datat.DefaultView;
            conn.Close();
        }
        private void dataTypePret_fill()
        {
            DataTable datat;
            SqlCommand cmd = new SqlCommand("SELECT CodePret,DesignationPret,TypePret,AbreviationPret FROM TypePret", conn);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            datat = new DataTable();
            ad.Fill(datat);
            dataTypePret.ItemsSource = datat.DefaultView;
            conn.Close();
        }

        public void fillDesignationtype2()
        {
            conn.Open();
            String t = "2";
            SqlCommand cmd = new SqlCommand();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT DesignationPret FROM TypePret WHERE TypePret='" + t + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ComboDesignation.Items.Add(dr["DesignationPret"].ToString());
                ComboDesignation1.Items.Add(dr["DesignationPret"].ToString());

            }
            conn.Close();
        }

        public bool fonctExiste(string nom, string prenom)
        {
            String cmd = "SELECT NomFonct,PrenFonct FROM Fonctionnaire";
            conn.Open();
            SqlCommand cmdUser = new SqlCommand(cmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmdUser);
            DataTable t = new DataTable();
            adapter.Fill(t);
            bool exist = false;
            foreach (DataRow dr in t.Rows)
            {
                if (dr["NomFonct"].ToString() == nom && dr["PrenFonct"].ToString() == prenom) exist = true;
            }
            conn.Close();
            return exist;

        }

        #endregion

         #region GestionPrime

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Gestion_Primes.IsSelected = true;
        }

        private void buttonClicked(Button btn)
		{
			if (btnPressed != null)
            {
                btnPressed.IsEnabled = true;
            }
            btn.IsEnabled = false;
        	btnPressed = btn;
		}


        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           #region Chergement historique

            try
            {
                conn.Open();
                string requette = "select DésignationPrime from TypePrime";
                SqlCommand cmd = new SqlCommand(requette, conn);
                comboBoxPrime.Items.Clear();
                comboPrime.Items.Clear();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBoxPrime.Items.Add(Convert.ToString(reader["DésignationPrime"]));
                    comboPrime.Items.Add(Convert.ToString(reader["DésignationPrime"]));
                }
                comboBoxPrime.Items.Add("Touts");
                comboPrime.Items.Add("Touts");
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { conn.Close(); }

            try
            {
                conn.Open();
                string requette = "select NomFonct+' '+PrenFonct AS Fonctionnaire, DésignationPrime as [type prime],DemandePrime.NumDem AS [Numéro demande],DateDem AS [Date demande],MontsantDem as Montant,CompteDem AS [Compte demandeur],EtatDem as [Etat demande],MotifEtat AS Motif from DemandePrime INNER join TypePrime on TypePrime.CodePrime = DemandePrime.CodePrime INNER JOIN dbo.Fonctionnaire ON Fonctionnaire.Matricule = DemandePrime.Matricule";
                SqlCommand cmd = new SqlCommand(requette, conn);
                SqlDataReader read = cmd.ExecuteReader();
                List<LigneHistorique> tab_ligne = new List<LigneHistorique>();
                String etat;
                while (read.Read())
                {
                    LigneHistorique ligne = new LigneHistorique();
                    ligne.Fonctionnaire = Convert.ToString(read["Fonctionnaire"]);
                    ligne.TypePrime = Convert.ToString(read["type prime"]);
                    //ligne.NumDem = Convert.ToInt32(read["Numéro demande"]);
                    DateTime date; DateTime.TryParse(read["Date Demande"].ToString(), out date);
                    ligne.DateDem = date;
                    //ligne.Montant = float.Parse((read["Montant"]).ToString());
                    //ligne.CompteFonct = Convert.ToString(read["Compte demandeur"]);
                    etat = Convert.ToString(read["Etat demande"]);
                    switch (etat)
                    {
                        case "A":
                            ligne.EtatDem = "Acceptée";
                            break;
                        case "R":
                            ligne.EtatDem = "Refusée";
                            break;
                        case "T":
                            ligne.EtatDem = "En attente";
                            break;
                        default:
                            ligne.EtatDem = "Non traitée";
                            break;

                    }
                    ligne.Motif = Convert.ToString(read["Motif"]);
                    tab_ligne.Add(ligne);
                }
                read.Close();
                dataGridHist.ItemsSource = tab_ligne;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { conn.Close(); }

            #endregion
            
        }

        #region Procédures virements

        //private void afficherListVir() //afficher la liste de virement
        //{   conn.Open();
        //    SqlCommand cmd = new SqlCommand("select *from dbo.Virement", connexionSql);
        //    DataTable dt = new DataTable("Virement");
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    //afficher_tous(virementGrid);
        //    da.Fill(dt);
               //   conn.Close();
        //    virementGrid.ItemsSource = dt.DefaultView;
        //}

        private void rech_2_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            LigneVirement row = (LigneVirement)VirementDataGrid.SelectedItem;
            if (row != null)
            {
                codeVr = int.Parse(row.CodeVir.ToString());
                detailVir fen = new detailVir(row,conn);
                Methodes.ShowWindow(this, fen);
            }
        }
        private void Virement_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<LigneVirement> virements = new ObservableCollection<LigneVirement>();
            try
            {
                conn.Open();
                string requette = "select * from Virement";
                SqlCommand cmd = new SqlCommand(requette, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    virements.Add(new LigneVirement() { CodeVir = reader[0].ToString(), DateVir = reader[1].ToString(), pv_codepv = reader[2].ToString(), DateCreatVir = reader[3].ToString().Substring(0, 10), CodeUser = reader[4].ToString(), MinistereVir = reader[5].ToString(), OrganismeVir = reader[6].ToString(), ChequeVir = reader[7].ToString(), DateChequeVir = reader[8].ToString().Substring(0, 10), CompteSocVir = reader[9].ToString(), CompteEsiVir = reader[10].ToString(), BenefVir = reader[11].ToString(), ObserVir = reader[12].ToString(), year = reader[3].ToString().Substring(6, 4) });

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { conn.Close(); }


            ICollectionView virementsView = CollectionViewSource.GetDefaultView(virements);

            // Set the grouping by city proprty
            virementsView.GroupDescriptions.Add(new PropertyGroupDescription("year"));

            // Set the view as the DataContext for the DataGrid
            VirementDataGrid.DataContext = virementsView;

        }

        private void ajouterVir_Click(object sender, RoutedEventArgs e)
        {
            ftrAjouterVirement ftr = new ftrAjouterVirement(user,conn);

            Methodes.ShowWindow(this, ftr);
            Virement_Loaded(null, null);
        }

        #endregion

        #region Procédures primes

        public void afficher_tabPrime()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TypePrime", conn);
                DataTable dt = new DataTable("TypePrime");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                tabPrime.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { conn.Close(); }
        }

        private void tabPrime_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                afficher_tabPrime();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { }
        }

        private void ajouterPrime_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
              
                AjouterPrime ajPrime = new AjouterPrime(user, conn);
                Methodes.ShowWindow(this, ajPrime);
                afficher_tabPrime();               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { }
        }

        

        private void rech_3_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            // Check if the user double-clicked a grid row and not something else
            //DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            DataRowView row = (DataRowView)tabPrime.SelectedItem;

            // If so, go ahead and do my thing
            if (row != null)
            {
                int codePrime = int.Parse(row.Row.ItemArray[0].ToString());

                infoPrime fen = new infoPrime(conn, user, codePrime);
                Methodes.ShowWindow(this, fen);
                afficher_tabPrime();
            }
        }
        
        #endregion

        #region Traitement historique

        private void textBoxNomFonc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).IsKeyboardFocusWithin)
            {

                try
                {
                    conn.Open();
                    dataGridNomFonc.UnselectAllCells();
                    string requette = "select NomFonct +' '+ PrenFonct as Fonctionnaire  from Fonctionnaire where NomFonct like '" + textBoxNomFonc.Text + "%' or PrenFonct like '" + textBoxNomFonc.Text + "%'";
                    SqlCommand cmd = new SqlCommand(requette, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<LigneFonct> tabNomFonc = new List<LigneFonct>();

                    while (reader.Read())
                    {
                        LigneFonct ligne = new LigneFonct();
                        ligne.Fonctionnaire = (reader["Fonctionnaire"]).ToString();
                        tabNomFonc.Add(ligne);
                    }
                    reader.Close();
                    cmd.Cancel();
                    dataGridNomFonc.ItemsSource = tabNomFonc;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally { conn.Close(); }
            }
        }

        private void dataGridNomFonc_SelectionChanged(object sender2, SelectionChangedEventArgs e2)
        {
            if (dataGridNomFonc.SelectedIndex >= 0)
            {
                LigneFonct LigneSel = new LigneFonct();
                LigneSel = (LigneFonct)dataGridNomFonc.SelectedItem;
                textBoxNomFonc.Text = LigneSel.Fonctionnaire;
            }

        }

        private void comboBoxPrime_Initialized(object sender, EventArgs e)
        {
            
        }

        private void comboBoxEtatDem_Initialized(object sender, EventArgs e)
        {
            comboBoxEtatDem.Items.Add("Acceptée");
            comboBoxEtatDem.Items.Add("Refusée");
            comboBoxEtatDem.Items.Add("En attente");
            comboBoxEtatDem.Items.Add("Non traitée");
            comboBoxEtatDem.Items.Add("Touts");
        }

        private void botton_rechercher_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            Boolean condition = true;
            string requette = "select NomFonct+' '+PrenFonct AS Fonctionnaire, DésignationPrime as [type prime],DemandePrime.NumDem AS [Numéro demande],DateDem AS [Date demande],MontsantDem as Montant,CompteDem AS [Compte demandeur],EtatDem as [Etat demande],MotifEtat AS Motif from DemandePrime INNER join TypePrime on TypePrime.CodePrime = DemandePrime.CodePrime INNER JOIN dbo.Fonctionnaire ON Fonctionnaire.Matricule = DemandePrime.Matricule";
            if (textBoxNomFonc.Text.Length > 0)
            {
                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                requette += " NomFonct+' '+PrenFonct = '" + textBoxNomFonc.Text + "'";
            }
            if (comboBoxPrime.SelectedIndex != -1)
            {
                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                requette += " DésignationPrime = '" + comboBoxPrime.SelectedItem.ToString() + "'";
            }
            if (comboBoxEtatDem.SelectedIndex != -1)
            {
                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                switch (comboBoxEtatDem.SelectedItem.ToString())
                {
                    case "Acceptée": requette += " EtatDem ='A'"; break;
                    case "Refusée": requette += " EtatDem ='R'"; break;
                    case "En attente": requette += " EtatDem ='T'"; break;
                    case "Non traitée": requette += " EtatDem = 'N'"; break;
                }
            }
            if (DateDeb.SelectedDate.HasValue)
            {
                if (!DateFin.SelectedDate.HasValue) MessageBox.Show(" Entrer la date de fin");
                else
                {
                    if (condition)
                    {
                        condition = false;
                        requette += " where";
                    }
                    else requette += " AND";
                    requette += " DateDem BETWEEN '" + (DateDeb.Text.Substring(6, 4) + "/" + DateDeb.Text.Substring(3, 3) + DateDeb.Text.Substring(0, 2)).ToString() + "' AND '" + (DateFin.Text.Substring(6, 4) + "/" + DateFin.Text.Substring(3, 3) + DateFin.Text.Substring(0, 2)).ToString() + "'";
                }
            }
            try
            {    
                SqlCommand cmd = new SqlCommand(requette, conn);
                SqlDataReader read = cmd.ExecuteReader();
                List<LigneHistorique> tab_ligne = new List<LigneHistorique>();
                String etat;
                while (read.Read())
                {
                    LigneHistorique ligne = new LigneHistorique();
                    ligne.Fonctionnaire = Convert.ToString(read["Fonctionnaire"]);
                    ligne.TypePrime = Convert.ToString(read["type prime"]);
                    //ligne.NumDem = Convert.ToInt32(read["Numéro demande"]);
                    DateTime date; DateTime.TryParse(read["Date Demande"].ToString(), out date);
                    ligne.DateDem = date;
                    //ligne.Montant = float.Parse((read["Montant"]).ToString());
                    //ligne.CompteFonct = Convert.ToString(read["Compte demandeur"]);
                    etat = Convert.ToString(read["Etat demande"]);
                    switch (etat)
                    {
                        case "A":
                            ligne.EtatDem = "Acceptée";
                            break;
                        case "R":
                            ligne.EtatDem = "Refusée";
                            break;
                        case "T":
                            ligne.EtatDem = "En attente";
                            break;
                        default:
                            ligne.EtatDem = "Non traitée";
                            break;
                    }
                    ligne.Motif = Convert.ToString(read["Motif"]);
                    tab_ligne.Add(ligne);
                }
                read.Close();
                dataGridHist.ItemsSource = tab_ligne;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { }
            conn.Close();
        }

        private void comboBoxPrime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxPrime.SelectedIndex != -1) if (comboBoxPrime.SelectedItem.ToString() == "Touts") comboBoxPrime.SelectedIndex = -1;

        }

        private void comboBoxEtatDem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxEtatDem.SelectedIndex != -1) if (comboBoxEtatDem.SelectedItem.ToString() == "Touts") comboBoxEtatDem.SelectedIndex = -1;
        }

        #endregion

        #region Bouttons de navigation


        private void virBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Virements.IsSelected = true;
        }


        private void fonctBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            fonctionnaire.IsSelected = true;
        }

        private void rechBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            histoprim.IsSelected = true;
        }

        private void statBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            statprim.IsSelected = true;
        }

        private void homeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	acceuil.IsSelected = true;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tmpItem = new TabItem();
            tmpItem = (TabItem)tabControl.SelectedItem;
            title.Content = tmpItem.Header;
        }

        #endregion

        #region Bouttons barre de titre

        private async void signOutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			MessageDialogResult result = await this.ShowMessageAsync("Déconnexion",
                "Voulez-vous vraiment vous déconnecter?",
                MessageDialogStyle.AffirmativeAndNegative, Methodes.ConfirmationSettings);
			if (result == MessageDialogResult.Affirmative)
			{
				Connexion fen = new Connexion();
				this.Close();
				fen.Show();
			}
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            Aide fen = new Aide();
            Methodes.ShowWindow(this, fen);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            Apropos fen = new Apropos();
            Methodes.ShowWindow(this, fen);
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            Parametres fen = new Parametres(conn, user);
            Methodes.ShowWindow(this, fen);
            user = fen.User;
            currentUser.Content = user.Nom;
            userLabel.Content = user.Nom;
        }

        #endregion

        #region Utilisateur

        private void currentUser_Click(object sender, RoutedEventArgs e)
        {
            FenUtilisateur fen = new FenUtilisateur(conn, user);
            if (user.Droits == 'S')
            {
                fen.userPermissions.Visibility = Visibility.Collapsed;
                fen.droits.Visibility = Visibility.Collapsed;
                fen.rect.Height -= 30;
            }
            Methodes.ShowWindow(this, fen);
            user = fen.User;
            currentUser.Content = user.Nom;
            userLabel.Content = user.Nom;
        }

        #endregion

        #region Demandes

        private void nomFonct_TextChanged(object sender, TextChangedEventArgs e)
        {
            Demande dem = new Demande(conn); 
            dataGridFonct.UnselectAllCells();
            dataGridFonct.ItemsSource = dem.fonctionaire(nomFonct.Text);
        }

        private void primes_Loaded(object sender, RoutedEventArgs e)
        {
            Demande dem = new Demande(conn);
            if (primes.Items.Count == 0)
            {
                foreach (String s in dem.typePrime())
                {
                    primes.Items.Add(s);
                }
            }
            
        }

        private void dataGridFonct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridFonct.SelectedIndex >= 0)
            {
                LigneNomPren ligne = new LigneNomPren();
                ligne = (LigneNomPren)dataGridFonct.SelectedItems[0];
                nom = ligne.nom;
                prenom = ligne.prenom;
                nomFonct.Text = nom + " " + prenom;
            }

        }
        public void nvDem()
        {
            Demande dem = new Demande(conn);
            if (!dem.isBatch(primes.SelectedItem.ToString()))
            {
                switch (primes.SelectedItem.ToString())
                {


                    case "Décès salarié":
                        textBoxDefunt.Visibility = Visibility.Visible;
                        comboLien.Visibility = Visibility.Visible;
                        labeLien.Visibility = Visibility.Visible;
                        sitFamLabel.Visibility = Visibility.Visible;
                        SitFam.Visibility = Visibility.Visible;
                        nomFonct.Visibility = Visibility.Visible;
                        dataGridFonct.Visibility = Visibility.Visible;
                        montantBox.Visibility = Visibility.Hidden;
                        batch.Visibility = Visibility.Hidden;
                        TextBoxHelper.SetWatermark(nomFonct, "Défunt");
                        TextBoxHelper.SetWatermark(textBoxDefunt, "Demandeur");
                        break;
                    case "don":
                        montantBox.Visibility = Visibility.Visible;
                        textBoxDefunt.Visibility = Visibility.Hidden;
                        comboLien.Visibility = Visibility.Hidden;
                        labeLien.Visibility = Visibility.Hidden;
                        sitFamLabel.Visibility = Visibility.Hidden;
                        SitFam.Visibility = Visibility.Hidden;
                        batch.Visibility = Visibility.Hidden;
                        TextBoxHelper.SetWatermark(nomFonct, "Demandeur");
                        break;
                    default:
                        textBoxDefunt.Visibility = Visibility.Hidden;
                        comboLien.Visibility = Visibility.Hidden;
                        labeLien.Visibility = Visibility.Hidden;
                        sitFamLabel.Visibility = Visibility.Hidden;
                        SitFam.Visibility = Visibility.Hidden;
                        batch.Visibility = Visibility.Hidden;
                        nomFonct.Visibility = Visibility.Visible;
                        dataGridFonct.Visibility = Visibility.Visible;
                        TextBoxHelper.SetWatermark(nomFonct, "Demandeur");
                        break;
                }

            }
            else
            {
                nomFonct.Visibility = Visibility.Hidden;
                dataGridFonct.Visibility = Visibility.Hidden;
                textBoxDefunt.Visibility = Visibility.Hidden;
                comboLien.Visibility = Visibility.Hidden;
                labeLien.Visibility = Visibility.Hidden;
                sitFamLabel.Visibility = Visibility.Hidden;
                SitFam.Visibility = Visibility.Hidden;
                montantBox.Visibility = Visibility.Hidden;
                batch.Visibility = Visibility.Visible;

            }

        }

        private void primes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (primes.SelectedIndex != -1)
                nvDem();
        }

        public Boolean decision()
        {
            Demande dem = new Demande(conn);
            Boolean bol = false;
            if (primes.SelectedItem.ToString() != "")
            {
                if (DateDem.Text != "")
                {
                    if (DatEvent.Text != "")
                    {
                        if (primes.SelectedItem.ToString() == "Décès salarié")
                        {
                            ReportDeces rapport = new ReportDeces();
                            if (nomFonct.Text != "")
                            {
                                if (textBoxDefunt.Text != "")
                                {
                                    if (comboLien.Text != "")
                                    {
                                        if (SitFam.Text != "")
                                        {
                                           
                                            int i = dem.getMontant("Décès salarié");
                                            rapport.SetParameterValue("client", textBoxDefunt.Text);
                                            rapport.SetParameterValue("defunt", nomFonct.Text);
                                            rapport.SetParameterValue("lettre", dem.converti(i) + " dinars");
                                            rapport.SetParameterValue("lien", comboLien.Text);
                                            rapport.SetParameterValue("montant", i.ToString());
                                            rapport.SetParameterValue("sitFamFon", dem.fonctInfo(nom, prenom).Rows[0]["SitFamFonct"].ToString());
                                            rapport.SetParameterValue("sitFamFon2", SitFam.Text);
                                            bol = true;
                                            Decision f = new Decision();
                                            f.afficher(rapport);
                                            //this.Hide();
                                            f.Show();
                                        }
                                        else MessageBox.Show(" donnez la situation familiale ");
                                    }
                                    else MessageBox.Show("Donner le lien");

                                }
                                else MessageBox.Show("Donner le nom defunt");
                            }
                            else MessageBox.Show("Donner le nom du client");
                        }
                        else
                        {
                            if (primes.SelectedItem.ToString() == "don")
                            {
                                if (montantBox.Text != "")
                                { 
                                    ReportDon rapport = new ReportDon();
                                    rapport.SetParameterValue("client", nomFonct.Text);
                                    try
                                    {
                                   
                                        rapport.SetParameterValue("lettre", dem.converti(int.Parse(montantBox.Text)) + " dinars");
                                        rapport.SetParameterValue("montant", montantBox.Text);
                                        bol = true;
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show(e.Message);
                                        return false;
                                    }
                                    rapport.SetParameterValue("sitFamFon",dem.fonctInfo(nom, prenom).Rows[0]["SitFamFonct"].ToString());
                                    Decision f = new Decision();
                                    f.afficher(rapport);
                                    //this.Hide();
                                    f.Show();

                                }
                                else MessageBox.Show("donnez le montant ");
                            }
                            else
                            {
                                if (nomFonct.Text != "")
                                {
                                   
                                    ReportPrime rapport = new ReportPrime();
                                    int i = dem.getMontant(primes.SelectedItem.ToString());
                                    rapport.SetParameterValue("client", nomFonct.Text);
                                    rapport.SetParameterValue("typePrime", primes.SelectedItem.ToString());
                                    rapport.SetParameterValue("tel", dem.fonctInfo(nom, prenom).Rows[0][1].ToString());
                                    rapport.SetParameterValue("lettre", dem.converti(i) + " dinars");
                                    rapport.SetParameterValue("montant", i.ToString());
                                    rapport.SetParameterValue("sitFamFon", dem.fonctInfo(nom, prenom).Rows[0]["SitFamFonct"].ToString());
                                    Decision f = new Decision();
                                    f.afficher(rapport);
                                    //this.Hide();
                                    f.Show();
                                    bol = true;
                                }
                                else MessageBox.Show("Donner le nom du client");

                            }
                        }
                    }
                    else MessageBox.Show("donnez la date de l'événement");

                }
                else MessageBox.Show("donnez la date de la demande");

            }
            return bol;
        }
        private void sauvDem_Click(object sender, RoutedEventArgs e)
        {
            Demande dem = new Demande(conn);
            if (primes.Text != "")
            {
                if (!dem.isBatch(primes.SelectedItem.ToString()))
                {

                    MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder la demande ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultat == MessageBoxResult.Yes)
                    {
                        if (decision())
                        {
                            if (primes.Text == "Don")
                            {
                                dem.creerDemande(nom, prenom, montantBox.Text, user.Code.ToString(), primes.SelectedItem.ToString(), DatEvent, DateDem, "N");
                            }
                            else
                            {
                                dem.creerDemande(nom, prenom, dem.getMontant(primes.SelectedItem.ToString()).ToString(), user.Code.ToString(), primes.SelectedItem.ToString(), DatEvent, DateDem, "N");
                            }
                        }
                    }
                }
                else
                {
                    if (DateDem.Text != "")
                    {
                        if (DatEvent.Text != "")
                        {
                            MessageBoxResult resultat = MessageBox.Show("Voulez vous lancer le batch?", "Confirmation batch ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (resultat == MessageBoxResult.Yes)
                            {
                                
                                string codePv = dem.lastPv(DateDem, user.Code.ToString())[0];
                                foreach (DataRow r in dem.batch_accepte().Rows)
                                {
                                    string nom = r[1].ToString();
                                    string prenom = r[2].ToString();
                                    dem.creerDemande(nom, prenom, dem.getMontant(primes.SelectedItem.ToString()).ToString(), user.Code.ToString(), primes.SelectedItem.ToString(), DatEvent, DateDem, "A", codePv);
                                }
                            }
                        }
                        else MessageBox.Show("veillez donner la date de l'événement ");
                    }
                    else MessageBox.Show("veillez donner la date de la demande ");
                }
            }
            else MessageBox.Show("veillez choisir le type dela prime");
            MetroWindow_Loaded(null, null);
            buttonAnnulerPv.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            foreach (var item in gridNewDem.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).Clear();

                if (item is DatePicker)
                    ((DatePicker)item).Text = "";

                if (item is ComboBox)
                    ((ComboBox)item).SelectedIndex = -1;
            }
        }

        #endregion

        #region Fontionnaires

        private void ajouterFonct_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void tabFonct_Loaded(object sender, RoutedEventArgs e)
        {
            Methodes.afficher_tabFonct(tabFonct, conn);
        }

        private void suppFonct_Click(object sender, RoutedEventArgs e)
        {
            String MessageSup = "";
            MessageBoxResult resultat = MessageBox.Show("Voulez vous vraiment supprimer les fonctionnaires sélectionées ?", "Supprimer fonctionnaire", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultat == MessageBoxResult.Yes)
            {
                for (int i = 0; i < tabFonct.SelectedItems.Count; i++)
                {
                    LigneFonctionR LigneSel = new LigneFonctionR();
                    LigneSel = (LigneFonctionR)tabFonct.SelectedItems[i];
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Fonctionnaire WHERE Matricule = " + LigneSel.Matricule, conn);
                    SqlDataReader rd = cmd.ExecuteReader();
                    rd.Close();
                    conn.Close();
                    MessageSup = MessageSup + "Nom " + LigneSel.Nom + " Prenom " + LigneSel.Prenom + " supprimée\n";
                }
                MessageBox.Show(MessageSup, "Suppression terminée", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Methodes.afficher_tabFonct(tabFonct, conn);
        }

        private void Recherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            conn.Open();
            string SQLCmd = "SELECT * FROM Fonctionnaire ";
            if (Recherche.Text.Length > 0) SQLCmd = SQLCmd + " where NomFonct+' '+PrenFonct like'" + Recherche.Text + "%'";
            SqlCommand cmd = new SqlCommand(SQLCmd, conn);
            SqlDataReader read = cmd.ExecuteReader();
            List<LigneFonctionR> tab_ligne = new List<LigneFonctionR>();
            while (read.Read())
            {
                LigneFonctionR Ligne = new LigneFonctionR();
                Ligne.Matricule = Convert.ToString(read["Matricule"]);
                Ligne.Nom = Convert.ToString(read["NomFonct"]);
                Ligne.Prenom = Convert.ToString(read["PrenFonct"]);
                Ligne.Date = Convert.ToString(read["DateRecrut"]);
                Ligne.Telephone = Convert.ToString(read["TelFonct"]);
                Ligne.Email = Convert.ToString(read["EmailFonct"]);
                Ligne.CompteFonct = Convert.ToString(read["CompteFonct"]);
                Ligne.CodeBanque = Convert.ToString(read["CodeBanque"]);
                Ligne.SitFam = Convert.ToString(read["SitFamFonct"]);
                Ligne.NomMme = Convert.ToString(read["NomJFilleFonct"]);
                Ligne.DateDD = Convert.ToString(read["DateDepartDefi"]);
                Ligne.MotifDD = Convert.ToString(read["MotifDepartDefi"]);
                Ligne.DateDT = Convert.ToString(read["DateDepartTmp"]);
                Ligne.MotifDT = Convert.ToString(read["MotifDepartTmp"]);
                Ligne.DateRT = Convert.ToString(read["DateRetrTmp"]);
                Ligne.AdrFonct = Convert.ToString(read["AdresseFonct"]);
                Ligne.FonctionFonct = Convert.ToString(read["FonctionFonct"]);
                Ligne.GradeFonct = Convert.ToString(read["GradeFonct"]);
                tab_ligne.Add(Ligne);
            }
            read.Close();
            conn.Close();
            tabFonct.ItemsSource = tab_ligne;
        }

        private void ModifierFonct_Click(object sender, RoutedEventArgs e)
        {
            UpdateFonctionrList fen = new UpdateFonctionrList(conn);
            Methodes.ShowWindow(this, fen);
            Methodes.afficher_tabFonct(tabFonct, conn);
        }

        private void ModifierFonct_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateFonctionrList fen = new UpdateFonctionrList(conn);
            Methodes.ShowWindow(this, fen);
            Methodes.afficher_tabFonct(tabFonct, conn);
        }

        private void tabFonct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tabFonct.Items.Count != 0)
            {
                try
                {
                    var grid = sender as DataGrid;
                    LigneFonctionR lig = new LigneFonctionR();
                    lig = (LigneFonctionR)grid.SelectedValue;
                    Window2 wind = new Window2();
                    wind.np.Content = lig.Nom + " " + lig.Prenom;
                    if (lig.SitFam.Substring(0, 2) != "Mr" && lig.SitFam != "Mlle")
                    {
                        wind.NAM.Visibility = System.Windows.Visibility.Visible;
                        wind.nm.Content = lig.NomMme;
                    }
                    else wind.NAM.Visibility = System.Windows.Visibility.Hidden;

                    if (lig.DateDD != "") wind.ddd.Content = lig.DateDD.Substring(0, 10);
                    if (lig.DateDT != "") wind.ddt.Content = lig.DateDT.Substring(0, 10);
                    if (lig.DateRT != "") wind.drt.Content = lig.DateRT.Substring(0, 10);
                    wind.mdd.Content = lig.MotifDD;
                    wind.mdt.Content = lig.MotifDT;
                    Methodes.ShowWindow(this, wind);
                }
                catch (Exception)
                {
                    Console.WriteLine("PAS DE SELECTION");
                }
            }
            else
            {
                MessageBox.Show("La table est vide !!!");
            }
        }

        #endregion

        #region PV Demandes

        public static String maxCodePV(String date)
        {
            String cmd = "select MAX(CodePV) as CodePV from PV where DatePV like '" + date + "%'";
            //SqlConnection con = new SqlConnection("Data Source=DELL\\SIDAHMED;Initial Catalog=OeuvresSociales;Integrated Security=SSPI; ");
            try
            {
                conn.Open();
                SqlCommand cmdCodePV = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdCodePV.ExecuteReader();
                ReadUser.Read();

                if (ReadUser["CodePV"] != DBNull.Value)
                {
                    return ReadUser["CodePV"].ToString();
                }

                else return null;
            }
            catch (Exception )
            {
                //MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        /***********************************************/
        public static int CodePV(String d)
        {
            int date = int.Parse(d.Substring(2, 2));
            String date2 = d.Substring(0, 4);
            int maxCode = 0;
            if (maxCodePV(date2) == null)
            {
                maxCode = 0;
            }
            else
            {
                if (int.Parse(maxCodePV(date2).Substring(0, 2)) < date)
                {
                    maxCode = 0;
                }
                else
                {
                    maxCode = (int.Parse(maxCodePV(date2)) % 100) + 1;
                }

            }
            int i = ((date * 100) + maxCode);
            return i;
        }

        public class LignePV
        {
            public string TypePrime { get; set; }

            public int NumDem { get; set; }
            public float Montant { get; set; }
            public string Fonctionnaire { get; set; }
            public int Matricule { get; set; }
            public string etatDem { get; set; }
            public string Motif { get; set; }

        }

        public class LignePVpret
        {
            public int NumDemPret { get; set; }

            public string DateDemPret { get; set; }
            public float MontantVoulu { get; set; }
            public float MontantAcc { get; set; }
            public string PremiereEcheance { get; set; }
            public int Periodicite { get; set; }
            public float RetenuPeriodique { get; set; }
            public string Etat { get; set; }
            public string Motif { get; set; }
            public int Matricule { get; set; }

        }
        private void TabPv_Loaded(object sender, RoutedEventArgs e)
        {
            buttonAnnulerPv.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));            
        }



        private void buttonExportPdf_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand cmdEntetes = new SqlCommand("select Ministere, Organisme from Parametres", conn);
            SqlDataReader readerEntetes = cmdEntetes.ExecuteReader(); readerEntetes.Read();
            String ministere = readerEntetes["Ministere"].ToString(), organisme = readerEntetes["Organisme"].ToString();
            readerEntetes.Close();
            conn.Close();
            try
            {
                System.IO.FileStream fs = new FileStream("doc\\listDem.pdf", FileMode.Create);

                // Create an instance of the document class which represents the PDF document itself.
                Document document = new Document(PageSize.A4, 2f, 2f, 2f, 2f);
                // Create an instance to the PDF file by creating an instance of the PDF
                // Writer class using the document and the filestrem in the constructor.
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                //------add image to pdf ----------------------
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance("img" + "\\logo.png");
                imgLogo.ScalePercent(70f);
                imgLogo.SetAbsolutePosition(20, 760);

                document.Add(imgLogo);

                iTextSharp.text.Image imgEsi = iTextSharp.text.Image.GetInstance("img" + "\\esi_logo.png");
                imgEsi.ScalePercent(7f);
                imgEsi.SetAbsolutePosition(450, 775);
                document.Add(imgEsi);


                //------ajouter entete------------------------
                Phrase ph3 = new Phrase("\n\n");
                iTextSharp.text.Paragraph p3 = new iTextSharp.text.Paragraph(ph3);
                document.Add(p3);
                document.Add(p3);
                document.Add(p3);

                Phrase ph = new Phrase(ministere);
                iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(ph);
                p.FirstLineIndent = 100;
                document.Add(p);
                ph = new Phrase(organisme);
                p = new iTextSharp.text.Paragraph(ph);
                p.FirstLineIndent = 100;
                document.Add(p);
                Phrase ph2 = new Phrase("	Comité  des  œuvres  sociales ");
                iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(ph2);
                p2.FirstLineIndent = 195;
                document.Add(p2);
                document.Add(p3);
                Phrase ph5 = new Phrase("Le : " + DateTime.Today.ToShortDateString());
                iTextSharp.text.Paragraph p5 = new iTextSharp.text.Paragraph(ph5);
                p5.FirstLineIndent = 400;
                document.Add(p5);
                Phrase ph4 = new Phrase("Liste des demandes de primes :");
                Font fontP4 = new Font();
                fontP4.SetStyle(iTextSharp.text.Font.UNDERLINE);
                iTextSharp.text.Paragraph p4 = new iTextSharp.text.Paragraph(ph4);
                p4.FirstLineIndent = 100;
                p4.Font = fontP4;
                document.Add(p4);
                document.Add(p3);
                //------ export table to pdf ------------------
                PdfPTable table = new PdfPTable(dataGridPV.Columns.Count);
                Font fontHeader = new Font();
                fontHeader.SetStyle("Italic");
                fontHeader.Size = 8;
                for (int j = 0; j < dataGridPV.Columns.Count; j++)
                {
                    table.AddCell(new Phrase(dataGridPV.Columns[j].Header.ToString(), fontHeader));
                }
                table.HeaderRows = 1;
                Font font = new Font();
                font.Size = 10;
                for (int h = 0; h < dataGridPV.Items.Count; h++)
                {
                    LignePV lignePdf = new LignePV();
                    lignePdf = (LignePV)dataGridPV.Items[h];
                    table.AddCell(new Phrase(lignePdf.NumDem.ToString(), font));
                    table.AddCell(new Phrase(lignePdf.TypePrime, font));
                    table.AddCell(new Phrase(lignePdf.Montant.ToString(), font));
                    table.AddCell(new Phrase(lignePdf.Fonctionnaire, font));
                    table.AddCell(new Phrase(lignePdf.Matricule.ToString(), font));
                    table.AddCell(new Phrase("", font));
                    table.AddCell(new Phrase("", font));
                }
                Phrase phra4 = new Phrase("Liste des demandes de prets :");
                Font font4 = new Font();
                font4.SetStyle(iTextSharp.text.Font.UNDERLINE);
                iTextSharp.text.Paragraph para4 = new iTextSharp.text.Paragraph(phra4);
                para4.FirstLineIndent = 100;
                para4.Font = font4;
                document.Add(para4);

                //------ export table to pdf 2------------------
                PdfPTable table1 = new PdfPTable(datagrid2.Columns.Count);
                for (int j = 0; j < datagrid2.Columns.Count; j++)
                {
                    table1.AddCell(new Phrase(datagrid2.Columns[j].Header.ToString(), fontHeader));
                }
                table1.HeaderRows = 1;
                for (int h = 0; h < datagrid2.Items.Count; h++)
                {
                    LignePVpret lignePdf1 = new LignePVpret();
                    lignePdf1 = (LignePVpret)datagrid2.Items[h];
                    table1.AddCell(new Phrase(lignePdf1.NumDemPret.ToString(), font));
                    table1.AddCell(new Phrase(lignePdf1.DateDemPret, font));
                    table1.AddCell(new Phrase(lignePdf1.MontantVoulu.ToString(), font));
                    table1.AddCell(new Phrase(lignePdf1.MontantAcc.ToString(), font));
                    table1.AddCell(new Phrase((lignePdf1.PremiereEcheance).Substring(0, 10), font));
                    table1.AddCell(new Phrase(lignePdf1.Periodicite.ToString(), font));
                    table1.AddCell(new Phrase(lignePdf1.RetenuPeriodique.ToString(), font));
                    table1.AddCell(new Phrase(lignePdf1.Etat, font));
                    table1.AddCell(new Phrase(lignePdf1.Motif, font));
                    table1.AddCell(new Phrase(lignePdf1.Matricule.ToString(), font));
                    table1.AddCell(new Phrase("", font));
                    table1.AddCell(new Phrase("", font));
                }
                float[] widths = new float[] { 55f, 90f, 60f, 60f, 90f, 61f, 62f, 75f, 60f, 60f };
                table1.SetWidths(widths);
                document.Add(table1);
                document.Close();
                fs.Close();

                ViewPdf fen = new ViewPdf("doc\\listDem.pdf", "Liste des demandes non traitées");
                
                Methodes.ShowWindow(this, fen);
            }
                
            catch { MessageBox.Show("Fichier déja ouvert : veuillez le fermer depuis le gestionnaire de tache,ou redemmarer l'application"); };
        }


        private void buttonEnregPV_Click(object sender, RoutedEventArgs e)  //////////c laaaaaaaaaaa
        {

            if (!DatePV.SelectedDate.HasValue) MessageBox.Show(" Entrer la date du PV");
            else
            {
                Boolean decisionOK = true;
                for (int t = 0; t < dataGridPV.Items.Count; t++) // pour vérifier  si tout les  decision sont remplit
                {
                    try
                    {
                        //assessmentBox = dataGridPV.Columns[5].GetCellContent(dataGridPV.ItemContainerGenerator.ContainerFromIndex(t) as DataGridRow) as ComboBox;
                        LignePV lignePvTest = new LignePV();
                        lignePvTest = (LignePV)dataGridPV.Items[t];
                        lignePvTest.etatDem.ToString();
                    }
                    catch
                    {
                        MessageBox.Show("Vérifiez que vous avez introduit l'état de la demande dans les demandes de primes");
                        decisionOK = false;
                        break;
                    }
                }

                for (int t = 0; t < datagrid2.Items.Count; t++) // pour vérifier  si tout les  decision sont remplit
                {
                    try
                    {
                        //assessmentBox = dataGridPV.Columns[5].GetCellContent(dataGridPV.ItemContainerGenerator.ContainerFromIndex(t) as DataGridRow) as ComboBox;
                        LignePVpret lignePvpretTest = new LignePVpret();
                        lignePvpretTest = (LignePVpret)datagrid2.Items[t];
                        lignePvpretTest.Etat.ToString();
                        lignePvpretTest.PremiereEcheance.ToString();
                    }
                    catch
                    {
                        MessageBox.Show("Vérifiez que vous avez introduit l'état de la demande et la date de la première échéance dans les demandes de prets");
                        decisionOK = false;
                        break;
                    }
                }

                if (decisionOK)
                {

                    conn.Open();
                    SqlCommand cmdEntetes = new SqlCommand("select Ministere, Organisme from Parametres", conn);
                    SqlDataReader readerEntetes = cmdEntetes.ExecuteReader(); readerEntetes.Read();
                    String ministere = readerEntetes["Ministere"].ToString(), organisme = readerEntetes["Organisme"].ToString();
                    readerEntetes.Close();
                    conn.Close();
                    //------------création du PV
                    int codePV = CodePV(DatePV.Text.Substring(6, 4) + "/" + DatePV.Text.Substring(3, 3) + DatePV.Text.Substring(0, 2));
                    int codeuser = user.Code;
                    DateTime dtPV = DateTime.Today;
                    conn.Open();
                    String reqtPv = "insert into PV values( " + codePV + ",'" + DatePV.Text.Substring(6, 4) + "/" + DatePV.Text.Substring(3, 3) + DatePV.Text.Substring(0, 2) + "','" + dtPV.ToString().Substring(6, 4) + "/" + dtPV.ToString().Substring(3, 3) + dtPV.ToString().Substring(0, 2) + "'," + codeuser + ",'N')";
                    SqlCommand cmdAddPv = new SqlCommand(reqtPv, conn);
                    cmdAddPv.ExecuteNonQuery();

                    int numDem = 0;
                    String decision = "";
                    String motif = "";
                    String etatDem = "";

                    conn.Close();
                    System.IO.FileStream fs = new FileStream("doc\\PV" + codePV.ToString() + ".pv", FileMode.Create);
                    // Create an instance of the document class which represents the PDF document itself.
                    Document documentPV = new Document(PageSize.A4, 2f, 2f, 2f, 2f);
                    // Create an instance to the PDF file by creating an instance of the PDF 
                    // Writer class using the document and the filestrem in the constructor.
                    PdfWriter writer = PdfWriter.GetInstance(documentPV, fs);
                    documentPV.Open();

                    //------add image to pdf ----------------------
                    iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance("img" + "\\logo.png");
                    imgLogo.ScalePercent(70f);
                    imgLogo.SetAbsolutePosition(20, 760);

                    documentPV.Add(imgLogo);

                    iTextSharp.text.Image imgEsi = iTextSharp.text.Image.GetInstance("img" + "\\esi_logo.png");
                    imgEsi.ScalePercent(7f);
                    imgEsi.SetAbsolutePosition(450, 775);
                    documentPV.Add(imgEsi);


                    //------ajouter entete------------------------
                    Phrase ph3 = new Phrase("\n\n");
                    iTextSharp.text.Paragraph p3 = new iTextSharp.text.Paragraph(ph3);
                    documentPV.Add(p3);
                    documentPV.Add(p3);
                    documentPV.Add(p3);
                    Phrase ph = new Phrase(ministere);
                    iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(ph);
                    p.FirstLineIndent = 100;
                    documentPV.Add(p);
                    ph = new Phrase(organisme);
                    p = new iTextSharp.text.Paragraph(ph);
                    p.FirstLineIndent = 100;
                    documentPV.Add(p);
                    Phrase ph2 = new Phrase("	Comité  des  œuvres  sociales ");
                    iTextSharp.text.Paragraph p2 = new iTextSharp.text.Paragraph(ph2);
                    p2.FirstLineIndent = 195;
                    documentPV.Add(p2);
                    documentPV.Add(p3);
                    Phrase ph5 = new Phrase("Le : " + DatePV.SelectedDate.ToString().Substring(0, 10));
                    iTextSharp.text.Paragraph p5 = new iTextSharp.text.Paragraph(ph5);
                    p5.FirstLineIndent = 400;
                    documentPV.Add(p5);
                    Phrase ph4 = new Phrase(" PV N°:" + codePV.ToString());
                    Font fontP4 = new Font();
                    fontP4.SetStyle(iTextSharp.text.Font.UNDERLINE);
                    iTextSharp.text.Paragraph p4 = new iTextSharp.text.Paragraph(ph4);
                    p4.FirstLineIndent = 100;
                    p4.Font = fontP4;
                    documentPV.Add(p4);
                    documentPV.Add(p3);

                    Phrase ind = new Phrase("    Les demandes de primes traitées :\n\n");
                    iTextSharp.text.Paragraph pind = new iTextSharp.text.Paragraph(ind);
                    documentPV.Add(pind);

                    PdfPTable tablePV = new PdfPTable(dataGridPV.Columns.Count);
                    Font fontHeader = new Font();
                    fontHeader.SetStyle("Italic");
                    fontHeader.Size = 8;
                    for (int j = 0; j < dataGridPV.Columns.Count; j++)
                    {
                        tablePV.AddCell(new Phrase(dataGridPV.Columns[j].Header.ToString(), fontHeader));
                    }
                    tablePV.HeaderRows = 1;
                    Font font = new Font();
                    font.Size = 10;

                    PdfPTable tablepv1 = new PdfPTable(datagrid2.Columns.Count);
                    for (int j = 0; j < datagrid2.Columns.Count; j++)
                    {
                        tablepv1.AddCell(new Phrase(datagrid2.Columns[j].Header.ToString(), fontHeader));
                    }
                    tablepv1.HeaderRows = 1;

                    for (int i = 0; i < dataGridPV.Items.Count; i++)
                    {
                        LignePV lignePv = new LignePV();
                        lignePv = (LignePV)dataGridPV.Items[i];
                        numDem = lignePv.NumDem;
                        decision = lignePv.etatDem;
                        motif = lignePv.Motif;
                        switch (decision)
                        {
                            case "Acceptée": etatDem = "A"; break;
                            case "Refusée": etatDem = "R"; break;
                            case "En attente": etatDem = "T"; break;
                        }
                        conn.Open();
                        string requette = "update DemandePrime Set EtatDem ='" + etatDem + "',MotifEtat='" + motif + "',pv_codepv=" + codePV + " where NumDem =" + numDem;
                        SqlCommand cmdMajDem = new SqlCommand(requette, conn);
                        cmdMajDem.ExecuteNonQuery();
                        conn.Close();

                        // remplir PV(pdf)
                        tablePV.AddCell(new Phrase(lignePv.NumDem.ToString(), font));
                        tablePV.AddCell(new Phrase(lignePv.TypePrime, font));
                        tablePV.AddCell(new Phrase(lignePv.Montant.ToString(), font));
                        tablePV.AddCell(new Phrase(lignePv.Fonctionnaire, font));
                        tablePV.AddCell(new Phrase(lignePv.Matricule.ToString(), font));
                        tablePV.AddCell(new Phrase(lignePv.etatDem.ToString(), font));
                        tablePV.AddCell(new Phrase(motif, font));
                    }
                    float[] widths = new float[] { 30f, 60f, 35f, 55f, 35f, 45f, 85f };
                    tablePV.SetWidths(widths);
                    documentPV.Add(tablePV);

                    ph3 = new Phrase("\n\n");
                    p3 = new iTextSharp.text.Paragraph(ph3);
                    documentPV.Add(p3);
                    documentPV.Add(p3);

                    Phrase ind2 = new Phrase("    Les demandes de prets traitées :\n\n");
                    iTextSharp.text.Paragraph pind2 = new iTextSharp.text.Paragraph(ind2);
                    documentPV.Add(pind2);

                    int numedempret = 0;
                    float montantaccor = 0;
                    int perio = 0;
                    float retperio = 0;
                    string eta = "";
                    string moti = "";
                    string etatd = "";
                    string preEch = "";
                    for (int i = 0; i < datagrid2.Items.Count; i++)
                    {
                        LignePVpret lignePv1 = new LignePVpret();
                        lignePv1 = (LignePVpret)datagrid2.Items[i];
                        numedempret = lignePv1.NumDemPret;
                        montantaccor = lignePv1.MontantAcc;
                        perio = lignePv1.Periodicite;
                        retperio = lignePv1.RetenuPeriodique;
                        eta = lignePv1.Etat;
                        moti = lignePv1.Motif;
                        preEch = lignePv1.PremiereEcheance;

                        switch (eta)
                        {
                            case "Acceptée": etatd = "A"; break;
                            case "Refusée": etatd = "R"; break;
                            case "En attente": etatd = "E"; break;
                        }
                        SqlCommand cmdMajDem1;
                        preEch = String.Copy(preEch.Substring(0, 9));
                        string s = preEch.Substring(0, 4);
                        if (s[1] == '/') preEch = "0" + preEch;
                        if (s[3] == '/') preEch = preEch.Substring(0, 3) + "0" + preEch.Substring(3);
                        DateTime dt = DateTime.ParseExact(preEch, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                        conn.Open();
                        if (retperio!=0) cmdMajDem1 = new SqlCommand("update DemandePret Set MontantAcc ='" + montantaccor + "',PremiereEcheance=@pre ,Periodicite = '" + perio + "' ,RetenuPeriodique = '" + retperio + "',Etat ='" + etatd + "',Motif ='" + moti + "',pv_codepv=" + codePV + " where NumDemPret =" + numedempret, conn);
                        else cmdMajDem1 = new SqlCommand("update DemandePret Set MontantAcc ='" + montantaccor + "',PremiereEcheance=@pre ,Periodicite = '" + perio + "',Etat ='" + etatd + "',Motif ='" + moti + "',pv_codepv=" + codePV + " where NumDemPret =" + numedempret, conn);
                        cmdMajDem1.Parameters.Add("@pre", SqlDbType.Date).Value = dt.Date.ToString();
                        cmdMajDem1.ExecuteNonQuery();
                        conn.Close();

                        // remplir PV(pdf)
                        tablepv1.AddCell(new Phrase(lignePv1.NumDemPret.ToString(), font));
                        tablepv1.AddCell(new Phrase(lignePv1.DateDemPret, font));
                        tablepv1.AddCell(new Phrase(lignePv1.MontantVoulu.ToString(), font));
                        tablepv1.AddCell(new Phrase(lignePv1.MontantAcc.ToString(), font));
                        tablepv1.AddCell(new Phrase(preEch, font));
                        tablepv1.AddCell(new Phrase(lignePv1.Periodicite.ToString(), font));
                        tablepv1.AddCell(new Phrase(lignePv1.RetenuPeriodique.ToString(), font));
                        tablepv1.AddCell(new Phrase(lignePv1.Etat, font));
                        tablepv1.AddCell(new Phrase(lignePv1.Motif, font));
                        tablepv1.AddCell(new Phrase(lignePv1.Matricule.ToString(), font));
                    }
                    float[] widths1 = new float[] { 55f, 90f, 60f, 60f, 90f, 61f, 62f, 75f, 60f, 60f };
                    tablepv1.SetWidths(widths1);
                    documentPV.Add(tablepv1);
                    documentPV.Close();

                    ViewPdf fen = new ViewPdf("doc\\PV" + codePV.ToString() + ".pv", "Liste des demandes avec les décisions du PV");
                    Methodes.ShowWindow(this, fen);
                    MetroWindow_Loaded(null, null);
                    buttonAnnulerPv.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                try { buttonExportPdf_Click(sender, e); }
                catch
                { MessageBox.Show("Fichier déja ouvert"); };
            }
        }

        private void dataGridPV_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            dataGridPV.BeginEdit();
        }

        private void buttonAnnulerPv_Click(object sender, RoutedEventArgs e)
        {           
            conn.Open();
            string requette = "SELECT NumDem as [Num Demande],DésignationPrime as [Type Prime],MontsantDem as Montant,NomFonct +' '+PrenFonct AS Fonctionnaire , DemandePrime.Matricule FROM dbo.DemandePrime INNER JOIN dbo.Fonctionnaire  ON Fonctionnaire.Matricule = DemandePrime.Matricule INNER JOIN dbo.TypePrime ON TypePrime.CodePrime = DemandePrime.CodePrime WHERE EtatDem ='N' or EtatDem ='T'";
            SqlCommand cmd2 = new SqlCommand(requette, conn);
            SqlDataReader reader = cmd2.ExecuteReader();
            List<LignePV> tabPV = new List<LignePV>();
            List<String> listEtat = new List<string>();
            listEtat.Add("Acceptée");
            listEtat.Add("Refusée");
            listEtat.Add("En attente");           
            while (reader.Read())
            {
                LignePV lignePV = new LignePV();
                lignePV.NumDem = Convert.ToInt32(reader["Num Demande"]);
                lignePV.TypePrime = (reader["Type Prime"]).ToString();
                lignePV.Montant = float.Parse((reader["Montant"]).ToString());
                lignePV.Fonctionnaire = (reader["Fonctionnaire"]).ToString();
                lignePV.Matricule = Convert.ToInt32(reader["Matricule"]);
                tabPV.Add(lignePV);
            }
            reader.Close();

            dataGridPV.ItemsSource = tabPV;
            etatDem.ItemsSource = listEtat;
            
            SqlCommand cmd3 = new SqlCommand("select CodePret from TypePret WHERE TypePret='1'", conn);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable tpPret = new DataTable();
            da3.Fill(tpPret);


            DataTable ds2 = new DataTable();
            ds2.AcceptChanges();
            string st;
            List<LignePVpret> tabdem = new List<LignePVpret>();
            foreach (DataRow dr in tpPret.Rows)
            {               
                st = dr["CodePret"].ToString();
                cmd3.CommandText = "select NumDemPret,DateDemPret,MontantVoulu,MontantAcc,Periodicite,RetenuPeriodique,Motif,Matricule from DemandePret WHERE CodePret = '" + st + "' AND ((Etat IS NULL) OR (Etat='E'))";
                SqlDataReader rd = cmd3.ExecuteReader();
                while (rd.Read())
                {
                    LignePVpret lignePV = new LignePVpret();
                    lignePV.NumDemPret = Convert.ToInt32(rd["NumDemPret"]);
                    lignePV.DateDemPret = (rd["DateDemPret"]).ToString();
                    lignePV.MontantVoulu = float.Parse((rd["MontantVoulu"]).ToString());
                    //lignePV.MontantAcc = float.Parse((rd["MontantAcc"]).ToString());
                    lignePV.Periodicite = Convert.ToInt32(rd["Periodicite"]);
                    lignePV.RetenuPeriodique = float.Parse((rd["RetenuPeriodique"]).ToString());
                    lignePV.Motif = (rd["Motif"]).ToString();
                    lignePV.Matricule = Convert.ToInt32(rd["Matricule"]);
                    tabdem.Add(lignePV);
                }
                rd.Close();               
            }

            List<string> et = new List<string>();
            et.Add("Acceptée");
            et.Add("Refusée");
            et.Add("En Attente");
            Etat.ItemsSource = et;
            datagrid2.ItemsSource = tabdem;
            conn.Close();
        }
        private void datagrid2_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            datagrid2.BeginEdit();
        }

        private void browsePv_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".pv";
            dlg.Filter = "Fichiers PV|*.pv";
            dlg.InitialDirectory = Directory.GetCurrentDirectory() + "\\doc";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                String[] tab = filename.Split('\\');
                ViewPdf fen = new ViewPdf(filename, tab[tab.Length - 1]);
                Methodes.ShowWindow(this, fen);
            }
        }

        #endregion

        #region Statistiques

        private void botton_aff_Click(object sender, RoutedEventArgs e)
        {
            StatDemande fen = new StatDemande(conn, comboPrime.SelectedItem.ToString(), comboEtatDem.SelectedItem.ToString(), DaDeb, DaFin);
            fen.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            fen.Show();
        }

        private void comboEtatDem_Initialized(object sender, EventArgs e)
        {
            comboEtatDem.Items.Add("Acceptée");
            comboEtatDem.Items.Add("Refusée");
            comboEtatDem.Items.Add("En atente");
            comboEtatDem.Items.Add("En instance");
            comboEtatDem.Items.Add("Touts");
        }

        #endregion

        private void montantBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void dateDebutVirStat_Loaded(object sender, RoutedEventArgs e)
        {
            dateDebutVirStat.ItemsSource = Enumerable.Range(2015, DateTime.Now.Year - 2015 + 1).ToList();
        }

        private void dateDebutVirStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateFinVirStat.ItemsSource = Enumerable.Range(Convert.ToInt32(dateDebutVirStat.SelectedItem), (DateTime.Now.Year - 2015 + 1) - (Convert.ToInt32(dateDebutVirStat.SelectedItem) - 2015)).ToList();
            dateFinVirStat.IsEnabled = true; dateFinVirStat.SelectedIndex = 0;
            applyStatVir.IsEnabled = true;
        }

        private void applyStatVir_Click(object sender, RoutedEventArgs e)
        {
            chartVir.Series.Clear();
            //chartVir.Header = "Montants totaux des virements par année";
            CategoryAxis primaryAxis = new CategoryAxis();
            primaryAxis.Header = "Années";
            chartVir.PrimaryAxis = primaryAxis;
            ColumnSeries series1 = new ColumnSeries();
            series1.ItemsSource = Methodes.montantIntervalAnnees(Convert.ToInt32(dateDebutVirStat.SelectedItem), Convert.ToInt32(dateFinVirStat.SelectedItem),conn);
            series1.XBindingPath = "Année";
            series1.YBindingPath = "Montant";
            chartVir.Series.Add(series1);
        }

        private void newDemTab_Initialized(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region GestionPret
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Gestion_Prets.IsSelected = true;
        }
        public static string converti(int chiffre)
        {
            int centaine, dizaine, unite, reste, y;
            bool dix = false;
            string lettre = "";
            //strcpy(lettre, "");

            reste = chiffre;

            for (int i = 1000000000; i >= 1; i /= 1000)
            {
                Console.WriteLine(i);
                y = reste / i;
                if (y != 0)
                {
                    centaine = y / 100;
                    dizaine = (y - centaine * 100) / 10;
                    unite = y - (centaine * 100) - (dizaine * 10);
                    switch (centaine)
                    {
                        case 0:
                            break;
                        case 1:
                            lettre += "cent ";
                            break;
                        case 2:
                            if ((dizaine == 0) && (unite == 0)) lettre += "deux cents ";
                            else lettre += "deux cent ";
                            break;
                        case 3:
                            if ((dizaine == 0) && (unite == 0)) lettre += "trois cents ";
                            else lettre += "trois cent ";
                            break;
                        case 4:
                            if ((dizaine == 0) && (unite == 0)) lettre += "quatre cents ";
                            else lettre += "quatre cent ";
                            break;
                        case 5:
                            if ((dizaine == 0) && (unite == 0)) lettre += "cinq cents ";
                            else lettre += "cinq cent ";
                            break;
                        case 6:
                            if ((dizaine == 0) && (unite == 0)) lettre += "six cents ";
                            else lettre += "six cent ";
                            break;
                        case 7:
                            if ((dizaine == 0) && (unite == 0)) lettre += "sept cents ";
                            else lettre += "sept cent ";
                            break;
                        case 8:
                            if ((dizaine == 0) && (unite == 0)) lettre += "huit cents ";
                            else lettre += "huit cent ";
                            break;
                        case 9:
                            if ((dizaine == 0) && (unite == 0)) lettre += "neuf cents ";
                            else lettre += "neuf cent ";
                            break;
                    }// endSwitch(centaine)

                    switch (dizaine)
                    {
                        case 0:
                            break;
                        case 1:
                            dix = true;
                            break;
                        case 2:
                            lettre += "vingt ";
                            break;
                        case 3:
                            lettre += "trente ";
                            break;
                        case 4:
                            lettre += "quarante ";
                            break;
                        case 5:
                            lettre += "cinquante ";
                            break;
                        case 6:
                            lettre += "soixante ";
                            break;
                        case 7:
                            dix = true;
                            lettre += "soixante ";
                            break;
                        case 8:
                            lettre += "quatre-vingt ";
                            break;
                        case 9:
                            dix = true;
                            lettre += "quatre-vingt ";
                            break;
                    } // endSwitch(dizaine)

                    switch (unite)
                    {
                        case 0:
                            if (dix) lettre += "dix ";
                            break;
                        case 1:
                            if (dix) lettre += "onze ";
                            else lettre += "un ";
                            break;
                        case 2:
                            if (dix) lettre += "douze ";
                            else lettre += "deux ";
                            break;
                        case 3:
                            if (dix) lettre += "treize ";
                            else lettre += "trois ";
                            break;
                        case 4:
                            if (dix) lettre += "quatorze ";
                            else lettre += "quatre ";
                            break;
                        case 5:
                            if (dix) lettre += "quinze ";
                            else lettre += "cinq ";
                            break;
                        case 6:
                            if (dix) lettre += "seize ";
                            else lettre += "six ";
                            break;
                        case 7:
                            if (dix) lettre += "dix-sept ";
                            else lettre += "sept ";
                            break;
                        case 8:
                            if (dix) lettre += "dix-huit ";
                            else lettre += "huit ";
                            break;
                        case 9:
                            if (dix) lettre += "dix-neuf ";
                            else lettre += "neuf ";
                            break;
                    } // endSwitch(unite)

                    switch (i)
                    {
                        case 1000000000:
                            if (y > 1) lettre += "milliards ";
                            else lettre += "milliard ";
                            break;
                        case 1000000:
                            if (y > 1) lettre += "millions ";
                            else lettre += "million ";
                            break;
                        case 1000:
                            if (y > 1) lettre += "milles ";
                            else lettre = "mille ";
                            break;
                    }
                } // end if(y!=0)
                reste -= y * i;
                dix = false;
            } // end for
            if (lettre.Length == 0) lettre += "zero";

            return lettre;
        }

        #region DemandePret
        private void demBtn1_Click(object sender, RoutedEventArgs e)
        {
            Dem.IsSelected = true;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            demp.IsSelected = true;
        }

        #region acceuilDemande

        private Boolean ty1 = false;
        private Boolean ty2 = false;
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ty2 = false;
            ty1 = true;
        }
        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            ty1 = false;
            ty2 = true;
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (combotype.SelectedIndex != -1)
            {
                if (ty1 == true)
                {
                    t1.IsSelected = true;
                }

                if (ty2 == true)
                {
                    t2.IsSelected = true;
                }
            }
            else
            {
                MessageBox.Show("  Veuillez choisir un type  ");
            }
            combotype.SelectedIndex = -1;
        }

        public Boolean decision2()
        {
            NvDemT1 nv1 = new NvDemT1(conn);
            Boolean bol = false;
            DemT1Rep rapport = new DemT1Rep();
            AutorisationPrécompte PrécompteAutorisation = new AutorisationPrécompte();
            if (ComboSousType.SelectedIndex != -1)
            {
                if (DatePickDem.Text != "")
                {
                    if (MontVoulu.Text != "")
                    {
                        if (Periodicité.Text != "")
                        {
                            int i = int.Parse(MontVoulu.Text);
                            rapport.SetParameterValue("Fonctionnaire", nomFonct1.Text);
                            rapport.SetParameterValue("MontantLettres", nv1.converti(i) + " dinars");
                            rapport.SetParameterValue("Perio", Periodicité.Text);
                            rapport.SetParameterValue("MontantPret", i);
                            rapport.SetParameterValue("MotifDemT1", ComboSousType.Text);
                            rapport.SetParameterValue("tel", nv1.fonctInfo(nom, prenom).Rows[0][1].ToString());
                            rapport.SetParameterValue("DateDemT1", DatePickDem.Text);
                            String dateDem = (DatePickDem.Text.Substring(6, 4) + "/" + DatePickDem.Text.Substring(3, 3) + DatePickDem.Text.Substring(0, 2)).ToString();
                            rapport.SetParameterValue("numdem", nv1.numDem(dateDem));


                            PrécompteAutorisation.SetParameterValue("Fonct", nomFonct1.Text);
                            PrécompteAutorisation.SetParameterValue("NbEch", Periodicité.Text);
                            //   PrécompteAutorisation.SetParameterValue("MontantPret", i);
                            PrécompteAutorisation.SetParameterValue("Grade", nv1.fonctInfo(nom, prenom).Rows[0][4].ToString());
                            PrécompteAutorisation.SetParameterValue("Fonction", nv1.fonctInfo(nom, prenom).Rows[0][5].ToString());
                            PrécompteAutorisation.SetParameterValue("AdresseFonct", nv1.fonctInfo(nom, prenom).Rows[0][6].ToString());

                            bol = true;
                            Decision f = new Decision();
                            Decision fp = new Decision();
                            f.afficher(rapport);
                            f.Show();
                            fp.afficher(PrécompteAutorisation);
                            fp.Show();
                        }

                        else MessageBox.Show("Veuillez donner la période du remboursement ");
                    }
                    else MessageBox.Show("Veuillez donner le montant demandé ");
                }
                else MessageBox.Show("Veuillez donner la date de la demande ");
            }
            else MessageBox.Show("Veuillez choisir le type du prêt");
            return bol;
        }
        #endregion

        #region Type1

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NvDemT1 nv1 = new NvDemT1(conn);
            DataGridFonct.UnselectAllCells();
            DataGridFonct.ItemsSource = nv1.fonctionaire(nomFonct1.Text);
        }
        private void DataGridFonct_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridFonct.SelectedIndex >= 0)
            {
                LigneNomPren ligne = new LigneNomPren();
                ligne = (LigneNomPren)DataGridFonct.SelectedItems[0];
                nom = ligne.nom;
                prenom = ligne.prenom;
                nomFonct1.Text = nom + " " + prenom;
            }
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Pr.IsSelected = true;
        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            NvDemT1 nv1 = new NvDemT1(conn); 
            MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder la demande ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultat == MessageBoxResult.Yes)
            {
                if (fonctExiste(nom, prenom))
                {
                    if (decision2())
                    {
                        nv1.creerDemande(nom, prenom, MontVoulu.Text, Periodicité.Text, ComboSousType.SelectedItem.ToString(), DatePickDem,rtPer.Text);
                    }
                    nom = string.Empty;
                    prenom = string.Empty;
                    anu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    buttonAnnulerPv.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else MessageBox.Show("Ce fonctionnaire n'existe pas");
            }
            foreach (var item in griddem1.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).Clear();

                if (item is DatePicker)
                    ((DatePicker)item).Text = "";

                if (item is ComboBox)
                    ((ComboBox)item).SelectedIndex = -1;
            }
        }

        #endregion

        #region Type2
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Pr.IsSelected = true;
        }
        private void nomFonct3_TextChanged(object sender, TextChangedEventArgs e)
        {
            NvDemT2 nv2 = new NvDemT2(conn);
            datafonct.UnselectAllCells();
            datafonct.ItemsSource = nv2.fonctionaire(nomFonct3.Text);
        }
        private void datafonct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datafonct.SelectedIndex >= 0)
            {
                LigneNomPren ligne = new LigneNomPren();
                ligne = (LigneNomPren)datafonct.SelectedItems[0];
                nom = ligne.nom;
                prenom = ligne.prenom;
                nomFonct3.Text = nom + " " + prenom;

            }
        }
        private void ComboDesignation1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboFournisseur1.Items.Clear();
            DataTable dat;
            String Designation = ComboDesignation1.SelectedItem.ToString();
            String cmd = "select NomFournisseur from Fournisseur where TypeFournisseur='" + Designation + "'";
            SqlCommand cmduser = new SqlCommand(cmd, conn);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmduser);
            dat = new DataTable();
            ad.Fill(dat);
            foreach (DataRow dr in dat.Rows)
            {
                ComboFournisseur1.Items.Add(dr["NomFournisseur"].ToString());

            }
            conn.Close();
        }
        private void ComboFournisseur1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NvDemT2 nv2 = new NvDemT2(conn);
            DataTable dat = new DataTable();
            if (ComboFournisseur1.SelectedIndex != -1)
            {
                String fournis = ComboFournisseur1.SelectedItem.ToString();
                bool dis = true;
                int refer = int.Parse(nv2.infoFournisseur(fournis).Rows[0]["RefFournisseur"].ToString());
                String cmd2 = "select Designation from Produit where Disponible= @dis and RefFournisseur= @refer";
                SqlCommand cmduser = new SqlCommand(cmd2, conn);
                cmduser.Parameters.AddWithValue("@dis", dis);
                cmduser.Parameters.AddWithValue("@refer", refer);
                conn.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmduser);
                ad.Fill(dat);
            }
            else
            {
                bool dis = true;
                String cmd2 = "select Designation from Produit where Disponible= @dis";
                SqlCommand cmduser = new SqlCommand(cmd2, conn);
                cmduser.Parameters.AddWithValue("@dis", dis);
                conn.Open();
                SqlDataAdapter ad2 = new SqlDataAdapter(cmduser);
                ad2.Fill(dat);
            }
            dat.Columns.Add("Quantité", typeof(int));
            dat.Columns.Add("Selection", typeof(Boolean));
            foreach (DataRow row in dat.Rows)
            { row["Selection"] = false; }

            datagradProduit.ItemsSource = dat.DefaultView;
            conn.Close();
        }

        public static string r;
        public static DataTable TableProduit;
        public static int total;
        public static DataTable dt5;


        private void datagradProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NvDemT2 nv2 = new NvDemT2(conn);
            if (nomFonct3.Text != "")
            { 
                if (fonctExiste(nom,prenom))
                { 
                if (date.Text != "")
                {
                    if (ComboDesignation1.SelectedIndex != -1)
                    {
                        String Des = ComboDesignation1.SelectedItem.ToString();
                        int codep =nv2.getCodePret(Des);
                        if (ComboFournisseur1.SelectedIndex != -1)
                        {
                            String fr = ComboFournisseur1.SelectedItem.ToString();
                            float ap = float.Parse(nv2.infoFournisseur(fr).Rows[0]["AppFournisseur"].ToString());
                            int per = int.Parse(nv2.infoFournisseur(fr).Rows[0]["PeriodeFournisseur"].ToString());
                            if (datagradProduit.SelectedIndex != -1)
                            {
                                try
                                {
                                    MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder la demande ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (resultat == MessageBoxResult.Yes)
                                    {
                                            DataTable dt5 = new DataTable();
                                            dt5 = ((DataView)datagradProduit.ItemsSource).ToTable();

                                            Boolean Var = false;
                                            String nomprod;
                                            float prix, s = 0;
                                            float cumul = 0;
                                            int Qt = 0;
                                            r = nomFonct3.Text;

                                            TableProduit = new DataTable("TableGlobal");
                                            TableProduit.Columns.Add("Désignation", typeof(String));
                                            TableProduit.Columns.Add("Ref", typeof(String));
                                            TableProduit.Columns.Add("Qté", typeof(Int32));
                                            TableProduit.Columns.Add("Prix unitaire TTC", typeof(Int32));
                                            TableProduit.Columns.Add("Montant total TTC ", typeof(Int32));
                                            TableProduit.Columns.Add("Echeance mensuelle sur 10 mois ", typeof(Int32));

                                            String reff = String.Empty;
                                            int i1 = 0;
                                            for (i1 = 0; i1 < dt5.Rows.Count; i1++)
                                            {
                                                Var = Boolean.Parse(dt5.Rows[i1]["Selection"].ToString());

                                                if (Var == true)
                                                {
                                                    nomprod = dt5.Rows[i1]["Designation"].ToString();
                                                    Qt = Convert.ToInt32(dt5.Rows[i1]["Quantité"]);
                                                    prix = float.Parse(nv2.produitInfo(nomprod).Rows[0]["PrixUnitTTC"].ToString());
                                                    cumul = (prix * Qt);
                                                    reff = nv2.produitInfo(nomprod).Rows[0]["RefProduit"].ToString();
                                                    s = (cumul / 10);
                                                    TableProduit.Rows.Add(nomprod, reff, Qt, prix, cumul, s);
                                                    cumul = 0;
                                                }
                                            }

                                            for (int i2 = 0; i2 < TableProduit.Rows.Count; i2++)
                                            {
                                                total = Convert.ToInt32(TableProduit.Rows[i2]["Montant total TTC "]) + total;
                                            }

                                            nv2.creerDemandeProduit(nom, prenom, dt5, date, per, ap, codep);
                                            RepT2Details f = new RepT2Details(conn);
                                            f.Show();
                                            nom = string.Empty;
                                            prenom = string.Empty;
                                    }
                                }
                                catch (Exception) { MessageBox.Show("Veuillez saisir la quantité voulue pour chaque produit"); }

                            }
                            else MessageBox.Show("Veuillez selectionner un produit ");
                        }
                        else MessageBox.Show("Veuillez selectionner un fournisseur ");
                    }
                    else MessageBox.Show("Veuillez selectionner une désignation ");
                }
                else MessageBox.Show("Veuillez entrer une date ");
               }
               else MessageBox.Show("Ce fonctionnaire n'existe pas");
            }
            else MessageBox.Show("Veuillez selectionner un demandeur ");

            fillDataProduit();
            foreach (var item in NvDem2.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).Clear();
               if (item is DatePicker)
                    ((DatePicker)item).Text = "";
                if (item is DataGrid)
                    ((DataGrid)item).SelectedIndex = -1;
            }
        }

        #endregion

        #region RembAnticip

        public String numDemRem;
        private void nomFonct2_TextChanged(object sender, TextChangedEventArgs e)
        {
            RembAnticip rm = new RembAnticip(conn);
            DataGridFonct1.UnselectAllCells();
            DataGridFonct1.ItemsSource = rm.fonctionaire(nomFonct2.Text);
        }
        private void DataGridFonct1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridFonct1.SelectedIndex >= 0)
            {
                RembAnticip rb = new RembAnticip(conn);
                LigneNomPren ligne = new LigneNomPren();
                ligne = (LigneNomPren)DataGridFonct1.SelectedItems[0];
                nom = ligne.nom;
                prenom = ligne.prenom;
                nomFonct2.Text = nom + " " + prenom;
                DataGridFonct1d.ItemsSource = rb.fonctDemandes(nom, prenom);
            }
        }

        private void DataGridFonct1d_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridFonct1d.SelectedIndex >= 0)
            {
                LigneDemandePret ligne = new LigneDemandePret();
                ligne = (LigneDemandePret)DataGridFonct1d.SelectedItems[0];
                numDemRem = ligne.NumDem;
            }
        }

        public Boolean decisionRemAnt()
        {
            Boolean bol = false;
            RembAnticip rm = new RembAnticip(conn);
            RembEntissRep rapport = new RembEntissRep();
            if (ComboBoxSur.SelectedIndex != -1)
            {
                if (DatePickerRem.Text != "")
                {
                    if (MontantRem.Text != "")
                    {
                        if (PeriodiciteAnt.Text != "")
                        {
                            rapport.SetParameterValue("Fonctionnaire", nomFonct2.Text);
                            rapport.SetParameterValue("typeremb", ComboBoxSur.Text + " dinars");
                            rapport.SetParameterValue("Perio", PeriodiciteAnt.Text);
                            rapport.SetParameterValue("montantR", MontantRem.Text);
                            rapport.SetParameterValue("tel", rm.fonctInfo(nom, prenom).Rows[0][1].ToString());



                            rapport.SetParameterValue("numdem", numDemRem);
                            bol = true;
                            Decision f = new Decision();
                            f.afficher(rapport);
                            f.Show();
                        }
                        else MessageBox.Show("Veuillez donner la Periode du remboursement anticipé");
                    }
                    else MessageBox.Show("Veuillez donner le nouveau Montant de remboursement ");
                }
                else MessageBox.Show("Veuillez donner la date du remboursement anticipé ");
            }
            else MessageBox.Show("Veuillez choisir sur quoi le remboursement sera effectué");


            return bol;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            RembAnticip rm = new RembAnticip(conn);
            if (fonctExiste(nom, prenom))
            {
                if (MontantRem.Text != "")
                {
                    if (DatePickerPremEch.Text != "")
                    {
                        if (ComboBoxSur.SelectedIndex != -1)
                        {
                            if (DatePickerRem.Text != "")
                            {
                                if (MontantRem.Text != "")
                                {
                                    MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder l'offre ?", "Confirmation offre ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                    if (resultat == MessageBoxResult.Yes)
                                    {
                                        if (fonctExiste(nom, prenom))
                                        {
                                            if (decisionRemAnt())
                                            {
                                                rm.creerDemande(nom, prenom, numDemRem, MontantRem.Text, PeriodiciteAnt.Text, ((ComboBoxItem)ComboBoxSur.SelectedItem).Content.ToString(), DatePickerRem, DatePickerPremEch);
                                            }
                                            nom = string.Empty;
                                            prenom = string.Empty;

                                        }
                                        else MessageBox.Show("Ce fonctionnaire n'existe pas");
                                    }
                                }
                                else MessageBox.Show("Veuillez donner le nouveau montant de remboursement ");
                            }
                            else MessageBox.Show("Veuillez donner la date du remboursement anticipé ");
                        }
                        else MessageBox.Show("Veuillez choisir sur quoi le remboursement sera effectué");
                    }
                    else MessageBox.Show("Veuillez choisir la date de la première échéance");
                }
                else MessageBox.Show("Veuillez donner la nouvelle périodicité ");
            }
            else MessageBox.Show("Ce fonctionnaire n'existe pas");
            foreach (var item in RemAnt.Children)
            {
                if (item is TextBox)
                    ((TextBox)item).Clear();

                if (item is DatePicker)
                    ((DatePicker)item).Text = "";

                if (item is ComboBox)
                    ((ComboBox)item).SelectedIndex = -1;
            }
        }

        #endregion

        #endregion

        #region AutoPrecompte
        private void AutoBtn_Click(object sender, RoutedEventArgs e)
        {
            Auto.IsSelected = true;
        }


        public DataTable tba;
        public Précompte pr;
        public static DataTable TablePrecomptes;
        private void btnValide_Click(object sender, RoutedEventArgs e)
        {
            string motif;
            DataTable dtSource = new DataTable();
            try
            {
                dtSource = ((DataView)dgPrecmpt.ItemsSource).ToTable();
                bool existe = false;
                foreach (DataRow row in dtSource.Rows)
                {
                    if (Convert.ToBoolean(row[5]) == true)
                    {
                        existe = true;
                    }
                }
                if (existe)
                {
                    foreach (DataRow rw in dtSource.Rows)
                    {   
                        if (Convert.ToBoolean(rw[5]) == true)
                        {
                            motif = rw[6].ToString();
                            pr = new Précompte(Convert.ToInt32(rw[0]), conn);
                            pr.upDatePrecompte(pr.montRet(),motif);
                            pr.setEtat();
                        }
                    }
                    MessageBox.Show("Validé !");
                }
                else MessageBox.Show("Veuillez valider au moins une demande");
            }
            catch (Exception) { MessageBox.Show("Aucune demande selectionnée !"); };
        }

        private void btImp_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtSource1 = new DataTable();
            dtSource1 = ((DataView)dgPrecmpt.ItemsSource).ToTable();

            Boolean Var = false;

            TablePrecomptes = new DataTable("TablePrecomptes");
            TablePrecomptes.Columns.Add("Nom fonctionnaire", typeof(String));
            TablePrecomptes.Columns.Add("Prénom fonctionnaire", typeof(String));
            TablePrecomptes.Columns.Add("Matricule", typeof(Int32));
            TablePrecomptes.Columns.Add("Numero de la demande", typeof(Int32));
            TablePrecomptes.Columns.Add("Montant à retrancher", typeof(Int32));

            String name, firstname = String.Empty;
            int montant, mat, i1, num = 0;
            for (i1 = 0; i1 < dtSource1.Rows.Count; i1++)
            {
                Var = Boolean.Parse(dtSource1.Rows[i1]["Validé"].ToString());

                if (Var == true)
                {
                    name = dtSource1.Rows[i1]["NomFonct"].ToString();
                    firstname = dtSource1.Rows[i1]["PrenFonct"].ToString();
                    montant = Convert.ToInt32(dtSource1.Rows[i1]["Montant a retrancher"]);
                    mat = Convert.ToInt32(dtSource1.Rows[i1]["Matricule"]);
                    num = Convert.ToInt32(dtSource1.Rows[i1]["NumDemPret"]);

                    TablePrecomptes.Rows.Add(name, firstname, mat, num, montant);

                }
            }

            Precompte fen = new Precompte();
            fen.Show();
        }

        private void afficher_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            ///Creation de la table a afficher 
            tba = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT NumDemPret,RetenuPeriodique,Matricule FROM DemandePret WHERE Etat='A' ", conn);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(tba);
            conn.Close();
            Précompte pre;
            string nom = "Montant a retrancher";
            tba.Columns[1].ColumnName = nom;
            tba.Columns.Add("NomFonct", typeof(string));
            tba.Columns.Add("PrenFonct", typeof(string));
            tba.Columns.Add("Validé", typeof(bool));
            tba.Columns.Add("MotifPrecpte", typeof(string));

            conn.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM Fonctionnaire", conn);
            SqlDataAdapter ad1 = new SqlDataAdapter(cmd1);
            DataTable fonc = new DataTable();
            ad1.Fill(fonc);
            conn.Close();

            foreach (DataRow rw in tba.Rows)
            {
                pre = new Précompte(int.Parse(rw["NumDemPret"].ToString()), conn);
                DataRow infoFonc = fonc.Select("Matricule=" + Convert.ToInt32(rw["Matricule"])).FirstOrDefault();
                if (infoFonc != null)
                {
                    rw["NomFonct"] = infoFonc["NomFonct"];
                    rw["PrenFonct"] = infoFonc["PrenFonct"];
                }
                rw["Validé"] = true;
                if (pre.verifDate())
                {
                    if (pre.consPrec() == false) rw.Delete();
                    else rw["Montant a retrancher"] = pre.montRet();
                }
                else rw.Delete();
            }


            dgPrecmpt.ItemsSource = tba.DefaultView;
            btImp.IsEnabled = true;
        }

        #endregion

        #region FournisseurEtTraitement2
        private void FournBtn_Click(object sender, RoutedEventArgs e)
        {
            Fourni.IsSelected = true;
        }

        #region Fournisseur
        private void ComboDesignation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboFournisseur.Items.Clear();
            DataTable dat;
            String Designation = ComboDesignation.SelectedItem.ToString();
            String cmd = "select NomFournisseur from Fournisseur where TypeFournisseur='" + Designation + "'";
            SqlCommand cmduser = new SqlCommand(cmd, conn);
            conn.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmduser);
            dat = new DataTable();
            ad.Fill(dat);
            foreach (DataRow dr in dat.Rows)
            {
                ComboFournisseur.Items.Add(dr["NomFournisseur"].ToString());

            }
            conn.Close();
        }
        private void ComboFournisseur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fournisseur fr = new Fournisseur(conn);
            DataTable dt = new DataTable();
            if (ComboFournisseur.SelectedIndex != -1)
            {
                String fournis = ComboFournisseur.SelectedItem.ToString();
                //ComboFournisseur.SelectedItem.ToString();
                bool dis = true;
                int refer = int.Parse(fr.infoFournisseur(fournis).Rows[0]["RefFournisseur"].ToString());
                String cmd2 = "select RefProduit,Designation,Disponible,PrixUnitHT,PrixUnitTTC from Produit where RefFournisseur= @refer";
                SqlCommand cmduser = new SqlCommand(cmd2, conn);
                cmduser.Parameters.AddWithValue("@refer", refer);
                conn.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmduser);
                ad.Fill(dt);
            }
            else
            {
                String cmd2 = "select RefProduit,Designation,Disponible,PrixUnitHT,PrixUnitTTC from Produit ";
                SqlCommand cmduser = new SqlCommand(cmd2, conn);
                conn.Open();
                SqlDataAdapter ad2 = new SqlDataAdapter(cmduser);
                ad2.Fill(dt);
            }
            DataProduit.ItemsSource = dt.DefaultView;
            conn.Close();
        }
        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            AjoutFourni fen = new AjoutFourni(conn);
            Methodes.ShowWindow(this, fen);
        }
        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            ModifFourni fen = new ModifFourni(conn);
            Methodes.ShowWindow(this, fen);
        }
        private void valider_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultat = MessageBox.Show("Voulez vous sauvegarder ces modifications ?", "Confirmation demande ", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultat == MessageBoxResult.Yes)
            {
                String reference; bool dis;
                DataTable dt = new DataTable();
                dt = ((DataView)DataProduit.ItemsSource).ToTable();
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow Row in dt.Rows)
                    {
                        reference = Row["RefProduit"].ToString();
                        dis = bool.Parse(Row["Disponible"].ToString());
                        String cmd = "UPDATE Produit SET Disponible= '" + dis + "' WHERE RefProduit='" + reference + "'";
                        conn.Open();
                        SqlCommand cmdUser = new SqlCommand(cmd, conn);
                        SqlDataReader reader = cmdUser.ExecuteReader();
                        conn.Close();
                    }

                }
                MessageBox.Show("Modifications effectuées");
            }
        }
       
        #endregion

        #region Traitement2

        #region AcceuilBonComGlo
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            eff.IsSelected = true;
        }
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            bon.IsSelected = true;
        }
        #endregion

        #region Effacementdette
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            bcg.IsSelected = true;
        }
        private void boutsup_Click(object sender, RoutedEventArgs e)
        {
            
            DataTable dtSource = new DataTable();
            try
            {
                conn.Open();
                dtSource = ((DataView)dataEf.ItemsSource).ToTable();
                bool existe = new bool();
                foreach (DataRow rw in dtSource.Rows)
                {
                    if (Convert.ToBoolean(rw[4]) == true)
                    {
                        existe = true;
                        SqlCommand cmd = new SqlCommand("UPDATE DemandePret SET Etat ='S' WHERE NumDemPret = @ndp", conn);
                        cmd.Parameters.Add("@ndp", SqlDbType.Char).Value = rw["NumDemPret"];
                        cmd.ExecuteNonQuery();
                    }
                }
                if (existe) MessageBox.Show("Demande(s) effacée(s)");
            }
            catch (Exception)
            {
                MessageBox.Show("Aucune demande selectionnée");
            }
            finally {conn.Close();};
        }

        #endregion

        #region bonDeCommande

        public static string NomFournisseur;
        public int refFOURNI;
        private void combobox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT RefFournisseur FROM Fournisseur WHERE NomFournisseur ='" + combobox2.SelectedItem + "'";
            NomFournisseur = combobox2.SelectedItem.ToString();
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            refFOURNI = Convert.ToInt32(dt.Rows[0][0]);

            string ma_variable = refFOURNI.ToString();
            DataTable dt1 = new DataTable();

            cmd.CommandText = "SELECT RefProduit FROM Produit WHERE RefFournisseur ='" + ma_variable + "'";
            cmd.ExecuteNonQuery();
            sda = new SqlDataAdapter(cmd);
            sda.Fill(dt1);

            ma_variable = string.Empty;
            dt = new DataTable();
            foreach (DataRow row in dt1.Rows)
            {
                ma_variable = row["RefProduit"].ToString();
                cmd.CommandText = "SELECT NumDemPret FROM Contient WHERE RefProduit ='" + ma_variable + "'";
                cmd.ExecuteNonQuery();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            };

            //elimination des doublons dans dt
            Hashtable ht = new Hashtable();
            ArrayList duplicatelist = new ArrayList();
            foreach (DataRow rw in dt.Rows)
            {
                if (ht.Contains(rw["NumDemPret"])) duplicatelist.Add(rw);
                else ht.Add(rw["NumDemPret"], string.Empty);
            }
            foreach (DataRow rw in duplicatelist) dt.Rows.Remove(rw);

            ma_variable = string.Empty;
            dt1 = new DataTable();
            foreach (DataRow row in dt.Rows)
            {
                ma_variable = row["NumDemPret"].ToString();
                cmd.CommandText = "SELECT Matricule,NumDemPret FROM DemandePret WHERE NumDemPret ='" + ma_variable + "' AND bondecdeglobal_numbon is NULL ";
                cmd.ExecuteNonQuery();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt1);
            };

            ma_variable = string.Empty;
            dt = new DataTable();
            foreach (DataRow row in dt1.Rows)
            {
                ma_variable = row["Matricule"].ToString();
                cmd.CommandText = "SELECT NomFonct,PrenFonct,Matricule FROM Fonctionnaire WHERE Matricule ='" + ma_variable + "'";
                cmd.ExecuteNonQuery();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            };
            DataColumn dc = new DataColumn("NumDemPret", typeof(Int32));
            dt.Columns.Add(dc);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][3] = dt1.Rows[i][1];
            }


            DataColumn cn = new DataColumn("Ajouter au bon", typeof(Boolean));
            dt.Columns.Add(cn);
            foreach (DataRow rw in dt.Rows) rw["Ajouter au bon"] = false;
            datag.DataContext = dt;
            conn.Close();
        }
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            bcg.IsSelected = true;
        }

        public static DataTable tbGlobal;
        private void valid_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtTest = new DataTable();
            bool existe = false;
            try
            {
                
                dtTest = ((DataView)datag.ItemsSource).ToTable();
                foreach (DataRow rw in dtTest.Rows)
                {
                    if (Convert.ToBoolean(rw[4]) == true) existe = true;
                }
            }
            catch (Exception) { MessageBox.Show("Aucune demande sélectionnée !"); }


             if (existe)
             {

                 try
                 {
                     conn.Open();
                     SqlCommand cmd = new SqlCommand("INSERT INTO BonDeCdeGlobal VALUES (@date)", conn);
                     cmd.Parameters.Add("@date", SqlDbType.Date).Value = DateTime.Today;
                     cmd.ExecuteNonQuery();


                     DataTable dtSource = new DataTable();
                     dtSource = ((DataView)datag.ItemsSource).ToTable();

                     SqlCommand cmd3 = new SqlCommand("SELECT MAX(NumBon) FROM BonDeCdeGlobal", conn);
                     int numBonG = Convert.ToInt32(cmd3.ExecuteScalar());

                     ///calcul premiere écheance 
                     DateTime dt1;
                     /* if (DateTime.Now.Day >= 25)
                      {
                          //dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month+2, DateTime.Now.Day);

                          dt1 = new DateTime(01, DateTime.Now.Month + 2, DateTime.Now.Day);
                      }
                      else
                      {
                          // dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day);
                          //int m = dt.Month;
                          //int y = dt.Year;
                          dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
                      }*/
                     dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day);
                     string date = dt1.Date.ToString("yyyy-MM-dd");

                     DateTime datePreApport = DateTime.Now.Date;
                     SqlCommand cmdApport = new SqlCommand("SELECT AppFournisseur FROM Fournisseur WHERE NomFournisseur=@nomf", conn);
                     cmdApport.Parameters.Add("@nomf", SqlDbType.Char).Value = combobox2.Text;
                     float appF = Convert.ToInt32(cmdApport.ExecuteScalar());


                     foreach (DataRow rw in dtSource.Rows)
                     {
                         if (Convert.ToBoolean(rw[4]) == true)
                         {
                             SqlCommand cmd1 = new SqlCommand("UPDATE DemandePret SET bondecdeglobal_numbon='" + numBonG + "' , Etat ='A',PremiereEcheance=@date WHERE NumDemPret = @ndp", conn);
                             cmd1.Parameters.Add("@ndp", SqlDbType.Char).Value = rw["NumDemPret"];
                             cmd1.Parameters.Add("@date", SqlDbType.Date).Value = dt1.Date;
                             cmd1.ExecuteNonQuery();
                             SqlCommand cmd2 = new SqlCommand("INSERT INTO Précompte (DatePrecompte,MontantPrecompte,NumDemPret) VALUES (@date,@appf,@nd) ", conn);
                             cmd2.Parameters.Add("@date", SqlDbType.Date).Value = datePreApport;

                             SqlCommand monAcc = new SqlCommand("SELECT MontantAcc FROM DemandePret WHERE NumDemPret = @numd", conn);
                             monAcc.Parameters.Add("@numd", SqlDbType.Char).Value = rw["NumDemPret"];
                             int monacc = Convert.ToInt32(monAcc.ExecuteScalar());

                             cmd2.Parameters.Add("@appf", SqlDbType.Float).Value = (appF * monacc) / 100;
                             cmd2.Parameters.Add("@nd", SqlDbType.Int).Value = Convert.ToInt32(rw["NumDemPret"]);
                             cmd2.ExecuteNonQuery();
                         }
                     }


                     tbGlobal = new DataTable("TableGlobal");
                     tbGlobal.Columns.Add("Référence", typeof(String));
                     tbGlobal.Columns.Add("Titre", typeof(String));
                     tbGlobal.Columns.Add("Quant", typeof(Int32));
                     tbGlobal.Columns.Add("Prix unit", typeof(Int32));
                     tbGlobal.Columns.Add("Montant", typeof(Int32));

                     DataTable tbProduit = new DataTable();
                     SqlCommand cmdf = new SqlCommand("SELECT * FROM Produit", conn);
                     SqlDataAdapter daa = new SqlDataAdapter(cmdf);
                     daa.Fill(tbProduit);

                     foreach (DataRow rw in dtSource.Rows)
                     {
                         if (Convert.ToBoolean(rw[4]) == true)
                         {
                             DataTable tbRefs = new DataTable();
                             SqlCommand cmd1 = new SqlCommand("SELECT * FROM Contient WHERE NumDemPret=@num", conn);
                             cmd1.Parameters.Add("@num", SqlDbType.Int).Value = rw["NumDemPret"];
                             SqlDataAdapter adapter = new SqlDataAdapter(cmd1);
                             adapter.Fill(tbRefs);

                             foreach (DataRow rw1 in tbRefs.Rows)
                             {
                                 DataRow existeDeja = tbGlobal.Select("Référence= '" + rw1["RefProduit"] + "'").FirstOrDefault();
                                 if (existeDeja != null)
                                 {
                                     existeDeja["Quant"] = Convert.ToInt32(existeDeja["Quant"]) + Convert.ToInt32(rw1["Quantité"]);
                                 }
                                 else tbGlobal.Rows.Add(Convert.ToString(rw1["RefProduit"]), "", Convert.ToInt32(rw1["Quantité"]), 0, 0);

                             }

                             foreach (DataRow rw2 in tbGlobal.Rows)
                             {
                                 DataRow infoProd = tbProduit.Select("RefProduit= '" + Convert.ToString(rw2["Référence"]) + "'").FirstOrDefault();
                                 if (infoProd != null)
                                 {
                                     rw2["Titre"] = infoProd["Designation"];
                                     rw2["Prix unit"] = infoProd["PrixUnitTTC"];
                                     rw2["Montant"] = (Convert.ToInt32(rw2["Prix unit"])) * (Convert.ToInt32(rw2["Quant"]));
                                 }
                             }
                         }
                     }
                     MessageBox.Show("Mise a jour effectuée");

                     
                     BonCmdGlobal fenBon = new BonCmdGlobal();
                     fenBon.Show();

                 }
                 catch (Exception)
                 {
                     MessageBox.Show("Veuillez choisir le fournisseur puis les demandes concernées");
                     
                 }
                 finally
                 {
                     conn.Close();
                 }

             }
             else
                 MessageBox.Show("Aucune demande selectionnée");
        }
        private void datagrid1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {



            if (e.PropertyType == typeof(DateTime))
            {
                DataGridTextColumn dataGridTextColumn = e.Column as DataGridTextColumn;
                if (dataGridTextColumn != null)
                {
                    dataGridTextColumn.Binding.StringFormat = "{0:dd/MM/yyyy}";
                }
            }
        }


        #endregion

        #endregion

        #endregion

        #region Prets
        private void pretBtn_Click(object sender, RoutedEventArgs e)
        {
            pret.IsSelected = true;
        }

        #region pretType1
        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            AjouterSousType1 fen = new AjouterSousType1(conn);
            Methodes.ShowWindow(this, fen);
        }
        #endregion

        #region pretType2
        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            AjouterSousType2 fen = new AjouterSousType2(conn);
            Methodes.ShowWindow(this, fen);
        }
        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            AjouterProduit fen = new AjouterProduit(conn);
            Methodes.ShowWindow(this, fen);
        }

        #endregion
        #endregion

        #region Statistiques
        public String nomprod;
        private void statBtn1_Click(object sender, RoutedEventArgs e)
        {
            stat.IsSelected = true;
        }
        private void bottonstatT1_aff_Click(object sender, RoutedEventArgs e)
        {
            if (DaDeb1.Text != "" && DaFin1.Text != "")
            {
                if (comboEtatDemT1.SelectedIndex != -1)
                {
                    if (comboPretT1.SelectedIndex != -1)
                    {
                        StatistiqueDemVir fen = new StatistiqueDemVir(conn, comboPretT1.SelectedItem.ToString(), comboEtatDemT1.SelectedItem.ToString(), DaDeb1, DaFin1);
                        fen.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                        fen.Show();
                    }
                    else MessageBox.Show("Veuillez selectionner la désignation");
                }
                else MessageBox.Show("Veuillez selectionner l'état");
            }
            else MessageBox.Show("Veuillez entrer les dates de début et de fin");

        }

        private void botton_aff_Click_ant(object sender, RoutedEventArgs e)
        {
            if (comboPretAnt.SelectedIndex != -1)
            {
                if (DaDeb3.Text != "" && DaFin3.Text != "")
                {  
                    StatistiqueDemVir fen = new StatistiqueDemVir(conn, comboPretAnt.SelectedItem.ToString(), DaDeb3, DaFin3);
                    fen.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    fen.Show();
                }
                else MessageBox.Show("Veuillez entrer les dates de début et de fin");
            }
            else MessageBox.Show("Veuillez choisir le remboursement sur");
        }
        private void botton_aff_Click_Fournisseur(object sender, RoutedEventArgs e)
        {
            if (comboFournisseur.SelectedIndex != -1)
            {
                if (DaDebFr.Text != "" && DaFinFr.Text != "")
                {
                    int r = 3;
                    StatistiqueDemVir fen = new StatistiqueDemVir(conn, comboFournisseur.SelectedItem.ToString(), DaDebFr, DaFinFr, r);
                    fen.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    foreach (var item in FournisseursGrid.Children)
                    {
                        if (item is TextBox)
                            ((TextBox)item).Clear();

                        if (item is DatePicker)
                            ((DatePicker)item).Text = "";
                        if (item is ComboBox)
                            ((ComboBox)item).SelectedIndex = -1;

                    }
                    fen.Show();
                }
                else MessageBox.Show("Veuillez saisir la date de début et de fin");
            }
            else MessageBox.Show("Veuillez choisir un fournisseur");
       
           
        }
        private void botton_aff_Click_Produits(object sender, RoutedEventArgs e)
        {
            if (DaDeb4.Text != "" && DaFin4.Text != "")
            {
                if (DesignationStat.SelectedIndex != -1)
                {
                    DataTable dt = new DataTable();
                    dt = ((DataView)datagradProduitStat.ItemsSource).ToTable();
                    int cpt = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToBoolean(dr[1]) == true) cpt++;
                    };
                    if (cpt == 1)
                    {
                        Boolean Var = false;
                        foreach (DataRow row in dt.Rows)
                        {
                            Var = Boolean.Parse(row["Selection"].ToString());
                            if (Var == true)
                            {
                                nomprod = row["Produit"].ToString();

                            }

                        }
                        String t = "t";
                        StatistiqueDemVir fen = new StatistiqueDemVir(conn, nomprod, DaDeb4, DaFin4, t);
                        fen.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                        fen.Show();
                    }
                    else MessageBox.Show("Veuillez selectionner un seul produit");
                }
                else MessageBox.Show("Veuillez choisir une désignation");
            }
            else MessageBox.Show("Veuillez choisir la date de début et de fin");
        }
 
        private void Retourchoice(object sender, RoutedEventArgs e)
        {
            choice.IsSelected = true;
        }
      
        private void comboEtatDemT1Stat_Initialized(object sender, EventArgs e)
        {
            comboEtatDemT1.Items.Add("Acceptée");
            comboEtatDemT1.Items.Add("Refusée");
            comboEtatDemT1.Items.Add("En atente");
            comboEtatDemT1.Items.Add("En instance");
            comboEtatDemT1.Items.Add("Touts");
        }
        

        private void dateDebutVirStatPret_Loaded(object sender, RoutedEventArgs e)
        {
            dateDebutVirStatPret.ItemsSource = Enumerable.Range(2015, DateTime.Now.Year - 2015 + 1).ToList();
        }

        private void dateDebutVirStatPret_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateFinVirStatPret.ItemsSource = Enumerable.Range(Convert.ToInt32(dateDebutVirStatPret.SelectedItem), (DateTime.Now.Year - 2015 + 1) - (Convert.ToInt32(dateDebutVirStatPret.SelectedItem) - 2015)).ToList();
            dateFinVirStatPret.IsEnabled = true; dateFinVirStat.SelectedIndex = 0;
            applyStatVirPret.IsEnabled = true;
        }

        private void applyStatVirPret_Click(object sender, RoutedEventArgs e)
        {/*
            StatPret st = new StatPret(conn);
            chartVirPret.Series.Clear();
            CategoryAxis primaryAxis = new CategoryAxis();
            primaryAxis.Header = "Années";
            chartVir.PrimaryAxis = primaryAxis;
            ColumnSeries series1 = new ColumnSeries();
            series1.ItemsSource = st.montantIntervalAnnees(Convert.ToInt32(dateDebutVirStatPret.SelectedItem), Convert.ToInt32(dateFinVirStatPret.SelectedItem));
            series1.XBindingPath = "Année";
            series1.YBindingPath = "Montant";
            chartVirPret.Series.Add(series1);*/
            chartVirPret.Series.Clear();
            CategoryAxis primaryAxis = new CategoryAxis();
            primaryAxis.Header = "Années";
            chartVir.PrimaryAxis = primaryAxis;
            ColumnSeries series1 = new ColumnSeries();
            series1.ItemsSource = StatPret.montantIntervalAnnees(Convert.ToInt32(dateDebutVirStatPret.SelectedItem), Convert.ToInt32(dateFinVirStatPret.SelectedItem),conn);
            series1.XBindingPath = "Année";
            series1.YBindingPath = "Montant";
            chartVirPret.Series.Add(series1);
        }


        public void fillfournisseur()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT NomFournisseur FROM Fournisseur";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                comboFournisseur.Items.Add(dr["NomFournisseur"].ToString());

            }
            comboFournisseur.Items.Add("Touts");
            conn.Close();
        }
        public void filldesignationT1()
        {
            
            conn.Open();
            String t = "1";
            SqlCommand cmd = new SqlCommand();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT DesignationPret FROM TypePret WHERE TypePret='" + t + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            
            foreach (DataRow dr in dt.Rows)
            {
                comboPretT1.Items.Add(dr["DesignationPret"].ToString());

            }
            comboPretT1.Items.Add("Touts");
            conn.Close();
           
        }
        public void filldesignationT2()
        {
            conn.Open();
            String t = "2";
            SqlCommand cmd = new SqlCommand();
            cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT DesignationPret FROM TypePret WHERE TypePret='" + t + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                DesignationStat.Items.Add(dr["DesignationPret"].ToString());

            }
            conn.Close();
            //DesignationStat.Items.Add("Touts");
        }


        public void fillRembAnticipé()
        {

            comboPretAnt.Items.Add("Salaire");
            comboPretAnt.Items.Add("Prime de rendement");
            comboPretAnt.Items.Add("Prime sociale");
            comboPretAnt.Items.Add("Touts");

        }


        private void comboPretT1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboPretT1.SelectedIndex != -1) if (comboPretT1.SelectedItem.ToString() == "Touts") comboPretT1.SelectedIndex = -1;

        }
        private void ComboBoxItemStatt_Selected(object sender, RoutedEventArgs e)
        {
            Type1Stat.IsSelected = true;
        }

        private void ComboBoxItemStatt_Selected_1(object sender, RoutedEventArgs e)
        {
            Type2Stat.IsSelected = true;
        }
        private void ComboBoxItemStatt_Selected_2(object sender, RoutedEventArgs e)
        {
            RemAnticStat.IsSelected = true;
        }
        private void ComboT2_Selected(object sender, RoutedEventArgs e)
        {
            Produits.IsSelected = true;
        }

        private void ComboT2_Selected_1(object sender, RoutedEventArgs e)
        {
            Fournisseurs.IsSelected = true;
        }

        public void datagradProduitStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void ComboDesignationStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataTable dat = new DataTable();
            bool dis = true;
            String Desi = DesignationStat.SelectedItem.ToString();
            conn.Open();
            DataTable TableProduit = new DataTable();
            SqlCommand cm = new SqlCommand("select Designation,Disponible,RefFournisseur from Produit", conn);
            SqlDataAdapter ad3 = new SqlDataAdapter(cm);
            ad3.Fill(TableProduit);
            conn.Close();

            DataTable TableFourni = new DataTable();
            conn.Open();
            SqlCommand cm1 = new SqlCommand("select RefFournisseur from Fournisseur where TypeFournisseur='" + Desi + "'", conn);
            SqlDataAdapter ad1 = new SqlDataAdapter(cm1);
            ad1.Fill(TableFourni);
            conn.Close();

            DataTable tableProduitFin = new DataTable();
            tableProduitFin.Columns.Add("Produit", typeof(String));

            String desing, REFfour1, REFfour2 = String.Empty;
            Boolean DIS = false;
            for (int i = 0; i < TableFourni.Rows.Count; i++)
            {
                REFfour1 = Convert.ToString(TableFourni.Rows[i]["RefFournisseur"]);

                for (int j = 0; j < TableProduit.Rows.Count; j++)
                {
                    REFfour2 = Convert.ToString(TableProduit.Rows[j]["RefFournisseur"]);
                    desing = Convert.ToString(TableProduit.Rows[j]["Designation"]);
                    DIS = Convert.ToBoolean(TableProduit.Rows[j]["Disponible"]);
                    if (REFfour1 == REFfour2)
                    {
                        if (DIS == true)
                        {
                            tableProduitFin.Rows.Add(desing);
                        }
                    }
                }
            }

            tableProduitFin.Columns.Add("Selection", typeof(Boolean));
            foreach (DataRow row in tableProduitFin.Rows)
            { row["Selection"] = false; }

            datagradProduitStat.ItemsSource = tableProduitFin.DefaultView;
            conn.Close();

        }

        private void comboEtatDem3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void comboEtatDemT1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void com_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboChoixStat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
      
        private void retour0_click(object sender, RoutedEventArgs e)
        {
            choice.IsSelected = true;
        }
          private void retour1_click(object sender, RoutedEventArgs e)
        {
            ChoixT2.IsSelected = true;
        }
          private void retour2_click(object sender, RoutedEventArgs e)
        {
            ChoixT2.IsSelected = true;
        }
          private void retour3_click(object sender, RoutedEventArgs e)
        {
            choice.IsSelected = true;
        }
        
        


        #region StatVirm
        #endregion

        #region StatDem
        #endregion
        #endregion

        #region Historique

        private void PVbtn_Click(object sender, RoutedEventArgs e)
        {
            Pv.IsSelected = true;
        }

        private void rechBtn1_Click(object sender, RoutedEventArgs e)
        {
            Histo.IsSelected = true;
        }

        private void comboFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboFiltre.SelectedItem.ToString())
            {
                case "Personne":
                    textFonct.Visibility = Visibility.Visible;labelFonct.Visibility=Visibility.Visible; gridFonct.Visibility = Visibility.Visible; DateDeb1.Visibility = Visibility.Hidden;lablDe.Visibility=Visibility.Hidden;
                    DateFin1.Visibility = Visibility.Hidden;lblA.Visibility=Visibility.Hidden; rad1.Visibility = Visibility.Hidden; rad2.Visibility = Visibility.Hidden; comboPret.Visibility = Visibility.Hidden;labelpret.Visibility=Visibility.Hidden;
                    break;
                case "Type":
                    textFonct.Visibility = Visibility.Hidden; labelFonct.Visibility=Visibility.Hidden;gridFonct.Visibility = Visibility.Hidden; DateDeb1.Visibility = Visibility.Hidden;lablDe.Visibility=Visibility.Hidden;
                    DateFin1.Visibility = Visibility.Hidden;lblA.Visibility=Visibility.Hidden; rad1.Visibility = Visibility.Visible; rad2.Visibility = Visibility.Visible; comboPret.Visibility = Visibility.Hidden;labelpret.Visibility=Visibility.Hidden;
                    break;
                case "Période":
                    textFonct.Visibility = Visibility.Hidden;labelFonct.Visibility=Visibility.Hidden; gridFonct.Visibility = Visibility.Hidden; DateDeb1.Visibility = Visibility.Visible;lablDe.Visibility=Visibility.Visible;
                    DateFin1.Visibility = Visibility.Visible; rad1.Visibility = Visibility.Hidden; rad2.Visibility = Visibility.Hidden; comboPret.Visibility = Visibility.Hidden;labelpret.Visibility=Visibility.Hidden;
                    label1.Visibility = Visibility.Hidden;lblA.Visibility=Visibility.Visible; comboNum.Visibility = Visibility.Hidden;
                    break;
                case "Motif":
                    textFonct.Visibility = Visibility.Hidden;labelFonct.Visibility=Visibility.Hidden; gridFonct.Visibility = Visibility.Hidden; DateDeb1.Visibility = Visibility.Hidden;lablDe.Visibility=Visibility.Hidden;
                    DateFin1.Visibility = Visibility.Hidden;lblA.Visibility=Visibility.Hidden; rad1.Visibility = Visibility.Hidden; rad2.Visibility = Visibility.Hidden; comboPret.Visibility = Visibility.Visible;labelpret.Visibility=Visibility.Visible;
                    break;

            }
        }

        private void comboNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Historique his = new Historique(conn);
            if (!comboNum.Items.IsEmpty)
                dataPret.ItemsSource =his.parNum(comboNum.SelectedItem.ToString()).DefaultView;
        }

        private void gridFonct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (gridFonct.SelectedIndex >= 0)
                {
                    LigneNomPren ligne = new LigneNomPren();
                    ligne = (LigneNomPren)gridFonct.SelectedItems[0];
                    nom = ligne.nom;
                    prenom = ligne.prenom;
                    textFonct.Text = nom + " " + prenom;
                }
            }
            catch (Exception) { MessageBox.Show("Modification interdite"); };
        }

        private void textFonct_TextChanged(object sender, TextChangedEventArgs e)
        {
            Historique his = new Historique(conn);
            gridFonct.UnselectAll();
            gridFonct.ItemsSource = his.fonctionaire(textFonct.Text);
        }

        private void butValider_Click(object sender, RoutedEventArgs e)
        {
            Historique his = new Historique(conn);
            if (comboFiltre.SelectedItem == null) MessageBox.Show("Veuillez choisir un filtre");
            else
            {
                if (!comboNum.Items.IsEmpty) comboNum.Items.Clear();
                switch (comboFiltre.SelectedItem.ToString())
                {
                    case "Personne":
                        if (string.IsNullOrEmpty(textFonct.Text)) MessageBox.Show("Veuillez saisir le nom et le prénom du fonctionnaire");
                        else
                        {
                            dataPret.ItemsSource = his.parPersonne(nom, prenom).DefaultView;
                            DataTable dt = new DataTable();
                            dt = his.Remplir_ComboNum(dataPret);
                            foreach (DataRow dr in dt.Rows)
                            {
                                comboNum.Items.Add(dr[0].ToString());
                            };
                            label1.Visibility = Visibility.Visible; comboNum.Visibility = Visibility.Visible;
                        }
                        break;
                    case "Type":
                        if (rad1.IsChecked.Value) dataPret.ItemsSource = his.parType("1").DefaultView;
                        else
                        {
                            dataPret.ItemsSource = his.parType("2").DefaultView;
                            DataTable dt = new DataTable();
                            dt = his.Remplir_ComboNum(dataPret);
                            foreach (DataRow dr in dt.Rows)
                            {
                                comboNum.Items.Add(dr[0].ToString());
                            };
                            label1.Visibility = Visibility.Visible; comboNum.Visibility = Visibility.Visible;
                        }
                        break;
                    case "Période":
                        if (DateDeb1.SelectedDate == null || DateFin1.SelectedDate == null) MessageBox.Show("Veuillez saisir les deux dates");
                        else
                        {
                            dataPret.ItemsSource = his.parPériode(DateDeb1, DateFin1).DefaultView;
                        }
                        break;
                    case "Motif":
                        if (comboPret.SelectedItem == null) MessageBox.Show("Veuillez choisir une désignation");
                        else
                        {
                            dataPret.ItemsSource = his.parPrêt(comboPret.SelectedItem.ToString()).DefaultView;
                            DataTable dt = new DataTable();
                            dt = his.Remplir_ComboNum(dataPret);
                            foreach (DataRow dr in dt.Rows)
                            {
                                comboNum.Items.Add(dr[0].ToString());
                            };
                            label1.Visibility = Visibility.Visible; comboNum.Visibility = Visibility.Visible;
                        }
                        break;
                };

            }
        }

        private void dataPret_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                DataGridTextColumn dataGridTextColumn = e.Column as DataGridTextColumn;
                if (dataGridTextColumn != null)
                {
                    dataGridTextColumn.Binding.StringFormat = "{0:dd/MM/yyyy}";
                }
            }
        }

        #endregion

        private void comboPret1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

  

       

       

        


#endregion


        private void comboEtatDem1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabFonct1_Loaded(object sender, RoutedEventArgs e)
        {
            Methodes.afficher_tabFonct(tabFonct, conn);
        }

        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MetroAnimatedTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboPretAnt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void ComboDesignation_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboFournisseur.Items.Clear();
            DataTable dat;
            String Designation = ComboDesignation.SelectedItem.ToString();
            String cmd = "select NomFournisseur from Fournisseur where TypeFournisseur='" + Designation + "'";
            conn.Open(); ;
            SqlCommand cmduser = new SqlCommand(cmd, conn);
            
            SqlDataAdapter ad = new SqlDataAdapter(cmduser);
            dat = new DataTable();
            ad.Fill(dat);
            foreach (DataRow dr in dat.Rows)
            {
                ComboFournisseur.Items.Add(dr["NomFournisseur"].ToString());

            }
            conn.Close();
        }

        private void tabPrime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void ModifierProduit(object sender, MouseButtonEventArgs e)
        {
            ModifProduit fen = new ModifProduit(conn);
            Methodes.ShowWindow(this, fen);
        }

        private void demBtn_Click(object sender, RoutedEventArgs e)
        {
            deman.IsSelected = true;
        }

        private void primBtn_Click(object sender, RoutedEventArgs e)
        {
            prim.IsSelected = true;
        }

        private void MontVoulu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void Periodicité_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void PeriodiciteAnt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void MontantRem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Methodes.allowNumber(sender))
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                Storyboard.SetTarget(blink, sender as TextBox);
                BeginStoryboard(blink);
            }
        }

        private void comboPretAnt_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        #region Barre glissante

        private void Slider(string p, Canvas pan)
        {
            Storyboard b = Resources[p] as Storyboard;
            b.Begin(pan);
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            Slider("afficher", pan);
            opnBtn.Visibility = Visibility.Hidden;
            hideBtn.Visibility = Visibility.Visible;
        }

        private async void btnclickclose(object sender, RoutedEventArgs e)
        {
            if (sauv.Visibility == Visibility.Visible)
            {
                anim("cachersauv", sauv);
                anim("cacherRes", rest);
                await Task.Delay(350);
                sauv.Visibility = Visibility.Hidden;
                rest.Visibility = Visibility.Hidden;
            }
            Slider("retour", pan);
            opnBtn.Visibility = Visibility.Visible;
            hideBtn.Visibility = Visibility.Hidden;           
        }

        #endregion

        private void buttonExportPdf_Copy_Click(object sender, RoutedEventArgs e)
        {
            SuppPv.IsSelected = true;
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            accPv.IsSelected = true;
        }
        #region BDD
        private async void bddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sauv.Visibility == Visibility.Hidden)
            {
                sauv.Visibility = Visibility.Visible;
                rest.Visibility = Visibility.Visible;
                anim("affichersauv", sauv);
                anim("afficherRes", rest);
            }
            else
            {
                anim("cachersauv", sauv);
                anim("cacherRes", rest);
                await Task.Delay(350);
                sauv.Visibility = Visibility.Hidden;
                rest.Visibility = Visibility.Hidden;
            }
        }
        private void anim(string p, Button btn)
        {
            Storyboard b = Resources[p] as Storyboard;
            b.Begin(btn);

            /* DoubleAnimation db = new DoubleAnimation();
             db.From = 63;
             db.To = 200;
            
             db.Duration = new Duration(TimeSpan.FromSeconds(1));
             btn.BeginAnimation(Button.WidthProperty, db);*/
        }

        private void sauv_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.ShowDialog();
            String path = dlg.FileName;
            conn.Open();
            SqlCommand cmd = new SqlCommand("BACKUP DATABASE [OeuvresSociales] TO  DISK = N'" + path + ".bak" + "' WITH NOFORMAT, NOINIT,  NAME = N'OeuvresSociales-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void rest_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".bak";
            dlg.Filter = "Sql backup|*.bak";
            dlg.ShowDialog();
            String path = dlg.FileName;
            try
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("USE [master] alter database [OeuvresSociales] set single_user with rollback immediate ", conn);
                cmd1.ExecuteNonQuery();
                SqlCommand cmd = new SqlCommand("RESTORE DATABASE [OeuvresSociales] FROM  DISK = N'" + path + "' WITH  FILE = 1,  NOUNLOAD,  STATS = 5", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                MessageBox.Show("Acces interromput !!!");
            }
        }

        #endregion

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {         
            DataTable dtSource = new DataTable();
            try
            {
                dtSource = ((DataView)dataeff1.ItemsSource).ToTable();
                bool existe = new bool();
                foreach (DataRow rw in dtSource.Rows)
                {
                    if (Convert.ToBoolean(rw[4]) == true)
                    {
                        conn.Open();
                        existe = true;
                        SqlCommand cmd = new SqlCommand("UPDATE DemandePret SET Etat ='S' WHERE NumDemPret = @ndp", conn);
                        cmd.Parameters.Add("@ndp", SqlDbType.Char).Value = rw["NumDemPret"];
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                if (existe) MessageBox.Show("Demande(s) effacée(s)");
                buttonAnnulerPv.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            catch (Exception)
            {
                MessageBox.Show("Aucune demande selectionnée");
            }
            finally { };
        }

        private void fonctionnaire_Loaded(object sender, RoutedEventArgs e)
        {
            Methodes.afficher_tabFonct(tabFonct, conn);
        }



    }
}
