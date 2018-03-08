using System;
using System.Threading;
using System.Windows.Forms;

namespace UBSApp
{
    public class UBSAppComponentFunctions
    {
        ///////////////////////////////////////////////////////////
        public UBSAppComponentFunctions()
        {

        }

        ///////////////////////////////////////////////////////////
        public void Log(string id, string message)
        {
            UBSLogger.Instance.Log(id, message);
        }

        ///////////////////////////////////////////////////////////
        public void Notify(string id, string message, bool writeInLog = false, bool isModal = true)
        {
            if (!isModal)
            {
                Thread notifyThread = new Thread(() => AuxNotify(id, message, writeInLog));
                notifyThread.Name = "CSRNotifyThread";
                notifyThread.IsBackground = true;
                notifyThread.Start();
            }
            else
                AuxNotify(id, message, writeInLog);
        }

        ///////////////////////////////////////////////////////////

        private void AuxNotify(string id, string message, bool writeInLog = false)
        {
            if (writeInLog)
                Log(id, message);
            MessageBox.Show(id + "::" + message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
        }

        ///////////////////////////////////////////////////////////
        public void Error(string id, string message, bool writeInLog = false, bool isModal = true)
        {
            if (!isModal)
            {
                Thread ErrorThread = new Thread(() => AuxError(id, message, writeInLog));
                ErrorThread.Name = "CSRErrorThread";
                ErrorThread.IsBackground = true;
                ErrorThread.Start();
            }
            else
                AuxError(id, message, writeInLog);
        }

        ///////////////////////////////////////////////////////////
        public void AuxError(string id, string message, bool writeInLog = false)
        {
            UBSStatusFunctions.Instance.SetModuleError(id);
            if (writeInLog)
                Log(id, "Error -> " + message);
            MessageBox.Show(id + "::" + message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        ///////////////////////////////////////////////////////////
        public void WriteConsole(string id, string message, bool write_on_log = false)
        {
            UBSStatusFunctions.Instance.ConsoleLog(id, message);

            if (write_on_log)
                Log(id, message);
        }

        ///////////////////////////////////////////////////////////

        public void WriteExecutionTime(string id, string message, DateTime time)
        {
            UBSExecutionTimeLogger.Instance.WriteExecutionTime(id, message, time);
        }

        ///////////////////////////////////////////////////////////
    }
}
