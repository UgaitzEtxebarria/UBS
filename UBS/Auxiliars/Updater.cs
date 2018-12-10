using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UBS.Auxiliars
{
    static public class Updater
    {
        static public void InstallUpdate()
        {
            try
            {
                if (Application.StartupPath.EndsWith("NewVersion"))
                {
                    File.WriteAllText(Path.Combine(Application.StartupPath, DateTime.Now.Ticks + ".txt"), "Laura sigues atenta?");
                    DirectoryInfo parentDirectory = Directory.GetParent(Application.StartupPath);
                    
                    foreach (string newFiles in Directory.GetFiles(Application.StartupPath))
                        File.Copy(newFiles, Path.Combine(parentDirectory.ToString(), Path.GetFileName(newFiles)), true);
                    
                    Process.Start(Path.Combine(parentDirectory.ToString(), "UBS.exe"));

                    CloseApp();
                }
                else
                {
                    if (Directory.Exists(Path.Combine(Application.StartupPath, "NewVersion")))
                        Directory.Delete(Path.Combine(Application.StartupPath, "NewVersion"), true);
                }
            }
            catch (Exception e)
            {
                File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "\\Error " + DateTime.Now.Ticks + ".txt", "Error " + e.Message);
            }
        }


        static public void FindUpdate()
        {
            IEnumerable<string> rars = Enumerable.Empty<string>(); ;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    rars = Directory.EnumerateFiles("/media", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".rar"));
                    break;

                default:
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        if (drive.DriveType == DriveType.Removable)
                            rars = Directory.EnumerateFiles(drive.RootDirectory.ToString(), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".rar"));
                    }
                    break;
            }

            //Extraer informacion del Rar para comprobar la version
            foreach (string rarFile in rars)
            {
                using (var archive = RarArchive.Open(rarFile))
                {
                    foreach (var exeFiles in archive.Entries.Where(entry => entry.Key == "UBS.exe"))
                    {
                        string tempPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "temp");
                        Directory.CreateDirectory(tempPath);
                        exeFiles.WriteToDirectory(tempPath, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                        Version newVersion = new Version(FileVersionInfo.GetVersionInfo(Path.Combine(tempPath, exeFiles.Key)).ProductVersion);
                        Version actualVersion = new Version(Application.ProductVersion);

                        Directory.Delete(tempPath, true);

                        if (newVersion.CompareTo(actualVersion) > 0)
                        {
                            string newVersionFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "NewVersion");
                            Directory.CreateDirectory(newVersionFolder);
                            foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                            {
                                entry.WriteToDirectory(newVersionFolder, new ExtractionOptions()
                                {
                                    ExtractFullPath = true,
                                    Overwrite = true
                                });
                            }
                            File.WriteAllText(Path.Combine(Application.StartupPath, DateTime.Now.Ticks + ".txt"), "Ejecutando " + Path.Combine(newVersionFolder, "UBS.exe"));
                            Process.Start(Path.Combine(newVersionFolder, "UBS.exe"));
                            File.WriteAllText(Path.Combine(Application.StartupPath, DateTime.Now.Ticks + ".txt"), "Cerrando");
                            CloseApp();
                        }
                    }
                }
            } 
        }

        private static void CloseApp()
        {
            if (Application.MessageLoop)
            {
                // Use this since we are a WinForms app
                Application.Exit();
            }
            else
            {
                // Use this since we are a console app
                Environment.Exit(1);
            }
        }
    }

}
