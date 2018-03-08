using System;
using UBSApp.Auxiliars;

namespace UBSApp
{
    //////////////////////////////////////////////////////////////
    public sealed class UBSLogger : FileWriter
    {

        //////////////////////////////////////////////////////////////
        private static volatile UBSLogger instance;
        private static object sync = new Object();

        //////////////////////////////////////////////////////////////
        private UBSLogger()
        {

        }

        //////////////////////////////////////////////////////////////
        public static UBSLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                            instance = new UBSLogger();
                    }
                }
                return instance;
            }
        }

        //////////////////////////////////////////////////////////////
        public bool Init(string _app_name = "", string _filename = "", bool _debug = false)
        {
            base.Init("Logger", _app_name, _filename, _debug);
            return true;
        }

        //////////////////////////////////////////////////////////////
        public bool Log(string owner, string input)
        {
            Write(owner, input);
            return true;
        }
    }

    //////////////////////////////////////////////////////////////
}
