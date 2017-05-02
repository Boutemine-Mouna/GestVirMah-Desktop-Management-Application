using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestVirMah.ClassePret;

namespace GestVirMah.FenetrePret
{
    public partial class Precompte : Form
    {
        public Precompte()
        {
            InitializeComponent();
        }
        private void pre_Load(object sender, EventArgs e)
        {

            FenetrePrincipale.TablePrecomptes.WriteXmlSchema("Shema1.xml");
            ListeDesPrecompte crp = new ListeDesPrecompte();
            crp.SetDataSource(FenetrePrincipale.TablePrecomptes);
            crystalReportViewer1.ReportSource = crp;

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}