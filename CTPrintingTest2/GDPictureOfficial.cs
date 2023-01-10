
using GdPicture14;
using NewRelic.Api.Agent;
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
            NewRelic.Api.Agent.NewRelic.SetTransactionName("CTPrintingTest2", "GDPicture Official Print");
            IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
            agent.CurrentTransaction.AddCustomAttribute("FileName", fileSelected);
            agent.CurrentTransaction.AddCustomAttribute("ProductVersion", System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).ProductVersion);
            agent.CurrentTransaction.AddCustomAttribute("DPI", dpiToUse);
 

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
                        agent.CurrentTransaction.AddCustomAttribute("Print Time", elapsedPrint);
                    }
                    else
                    {
                        txtOutput = txtOutput + "The file can't be printed.\nStatus: " + pdf.PrintGetStat().ToString() + System.Environment.NewLine;
                        if (pdf.PrintGetStat() == GdPictureStatus.PrintingException)
                            txtOutput = txtOutput + "    Error: " + pdf.PrintGetLastError() + System.Environment.NewLine;
                        agent.CurrentTransaction.AddCustomAttribute("Print Time", 0);
                    }
                }
                else
                {
                    txtOutput = txtOutput + "The file can't be loaded. Status: " + pdf.GetStat().ToString() + System.Environment.NewLine;
                }
            }

            agent.CurrentTransaction.AddCustomAttribute("Print Status", txtOutput);

            return txtOutput;

        }
    }
}
