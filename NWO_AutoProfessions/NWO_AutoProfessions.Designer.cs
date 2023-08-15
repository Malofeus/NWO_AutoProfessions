namespace NWO_AutoProfessions
{
    partial class NWO_AutoProfession_Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NWO_AutoProfession_Form1));
            //this.NWO_WebBrowser = new Gecko.GeckoWebBrowser();
            this.NWO_StatusStrip = new System.Windows.Forms.StatusStrip();
            this.NWO_URL_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.NWO_Timer_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ShrinkExpandForm_Button = new System.Windows.Forms.Button();
            this.NWO_Start_Button = new System.Windows.Forms.Button();
            this.NWO_Status_Textbox = new System.Windows.Forms.TextBox();
            this.NWO_Config_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.NWO_TabControl = new System.Windows.Forms.TabControl();
            this.label1 = new System.Windows.Forms.Label();
            this.NWO_ProfessionConfig_Button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this._NWO_NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NWO_StatusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NWO_WebBrowser
            // 
            //this.NWO_WebBrowser.Location = new System.Drawing.Point(0, 0);
            //this.NWO_WebBrowser.Name = "NWO_WebBrowser";
            //this.NWO_WebBrowser.Size = new System.Drawing.Size(555, 877);
            //this.NWO_WebBrowser.TabIndex = 0;
            //this.NWO_WebBrowser.UseHttpActivityObserver = false;
            //this.NWO_WebBrowser.DocumentCompleted += new System.EventHandler(this.NWO_WebBrowser_DocumentCompleted);
            // 
            // NWO_StatusStrip
            // 
            this.NWO_StatusStrip.AutoSize = false;
            this.NWO_StatusStrip.BackColor = System.Drawing.SystemColors.MenuText;
            this.NWO_StatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.NWO_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NWO_URL_StatusLabel,
            this.NWO_Timer_StatusLabel});
            this.NWO_StatusStrip.Location = new System.Drawing.Point(0, 880);
            this.NWO_StatusStrip.Name = "NWO_StatusStrip";
            this.NWO_StatusStrip.Size = new System.Drawing.Size(446, 22);
            this.NWO_StatusStrip.SizingGrip = false;
            this.NWO_StatusStrip.TabIndex = 1;
            // 
            // NWO_URL_StatusLabel
            // 
            this.NWO_URL_StatusLabel.BackColor = System.Drawing.SystemColors.WindowText;
            this.NWO_URL_StatusLabel.ForeColor = System.Drawing.Color.Gold;
            this.NWO_URL_StatusLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.NWO_URL_StatusLabel.Name = "NWO_URL_StatusLabel";
            this.NWO_URL_StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.NWO_URL_StatusLabel.Size = new System.Drawing.Size(127, 17);
            this.NWO_URL_StatusLabel.Text = "NWO_URL_StatusLabel";
            // 
            // NWO_Timer_StatusLabel
            // 
            this.NWO_Timer_StatusLabel.BackColor = System.Drawing.SystemColors.MenuText;
            this.NWO_Timer_StatusLabel.ForeColor = System.Drawing.Color.Gold;
            this.NWO_Timer_StatusLabel.Name = "NWO_Timer_StatusLabel";
            this.NWO_Timer_StatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.NWO_Timer_StatusLabel.Size = new System.Drawing.Size(137, 17);
            this.NWO_Timer_StatusLabel.Text = "NWO_Timer_StatusLabel";
            // 
            // ShrinkExpandForm_Button
            // 
            this.ShrinkExpandForm_Button.Location = new System.Drawing.Point(554, 0);
            this.ShrinkExpandForm_Button.Name = "ShrinkExpandForm_Button";
            this.ShrinkExpandForm_Button.Size = new System.Drawing.Size(14, 23);
            this.ShrinkExpandForm_Button.TabIndex = 2;
            this.ShrinkExpandForm_Button.Text = "<";
            this.ShrinkExpandForm_Button.UseVisualStyleBackColor = true;
            this.ShrinkExpandForm_Button.Click += new System.EventHandler(this.ShrinkExpandForm_Button_Click);
            // 
            // NWO_Start_Button
            // 
            this.NWO_Start_Button.BackColor = System.Drawing.Color.Lime;
            this.NWO_Start_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NWO_Start_Button.ForeColor = System.Drawing.Color.Black;
            this.NWO_Start_Button.Location = new System.Drawing.Point(449, 879);
            this.NWO_Start_Button.Name = "NWO_Start_Button";
            this.NWO_Start_Button.Size = new System.Drawing.Size(106, 23);
            this.NWO_Start_Button.TabIndex = 3;
            this.NWO_Start_Button.Text = "Start";
            this.NWO_Start_Button.UseVisualStyleBackColor = false;
            this.NWO_Start_Button.Click += new System.EventHandler(this.NWO_Start_Button_Click);
            // 
            // NWO_Status_Textbox
            // 
            this.NWO_Status_Textbox.AcceptsReturn = true;
            this.NWO_Status_Textbox.AcceptsTab = true;
            this.NWO_Status_Textbox.BackColor = System.Drawing.SystemColors.Window;
            this.NWO_Status_Textbox.Location = new System.Drawing.Point(573, 3);
            this.NWO_Status_Textbox.Multiline = true;
            this.NWO_Status_Textbox.Name = "NWO_Status_Textbox";
            this.NWO_Status_Textbox.ReadOnly = true;
            this.NWO_Status_Textbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.NWO_Status_Textbox.Size = new System.Drawing.Size(270, 506);
            this.NWO_Status_Textbox.TabIndex = 4;
            this.NWO_Status_Textbox.TabStop = false;
            // 
            // NWO_Config_Button
            // 
            this.NWO_Config_Button.Location = new System.Drawing.Point(573, 514);
            this.NWO_Config_Button.Name = "NWO_Config_Button";
            this.NWO_Config_Button.Size = new System.Drawing.Size(52, 23);
            this.NWO_Config_Button.TabIndex = 6;
            this.NWO_Config_Button.Text = "Config";
            this.NWO_Config_Button.UseVisualStyleBackColor = true;
            this.NWO_Config_Button.Click += new System.EventHandler(this.NWO_Config_Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gray;
            this.groupBox1.Controls.Add(this.NWO_TabControl);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(573, 543);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 359);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statistics";
            // 
            // NWO_TabControl
            // 
            this.NWO_TabControl.Location = new System.Drawing.Point(6, 19);
            this.NWO_TabControl.Name = "NWO_TabControl";
            this.NWO_TabControl.SelectedIndex = 0;
            this.NWO_TabControl.Size = new System.Drawing.Size(264, 334);
            this.NWO_TabControl.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Slot 1 Completed:";
            // 
            // NWO_ProfessionConfig_Button
            // 
            this.NWO_ProfessionConfig_Button.Location = new System.Drawing.Point(714, 514);
            this.NWO_ProfessionConfig_Button.Name = "NWO_ProfessionConfig_Button";
            this.NWO_ProfessionConfig_Button.Size = new System.Drawing.Size(129, 23);
            this.NWO_ProfessionConfig_Button.TabIndex = 8;
            this.NWO_ProfessionConfig_Button.Text = "Profession Config";
            this.NWO_ProfessionConfig_Button.UseVisualStyleBackColor = true;
            this.NWO_ProfessionConfig_Button.Click += new System.EventHandler(this.NWO_ProfessionConfig_Button_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(631, 514);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _NWO_NotifyIcon
            // 
            this._NWO_NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("_NWO_NotifyIcon.Icon")));
            this._NWO_NotifyIcon.Text = "NWO AutoProfessions";
            this._NWO_NotifyIcon.Visible = true;
            // 
            // NWO_AutoProfession_Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuText;
            this.ClientSize = new System.Drawing.Size(844, 902);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.NWO_ProfessionConfig_Button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.NWO_Config_Button);
            this.Controls.Add(this.NWO_Status_Textbox);
            this.Controls.Add(this.NWO_Start_Button);
            this.Controls.Add(this.ShrinkExpandForm_Button);
            this.Controls.Add(this.NWO_StatusStrip);
            //this.Controls.Add(this.NWO_WebBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "NWO_AutoProfession_Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NWO AutoProfessions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NWO_AutoProfession_Form1_FormClosing);
            this.SizeChanged += new System.EventHandler(this.NWO_AutoProfession_Form1_SizeChanged);
            this.NWO_StatusStrip.ResumeLayout(false);
            this.NWO_StatusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private Gecko.GeckoWebBrowser NWO_WebBrowser;
        private System.Windows.Forms.StatusStrip NWO_StatusStrip;
        private System.Windows.Forms.Button ShrinkExpandForm_Button;
        private System.Windows.Forms.ToolStripStatusLabel NWO_URL_StatusLabel;
        private System.Windows.Forms.Button NWO_Start_Button;
        private System.Windows.Forms.TextBox NWO_Status_Textbox;
        private System.Windows.Forms.Button NWO_Config_Button;
        private System.Windows.Forms.ToolStripStatusLabel NWO_Timer_StatusLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button NWO_ProfessionConfig_Button;
        private System.Windows.Forms.TabControl NWO_TabControl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NotifyIcon _NWO_NotifyIcon;

    }
}

