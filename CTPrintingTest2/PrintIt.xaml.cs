using GdPicture14;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;



namespace CTPrintingTest
{
    /// <summary>
    /// Interaction logic for PrintIt.xaml
    /// </summary>
    public partial class PrintIt : Page
    {
        

        public PrintIt()
        {
            InitializeComponent();


            try
            {
                //RegisterGdPictureKeys
                LicenseManager licenseManager = new LicenseManager();
                Assembly gdPictureAssembly = Assembly.GetAssembly(licenseManager.GetType());
                txtOutput.Text = txtOutput.Text + "Using " +
                    gdPictureAssembly.GetName().Name + ", " +
                    gdPictureAssembly.GetName().Version + System.Environment.NewLine;


                //old set hardcoded into the services
                String gdPictureDocumentImaging = "13294798890617477172411429960842027316";
                String gdPicturePDFPluginLicense = "72637474592727873112111497126291724196";
                String gdPictureXMPAnnotationsPlugin = "73476099571711969151913499944328750100";
                String gdPictureBarcodeRecognition = "72847377395707978112112498646373012148";

                licenseManager.RegisterKEY(gdPictureDocumentImaging);
                licenseManager.RegisterKEY(gdPicturePDFPluginLicense);
                licenseManager.RegisterKEY(gdPictureXMPAnnotationsPlugin);
                licenseManager.RegisterKEY(gdPictureBarcodeRecognition);

                txtOutput.Text = txtOutput.Text + "GD Picture registered successfully." + System.Environment.NewLine;
            }
            catch (Exception ex)
            {
                txtOutput.Text = "GD Picture failed to register." + System.Environment.NewLine;
                txtOutput.Text = txtOutput.Text + ex.Message + System.Environment.NewLine;
                txtOutput.Text = txtOutput.Text + ex.StackTrace + System.Environment.NewLine;
            }
         
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtFile.Text))
            {
                MessageBox.Show("Select file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create the print dialog object and set options
            PrintDialog pDialog = new PrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = true;



            if (cmbPrintType.SelectedItem == null)
            {
                MessageBox.Show("Select print type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string dpiToUse = String.Join("", cmbDPI.Text.ToCharArray().Where(char.IsDigit));

            if ("GD Picture Official".Equals(cmbPrintType.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                GDPictureOfficial workingClass = new GDPictureOfficial();
                txtOutput.Text = txtOutput.Text + workingClass.PrintGDPictureOfficial(txtFile.Text,dpiToUse);
            }
            else if ("GD Picture CareTend".Equals(cmbPrintType.Text, StringComparison.InvariantCultureIgnoreCase))
            {
                GDPictureViewPage myPage = new GDPictureViewPage(this, txtFile.Text, dpiToUse);
                NavigationService.Navigate(myPage); 
            }   
            else
            {
                txtOutput.Text = txtOutput.Text + "Not implemented yet." + System.Environment.NewLine;
            }
            //XpsDocument xpsDocument = new XpsDocument(fileSelected, FileAccess.ReadWrite);
            //FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();
            //pDialog.PrintDocument(fixedDocSeq.DocumentPaginator, "Test print job");
            

        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    txtFile.Text = file;
                    txtFile.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    txtFile.Text = null;
                    txtFile.ToolTip = null;
                    break;
            }
        }




        //private void PrintGDPictureCareTend(string fileSelected)
        //{
        //    using (GdViewer gdViewer = new GdViewer())
        //    {
        //        using (GdPicturePDF pdf = new GdPicturePDF())
        //        {
        //            var watch = System.Diagnostics.Stopwatch.StartNew();
        //            if (pdf.LoadFromFile(fileSelected, false) == GdPictureStatus.OK)
        //            {
        //                the code that you want to measure comes here
        //                watch.Stop();
        //            }
        //            gdViewer.DisplayFromGdPicturePDF(pdf);
        //        }

        //    }
        //}


    }
        
}
