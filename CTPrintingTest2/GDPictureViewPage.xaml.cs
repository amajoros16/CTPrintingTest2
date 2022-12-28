using GdPicture14;
using O2S.Components.PDFRender4NET.Printing;
using O2S.Components.PDFRender4NET.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cursor = System.Windows.Input.Cursor;
using Cursors = System.Windows.Input.Cursors;
using MessageBox = System.Windows.MessageBox;
using PrintDialog = System.Windows.Forms.PrintDialog;

namespace CTPrintingTest
{
    /// <summary>
    /// Interaction logic for GDPictureViewPage.xaml
    /// </summary>
    public partial class GDPictureViewPage : Page
    {
        private Page _previousPage = null;
        string _dpiToUse = null;

        public GDPictureViewPage(Page previousPage, string selectedDocument,string dpiToUse)
        {
            _previousPage = previousPage;
            _dpiToUse = dpiToUse;
            InitializeComponent();
            GdViewer1.DisplayFromFile(selectedDocument);
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_previousPage);
        }

        private void PrintLikeCareTend_Click(object sender, RoutedEventArgs e)
        {
            PrinterSettings printerSettings = new PrinterSettings();
            //1 display the dialog
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                using (PrintDialog printDlg = new PrintDialog())
                {
                    printDlg.AllowSomePages = true;
                    printDlg.AllowCurrentPage = false;
                    printDlg.UseEXDialog = true;
                    printDlg.PrinterSettings = printerSettings;

                    if (DialogResult.OK != printDlg.ShowDialog())
                    {
                        printerSettings = null;
                        return;
                    }

                    printerSettings = printDlg.PrinterSettings;

                }
            });

            if (printerSettings == null)
            {
                return;
            }

            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            long elapsedTime = 0;

            GdViewer1.PrintSetActivePrinter(printerSettings.PrinterName);
            GdViewer1.PrintSetPageSelection(""); //prints all
            GdViewer1.PrintSetCopies(1);

            if (GdViewer1.Print(PrintSizeOption.PrintSizeOptionFit) == GdPictureStatus.OK)
            {
                watch.Stop();
                elapsedTime = watch.ElapsedMilliseconds / 1000;
                
                string msg = "The file has been printed successfully in  " + elapsedTime + "s.";
                MessageBox.Show(msg, "GdViewer CareTend Print");

            }

            else
            {
                string msg = "The file can't be printed.\nStatus: " + GdViewer1.GetStat().ToString() + System.Environment.NewLine;

                if (GdViewer1.PrintGetStat() == GdPictureStatus.PrintingException)
                    msg = msg + "    Error: " + GdViewer1.PrintGetLastError();
                MessageBox.Show(msg, "GdViewer CareTend Print");
            }
            //GdViewer1.CloseDocument();


        }

        private void PrintExperimental_Click(object sender, RoutedEventArgs e)
        {
            PrinterSettings printerSettings = null;
            //1 display the dialog
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                using (PrintDialog printDlg = new PrintDialog())
                {
                    printDlg.AllowSomePages = true;
                    printDlg.AllowCurrentPage = false;
                    printDlg.UseEXDialog = true;
                    printDlg.PrinterSettings = printerSettings;

                    if (DialogResult.OK != printDlg.ShowDialog())
                    {
                        printerSettings = null;
                        return;
                    }

                    printerSettings = printDlg.PrinterSettings;

                }
            });

            if (printerSettings == null)
            {
                return;
            }

            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            long elapsedTime = 0;

            
            GdViewer1.PrintSetActivePrinter(printerSettings.PrinterName);
            GdViewer1.PrintSetPreRasterization(true);
            GdViewer1.PrintSetPreRasterizationDPI(float.Parse(_dpiToUse));
            GdViewer1.PrintSetPageSelection(""); //prints all
            GdViewer1.PrintSetCopies(1);

            if (GdViewer1.Print(PrintSizeOption.PrintSizeOptionFit) == GdPictureStatus.OK)
            {
                watch.Stop();
                elapsedTime = watch.ElapsedMilliseconds / 1000;

                string msg = "The file has been printed successfully in  " + elapsedTime + "s.";
                MessageBox.Show(msg, "GdViewer Experimental Print");

            }

            else
            {
                string msg = "The file can't be printed.\nStatus: " + GdViewer1.GetStat().ToString() + System.Environment.NewLine;

                if (GdViewer1.PrintGetStat() == GdPictureStatus.PrintingException)
                    msg = msg + "    Error: " + GdViewer1.PrintGetLastError();
                MessageBox.Show(msg, "GdViewer Experimental Print");
            }
            //GdViewer1.CloseDocument();

        }

        private void PrintO2S_Click(object sender, RoutedEventArgs e)
        {
            PrinterSettings printerSettings = new PrinterSettings();
            //1 display the dialog
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                using (PrintDialog printDlg = new PrintDialog())
                {
                    printDlg.AllowSomePages = true;
                    printDlg.AllowCurrentPage = false;
                    printDlg.UseEXDialog = true;
                    printDlg.PrinterSettings = printerSettings;

                    if (DialogResult.OK != printDlg.ShowDialog())
                    {
                        printerSettings = null;
                        return;
                    }

                    printerSettings = printDlg.PrinterSettings;

                }
            });

            if (printerSettings == null)
            {
                return;
            }

            //setup O2S printing 
            PrintQueue printQueue = null;

            if (printerSettings.PrinterName.IndexOf("\\") > -1)
            {
                string remoteServerName = printerSettings.PrinterName.Substring(0, printerSettings.PrinterName.LastIndexOf("\\"));
                string remotePrinterName = printerSettings.PrinterName.Substring(printerSettings.PrinterName.LastIndexOf("\\") + 1);
                using (var ps = new PrintServer(remoteServerName))
                {
                    printQueue = ps.GetPrintQueue(remotePrinterName);
                }
            }
            else
            {
                using (var lps = new LocalPrintServer())
                {
                    if (printerSettings != null)
                    {
                        printQueue = lps.GetPrintQueue(printerSettings.PrinterName);
                    }
                    else
                    {
                        printQueue = lps.DefaultPrintQueue;
                    }
                }
            }

            PDFPrintSettings pdfPrintSettings = new PDFPrintSettings(printQueue);
            pdfPrintSettings.AutoRotate = false;
            pdfPrintSettings.PageScaling = PageScaling.None;

            //Actually Print
            Cursor originalCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
            Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            long elapsedTime = 0;
            try
            {

                Guid myGuid = Guid.NewGuid();
                string myFileName = myGuid.ToString() + ".pdf";
                GdViewer1.SaveDocumentToPDF(myFileName);

                PDFFile pdfFile = PDFFile.Open(myFileName);

                if (pdfFile == null)
                {
                    return;
                }

                string PDFView4NetSerialNumber = "PDFVW4WPF50-JP1FH-0DVLW-RFX7P-R740N-95TYA";
                pdfFile.SerialNumber = PDFView4NetSerialNumber;

                if (pdfFile.PageCount > 0)
                {
                    var pageSize = pdfFile.GetPageSize(0);

                    if (pageSize.Height > 1000 && pageSize.Width / pageSize.Height < 0.61)  // "Is that leeeegal?"  "I WILL MAKE IT LEGAL!"
                        pdfPrintSettings.PrintQueue.UserPrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.NorthAmericaLegal);
                    else
                        pdfPrintSettings.PrintQueue.UserPrintTicket.PageMediaSize = pdfPrintSettings.PrintQueue.DefaultPrintTicket.PageMediaSize;
                }

                pdfFile.Print(pdfPrintSettings);
                pdfFile.Dispose();

                watch.Stop();
                elapsedTime = watch.ElapsedMilliseconds / 1000;

                string msg = "The file has been printed successfully in  " + elapsedTime + "s.";
                MessageBox.Show(msg, "GdViewer - O2S Print");
            }
            catch (Exception ex)
            {
                string msg = "The file can't be printed." + System.Environment.NewLine;
                msg = msg + ex.Message;
                MessageBox.Show(msg, "GdViewer - O2S Print");
            }
            finally
            {
                Mouse.OverrideCursor = originalCursor;
            }

            return;
        }


        private void GdViewer1_PageChanged(object sender, GdPicture14.WPF.GdViewer.PageChangedEventArgs e)
        {

        }

        private void GdViewer1_TransferEnded(object sender, GdPicture14.WPF.GdViewer.TransferEndedEventArgs e)
        {

        }

        private void GdViewer1_ZoomChanged(object sender, GdPicture14.WPF.GdViewer.ZoomChangedEventArgs e)
        {
 
        }

  
    }
}
