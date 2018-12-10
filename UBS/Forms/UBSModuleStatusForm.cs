using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using UBS.Auxiliars;

namespace UBSApp.Forms
{
    public partial class UBSModuleStatusForm : Form
    {
        public delegate void WriteConsoleDelegate(string id, string message);
        ///////////////////////////////////////////////////////////

        private Dictionary<string, UBSModuleStatusControl> statusControls;

        ///////////////////////////////////////////////////////////
        public UBSModuleStatusForm()
        {
            InitializeComponent();
            statusControls = new Dictionary<string, UBSModuleStatusControl>();
        }

        ///////////////////////////////////////////////////////////
        public void AddModule(string _id, string _name)
        {
            UBSModuleStatusControl status_control = new UBSModuleStatusControl(_name);
            status_control.Parent = groupModules;
            status_control.Location = new Point(5, 25 + ((status_control.Height + 10) * statusControls.Count));
            groupModules.Controls.Add(status_control);
            statusControls.Add(_id, status_control);
        }

        ///////////////////////////////////////////////////////////
        public void SetModuleError(string id, bool hasError)
        {
            UBSModuleStatusControl statuscontrol;
            if (statusControls.TryGetValue(id, out statuscontrol))
                statuscontrol.SetHasErrorsStatus(true);
        }

        ///////////////////////////////////////////////////////////
        public void SetModuleConnection(string id, bool isConnected)
        {
            UBSModuleStatusControl statuscontrol;
            if (statusControls.TryGetValue(id, out statuscontrol))
                statuscontrol.SetIsConnectedStatus(true);
        }

        ///////////////////////////////////////////////////////////
        public void WriteConsole(string id, string message)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (txtConsole.InvokeRequired)
                        txtConsole.Invoke(new WriteConsoleDelegate(WriteConsole), new object[] { id, message });  // invoking itself
                    else
                    {
                        string built_message = string.Format("[{0}]-[{1}]::{2}", DateTime.Now.ToString("HH:mm:ss.fff"), id, message);
                        if (IsDisposed)
                            return;
                        Size ConsoleSize = txtConsole.Size;
                        Font ConsoleFont = txtConsole.Font;
                        string completeText = "";
                        if (txtConsole.Text == "")
                            completeText = built_message;
                        else
                            completeText = string.Format("{0}" + Environment.NewLine + "{1}", built_message, txtConsole.Text);

                        int visibleChars = 0;
                        int visibleLines = 0;

                        string textoValido = completeText;

                        if (IsDisposed)
                            return;
                        CreateGraphics().MeasureString(completeText, ConsoleFont, new SizeF(ConsoleSize.Width, ConsoleSize.Height), new StringFormat(StringFormatFlags.FitBlackBox), out visibleChars, out visibleLines);
                        int maxLineas = (int)(ConsoleSize.Height / (2.6 + ConsoleFont.Size + 2.6));

                        while (visibleLines >= maxLineas && completeText.Contains("\r\n"))
                        {
                            textoValido = completeText;

                            completeText = completeText.Substring(0, completeText.LastIndexOf("\r\n"));
                            if (IsDisposed)
                                return;
                            CreateGraphics().MeasureString(completeText, ConsoleFont, new SizeF(ConsoleSize.Width, ConsoleSize.Height), new StringFormat(StringFormatFlags.FitBlackBox), out visibleChars, out visibleLines);
                        }

                        if (!completeText.Contains("\r\n"))
                            textoValido = completeText;

                        if (IsDisposed)
                            return;
                        txtConsole.Text = textoValido;
                    }
                }
            }
            catch (Exception e)
            {
                StackTrace sTrace = new StackTrace(e, true);
                StackFrame sFrame = sTrace.GetFrame(sTrace.FrameCount - 1);
                int line = sFrame.GetFileLineNumber();
                MessageBox.Show("Error interno del UBS. WriteConsole bloqueado. " + e.Message + " -> linea " + line);
            }
        }

        ///////////////////////////////////////////////////////////

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                WriteConsole("Updater", "No hay actualizaciones");
                Updater.FindUpdate();
            }
            catch (Exception ex)
            {
                StackTrace sTrace = new StackTrace(ex, true);
                StackFrame sFrame = sTrace.GetFrame(sTrace.FrameCount - 1);
                int line = sFrame.GetFileLineNumber();
                MessageBox.Show("Error interno del UBS. Error al actualizar. " + ex.Message + " -> linea " + line);
            }
            ///////////////////////////////////////////////////////////
        }
    }
}
