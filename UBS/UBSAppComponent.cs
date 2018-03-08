using System;

namespace UBSApp
{
    public class UBSAppComponent
    {
        ///////////////////////////////////////////////////////////
        private string Id { get; set; }
        private UBSAppComponentFunctions Functions { get; set; }

        ///////////////////////////////////////////////////////////
        public UBSAppComponent(string _id)
        {
            Id = _id;
            Functions = new UBSAppComponentFunctions();
        }

        ///////////////////////////////////////////////////////////
        protected void Log(string message)
        {
            Functions.Log(Id, message);
        }

        ///////////////////////////////////////////////////////////
        protected void Notify(string message, bool writeInLog = false, bool isModal = true)
        {
            Functions.Notify(Id, message, writeInLog, isModal);
        }

        ///////////////////////////////////////////////////////////
        protected void Error(string message, bool writeInLog = false, bool isModal = true)
        {
            Functions.Error(Id, message, writeInLog);
        }

        ///////////////////////////////////////////////////////////
        protected void WriteConsole(string message, bool writeInLog = false)
        {
            Functions.WriteConsole(Id, message, writeInLog);
        }

        ///////////////////////////////////////////////////////////

        protected void WriteExecutionTime(string message, DateTime time = new DateTime())
        {
            if (time == DateTime.MinValue)
                time = DateTime.Now;
            Functions.WriteExecutionTime(Id, message, time);
        }

        ///////////////////////////////////////////////////////////
    }
}

