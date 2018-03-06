using System;
using System.Windows.Forms;

namespace UBSApp.Carga
{
    public partial class frmCarga : Form
    {
        private string _StatusInfo;
        private int _progress;
        private bool _shown;

        public frmCarga()
        {
            InitializeComponent();
            Shown += new EventHandler(this.Form_Shown);
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            _shown = true;
        }

        public string StatusInfo
        {
            set
            {
                _StatusInfo = value;
                ChangeStatusText();
            }
            get
            {
                return _StatusInfo;
            }
        }

        public int Progress
        {
            set
            {
                _progress = value;
                ChangeProgress();
            }
            get
            {
                return _progress;
            }
        }

        public bool Loaded
        {
            get
            {
                return this._shown;
            }
        }

        public void ChangeStatusText()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.ChangeStatusText));
                    return;
                }

                lblStatusInfo.Text = _StatusInfo;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al cambiar de estado de la carga. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ChangeProgress()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.ChangeProgress));
                    return;
                }

                progressBar.Value = _progress;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al cambiar el progreso de la carga. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}