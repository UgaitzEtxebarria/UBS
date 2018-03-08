using System;
using System.IO;
using System.Runtime.Serialization;

namespace UBSApp.Managers.ConfigManager
{
    public class AppConfigLoader
    {
        ///////////////////////////////////////////////////////////
        private string m_filename;
        private DataContractSerializer xml_serializer;

        ///////////////////////////////////////////////////////////
        public AppConfigLoader(string _filename)
        {
            m_filename = _filename;
            xml_serializer = new DataContractSerializer(typeof(AppConfig));
        }

        ///////////////////////////////////////////////////////////
        public bool Load(out AppConfig _config)
        {
            try
            {
                FileStream filestream = new FileStream(m_filename, FileMode.Open);
                _config = (AppConfig)xml_serializer.ReadObject(filestream);
                filestream.Close();

                return true;
            }
            catch (Exception e)
            {
                _config = null;
                return false;
            }
        }

        ///////////////////////////////////////////////////////////
        public bool Save(AppConfig _config)
        {
            try
            {
                FileStream filestream = new FileStream(m_filename, FileMode.Create);
                xml_serializer.WriteObject(filestream, _config);
                filestream.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        ///////////////////////////////////////////////////////////
    }
}
