using System;
using System.Collections.Generic;
using UBSApp.Managers.ConfigManager;
using UBSApp.Carga;

namespace UBSApp.Managers.ModuleManager
{
    public class UBSModuleManager : UBSAppComponent
    {
        ///////////////////////////////////////////////////////////
        private AppConfig appConfig;
        private Dictionary<string, string> available_module_names;
        private Dictionary<string, UBSLib.UBSModule> available_local_modules;

        ///////////////////////////////////////////////////////////
        public UBSModuleManager(AppConfig _config) : base("ModuleManager")
        {
            appConfig = _config;
        }

        ///////////////////////////////////////////////////////////
        public bool LoadModules()
        {
            bool errors_found = false;

            // Load modules
            available_module_names = new Dictionary<string, string>();
            available_local_modules = new Dictionary<string, UBSLib.UBSModule>();

            int ind = 0;
            foreach (KeyValuePair<string, UBSLib.ModuleInfo> kvp in appConfig.Modules)
            {
                if (kvp.Key != kvp.Value.Id)
                    Error("Fichero de configuración mal configurado. El campo <a:Key> debe de ser igual que su identificador.", true);

                ind++;

                if (!available_module_names.ContainsKey(kvp.Key))
                {
                    UBSCarga.Status = "Loading " + kvp.Value.Name + " module";
                    Log("Loading " + kvp.Value.Name + " module.");
                    available_module_names.Add(kvp.Key, kvp.Value.Name);

                    //Load Local module
                    if (kvp.Value is UBSLib.ModuleInfoLocal)
                    {
                        AssemblyInfo assembly_info = new AssemblyInfo(kvp.Key, ((UBSLib.ModuleInfoLocal)kvp.Value).Filename);
                        Assembly<UBSLib.UBSModule> assembly = new Assembly<UBSLib.UBSModule>(assembly_info);
                        if (!assembly.Load())
                        {
                            if (assembly.IsAbstract)
                                Error("Se ha cargado un módulo del UBS que necesita un programa especifico. Crear una clase que herede del módulo " + kvp.Value + ".", true);
                            else
                                Error("No se ha podido cargar el módulo \"" + kvp.Key + "\"!\nCompruebe que la ruta a la librería es correcta y que contiene algún tipo de módulo UBSModule o derivado.", true);

                            errors_found = true;
                        }
                        else
                        {
                            try
                            {
                                //Create instance of the module and fill it.
                                available_local_modules.Add(kvp.Key, assembly.CreateInstance(new object[] { kvp.Key }));
                                available_local_modules[kvp.Key].Name = kvp.Value.Name;

                                Log("Módulo " + kvp.Value.Name + " cargado correctamente.");
                            }
                            catch (Exception e)
                            {
                                Error(String.Format("No se ha podido cargar el modulo \"{0}\"!\nCompruebe que no hay errores en su constructor.\nExceptionMessage: {1}\nInnerMessage: {2}", kvp.Key, e.Message, e.InnerException), true);
                                errors_found = true;
                            }
                        }
                    }

                    else if (kvp.Value is UBSLib.ModuleInfoRemote)
                    {
                        new NotImplementedException("Servicio aún no disponible");
                    }

                    #region ExecutionTime
                    if (available_local_modules.ContainsKey(kvp.Key) && Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                        WriteExecutionTime("Módulo " + kvp.Value.Name + " cargado.");
                    #endregion

                    UBSCarga.Progress = Convert.ToInt32((((float)ind) / ((float)appConfig.Modules.Count)) * 50);
                }
                else
                    Error("Hay 2 módulos del UBS con el identificador " + kvp.Key, true);
            }

            // Set Names to modules
            //SetModuleNames();

            return !errors_found;
        }

        ///////////////////////////////////////////////////////////
        public bool InitModules(System.Drawing.Size size)
        {
            bool errors_found = false;
            int ind = 0;
            foreach (UBSLib.UBSModule module in available_local_modules.Values)
            {
                try
                {
                    ind++;
                    UBSCarga.Status = "Initializing " + module.Id + " module";
                    Log("Initializing " + module.Id + " module");
                    if (module is UBSLib.UBSVisualModule)
                    {
                        if (!(module as UBSLib.UBSVisualModule).Init())
                        {
                            errors_found = true;
                            throw new Exception("FormModule init exception");
                        }
                    }
                    else if (module is UBSLib.UBSBackgroundModule)
                    {
                        if (!(module as UBSLib.UBSBackgroundModule).Init())
                        {
                            errors_found = true;
                            throw new Exception("BackgroundModule init exception");
                        }
                    }
                    else
                    {
                        if (!module.Init())
                        {
                            errors_found = true;
                            throw new Exception("Module init exception");
                        }
                    }

                    #region ExecutionTime
                    if (Convert.ToBoolean(module.GetGlobalParameter("Debug")))
                        WriteExecutionTime("Módulo " + module.Name + " inizializado.");
                    #endregion

                    UBSStatusFunctions.Instance.SetModuleConnection(module.Id, true);

                    WriteConsole("Módulo " + module.Name + " inicializado correctamente.", true);

                    UBSCarga.Progress = 50 + Convert.ToInt32((((float)ind) / ((float)appConfig.Modules.Count)) * 50);
                }
                catch (Exception e)
                {
                    Error(String.Format("Ocurrió un error durante la inicialización del módulo \"{0}\".\n{1}", module.Id, e.ToString()), true);
                }
            }
            return !errors_found;
        }

        ///////////////////////////////////////////////////////////
        public bool DestroyModules()
        {
            bool errors_found = false;
            foreach (UBSLib.UBSModule module in available_local_modules.Values)
                module.Status = UBSLib.UBSModuleStatus.Closing;
            foreach (UBSLib.UBSModule module in available_local_modules.Values)
            {
                try
                {
                    if (module is UBSLib.UBSVisualModule)
                    {
                        if (!(module as UBSLib.UBSVisualModule).Destroy())
                        {
                            errors_found = true;
                            Error("El módulo " + module.Name + " no se ha podido cerrar correctamente.", true);
                        }
                        else
                            Log("El módulo " + module.Name + " se ha cerrado correctamente.");
                    }
                    else if (module is UBSLib.UBSBackgroundModule)
                    {
                        if (!(module as UBSLib.UBSBackgroundModule).Destroy())
                        {
                            errors_found = true;
                            Error("El módulo " + module.Name + " no se ha podido cerrar correctamente.", true);
                        }
                        else
                            Log("El módulo " + module.Name + " se ha cerrado correctamente.");
                    }
                    else
                    {
                        if (!module.Destroy())
                        {
                            errors_found = true;
                            Error("El módulo " + module.Name + " no se ha podido cerrar correctamente.", true);
                        }
                        else
                            Log("El módulo " + module.Name + " se ha cerrado correctamente.");
                    }

                    #region ExecutionTime
                    if (Convert.ToBoolean(module.GetGlobalParameter("Debug")))
                        WriteExecutionTime("Módulo " + module.Name + " cerrado.");
                    #endregion
                }
                catch (Exception e)
                {
                    Error(String.Format("Ocurrió un error durante la destrucción del módulo \"{0}\".\n{1}", module.Id, e.ToString()), true);
                }
            }
            return !errors_found;
        }

        ///////////////////////////////////////////////////////////
        public void AppendDelegates(UBSLib.UBSModuleDelegates module_delegates)
        {
            foreach (UBSLib.UBSModule local_module in available_local_modules.Values)
                local_module.AppendDelegates(module_delegates);
        }

        ///////////////////////////////////////////////////////////
        public Dictionary<string, string> GetAvailableModuleNames()
        {
            return available_module_names;
        }

        ///////////////////////////////////////////////////////////
        public Dictionary<string, UBSLib.UBSModule> GetLocalModules()
        {
            return available_local_modules;
        }

        ///////////////////////////////////////////////////////////
        private void SetModuleNames()
        {
            foreach (string module_id in available_local_modules.Keys) { }

        }
        ///////////////////////////////////////////////////////////
    }
}
