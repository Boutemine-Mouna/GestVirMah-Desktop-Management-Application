using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Data;

namespace GestVirMah.Classes
{
    public abstract class Methodes
    {
        private static MetroDialogSettings confirmationSettings = new MetroDialogSettings()
        {
            AffirmativeButtonText = "OUI",
            NegativeButtonText = "NON",
            AnimateShow = true,
            AnimateHide = true,
            ColorScheme = MetroDialogColorScheme.Theme,
        };

        public static MetroDialogSettings ConfirmationSettings
        {
            get { return Methodes.confirmationSettings; }
            set { Methodes.confirmationSettings = value; }
        }

        public static void ShowWindow(MetroWindow originWindow, MetroWindow showingWindow)
        {
            showingWindow.Owner = originWindow;
            originWindow.ShowOverlayAsync();
            showingWindow.ShowDialog();
            originWindow.HideOverlayAsync();
        }

        public static void chargerUsersGrid(SqlConnection connexionSql, Utilisateur user, DataGrid usersGrid)
        {
            connexionSql.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM utilisateur WHERE Login != '" + user.Identifiant + "' AND Supprime = 'N'", connexionSql);
            SqlDataReader reader = cmd.ExecuteReader();
            UsersGridItem item;

            usersGrid.Items.Clear();
            while (reader.Read())
            {
                item = new UsersGridItem
                {
                    CodeUser = reader.GetInt32(0),
                    NomUser = reader["NomUser"].ToString(),
                    PrenUser = reader["PrenUser"].ToString(),
                    Login = reader["Login"].ToString(),
                    droit = reader["droit"].ToString()                   
                };
                item.NomPren = item.NomUser + " " + item.PrenUser;
                if (item.droit == "A")
                    item.droit = "Administrateur";
                else item.droit = "Simple";
                usersGrid.Items.Add(item);            
            }
            reader.Close();
            connexionSql.Close();
        }

        public static void afficher_tabFonct(DataGrid tab, SqlConnection conn)
        {
            conn.Open();
            string SQLCmd = "SELECT * FROM Fonctionnaire";
            SqlCommand cmd = new SqlCommand(SQLCmd, conn);
            SqlDataReader read = cmd.ExecuteReader();
            List<LigneFonctionR> tab_ligne = new List<LigneFonctionR>();
            while (read.Read())
            {
                LigneFonctionR Ligne = new LigneFonctionR();
                Ligne.Matricule = Convert.ToString(read["Matricule"]);
                Ligne.Nom = Convert.ToString(read["NomFonct"]);
                Ligne.Prenom = Convert.ToString(read["PrenFonct"]);             
                Ligne.Date = Convert.ToString(read["DateRecrut"]).Substring(0,10); ;
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
            tab.ItemsSource = tab_ligne;
            conn.Close();
        }

        private static double calculeSommeVir(int cp, SqlConnection conn)
        {
           // SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
            double som = 0;
            string cmd = "select MontsantDem from DemandePrime where pv_codepv = " + cp + " and EtatDem ='A' ";
            try
            {

                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader reader = cmdUser.ExecuteReader();
                while (reader.Read())
                {
                    som += int.Parse(reader[0].ToString());
                }
                reader.Close();
               
                return som;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source11" + ex.ToString());
                return som;
            }
            finally { conn.Close(); }
        }

        private static double montantAnnee(int annee, SqlConnection conn)
        {
           // SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
            conn.Open();
            int codePv = 0;
            double som = 0;

            SqlCommand cmdUser = new SqlCommand("select DateCreatVir,pv_codepv from Virement where CodeVir like '" + annee.ToString().Substring(2, 2) + "%'", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmdUser);
            DataTable t = new DataTable();
            adapter.Fill(t);
            conn.Close();
            foreach (DataRow row in t.Rows)
            {
                codePv = int.Parse(row["pv_codepv"].ToString());
                som += calculeSommeVir(codePv,conn);
            }
            
            return som;
        }

        public static List<SeriesVir> montantIntervalAnnees(int annee1, int annee2,SqlConnection conn)
        {
            List<SeriesVir> virs = new List<SeriesVir>();
            SeriesVir item = null;
            int diff = annee2 - annee1 + 1;

            int i = 0; String annee = ""; double mnt = 0;
            while (i < diff)
            {
                annee = (annee1 + i).ToString();
                mnt = montantAnnee(annee1 + i,conn);
                item = new SeriesVir { Année = annee, Montant = mnt };
                virs.Add(item);
                i++;
            }
            return virs;
        }

        public static Boolean allowNumber(object sender)
        {
            Boolean res = false;
            TextBox textBox = sender as TextBox;
            Int32 selectionStart = textBox.SelectionStart;
            Int32 selectionLength = textBox.SelectionLength;
            String newText = String.Empty;
            int count = 0;
            foreach (Char c in textBox.Text.ToCharArray())
            {
                if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && count < 1))
                {
                    newText += c;
                    if (c == '.')
                        count += 1;
                    res = true;
                }
                else res = false;

            }
            textBox.Text = newText;
            textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
            return res;
        }
		
    }
}
