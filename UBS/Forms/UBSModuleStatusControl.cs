using System.Drawing;
using System.Windows.Forms;

namespace UBSApp.Forms
{
    public partial class UBSModuleStatusControl : UserControl
    {
        ///////////////////////////////////////////////////////////
        private string name;
        private bool is_local;
        private bool has_errors;
        private bool is_connected;

        ///////////////////////////////////////////////////////////
        public UBSModuleStatusControl(string _name)
        {
            name = _name;
            is_connected = false;
            has_errors = false;

            InitializeComponent();

            UpdateControl();
        }

        ///////////////////////////////////////////////////////////
        private void UpdateControl()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(UpdateControl));
                return;
            }
            else
            {
                txtNombre.Text = name;

                txtConectado.Text = (is_connected) ? "Si" : "No";
                txtConectado.ForeColor = (is_connected) ? Color.ForestGreen : Color.Red;

                txtErrores.Text = (has_errors) ? "Si" : "No";
                txtErrores.ForeColor = (has_errors) ? Color.Red : Color.ForestGreen;
            }
        }

        ///////////////////////////////////////////////////////////
        public void SetIsConnectedStatus(bool status)
        {
            is_connected = status;
            UpdateControl();
        }

        ///////////////////////////////////////////////////////////
        public void SetHasErrorsStatus(bool status)
        {
            has_errors = status;
            UpdateControl();
        }

        ///////////////////////////////////////////////////////////
    }
}
