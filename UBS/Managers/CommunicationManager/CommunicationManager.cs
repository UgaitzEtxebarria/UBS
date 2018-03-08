using System;
using System.Collections.Generic;
using UBSApp.Managers.ConfigManager;

namespace UBSApp.Managers.CommunicationManager
{
    public class CommunicationManager : UBSAppComponent
    {
        ///////////////////////////////////////////////////////////
        private AppConfig current_config;
        private Dictionary<string, UBSLib.UBSModule> local_modules;

        ///////////////////////////////////////////////////////////
        public CommunicationManager(AppConfig _config) : base("CommunicationManager")
        {
            local_modules = new Dictionary<string, UBSLib.UBSModule>();
            current_config = _config;
        }

        ///////////////////////////////////////////////////////////
        public bool Init()
        {
            return true;
        }

        ///////////////////////////////////////////////////////////
        public bool SendMessage(string origin, string destiny, string message)
        {
            if (!current_config.Modules.ContainsKey(destiny))
                return false;

            UBSLib.ModuleInfo module_info = current_config.Modules[destiny];

            if (module_info == null)
                return false;

            if (module_info is UBSLib.ModuleInfoLocal)
            {
                // local message
                UBSLib.UBSModule module = local_modules[module_info.Id];
                UBSLib.UBSMessage message_obj = new UBSLib.UBSMessage(destiny, message);
                message_obj.OriginId = origin;
                module.HandleMessages(message_obj);
            }

            else if (module_info is UBSLib.ModuleInfoRemote)
            {
                // do remote call (not implemented)
                // can be done through two bridge tcp/ip client/server modules
                new NotImplementedException("Servicio aun no disponible");
            }

            return true;
        }

        ///////////////////////////////////////////////////////////
        public void EnableLocalModules(Dictionary<string, UBSLib.UBSModule> _local_modules)
        {
            local_modules = _local_modules;
        }

        ///////////////////////////////////////////////////////////
    }
}
