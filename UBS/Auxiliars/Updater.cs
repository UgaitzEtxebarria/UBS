using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
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
                    DirectoryInfo parentDirectory = Directory.GetParent(Application.StartupPath);
                    
                    foreach (string newFiles in Directory.GetFiles(Application.StartupPath))
                        File.Copy(newFiles, parentDirectory + "\\" + Path.GetFileName(newFiles), true);
                    
                    Process.Start(parentDirectory + "\\UBS.exe");

                    CloseApp();
                }
                else
                {
                    if (Directory.Exists(Application.StartupPath + "\\NewVersion"))
                        Directory.Delete(Application.StartupPath + "\\NewVersion", true);
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("E:\\" + DateTime.Now.Ticks + ".txt", "Error " + e.Message);
            }
        }


        static public void FindUpdate()
        {

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    foreach (string rarFile in Directory.EnumerateFiles(drive.RootDirectory.ToString(), "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".rar")))
                    {
                        //Extraer
                        using (var archive = RarArchive.Open(rarFile))
                        {
                            foreach (var exeFiles in archive.Entries.Where(entry => entry.Key == "UBS.exe"))
                            {
                                exeFiles.WriteToDirectory(drive.RootDirectory.ToString(), new ExtractionOptions()
                                {
                                    ExtractFullPath = true,
                                    Overwrite = true
                                });
                                Version newVersion = new Version(FileVersionInfo.GetVersionInfo(drive.RootDirectory + exeFiles.Key).ProductVersion);
                                Version actualVersion = new Version(Application.ProductVersion);

                                File.Delete(drive.RootDirectory + exeFiles.Key);

                                if (newVersion.CompareTo(actualVersion) > 0)
                                {
                                    string newVersionFolder = Path.GetDirectoryName(Application.ExecutablePath) + "\\NewVersion";
                                    Directory.CreateDirectory(newVersionFolder);
                                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                                    {
                                        entry.WriteToDirectory(newVersionFolder, new ExtractionOptions()
                                        {
                                            ExtractFullPath = true,
                                            Overwrite = true
                                        });
                                    }

                                    Process.Start(newVersionFolder + "\\UBS.exe");

                                    CloseApp();
                                }
                            }
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
