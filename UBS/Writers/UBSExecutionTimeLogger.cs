using System;
using UBSApp.Auxiliars;

namespace UBSApp
{
    public sealed class UBSExecutionTimeLogger : FileWriter
    {

        //////////////////////////////////////////////////////////////
        private static volatile UBSExecutionTimeLogger instance;
        private static object sync = new Object();

        private DateTime startTime = DateTime.MinValue;
        private DateTime prevTime = DateTime.MinValue;

        //////////////////////////////////////////////////////////////
        private UBSExecutionTimeLogger()
        {

        }

        //////////////////////////////////////////////////////////////
        public static UBSExecutionTimeLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                            instance = new UBSExecutionTimeLogger();
                    }
                }
                return instance;
            }
        }

        //////////////////////////////////////////////////////////////
        public bool Init(DateTime startTime, string _app_name = "", string _filename = "", bool _debug = false)
        {
            base.Init("ExecutionTimeLogger", _app_name, _filename, _debug);
            this.startTime = startTime;
            prevTime = startTime;
            return true;
        }

        //////////////////////////////////////////////////////////////
        public bool WriteExecutionTime(string owner, string input, DateTime time)
        {
            string str = "(+" + (time - prevTime) + " / " + (time - startTime).ToString() + ") " + input;
            prevTime = time;

            Write(owner, str);
            return true;
        }
    }

    //////////////////////////////////////////////////////////////
}
