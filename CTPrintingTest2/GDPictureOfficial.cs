
using GdPicture14;
using System;
using System.IO;
using System.Reflection;
using File = System.IO.File;

namespace CTPrintingTest
{
    internal class GDPictureOfficial
    {
        public string PrintGDPictureOfficial(string fileSelected, string dpiToUse)
        {
            string txtOutput = "";

            //We assume that GdPicture has been correctly installed and unlocked.
            //Initializing the GdPicturePDF object.
            using (GdPicturePDF pdf = new GdPicturePDF())
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                if (pdf.LoadFromFile(fileSelected, false) == GdPictureStatus.OK)
                {
                    watch.Stop();
                    var elapsedLoad = watch.ElapsedMilliseconds / 1000;

                    //Enabling the pre-rasterization option.
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    pdf.PrintSetPreRasterization(true);                
                    pdf.PrintSetPreRasterizationDPI(float.Parse(dpiToUse));
                    pdf.PrintSetFromToPage(1, pdf.GetPageCount());
                    if (pdf.PrintDialog())
                    {
                        watch.Stop();
                        var elapsedPrint = watch.ElapsedMilliseconds / 1000;
                        txtOutput = txtOutput + "The file has been printed successfully. Loaded " + elapsedLoad + "s, Printed " + elapsedPrint + "s." + System.Environment.NewLine;
                    }
                    else
                    {
                        txtOutput = txtOutput + "The file can't be printed.\nStatus: " + pdf.PrintGetStat().ToString() + System.Environment.NewLine;
                        if (pdf.PrintGetStat() == GdPictureStatus.PrintingException)
                            txtOutput = txtOutput + "    Error: " + pdf.PrintGetLastError() + System.Environment.NewLine;
                    }
                }
                else
                {
                    txtOutput = txtOutput + "The file can't be loaded. Status: " + pdf.GetStat().ToString() + System.Environment.NewLine;
                }
            }

            return txtOutput;

        }
    }
}
