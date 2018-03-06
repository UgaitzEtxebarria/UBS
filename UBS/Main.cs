using System;
using System.Windows.Forms;
using UBSApp.Carga;

namespace UBSApp
{
    static class main
    {
        [STAThread]
        static void Main()
        {
            UBSApp app = new UBSApp(DateTime.Now);

            UBSCarga.Show();
            if (app.Init())
            {
                UBSCarga.Close();
                app.RunApp();
            }
            else
                MessageBox.Show("Ha ocurrido un error durante la ejecución de la función Init de la aplicación.\nLa aplicación no puede continuar la ejecución.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            app.Destroy();
        }
    }
}
