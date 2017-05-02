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
using GestVirMah.FenetrePret;
using GestVirMah.ClassePret;


namespace GestVirMah.FenetrePret
{
    /// <summary>
    /// Logique d'interaction pour StatistiqueDemVir.xaml
    /// </summary>
    public partial class StatistiqueDemVir : Window
    {
        public DataTable tabFinale;
     //   public static SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");

        SqlConnection connexionSql;
        string Pret;
        string Etat;
        public String requette;
        DatePicker DaDebut;
        DatePicker DaFin;

        public StatistiqueDemVir()
        {
            InitializeComponent();
        }

        /**********************************************************************************************************************************************************/
        public StatistiqueDemVir(SqlConnection connex, string Pret, string Etat, DatePicker DaDebut, DatePicker DaFin)
        {
            this.InitializeComponent();
            this.connexionSql = connex;
            this.Pret = Pret;
            this.Etat = Etat;
            this.DaDebut = DaDebut;
            this.DaFin = DaFin;
            int moisDe = DaDebut.SelectedDate.Value.Month;
            int annéeDe = DaDebut.SelectedDate.Value.Year;
            FenetrePrincipale win = new FenetrePrincipale(connexionSql);




            //Adding horizontal axis to the chart
            CategoryAxis primaryAxis = new CategoryAxis();
            primaryAxis.Header = "Mois / Année";
            chart.PrimaryAxis = primaryAxis;
            //Adding vertical axis to the chart
            NumericalAxis secondaryAxis = new NumericalAxis();
            secondaryAxis.Header = "Nombre";
            chart.SecondaryAxis = secondaryAxis;



            List<Str> tabDem = new List<Str> { };



            while ((moisDe != (DaFin.SelectedDate.Value.Month + 1)) || (annéeDe != DaFin.SelectedDate.Value.Year))
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

            String choix = win.comboChoix.Text;
            Boolean condition = true;

            if (Pret != "Touts")
            {

                requette = "select DesignationPret AS [type pret],DateDemPret AS [Date demande],Etat as [Etat demande] from DemandePret INNER join TypePret on TypePret.CodePret = DemandePret.CodePret";

                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                requette += " DesignationPret = '" + Pret + "'";
            }
        
            if (Pret == "Touts")
            {
                DataTable Tabl = new DataTable();
                connexionSql.Open();
                SqlCommand cmm = new SqlCommand("select CodePret from TypePret where TypePret='1'", connexionSql);
                SqlDataAdapter ad1 = new SqlDataAdapter(cmm);
                ad1.Fill(Tabl);
                connexionSql.Close();

                String etat = String.Empty;
                if (Etat == "Acceptée") etat = "A";
                if (Etat == "Refusée") etat = "R";
                if (Etat == "En instance") etat = "I";
                if (Etat == "En atente") etat = "E";

                requette = "select CodePret,DateDemPret AS [Date demande],Etat as [Etat demande] from DemandePret";

                for (int e = 0; e < Tabl.Rows.Count; e++)
                {

                    if (condition)
                    {
                        condition = false;
                        requette += " where";
                    }
                    else
                    {
                        if (e == 0) requette += " AND";
                        else requette += " OR";
                    }

                    if (Etat != "Touts")
                    { requette += " CodePret =" + Convert.ToInt32(Tabl.Rows[e]["CodePret"]) + "AND Etat='" + etat + "'"; }
                    else
                    {
                        requette += " CodePret =" + Convert.ToInt32(Tabl.Rows[e]["CodePret"]) + "AND (Etat='A' or Etat='R' or Etat='I' or Etat='E')";
                    }

                }

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
                    case "Acceptée": requette += " Etat ='A'"; break;
                    case "Refusée": requette += " Etat ='R'"; break;
                    case "En instance": requette += " Etat ='I'"; break;
                    case "En atente": requette += " Etat ='E'"; break;
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
                    requette += " DateDemPret BETWEEN '" + (DaDebut.Text.Substring(6, 4) + "/" + DaDebut.Text.Substring(3, 3) + DaDebut.Text.Substring(0, 2)).ToString() + "' AND '" + (DaFin.Text.Substring(6, 4) + "/" + DaFin.Text.Substring(3, 3) + DaFin.Text.Substring(0, 2)).ToString() + "'";
                }
            }

