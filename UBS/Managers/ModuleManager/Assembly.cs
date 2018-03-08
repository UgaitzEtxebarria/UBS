using System;

namespace UBSApp.Managers.ModuleManager
{
    #region AssemblyInfo Class
    public sealed class AssemblyInfo
    {
        private string strID;
        private string strPath;

        public string ID { get { return strID; } }
        public string Path { get { return strPath; } }

        public AssemblyInfo(string id, string path)
        {
            strID = id;
            strPath = path;
        }
    }
    #endregion

    #region Assembly
    public sealed class Assembly<T>
    {
        private AssemblyInfo oAssemblyInfo;
        private bool bHasType;
        private bool bLoaded;
        private bool bHasErrors;
        private bool bIsAbstract;
        private Type oFormType;

        System.AppDomain oDomain;
        System.Reflection.Assembly oAssembly;


        public Assembly(AssemblyInfo assembly_info)
        {
            oAssemblyInfo = new AssemblyInfo(assembly_info.ID, assembly_info.Path);
            oDomain = null;
            oAssembly = null;
            bLoaded = false;
            bHasErrors = false;
            bHasType = false;
        }

        public bool Load()
        {
            try
            {
                oDomain = AppDomain.CreateDomain(oAssemblyInfo.ID);
                oAssembly = System.Reflection.Assembly.LoadFrom(oAssemblyInfo.Path);
                bLoaded = true;

                return TypeTest(); // Testear si tenemos clases del tipo requerido     
            }
            catch (Exception e)
            {
                bHasErrors = true;
                bLoaded = false;
                return false;
            }
        }

        public bool HasErrors { get { return bHasErrors; } }

        public bool IsLoaded { get { return bLoaded; } }

        public bool HasType { get { return bHasType; } }

        public bool IsAbstract { get { return bLoaded; } }

        public T CreateInstance()
        {
            try
            {
                if (!bHasErrors && bLoaded && bHasType)
                    return (T)Activator.CreateInstance(oFormType);
                else
                    return default(T);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public T CreateInstance(object[] oParams)
        {
            try
            {
                if (!bHasErrors && bLoaded && bHasType)
                    return (T)Activator.CreateInstance(oFormType, oParams);
                else
                    return default(T);
            }
            catch (Exception e)
            {
                //return default(T);
                throw e;
            }
        }

        private bool TypeTest()
        {
            bIsAbstract = false;
            bHasType = false;
            if (oAssembly != null && bLoaded && !bHasErrors)
            {
                Type[] aModTypes = oAssembly.GetTypes();
                for (int i = 0; i < aModTypes.Length; i++)
                {
                    if (typeof(T).IsAssignableFrom(aModTypes[i]))
                    {
                        if (!aModTypes[i].IsAbstract)
                        {
                            bHasType = true;
                            oFormType = aModTypes[i];
                            break;
                        }
                        else
                            bIsAbstract = true;
                    }
                }
            }

            return bHasType;
        }
    }

    #endregion

    #region AssemblyResolver Static Class

    public static class AssemblyResolver
    {
        public static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            System.Reflection.Assembly ayResult = null;
            string sShortAssemblyName = args.Name.Split(',')[0];
            System.Reflection.Assembly[] ayAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (System.Reflection.Assembly ayAssembly in ayAssemblies)
            {
                if (sShortAssemblyName == ayAssembly.FullName.Split(',')[0])
                {
                    ayResult = ayAssembly;
                    break;
                }
            }
            return ayResult;
        }
    }

    #endregion

}