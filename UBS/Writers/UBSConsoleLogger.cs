using System;

namespace UBSApp
{
    public delegate void ConsoleLoggerDelegate(string id, string message);
    public delegate void ModuleErrorDelegate(string id, bool hasError);
    public delegate void ModuleConnectionDelegate(string id, bool isConnected);

    public sealed class UBSStatusFunctions
    {
        //////////////////////////////////////////////////////////////
        private static volatile UBSStatusFunctions instance;
        private static object sync = new Object();
        private static ConsoleLoggerDelegate console_write_function;
        private static ModuleErrorDelegate module_error_function;
        private static ModuleConnectionDelegate module_connection_function;

        //////////////////////////////////////////////////////////////
        private UBSStatusFunctions()
        {

        }

        //////////////////////////////////////////////////////////////
        public static UBSStatusFunctions Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                            instance = new UBSStatusFunctions();
                    }
                }
                return instance;
            }
        }

        //////////////////////////////////////////////////////////////
        public void SetConsoleLoggerDelegate(ConsoleLoggerDelegate console_logger_function)
        {
            console_write_function = console_logger_function;
        }

        //////////////////////////////////////////////////////////////
        public void SetModuleErrorDelegate(ModuleErrorDelegate _module_error_function)
        {
            module_error_function = _module_error_function;
        }

        //////////////////////////////////////////////////////////////
        public void SetModuleConnectionDelegate(ModuleConnectionDelegate _module_connection_function)
        {
            module_connection_function = _module_connection_function;
        }

        //////////////////////////////////////////////////////////////
        public void ConsoleLog(string id, string message)
        {
            if (console_write_function != null)
                console_write_function(id, message);
        }

        //////////////////////////////////////////////////////////////

        public void SetModuleError(string id)
        {
            if (module_error_function != null)
                module_error_function(id, true);
        }

        //////////////////////////////////////////////////////////////

        public void SetModuleConnection(string id, bool isConnected)
        {
            if (module_connection_function != null)
                module_connection_function(id, isConnected);
        }

        //////////////////////////////////////////////////////////////
    }
}