            try
            {
                connexionSql.Open();
                SqlCommand cmd = new SqlCommand(requette, connexionSql);
                cmd.Connection = connexionSql;
               
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
        /***********************************************************************************************************************************************/
        public StatistiqueDemVir(SqlConnection connex, string Pret, DatePicker DaDebut, DatePicker DaFin, String type)
        {
            this.InitializeComponent();
            this.connexionSql = connex;
            this.Pret = Pret;
            
            this.DaDebut = DaDebut;
            this.DaFin = DaFin;
            int moisDe = DaDebut.SelectedDate.Value.Month;
            int annéeDe = DaDebut.SelectedDate.Value.Year;
            FenetrePrincipale win = new FenetrePrincipale(connexionSql);




            //Adding horizontal axis to the chart
            CategoryAxis primaryAxis = new CategoryAxis();
            primaryAxis.Header = "Mois / Année";
            chart.PrimaryAxis = primaryAxis;
            //Adding vertical axis to the chart
            NumericalAxis secondaryAxis = new NumericalAxis();
            secondaryAxis.Header = "Nombre";
            chart.SecondaryAxis = secondaryAxis;



            List<Str> tabDem = new List<Str> { };



            while ((moisDe != (DaFin.SelectedDate.Value.Month + 1)) || (annéeDe != DaFin.SelectedDate.Value.Year))
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
            String Ref = string.Empty;

            DataTable RefDE = new DataTable();
            connexionSql.Open();
            SqlCommand cmd0 = new SqlCommand("select RefProduit,Designation from Produit", connexionSql);
            SqlDataAdapter ad0 = new SqlDataAdapter(cmd0);
            ad0.Fill(RefDE);
            foreach (DataRow r1 in RefDE.Rows)
            {
                if (Convert.ToString(r1["Designation"]) == Pret)
                {
                    Ref = Convert.ToString(r1["RefProduit"]);
                }
            }
            connexionSql.Close();
            requette = "select RefProduit ,DateDemPret AS [Date demande],Etat as [Etat demande] from DemandePret INNER join Contient on Contient.NumDemPret = DemandePret.NumDemPret";

            if (Pret != "Touts")
            {
                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                requette += " RefProduit = '" + Ref + "'";
            }

          
                if (condition)
                {
                    condition = false;
                    requette += " where";
                }
                else requette += " AND";
                requette += " (Etat='S' or Etat='B' or Etat='A')";
            
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
                    requette += " DateDemPret BETWEEN '" + (DaDebut.Text.Substring(6, 4) + "/" + DaDebut.Text.Substring(3, 3) + DaDebut.Text.Substring(0, 2)).ToString() + "' AND '" + (DaFin.Text.Substring(6, 4) + "/" + DaFin.Text.Substring(3, 3) + DaFin.Text.Substring(0, 2)).ToString() + "'";
                }
            }

