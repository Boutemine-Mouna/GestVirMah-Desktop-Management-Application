using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Media.Animation;
using System.Data.SqlClient;
using GestVirMah.Classes;

namespace GestVirMah
{
	public partial class FenUtilisateur : MetroWindow
	{
        private SqlConnection connexionSql;
        private Utilisateur user;

        public Utilisateur User
        {
            get { return user; }
            set { user = value; }
        }

		public FenUtilisateur()
		{
			this.InitializeComponent();
		}

        public FenUtilisateur(SqlConnection connexionSql, Utilisateur user)
        {
            this.InitializeComponent();
            this.connexionSql = connexionSql;
            this.user = user;
            nameBox.Text = user.Nom;
            surnameBox.Text = user.Prenom;
            idBox.Text = user.Identifiant;
            if (user.Droits == 'A')
                userPermissions.SelectedIndex = 0;
            else
            {
                if (user.Droits == 'S')
                    userPermissions.SelectedIndex = 1;
            }
        }

        public FenUtilisateur(SqlConnection connexionSql)
        {
            this.InitializeComponent();
            this.connexionSql = connexionSql;
            expander.Header = "Mot de passe";
            oldLabel.Visibility = Visibility.Collapsed;
            oldBox.Visibility = Visibility.Collapsed;
            newLabel.Content = "Mot de passe:";
            newBox.Width -= 10;
            confirmBox.Width -= 10;
            confirmLabel.Margin = new Thickness(30, 0, 0, 0);
        }

        private void userWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (user == null) 
                expander.IsExpanded = true;             
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            this.connexionSql.Open();
            Storyboard blink;
            blink = (Storyboard)this.FindResource("blink");

