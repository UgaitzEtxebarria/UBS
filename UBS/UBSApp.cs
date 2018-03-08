using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using UBSApp.Managers.CommunicationManager;
using UBSApp.Forms;
using UBSApp.Managers.ModuleManager;
using UBSApp.Managers.ConfigManager;
using UBSLib;

namespace UBSApp
{
    public class UBSApp : UBSAppComponent
    {
        ///////////////////////////////////////////////////////////
        private UBSFormContainer form_container;
        private UBSModuleManager module_manager;
        private CommunicationManager communication_manager;

        private string config_filename = "config.xml";
        private UBSAppComponentFunctions module_functions;

        ///////////////////////////////////////////////////////////
        public UBSApp(DateTime fechaInicio) : base("root")
        {
            // Init Global config manager
            GlobalConfigManager.Init();

            //Cargar el modo debug
            try
            {
                if (File.Exists("Debug.txt"))
                    GlobalConfigManager.SetParameter("Debug", Convert.ToBoolean(File.ReadAllText("Debug.txt")));
                else
                    GlobalConfigManager.SetParameter("Debug", false);
            }
            catch (Exception e)
            {
                Error("Error al abrir el archivo del modo depuración. Revisa el fichero Debug.txt, debe de contener únicamente el valor \"true\" o \"false\". " + e.Message, true);
                GlobalConfigManager.SetParameter("Debug", false);
            }

            //[Debug] Cargar el escritor de tiempos de ejecución.
            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
            {
                UBSExecutionTimeLogger.Instance.Init(fechaInicio, "", "ExecutionTime.log", true);
                WriteExecutionTime("Iniciacion del programa ", fechaInicio);
            }
            #endregion

            // Get UBS version
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string UBSversion = fvi.FileVersion;

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Version del UBS cargada");
            #endregion

            // Init logger
            UBSLogger.Instance.Init("UBS " + UBSversion, "UBS.log", Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")));

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Logger inicializado");
            #endregion

            // Load config
            AppConfig app_config;
            AppConfigLoader loader = new AppConfigLoader(config_filename);
            if (!loader.Load(out app_config))
                Error("Error en la carga del fichero de configuración. [" + config_filename + "]", true);

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Configuracion del UBS cargada");
            #endregion

            //Save dummy Config
            /*app_config = new config.AppConfig();
            app_config.isMinimized = false;
            app_config.AppName = "UBS Dummy";
            app_config.AppSize = new System.Drawing.Size(600, 800);
            System.Collections.Generic.Dictionary<string, config.ModuleInfo> aux = new System.Collections.Generic.Dictionary<string, config.ModuleInfo>();
            config.ModuleInfoLocal mi = new config.ModuleInfoLocal();
            mi.Id = "LocMod";
            mi.Name = "LocalModule";
            mi.Filename = "LocMod.dll";
            mi.ModuleType = "modules.UBSPageModule";
            aux.Add(mi.Id, mi);
            mi.Id += 1;
            aux.Add(mi.Id + 1, mi);
            config.ModuleInfoRemote miRemoto = new config.ModuleInfoRemote();
            miRemoto.Id = "RemMod";
            miRemoto.Name = "RemoteModule";
            miRemoto.Filename = "RemMod.dll";
            miRemoto.ModuleType = "modules.UBSPageModule";
            miRemoto.Ip = "192.168.10.10";
            aux.Add(miRemoto.Id, miRemoto);
            miRemoto.Id += 2;
            aux.Add(miRemoto.Id + 2, miRemoto);
            app_config.Modules = aux;
            loader.Save(app_config);*/

            // Load modules
            module_manager = new UBSModuleManager(app_config);
            if (!module_manager.LoadModules())
                Error("Algunos módulos no se han podido cargar correctamente", true);

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Módulos cargados");
            #endregion

            // Load communications
            // each handle message in a thread
            communication_manager = new CommunicationManager(app_config);
            communication_manager.EnableLocalModules(module_manager.GetLocalModules());

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Comunicaciones cargadas");
            #endregion

            // Load Container Form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form_container = new UBSFormContainer();
            form_container.Text = "UBS " + UBSversion + " :: " + app_config.AppName;

            //Set the Form Size
            if (app_config.AppSize.Height == 0 && app_config.AppSize.Height == 0)// Full screen
            {
                //Si el modo debug está activo que la pantalla no sea completa del todo
                if (!Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                {
                    form_container.FormBorderStyle = FormBorderStyle.None;
                    form_container.WindowState = FormWindowState.Maximized;
                }
                else
                    form_container.Scale(new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            }
            else
                form_container.Scale(new System.Drawing.Size(app_config.AppSize.Width, app_config.AppSize.Height));

            if (app_config.isMinimized)
                form_container.WindowState = FormWindowState.Minimized;

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Formularios cargados");
            #endregion

            // Append ConsoleWrite delegate
            UBSStatusFunctions.Instance.SetConsoleLoggerDelegate(form_container.WriteConsole);
            UBSStatusFunctions.Instance.SetModuleErrorDelegate(form_container.ModuleError);
            UBSStatusFunctions.Instance.SetModuleConnectionDelegate(form_container.ModuleConnection);

            // Append delegates
            module_functions = new UBSAppComponentFunctions();
            UBSModuleDelegates module_delegates = new UBSModuleDelegates
                (
                communication_manager.SendMessage,
                module_manager.GetAvailableModuleNames,
                module_manager.GetLocalModules,
                module_functions.Log,
                module_functions.WriteConsole,
                module_functions.Notify,
                module_functions.Error,
                module_functions.WriteExecutionTime,
                GlobalConfigManager.SetParameter,
                GlobalConfigManager.GetParameter,
                form_container.GoToModule,
                form_container.ButtonColor
                );

            module_manager.AppendDelegates(module_delegates);

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Delegados asociados");
            #endregion

            // Initialization of global parameters
            GlobalConfigManager.SetParameter("Communication", "message");

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Parametros globales cargados");
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("UBS Constructor OK");
            #endregion

        }

        ///////////////////////////////////////////////////////////
        public bool Init()
        {
            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Inicio de la inicialización del UBS");
            #endregion

            // Add module data to status
            foreach (string module_id in module_manager.GetAvailableModuleNames().Keys)
                form_container.AddModuleToStatus(module_id, module_manager.GetAvailableModuleNames()[module_id]);

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Añadidos los módulos al módulo Status");
            #endregion

            // Init modules and attach them to the view
            if (!module_manager.InitModules(this.form_container.controls_panel.Size))
                Error("Algunos módulos no se han podido iniciar correctamente.", true);
            form_container.AttachModules(module_manager.GetLocalModules());

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Módulos inicializados");
            #endregion

            // Init communications
            if (!communication_manager.Init())
                Error("La counicación no se ha podido iniciar correctamente.", true);

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Comunicaciones inicializadas");
            #endregion

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("UBS Inicialización OK");
            #endregion

            return true;
        }

        ///////////////////////////////////////////////////////////
        public bool Destroy()
        {
            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Programa cerrándose");
            #endregion

            //Destroy all modules
            if (!module_manager.DestroyModules())
            {
                Error("Algunos módulos no se han podido destruir correctamente.", true);
                return false;
            }

            #region ExecutionTime
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Programa cerrado.");
            #endregion

            Log("Todos los módulos cerrados correctamente.");

            UBSExecutionTimeLogger.Instance.Close();
            UBSLogger.Instance.Close();

            return true;
        }
        ///////////////////////////////////////////////////////////
        public void RunApp()
        {
            #region ExecutionTime
            //[Debug] Guardar en el fichero el tiempo tardado en cargar e inicializar los modulos. 
            if (Convert.ToBoolean(GlobalConfigManager.GetParameter("Debug")))
                WriteExecutionTime("Carga e inicialización finalizadas.");
            #endregion

            Log("Carga e inicialización finalizadas.");
            try
            {
                Application.Run(form_container);
            }
            catch (Exception e)
            {
                Error("Error general del UBS. " + e.Message, true, false);
            }
        }

        ///////////////////////////////////////////////////////////

    }
}