            try
            {
                connexionSql.Open(); 
                SqlCommand cmd = new SqlCommand(requette, connexionSql);
                cmd.Connection = connexionSql;
               
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
        /*************************************************************************************/
        public StatistiqueDemVir(SqlConnection connex, string Pret, DatePicker DaDebut, DatePicker DaFin)
        {
            this.InitializeComponent();
            this.connexionSql = connex;
            this.Pret = Pret;

            this.DaDebut = DaDebut;
            this.DaFin = DaFin;
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



            List<Str> tabDem = new List<Str> { };



            while ((moisDe != (DaFin.SelectedDate.Value.Month + 1)) || (annéeDe != DaFin.SelectedDate.Value.Year))
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


            string requette = "select RemSur AS [type pret],DateAnt AS [Date demande] from RemboursementAnticipe";
            if (Etat != "Touts")
            {
                if (Pret != "Touts")
                {
                    if (condition)
                    {
                        condition = false;
                        requette += " where";
                    }
                    else requette += " AND";
                    requette += " RemSur = '" + Pret + "'";
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
                    requette += " DateAnt BETWEEN '" + (DaDebut.Text.Substring(6, 4) + "/" + DaDebut.Text.Substring(3, 3) + DaDebut.Text.Substring(0, 2)).ToString() + "' AND '" + (DaFin.Text.Substring(6, 4) + "/" + DaFin.Text.Substring(3, 3) + DaFin.Text.Substring(0, 2)).ToString() + "'";
                }
            }
            try
            {
                connexionSql.Open(); 
                SqlCommand cmd = new SqlCommand(requette, connexionSql);
                cmd.Connection = connexionSql;
               
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

        /***********************************************************************************************************************************************/

        public StatistiqueDemVir(SqlConnection connex, string Pret, DatePicker DaDebut, DatePicker DaFin, int type)
        {
            this.InitializeComponent();
            this.connexionSql = connex;
            this.DaDebut = DaDebut;
            this.DaFin = DaFin;
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

            List<Str> tabDem = new List<Str> { };

            while ((moisDe != (DaFin.SelectedDate.Value.Month + 1)) || (annéeDe != DaFin.SelectedDate.Value.Year))
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

            requette = "select NumDemPret,DateDemPret AS [Date demande],Etat as [Etat demande] from DemandePret";

            DataTable Table3 = new DataTable();
            connexionSql.Open();
            SqlCommand c13 = new SqlCommand("select NomFournisseur,RefFournisseur from Fournisseur", connexionSql);
            SqlDataAdapter ad3 = new SqlDataAdapter(c13);
            ad3.Fill(Table3);
            DataTable fouR = new DataTable();
            fouR.Columns.Add("ref", typeof(String));
            fouR.Columns.Add("fournisseur", typeof(String));
            String fourniNAME, fourniREF, REF = String.Empty;
            for (int t = 0; t < Table3.Rows.Count; t++)
            {
                fourniNAME = Convert.ToString(Table3.Rows[t]["NomFournisseur"]);
                fourniREF = Convert.ToString(Table3.Rows[t]["RefFournisseur"]);
                if (fourniNAME == Pret)
                {
                    REF = fourniREF;
                }

            }
            connexionSql.Close();
            DataTable Table = new DataTable();
            connexionSql.Open();
            SqlCommand c1 = new SqlCommand("select NumDemPret,RefProduit from Contient", connexionSql);
            SqlDataAdapter ad1 = new SqlDataAdapter(c1);
            ad1.Fill(Table);
            connexionSql.Close();

            DataTable Tab = new DataTable();
            connexionSql.Open();
            SqlCommand c8 = new SqlCommand("select RefProduit,RefFournisseur from Produit", connexionSql);
            SqlDataAdapter ad8 = new SqlDataAdapter(c8);
            ad8.Fill(Tab);
            connexionSql.Close();

            DataTable tabi = new DataTable();
            tabi.Columns.Add("refpro", typeof(string));
            tabi.Columns.Add("numdem", typeof(string));

            String ref1, ref2, ref3, numm = String.Empty;
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                ref1 = Convert.ToString(Table.Rows[i]["RefProduit"]);
                numm = Convert.ToString(Table.Rows[i]["NumDemPret"]);

                for (int i1 = 0; i1 < Tab.Rows.Count; i1++)
                {
                    ref2 = Convert.ToString(Tab.Rows[i1]["RefProduit"]);
                    ref3 = Convert.ToString(Tab.Rows[i1]["RefFournisseur"]);
                    if (ref1 == ref2)
                    {
                        if (ref3 == REF)
                        {
                            tabi.Rows.Add(ref2, numm);
                        }
                    }
                }
                
            }

            if (Pret == "Touts")
            {
                connexionSql.Open(); ;
                DataTable Tabl2 = new DataTable();
                SqlCommand cmm2 = new SqlCommand("select NumDemPret from Contient");
                cmm2.Connection = connexionSql;
               
                SqlDataAdapter ad2 = new SqlDataAdapter(cmm2);
                ad2.Fill(Tabl2);
                connexionSql.Close();
                requette = "select NumDemPret,DateDemPret AS [Date demande] from DemandePret";

                for (int e = 0; e < Tabl2.Rows.Count; e++)
                {

                    if (condition)
                    {
                        condition = false;
                        requette += " where";
                    }
                    else
                    {
                        if (e == 0) requette += " AND";
                        else requette += " OR";
                    }


                    requette += " NumDemPret =" + Convert.ToInt32(Tabl2.Rows[e]["NumDemPret"]);


                }
                
            }
            for (int e = 0; e < tabi.Rows.Count; e++)
            {
                if (Pret != "Touts")
                {
                    if (condition)
                    {
                        condition = false;
                        requette += " where";
                    }
                    else
                    {
                        if (e == 0) requette += " AND";
                        else requette += " OR";
                    }
                    //   requette += " RefProduit ='" + Convert.ToString(tabi.Rows[e]["refpro"]) + "' AND (Etat='S' or Etat='B' or Etat='A')";
                    requette += " NumDemPret ='" + Convert.ToString(tabi.Rows[e]["numdem"]) + "' AND (Etat='S' or Etat='B' or Etat='A')";

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
                    requette += " DateDemPret BETWEEN '" + (DaDebut.Text.Substring(6, 4) + "/" + DaDebut.Text.Substring(3, 3) + DaDebut.Text.Substring(0, 2)).ToString() + "' AND '" + (DaFin.Text.Substring(6, 4) + "/" + DaFin.Text.Substring(3, 3) + DaFin.Text.Substring(0, 2)).ToString() + "'";
                }
            }

            try
            {
                connexionSql.Open(); 
                SqlCommand cmd = new SqlCommand(requette, connexionSql);
                cmd.Connection = connexionSql;
             
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
            connexionSql.Close(); 

            
        }


        public class Str
        {
            public int nbr { get; set; }
            public int mois { get; set; }
            public int année { get; set; }
            public string axe { get; set; }
        }


    }
}
