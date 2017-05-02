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

namespace GestVirMah.Fenetres
{
    public partial class BonCmdGlobal : Form
    {
        public BonCmdGlobal()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BonG crp = new BonG();

            FenetrePrincipale.conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = FenetrePrincipale.conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT CodeRC,MatFiscal,CodeBNP,AppFournisseur FROM Fournisseur WHERE NomFournisseur = '" + FenetrePrincipale.NomFournisseur + "'";
            cmd.ExecuteNonQuery();
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable("detfourni");
            sda1.Fill(dt1);
            DataSet ds = new DataSet();
            ds.Tables.Add(FenetrePrincipale.tbGlobal);
            ds.Tables.Add(dt1);
            ds.WriteXmlSchema("Les2tables.xml");
            crp.SetDataSource(ds);
            crystalReportViewer1.ReportSource = crp;
            crp.SetParameterValue("Fournisseur", FenetrePrincipale.NomFournisseur);
            int montant = 0;
            foreach (DataRow rw in FenetrePrincipale.tbGlobal.Rows)
            {
                montant = Convert.ToInt32(rw["Montant"]) + montant;
            }
            int app = Convert.ToInt32(dt1.Rows[0][3]);
            int result = (montant * app) / 100;
            string lettre = FenetrePrincipale.converti(result);
            crp.SetParameterValue("Lettre", lettre);
            FenetrePrincipale.conn.Close();
        }
    }
}
