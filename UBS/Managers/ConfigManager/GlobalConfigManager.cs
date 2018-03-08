
using System;
using System.Collections.Concurrent;

namespace UBSApp.Managers.ConfigManager
{
    public static class GlobalConfigManager
    {
        ///////////////////////////////////////////////////////////
        private static ConcurrentDictionary<string, object> aValues;

        ///////////////////////////////////////////////////////////
        public static void Init()
        {
            aValues = new ConcurrentDictionary<string, object>();
        }

        ///////////////////////////////////////////////////////////
        public static object GetParameter(string strParam)
        {
            try
            {
                object oValue;
                if (aValues.TryGetValue(strParam, out oValue))
                    return oValue;
                else
                    return (int)-1;
            }
            catch (Exception e)
            {
                throw new Exception("Error al conseguir el valor del parametro global. " + e.Message);
            }
        }

        ///////////////////////////////////////////////////////////
        public static void SetParameter(string strParam, object value)
        {
            aValues.AddOrUpdate(strParam, value, (key, oldValue) => value);
        }

        ///////////////////////////////////////////////////////////
    }
}