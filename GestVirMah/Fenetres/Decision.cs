using CrystalDecisions.CrystalReports.Engine;
using GestVirMah.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GestVirMah.Fenetres
{
    public partial class Decision : Form
    {
        public Decision()
        {
            InitializeComponent();
        }

        public void afficher(ReportClass rapport)
        {

            crystalReportViewer1.ReportSource = rapport;
            crystalReportViewer1.Refresh();
        }

        public void setToAvisLayout()
        {
            crystalReportViewer1.ShowPageNavigateButtons = true;
            crystalReportViewer1.ShowGotoPageButton = true;
        }
      
    }
}
