using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

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
                if (e.HResult == -2147024894) //File not found
                {
                    MessageBox.Show("No se ha encontrado el fichero de configuración del UBSApp. Creando fichero vacío en \"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\" + m_filename + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    CreateConfigDummy();
                }
                else
                    MessageBox.Show("Error al abrir el fichero de configuración del UBSApp \"" + Path.GetDirectoryName(Application.ExecutablePath) + "\\" + m_filename + "\". " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void CreateConfigDummy()
        {
            FileStream fs = File.Open(m_filename, FileMode.Create, FileAccess.ReadWrite);

            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("<AppConfig xmlns=\"http://schemas.datacontract.org/2004/07/UBSApp.Managers.ConfigManager\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">");
            sw.WriteLine("<AppName>Name</AppName>");
            sw.WriteLine("<AppSize xmlns:a=\"http://schemas.datacontract.org/2004/07/System.Drawing\">");
            sw.WriteLine("<a:height>0</a:height>");
            sw.WriteLine("<a:width>0</a:width>");
            sw.WriteLine("</AppSize>");
            sw.WriteLine("<Modules xmlns:a=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">");
            sw.WriteLine("</Modules>");
            sw.WriteLine("<isMinimized> false </isMinimized>");
            sw.WriteLine("</AppConfig> ");
              
            sw.Close();
            fs.Close();
        }
    }
}