            if (user == null)   // nouveau utilisateur
            {
                if (nameBox.Text == "" ||
                    surnameBox.Text == "" ||
                    idBox.Text == "" ||
                    userPermissions.SelectedIndex == -1 ||
                    newBox.Password == "" ||
                    confirmBox.Password == "")  // au moin un champ vide
                {
                    if (nameBox.Text == "")
                    {
                        Storyboard.SetTarget(blink, nameBox);
                        BeginStoryboard(blink);
                    }

                    if (surnameBox.Text == "")
                    {
                        Storyboard.SetTarget(blink, surnameBox);
                        BeginStoryboard(blink);
                    }

                    if (idBox.Text == "")
                    {
                        Storyboard.SetTarget(blink, idBox);
                        BeginStoryboard(blink);
                    }

                    if (userPermissions.SelectedIndex == -1)
                    {
                        Storyboard.SetTarget(blink, userPermissions);
                        BeginStoryboard(blink);
                    }

                    if (newBox.Password == "")
                    {
                        Storyboard.SetTarget(blink, newBox);
						if (expander.IsExpanded == false)
                            {
                                expander.IsExpanded = true;
                            }
                        BeginStoryboard(blink);
                    }

                    if (confirmBox.Password == "")
                    {
                        Storyboard.SetTarget(blink, confirmBox);
						if (expander.IsExpanded == false)
                            {
                                expander.IsExpanded = true;
                            }
                        BeginStoryboard(blink);
                    }                   
                }
                else    // tous les champs sont remplis
                {
                    if (confirmBox.Password.Equals(newBox.Password))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM utilisateur WHERE Login = '" + idBox.Text + "' AND Supprime = 'N'", connexionSql);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            cmd.Cancel();
                            reader.Close();
                            cmd = new SqlCommand("INSERT INTO utilisateur VALUES('" + nameBox.Text +
                            "', '" + surnameBox.Text + "', '" + idBox.Text + "', '" +
                            newBox.Password + "', '" + (((String)((ComboBoxItem)userPermissions.SelectedItem).Content))[0] + "', 'N')",
                            connexionSql);
                            int nbrRowsAffected = cmd.ExecuteNonQuery();
                            if (nbrRowsAffected == 1)
                            {
                                this.Close();
                            }
                            else
                            {
                                this.ShowMessageAsync("Erreur", "Une erreur inattendu s'est produite lors de l'ajout !");
                            }
                        }
                        else
                        {
                            this.ShowMessageAsync("Erreur", "Un utilisateur avec l'identifiant " + '"' +
                                idBox.Text + '"' + " existe déjà !");
                        }
                    }
                    else
                    {
                        this.ShowMessageAsync("Erreur", "Les mots de psse ne correspodent pas !");
                    }
                }            
            }
            else    // modifier utilisateur
            {
                if (nameBox.Text == "" ||
                    surnameBox.Text == "" ||
                    idBox.Text == "" ||
                    userPermissions.SelectedIndex == -1)    // un champs (hors les champs de mot de passe) est vide
                {
                    if (nameBox.Text == "")
                    {
                        Storyboard.SetTarget(blink, nameBox);
                        BeginStoryboard(blink);
                    }

                    if (surnameBox.Text == "")
                    {
                        Storyboard.SetTarget(blink, surnameBox);
                        BeginStoryboard(blink);
                    }

                    if (idBox.Text == "")
                    {
                        Storyboard.SetTarget(blink, idBox);
                        BeginStoryboard(blink);
                    }

                    if (userPermissions.SelectedIndex == -1)
                    {
                        Storyboard.SetTarget(blink, userPermissions);
                        BeginStoryboard(blink);
                    }
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SELECT MotPasse FROM utilisateur WHERE Login = '" + user.Identifiant + "' AND Supprime = 'N'", connexionSql);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int codeUser = user.Code; String motDePasse = "";
                    if (reader.Read())
                    {
                        motDePasse = reader["MotPasse"].ToString();
                        reader.Close();
                    }
                    SqlCommand cmdVerifId = new SqlCommand("SELECT * FROM utilisateur WHERE Login = '" + idBox.Text + "' AND Supprime = 'N'", connexionSql);
                    SqlDataReader readerVerifId = cmd.ExecuteReader();
                    if (!readerVerifId.HasRows)
                    {
                        cmd = new SqlCommand("UPDATE utilisateur SET NomUser = '" + nameBox.Text +
                            "', PrenUser = '" + surnameBox.Text + "', Login = '" + idBox.Text +
                            "', droit = '" + ((ComboBoxItem)userPermissions.SelectedItem).Content.ToString()[0] +
                            "' WHERE CodeUser = '" + codeUser + "'", connexionSql);
                        if (!(oldBox.Password != "" ||
                            newBox.Password != "" ||
                            confirmBox.Password != ""))     // tous les champs mot de passe sont vides
                        {
                            int nbrAffectedRows = cmd.ExecuteNonQuery();
                            if (nbrAffectedRows == 1)
                            {
                                cmd.Cancel();
                                this.Close();
                            }
                            else
                            {
                                this.ShowMessageAsync("Erreur", "Une erreur inattendu s'est produite lors de l'ajout !");
                            }
                        }
                        else    // au moin un champ mot de passe est rempli
                        {
                            if (oldBox.Password == "" ||
                                newBox.Password == "" ||
                                confirmBox.Password == "")  // un champ mot de passe est vide
                            {
                                if (expander.IsExpanded == false)
                                {
                                    expander.IsExpanded = true;
                                }
                                if (oldBox.Password == "")
                                {
                                    Storyboard.SetTarget(blink, oldBox);
                                    BeginStoryboard(blink);
                                }

                                if (newBox.Password == "")
                                {
                                    Storyboard.SetTarget(blink, newBox);
                                    BeginStoryboard(blink);
                                }

                                if (confirmBox.Password == "")
                                {
                                    Storyboard.SetTarget(blink, confirmBox);
                                    BeginStoryboard(blink);
                                }
                            }
                            else    // tous les champs mot de passe sont remplis
                            {
                                if (oldBox.Password == motDePasse)
                                {
                                    if (newBox.Password == confirmBox.Password)
                                    {
                                        cmd.CommandText = "UPDATE utilisateur SET NomUser = '" + nameBox.Text +
                                        "', PrenUser = '" + surnameBox.Text + "', Login = '" + idBox.Text +
                                        "', droit = '" + ((ComboBoxItem)userPermissions.SelectedItem).Content.ToString()[0] +
                                        "', MotPasse = '" + newBox.Password +
                                        "' WHERE CodeUser = '" + codeUser + "'";
                                        int nbrAffectedRows = cmd.ExecuteNonQuery();
                                        if (nbrAffectedRows == 1)
                                        {
                                            cmd.Cancel();
                                            this.Close();
                                        }
                                        else
                                        {
                                            this.ShowMessageAsync("Erreur", "Une erreur inattendue s'est produite lors de la modification !");
                                        }
                                    }
                                    else
                                    {
                                        this.ShowMessageAsync("Erreur", "Les mots de psse ne correspondent pas !");
                                    }
                                }
                                else
                                {
                                    this.ShowMessageAsync("Erreur", "Le mot de psse ne'est pas correct !");
                                }
                            }
                        }
                    }
                    else
                    {
                        this.ShowMessageAsync("Erreur", "Un utilisateur avec l'identifiant " + '"' +
                                idBox.Text + '"' + " existe déjà !");
                    }
                    readerVerifId.Close();
                    reader.Close();
                    SqlCommand cmdUser = new SqlCommand("select * from utilisateur where CodeUser ='" + codeUser + "'", connexionSql);
                    SqlDataReader userReader = cmdUser.ExecuteReader();
                    if (userReader.Read())
                    {
                        user = new Utilisateur(userReader.GetInt32(0), userReader["NomUser"].ToString(), userReader["PrenUser"].ToString(), userReader["Login"].ToString(), userReader["droit"].ToString()[0]);
                    }
                    userReader.Close();
                }
            }
            this.connexionSql.Close();
        }

        private void chmpsSaisie_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox eventSender = (TextBox)sender;
            if (eventSender.IsFocused)
            {
                save.IsEnabled = true;
            }
        }

        private void chmpsSaisie_TextChanged(object sender, RoutedEventArgs e)
        {
            Control eventSender = (Control)sender;
            if (eventSender.IsFocused)
            {
                save.IsEnabled = true;
            }
        }

        private void userPermissions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsKeyboardFocusWithin)
            {
                save.IsEnabled = true;
            }    
        }

        private void userWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

		
        /*private void saveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Test vide
            if (nameBox.Text == "")
			{
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blinkName");
                Storyboard.SetTarget(blink, nameBox);
                BeginStoryboard(blink);
			}
			
			if(surnameBox.Text == "")
			{
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blinkName");
                Storyboard.SetTarget(blink, surnameBox);
                BeginStoryboard(blink);
			}

            if (oldPass.Password == "")
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blinkName");
                Storyboard.SetTarget(blink, oldPass);
                BeginStoryboard(blink);
            }

            if (newPass.Password == "")
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blinkName");
                Storyboard.SetTarget(blink, newPass);
                BeginStoryboard(blink);
            }

            if (confirmPass.Password == "")
            {
                Storyboard blink;
                blink = (Storyboard)this.FindResource("blinkName");
                Storyboard.SetTarget(blink, confirmPass);
                BeginStoryboard(blink);
            }
            #endregion

        }*/

	}
}