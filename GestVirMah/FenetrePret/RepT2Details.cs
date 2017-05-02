using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using GestVirMah.ClassePret;

namespace GestVirMah.FenetrePret
{
    public partial class RepT2Details : Form
    {
        private SqlConnection con;
      //  public static SqlConnection con = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
        private string nom;
        private string prenom;
        public RepT2Details(SqlConnection conn )
        {
            this.con = conn;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FenetrePrincipale.TableProduit.WriteXmlSchema("shema1.xml");
            DemT2Rep crp = new DemT2Rep();
            //MainWindow w = new MainWindow();
            //  String r = w.ComboDesignation.SelectedItem.ToString();
            crp.SetDataSource(FenetrePrincipale.TableProduit);
            crystalReportViewer1.ReportSource = crp;
            crp.SetParameterValue("Fonct", FenetrePrincipale.r);
            String name = FenetrePrincipale.r;


            DataTable TableFon = new DataTable();

            SqlCommand cmd = new SqlCommand("select EmailFonct,TelFonct,AdresseFonct from Fonctionnaire where NomFonct +' '+ PrenFonct like '" + name + "%' or PrenFonct +' '+ NomFonct like '" + name + "%'");
            cmd.Connection = con;
            con.Open();
            SqlDataAdapter ad1 = new SqlDataAdapter(cmd);
            ad1.Fill(TableFon);

            String tel = Convert.ToString(TableFon.Rows[0]["TelFonct"]);
            String adresse = Convert.ToString(TableFon.Rows[0]["AdresseFonct"]);
            String mail = Convert.ToString(TableFon.Rows[0]["EmailFonct"]);
            crp.SetParameterValue("Tel", tel);
            crp.SetParameterValue("Adrss", adresse);
            crp.SetParameterValue("Mail", mail);
            crp.SetParameterValue("Total", FenetrePrincipale.total);
            con.Close();

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

    }
}
