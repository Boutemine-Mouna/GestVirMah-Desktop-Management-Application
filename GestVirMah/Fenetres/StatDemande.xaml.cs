using GestVirMah.Classes;
using GestVirMah.Fenetres;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Collections;
using Syncfusion.UI.Xaml.Charts;

namespace GestVirMah
{
    /// <summary>
    /// Interaction logic for StatDemande.xaml
    /// </summary>
    public partial class StatDemande : MetroWindow
    {
        SqlConnection connexionSql;
        string Prime;
        string Etat;
        DatePicker DaDebut;
        DatePicker DaFin;


        public StatDemande()
        {
            InitializeComponent();
        }


        public StatDemande(SqlConnection connexionSql, string Prime, string Etat, DatePicker DaDebut, DatePicker DaFin)
        {
            this.InitializeComponent();
            this.connexionSql = connexionSql;
            this.Prime=Prime;
            this.Etat=Etat;
            this.DaDebut=DaDebut;
            this.DaFin=DaFin;
            int moisDe = DaDebut.SelectedDate.Value.Month;
            int annéeDe = DaDebut.SelectedDate.Value.Year;





            //Adding horizontal axis to the chart
            CategoryAxis primaryAxis = new CategoryAxis();
            primaryAxis.Header = "Mois / Année";
            chart.PrimaryAxis = primaryAxis;
            //Adding vertical axis to the chart
            NumericalAxis secondaryAxis = new NumericalAxis();
            secondaryAxis.Header = "Nombre";
            chart.SecondaryAxis = secondaryAxis;

          

            List<Str> tabDem = new List<Str> {};



            while ((moisDe != (DaFin.SelectedDate.Value.Month+1)) || (annéeDe != DaFin.SelectedDate.Value.Year))
            {
                Str str = new Str();
                str.mois = moisDe;
                str.année = annéeDe;
                str.axe = moisDe.ToString() + "/" + annéeDe.ToString(); 
                str.nbr = 0;
                tabDem.Add(str);
                moisDe = moisDe + 1;
                if (moisDe == 13)
                {
                    moisDe = 1;
                    annéeDe = annéeDe + 1;
                }

            }


            Boolean condition = true;
            string requette = "select DésignationPrime AS [type prime],DateDem AS [Date demande],EtatDem as [Etat demande] from DemandePrime INNER join TypePrime on TypePrime.CodePrime = DemandePrime.CodePrime";

            if (Prime != "Touts")
            {
                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                requette += " DésignationPrime = '" + Prime + "'";
            }
            if (Etat != "Touts")
            {
                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                switch (Etat)
                {
                    case "Acceptée": requette += " EtatDem ='A'"; break;
                    case "Refusée": requette += " EtatDem ='R'"; break;
                    case "En instance": requette += " EtatDem ='I'"; break;
                    case "En atente": requette += " EtatDem ='T'"; break;
                }
            }
            if (DaDebut.SelectedDate.HasValue)
            {
                if (!DaFin.SelectedDate.HasValue) MessageBox.Show(" Entrer la date de fin");
                else
                {
                    if (condition)
                    {
                        condition = false;
                        requette += " where";
                    }
                    else requette += " AND";
                    requette += " DateDem BETWEEN '" + (DaDebut.Text.Substring(6, 4) + "/" + DaDebut.Text.Substring(3, 3) + DaDebut.Text.Substring(0, 2)).ToString() + "' AND '" + (DaFin.Text.Substring(6, 4) + "/" + DaFin.Text.Substring(3, 3) + DaFin.Text.Substring(0, 2)).ToString() + "'";
                }
            }
            try
            {
                connexionSql.Open();
                SqlCommand cmd = new SqlCommand(requette, connexionSql);
                SqlDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    DateTime dt = (DateTime)read["Date Demande"];
                    int m = dt.Month;
                    int y = dt.Year;
                    for (int j = 0; j < tabDem.Count; j++)
                    {
                        if ((m == tabDem[j].mois) && (y == tabDem[j].année))
                        {
                            tabDem[j].nbr++;
                        }
                    }

                }
                read.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { connexionSql.Close(); }




            ColumnSeries series1 = new ColumnSeries();
            series1.ItemsSource = tabDem;
            series1.XBindingPath = "axe";
            series1.YBindingPath = "nbr";
            chart.Series.Add(series1);


        }



        }


    public class Str
    {
        public int nbr { get; set; }
        public int mois { get; set; }
        public int année { get; set; }
        public string axe { get; set; }
    }


}
