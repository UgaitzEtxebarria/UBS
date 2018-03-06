namespace UBSApp.Carga
{
    partial class frmCarga
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatusInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(88, 91);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(458, 23);
            this.progressBar.TabIndex = 4;
            // 
            // lblStatusInfo
            // 
            this.lblStatusInfo.AutoSize = true;
            this.lblStatusInfo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblStatusInfo.Location = new System.Drawing.Point(270, 62);
            this.lblStatusInfo.Name = "lblStatusInfo";
            this.lblStatusInfo.Size = new System.Drawing.Size(104, 13);
            this.lblStatusInfo.TabIndex = 3;
            this.lblStatusInfo.Text = "Loading, please wait";
            // 
            // frmCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 126);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatusInfo);
            this.Name = "frmCarga";
            this.Text = "Cargando";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatusInfo;
    }
}