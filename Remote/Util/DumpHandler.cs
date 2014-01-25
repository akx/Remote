using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Remote.Util
{
    internal static class DumpHandler
    {
        public static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var path = Path.Combine(Path.GetTempPath(), "RemoteCrash-" + DateTime.Now.Ticks + ".txt");
            try
            {
                using (var s = new StreamWriter(path, true, Encoding.UTF8))
                {
                    s.WriteLine("Remote hit an unhandled exception of some sort. This log file may help in finding out why.\r\n\r\n");
                    s.WriteLine("{0} -- Unhandled exception", DateTime.Now);
                    s.WriteLine("ExceptionObject = {0}", e.ExceptionObject);
                    var exc = e.ExceptionObject as Exception;
                    while (exc != null)
                    {
                        s.WriteLine("--- " + exc.GetType());
                        s.WriteLine("M=" + exc.Message);
                        s.WriteLine("T=" + exc.TargetSite);
                        s.WriteLine("S=" + exc.Source);
                        s.WriteLine("...");
                        s.WriteLine(exc.StackTrace);
                        exc = exc.InnerException;
                    }
                }
            }
            catch
            {
                
            }
            Process.Start(path);
        }

        public static void UnhandledException(object sender, ThreadExceptionEventArgs e)
        {
            UnhandledException(sender, new UnhandledExceptionEventArgs(e.Exception, false));
            (new ThreadExceptionDialog(e.Exception)).ShowDialog();
        }
    }
}
