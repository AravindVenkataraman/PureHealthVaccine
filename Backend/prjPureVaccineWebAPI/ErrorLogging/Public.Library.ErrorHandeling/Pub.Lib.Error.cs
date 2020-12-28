using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace Public.Library.ErrorHandeling
{
    public class clsEvntvwrLogging
    {
        public static void fnLogWritter(Exception ex)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("Exception Type " + Environment.NewLine);
            strBuilder.Append(ex.GetType().Name);
            strBuilder.Append(Environment.NewLine + Environment.NewLine);
            strBuilder.Append("Message" + Environment.NewLine);
            strBuilder.Append(ex.Message + Environment.NewLine + Environment.NewLine);
            strBuilder.Append("Stack Trace" + Environment.NewLine);
            strBuilder.Append(ex.StackTrace + Environment.NewLine + Environment.NewLine);

            Exception Innerex = ex.InnerException;
            while (Innerex != null)
            {
                strBuilder.Append("Exception Type " + Environment.NewLine);
                strBuilder.Append(Innerex.GetType().Name);
                strBuilder.Append(Environment.NewLine + Environment.NewLine);
                strBuilder.Append("Message" + Environment.NewLine);
                strBuilder.Append(Innerex.Message + Environment.NewLine + Environment.NewLine);
                strBuilder.Append("Stack Trace" + Environment.NewLine);
                strBuilder.Append(Innerex.StackTrace + Environment.NewLine + Environment.NewLine);

                Innerex = Innerex.InnerException;
            }

            if (!EventLog.SourceExists("PURECS"))
            {
                EventLog.CreateEventSource("PURECS", "PURECS Log");
                EventLog log = new EventLog();
                log.Source = "PURECS";

                log.WriteEntry(strBuilder.ToString(), EventLogEntryType.Error);
            }

            else if (EventLog.SourceExists("PURECS"))
            {
                EventLog log = new EventLog();
                log.Source = "PURECS";

                log.WriteEntry(strBuilder.ToString(), EventLogEntryType.Error);
            }
        }

        public static void fnMsgWritter(string ex)
        {

            if (!EventLog.SourceExists("PURECS"))
            {
                EventLog.CreateEventSource("PURECS", "PURECS Log");
                EventLog log = new EventLog();
                log.Source = "PURECS";

                log.WriteEntry(ex.ToString(), EventLogEntryType.Information);
            }

            else if (EventLog.SourceExists("PURECS"))
            {
                EventLog log = new EventLog();
                log.Source = "PURECS";

                log.WriteEntry(ex.ToString(), EventLogEntryType.Information);
            }

        }

        public static void SaveErrorInLogFile(string strerrormsg)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Logs";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                string strLogFileName = path + @"\Error-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                if (!File.Exists(strLogFileName)) File.Create(strLogFileName).Dispose();
                File.AppendAllText(strLogFileName, strerrormsg);
                string strBreack = "---------------------------------------------------------" + DateTime.Now.ToString() + "---------------------------------------------------------";
                File.AppendAllText(strLogFileName, strBreack);

            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Write Log");
            }


        }
    }

    public class clsDatabaseLogging
    {
        public static void SaveErrorInLogFile(string strerrormsg)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Logs";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                string strLogFileName = path + @"\Error-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                if (!File.Exists(strLogFileName)) File.Create(strLogFileName).Dispose();
                File.AppendAllText(strLogFileName, strerrormsg);
                string strBreack = "---------------------------------------------------------" + DateTime.Now.ToString() + "---------------------------------------------------------";
                File.AppendAllText(strLogFileName, strBreack);

            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Write Log");
            }


        }

        public static void SaveTextLogFile(string strerrormsg)
        {
            try
            {

                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Logs";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                string strLogFileName = path + @"\log-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                if (!File.Exists(strLogFileName)) File.Create(strLogFileName).Dispose();
                File.AppendAllText(strLogFileName, "\n");
                File.AppendAllText(strLogFileName, strerrormsg);
                File.AppendAllText(strLogFileName, "\n");
                string strBreack = "---------------------------------------------------------" + DateTime.Now.ToString() + "---------------------------------------------------------";
                File.AppendAllText(strLogFileName, "\n");
                File.AppendAllText(strLogFileName, strBreack);
                
            }
            catch (System.IO.IOException ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }


        }

        public static void SaveHL7LogFile(string strerrormsg)
        {
            try
            {

                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Logs1";
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                string strLogFileName = path + "\\"+ Guid.NewGuid().ToString().ToUpper() + ".hl7";
                if (!File.Exists(strLogFileName)) File.Create(strLogFileName).Dispose(); 
                File.AppendAllText(strLogFileName, strerrormsg); 

            }
            catch (System.IO.IOException ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }


        }

    }

    public class clsShowMessageWindow
    {
        public enum AppMsgType
        {
            Error = 0,
            Info = 1,
            Warning = 2,
            Exception = 3,
            Stop = 4,
            Confirmation = 5
        };
        public static DialogResult ShowMessageBox(string strerrormsg, string strCaption, AppMsgType _msgType)
        {
            DialogResult dr = new DialogResult();
            try
            {
                switch (_msgType)
                {
                    case AppMsgType.Error:
                        dr =  MessageBox.Show(strerrormsg,strCaption,MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return dr;
                    case AppMsgType.Info:
                        dr = MessageBox.Show(strerrormsg, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return dr;
                    case AppMsgType.Warning:
                        dr = MessageBox.Show(strerrormsg, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return dr;
                    case AppMsgType.Exception:
                        dr = MessageBox.Show(strerrormsg, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return dr;
                    case AppMsgType.Stop:
                        dr = MessageBox.Show(strerrormsg, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return dr;
                    case AppMsgType.Confirmation:
                        dr = MessageBox.Show(strerrormsg, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return dr;
                }

            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message, "Show Message");
            }

            return dr;
        }
    }

    public class clsHL7FileWritter
    {
        public static bool CreateHl7File(string strRequest, string fileName)
        {
            bool FileCrtd = false;
            try
            {
                string path = System.IO.Path.GetDirectoryName(fileName);
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                if (!File.Exists(fileName)) File.Delete(fileName);
                File.AppendAllText(fileName, strRequest);
                FileCrtd = true;
                clsDatabaseLogging.SaveTextLogFile(strRequest);
            }
            catch (System.IO.IOException ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            catch (Exception ex)
            {
                clsEvntvwrLogging.fnLogWritter(ex);
            }
            return  FileCrtd;

        }
    }
}
