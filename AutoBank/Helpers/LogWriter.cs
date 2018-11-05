using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoBank.Helpers
{
    public class LogWriter
    {
        public void WriteTolog(String Message)
        {
            try
            {

                String path = HttpContext.Current.Server.MapPath("~/Logs");
                DateTime t = DateTime.Now;
                System.Text.StringBuilder fileNameBuilder = new System.Text.StringBuilder();

                fileNameBuilder.Append("Chilindo_AutoBank").Append(t.Year).Append("_").Append(t.Month).Append("_").Append(t.Day).Append(".txt");
                path = System.IO.Path.Combine(path, fileNameBuilder.ToString());

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path, true))
                {
                    writer.AutoFlush = true;
                    writer.WriteLine(Message + ". Time Stamp: " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {

            }

        }
    
    }
}