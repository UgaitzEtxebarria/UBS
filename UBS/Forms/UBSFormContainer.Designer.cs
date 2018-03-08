namespace UBSApp.Forms
{
    partial class UBSFormContainer
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
            this.controls_panel = new System.Windows.Forms.Panel();
            this.menu_panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // controls_panel
            // 
            this.controls_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controls_panel.BackColor = System.Drawing.SystemColors.Control;
            this.controls_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controls_panel.Location = new System.Drawing.Point(129, 0);
            this.controls_panel.Name = "controls_panel";
            this.controls_panel.Size = new System.Drawing.Size(1165, 600);
            this.controls_panel.TabIndex = 3;
            // 
            // menu_panel
            // 
            this.menu_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.menu_panel.BackColor = System.Drawing.SystemColors.Control;
            this.menu_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.menu_panel.Location = new System.Drawing.Point(0, 0);
            this.menu_panel.Name = "menu_panel";
            this.menu_panel.Size = new System.Drawing.Size(126, 600);
            this.menu_panel.TabIndex = 2;
            // 
            // UBSFormContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(1294, 600);
            this.Controls.Add(this.controls_panel);
            this.Controls.Add(this.menu_panel);
            this.Name = "UBSFormContainer";
            this.Text = "UBSFormContainer";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel controls_panel;
        public System.Windows.Forms.Panel menu_panel;
    }
}