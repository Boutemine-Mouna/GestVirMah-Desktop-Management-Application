using GestVirMah.Classes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestVirMah.Fenetres
{
    public partial class Parametres : MetroWindow
    {
        private SqlConnection connexionSql;
        private Utilisateur user;

        public Utilisateur User
        {
            get { return user; }
            set { user = value; }
        }

        public Parametres()
        {
            InitializeComponent();
        }

        public Parametres(SqlConnection connexionSql, Utilisateur user)
        {
            InitializeComponent();
            this.connexionSql = connexionSql;
            this.user = user;
        }

        private void settingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            connexionSql.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Parametres", connexionSql);
            SqlDataReader reader = cmd.ExecuteReader();

            currentUserLabel.Content = user.Nom + " " + user.Prenom;
            idLabel.Content = user.Identifiant;
            if (reader.Read())
            {
                ministrBox.Text = (String)reader["Ministere"];
                orgaBox.Text = (String)reader["Organisme"];
                dayNum.Value = (int)reader["JourDebAnSoc"];
                monthBox.SelectedIndex = (int)reader["MoisDebAnSoc"] - 1;
                cotisNum.Value = (int)reader["DurCot"]; ;
                esicomptBox.Text = (String)reader["CompteSocEsi"];
                trescomptBox.Text = (String)reader["CompteEsiTresor"];
                adrBox.Text = (String)reader["AdresseFacturation"];
                email.Text = (String)reader["Email"];
            }
            cmd.Cancel();
            reader.Close();
            connexionSql.Close();
            Methodes.chargerUsersGrid(connexionSql, user, usersGrid);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (ministrBox.Text != "" &&
                orgaBox.Text != "" &&
                adrBox.Text != "" &&
                email.Text != "" &&
                cotisNum.Value != null &&
                dayNum.Value != null &&
                esicomptBox.Text != "" &&
                trescomptBox.Text != "")
            {
                connexionSql.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Parametres SET Ministere = @minstrString, Organisme = @orgaString, AdresseFacturation = @adrString , Email = @ema,DurCot = '" + cotisNum.Value +
                "', JourDebAnSoc = '" + dayNum.Value + "', MoisDebAnSoc = @month, CompteSocEsi = '" + esicomptBox.Text +
                "', CompteEsiTresor = '" + trescomptBox.Text +
                "'", connexionSql);
                cmd.Parameters.AddWithValue("@minstrString", ministrBox.Text);
                cmd.Parameters.AddWithValue("@orgaString", orgaBox.Text);
                cmd.Parameters.AddWithValue("@adrString", adrBox.Text);
                cmd.Parameters.AddWithValue("@ema", email.Text);
                cmd.Parameters.AddWithValue("@month", monthBox.SelectedIndex + 1);
                int nbrRowsAffected = cmd.ExecuteNonQuery();
                cmd.Cancel();
                connexionSql.Close();
                this.Close();
            }
            else
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blink");
                
                if (ministrBox.Text == "")
                {
                    Storyboard.SetTarget(blink, ministrBox);
                    BeginStoryboard(blink);
                }

                if (orgaBox.Text == "")
                {
                    Storyboard.SetTarget(blink, orgaBox);
                    BeginStoryboard(blink);
                }

                if (adrBox.Text == "")
                {
                    Storyboard.SetTarget(blink, adrBox);
                    BeginStoryboard(blink);
                }

                if (email.Text == "")
                {
                    Storyboard.SetTarget(blink, email);
                    BeginStoryboard(blink);
                }

                if (esicomptBox.Text == "")
                {
                    Storyboard.SetTarget(blink, esicomptBox);
                    BeginStoryboard(blink);
                }

                if (trescomptBox.Text == "")
                {
                    Storyboard.SetTarget(blink, trescomptBox);
                    BeginStoryboard(blink);
                }

                if (cotisNum.Value == null)
                {
                    Storyboard.SetTarget(blink, trescomptBox);
                    BeginStoryboard(blink);
                }

                if (dayNum.Value == null)
                {
                    Storyboard.SetTarget(blink, dayNum);
                    BeginStoryboard(blink);
                }
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.IsFocused)
            {
                save.IsEnabled = true;
            }
        }

        private void settingsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void numBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            NumericUpDown numBox = (NumericUpDown)sender;
            if (numBox.IsMouseOver || numBox.IsKeyboardFocusWithin)
            {
                save.IsEnabled = true;
            }
        }

        private void monthBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsKeyboardFocusWithin)
            {
                save.IsEnabled = true;
            }          
        }

        private void usersGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	if(((DataGrid)sender).SelectedIndex < 0)
			{
				editUserButton.IsEnabled = false;
				delUserButton.IsEnabled = false;
			}
            else
            {
                editUserButton.IsEnabled = true;
                delUserButton.IsEnabled = true;     
            }
        }

        private void modifyButton_Click(object sender, RoutedEventArgs e)
        {
            FenUtilisateur fen = new FenUtilisateur(connexionSql, user);
            Methodes.ShowWindow(this, fen);
            user = fen.User;
            currentUserLabel.Content = user.Nom + " " + user.Prenom;
            idLabel.Content = user.Identifiant;
            //save.IsEnabled = true;
        }

        private void editUserButton_Click(object sender, RoutedEventArgs e)
        {         
            UsersGridItem item = (UsersGridItem)usersGrid.SelectedItem;
            Utilisateur selectedUser = new Utilisateur(item.CodeUser, item.NomUser, item.PrenUser, item.Login, item.droit[0]);
            FenUtilisateur fen = new FenUtilisateur(connexionSql, selectedUser);
            Methodes.ShowWindow(this, fen);
            //save.IsEnabled = true;
            Methodes.chargerUsersGrid(connexionSql, user, usersGrid);
        }

        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            FenUtilisateur fen = new FenUtilisateur(connexionSql);
            Methodes.ShowWindow(this, fen);
            //save.IsEnabled = true;
            Methodes.chargerUsersGrid(connexionSql, user, usersGrid);
        }

        private async void delUserButton_Click(object sender, RoutedEventArgs e)
        {
            UsersGridItem item = (UsersGridItem)usersGrid.SelectedItem;           
            MessageDialogResult result = await this.ShowMessageAsync("Suppression",
                "Voulez-vous vraiment supprimer l'utilisateur " + '"' + item.NomPren + '"' + "?",
                MessageDialogStyle.AffirmativeAndNegative, Methodes.ConfirmationSettings);
            if (result == MessageDialogResult.Affirmative)
            {
                connexionSql.Open();
                SqlCommand cmd = new SqlCommand("UPDATE utilisateur SET Supprime = 'O' WHERE Login = '" + item.Login + "'", connexionSql);
                cmd.ExecuteNonQuery();
                usersGrid.Items.RemoveAt(usersGrid.SelectedIndex);
                connexionSql.Close();
            }
                
            //save.IsEnabled = true;
        }

    }
}
