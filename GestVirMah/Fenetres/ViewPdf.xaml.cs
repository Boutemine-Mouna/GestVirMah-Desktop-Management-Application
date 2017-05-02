using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using GestVirMah;
using CrystalDecisions.CrystalReports.Engine;
using GestVirMah.Fenetres;
using GestVirMah.Classes;
using Syncfusion.Pdf.Parsing;
using System.IO;
using Syncfusion.Windows.PdfViewer;

namespace GestVirMah
{
    /// <summary>
    /// Interaction logic for NvDemande.xaml
    /// </summary>
    public partial class ViewPdf : MetroWindow
    {
        private String filePath;

        public ViewPdf()
        {
            InitializeComponent();
        }

        public ViewPdf(String filePath, String title)
        {
            InitializeComponent();
            this.Title = title;
            this.filePath = filePath;
            wb.Navigate(System.IO.Path.GetFullPath(filePath));           
        }

        private void manuelButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog dialog = new System.Windows.Controls.PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                PdfDocumentView documentViewer = new PdfDocumentView();
                PdfLoadedDocument ldoc = new PdfLoadedDocument(filePath);
                documentViewer.Load(ldoc);
                dialog.PrintDocument(documentViewer.PrintDocument.DocumentPaginator, "Imprimer"); ;
            }
        }

    }

}

