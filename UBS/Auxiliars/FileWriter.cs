using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace UBSApp.Auxiliars
{

    public class AuxFileWriter
    {
        public AuxFileWriter(string owner, string input)
        {
            Owner = owner;
            Input = input;

            Date = DateTime.Now.ToString("HH:mm:ss.fff");
        }

        public string Owner { get; }

        public string Input { get; }

        public string Date { get; }
    }

    public class FileWriter
    {
        public string AppName { get; set; }
        public string Filename { get; set; }

        private Thread write_thread;
        private bool Available = true;
        private ConcurrentQueue<AuxFileWriter> queueInputs;

        //////////////////////////////////////////////////////////////
        public FileWriter()
        {
            AppName = "UBS";
            Filename = "FileWriter.log";

            Available = true;

            queueInputs = new ConcurrentQueue<AuxFileWriter>();
        }

        //////////////////////////////////////////////////////////////
        public bool Init(string _use, string _app_name = "", string _filename = "", bool _debug = false)
        {
            if (_app_name != "")
                AppName = _app_name;
            if (_filename != "")
                Filename = _filename;
            if (_debug)
                Filename = Path.GetFileNameWithoutExtension(Filename) + " (" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ")" + Path.GetExtension(Filename);
            if (!Directory.GetDirectories("./").Contains("Logs"))
                Directory.CreateDirectory("./Logs");

            Filename = "Logs/" + Filename;

            FileStream fs = File.Open(Filename, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(String.Format("/////////////////////////////////////////////////"));
            sw.WriteLine(String.Format("/////  {0} :: {1}             ", AppName, _use));
            sw.WriteLine(String.Format("/////////////////////////////////////////////////"));
            sw.WriteLine();
            sw.WriteLine();

            sw.Close();
            fs.Close();

            write_thread = new Thread(ThreadWriteFile);
            write_thread.Name = "UBS " + _use;
            write_thread.IsBackground = true;
            write_thread.Start();

            return true;
        }

        //////////////////////////////////////////////////////////////
        public bool Close()
        {
            Available = false;
            write_thread.Join();
            return true;
        }

        //////////////////////////////////////////////////////////////
        public bool Write(string owner, string input)
        {
            queueInputs.Enqueue(new AuxFileWriter(owner, input));
            return true;
        }

        //////////////////////////////////////////////////////////////
        public void ThreadWriteFile()
        {
            try
            {
                AuxFileWriter aux = null;
                while (Available)
                {
                    if (queueInputs.Count > 0)
                    {
                        FileStream fs = File.Open(Filename, FileMode.Append, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);

                        while (queueInputs.Count > 0)
                        {
                            if (queueInputs.TryDequeue(out aux))
                                sw.WriteLine(String.Format("[{0}-{1}]::{2}", aux.Date, aux.Owner, aux.Input));
                        }

                        sw.Close();
                        fs.Close();
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error interno del UBS. Error en el escritor de ficheros. " + e.Message);
                ThreadWriteFile();
            }
        }
    }
}
