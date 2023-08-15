namespace NWO_AutoProfessions
{
    partial class NWO_Config
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.NWO_Password_Textbox = new System.Windows.Forms.TextBox();
            this.NWO_AutoLogin_CheckBox = new System.Windows.Forms.CheckBox();
            this.NWO_Username_Textbox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.NWO_RemoveCharacter_Button = new System.Windows.Forms.Button();
            this.NWO_AddCharacter_Button = new System.Windows.Forms.Button();
            this.NWO_Character_TextBox = new System.Windows.Forms.TextBox();
            this.NWO_Character_ListView = new System.Windows.Forms.ListBox();
            this.NWO_AccountNick_Textbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.NWO_ForBotting_CheckBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NWO_WaitTimePageLoading_TextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NWO_WaitTimeBetweenLoginAttemts_TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.NWO_DevLogging_CheckBox = new System.Windows.Forms.CheckBox();
            this.NWO_AppendLogFiles_CheckBox = new System.Windows.Forms.CheckBox();
            this.NWO_VerboseLogging_Checkbox = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NWO_TimeBetweenDelay_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NWO_MaxTimeoutsReached_ComboBox = new System.Windows.Forms.ComboBox();
            this.then = new System.Windows.Forms.Label();
            this.NWO_MaxTimeoutsAllowed_Textbox = new System.Windows.Forms.TextBox();
            this.NWO_Close_Button = new System.Windows.Forms.Button();
            this.NWO_ForBottingCharacter_ComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.NWO_Password_Textbox);
            this.groupBox1.Controls.Add(this.NWO_AutoLogin_CheckBox);
            this.groupBox1.Controls.Add(this.NWO_Username_Textbox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 76);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Login?";
            // 
            // NWO_Password_Textbox
            // 
            this.NWO_Password_Textbox.Enabled = false;
            this.NWO_Password_Textbox.Location = new System.Drawing.Point(66, 41);
            this.NWO_Password_Textbox.Name = "NWO_Password_Textbox";
            this.NWO_Password_Textbox.PasswordChar = '*';
            this.NWO_Password_Textbox.Size = new System.Drawing.Size(263, 20);
            this.NWO_Password_Textbox.TabIndex = 2;
            this.NWO_Password_Textbox.Text = "Password";
            this.NWO_Password_Textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NWO_Password_Textbox.Enter += new System.EventHandler(this.NWO_Password_Textbox_Enter);
            this.NWO_Password_Textbox.Leave += new System.EventHandler(this.NWO_Password_Textbox_Leave);
            // 
            // NWO_AutoLogin_CheckBox
            // 
            this.NWO_AutoLogin_CheckBox.AutoSize = true;
            this.NWO_AutoLogin_CheckBox.Location = new System.Drawing.Point(6, 30);
            this.NWO_AutoLogin_CheckBox.Name = "NWO_AutoLogin_CheckBox";
            this.NWO_AutoLogin_CheckBox.Size = new System.Drawing.Size(59, 17);
            this.NWO_AutoLogin_CheckBox.TabIndex = 0;
            this.NWO_AutoLogin_CheckBox.Text = "Enable";
            this.NWO_AutoLogin_CheckBox.UseVisualStyleBackColor = true;
            this.NWO_AutoLogin_CheckBox.CheckedChanged += new System.EventHandler(this.NWO_AutoLogin_CheckBox_CheckedChanged);
            // 
            // NWO_Username_Textbox
            // 
            this.NWO_Username_Textbox.Enabled = false;
            this.NWO_Username_Textbox.Location = new System.Drawing.Point(66, 15);
            this.NWO_Username_Textbox.Name = "NWO_Username_Textbox";
            this.NWO_Username_Textbox.Size = new System.Drawing.Size(263, 20);
            this.NWO_Username_Textbox.TabIndex = 1;
            this.NWO_Username_Textbox.Text = "Username";
            this.NWO_Username_Textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NWO_Username_Textbox.Enter += new System.EventHandler(this.NWO_Username_Textbox_Enter);
            this.NWO_Username_Textbox.Leave += new System.EventHandler(this.NWO_Username_Textbox_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.NWO_RemoveCharacter_Button);
            this.groupBox2.Controls.Add(this.NWO_AddCharacter_Button);
            this.groupBox2.Controls.Add(this.NWO_Character_TextBox);
            this.groupBox2.Controls.Add(this.NWO_Character_ListView);
            this.groupBox2.Controls.Add(this.NWO_AccountNick_Textbox);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(343, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 273);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Multiple Characters";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.NWO_ForBottingCharacter_ComboBox);
            this.groupBox4.Controls.Add(this.NWO_ForBotting_CheckBox);
            this.groupBox4.Location = new System.Drawing.Point(0, 211);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(253, 62);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            // 
            // NWO_RemoveCharacter_Button
            // 
            this.NWO_RemoveCharacter_Button.ForeColor = System.Drawing.Color.Black;
            this.NWO_RemoveCharacter_Button.Location = new System.Drawing.Point(139, 188);
            this.NWO_RemoveCharacter_Button.Name = "NWO_RemoveCharacter_Button";
            this.NWO_RemoveCharacter_Button.Size = new System.Drawing.Size(108, 23);
            this.NWO_RemoveCharacter_Button.TabIndex = 4;
            this.NWO_RemoveCharacter_Button.Text = "Remove";
            this.NWO_RemoveCharacter_Button.UseVisualStyleBackColor = true;
            this.NWO_RemoveCharacter_Button.Click += new System.EventHandler(this.NWO_RemoveCharacter_Button_Click);
            // 
            // NWO_AddCharacter_Button
            // 
            this.NWO_AddCharacter_Button.ForeColor = System.Drawing.Color.Black;
            this.NWO_AddCharacter_Button.Location = new System.Drawing.Point(6, 188);
            this.NWO_AddCharacter_Button.Name = "NWO_AddCharacter_Button";
            this.NWO_AddCharacter_Button.Size = new System.Drawing.Size(114, 23);
            this.NWO_AddCharacter_Button.TabIndex = 3;
            this.NWO_AddCharacter_Button.Text = "Add";
            this.NWO_AddCharacter_Button.UseVisualStyleBackColor = true;
            this.NWO_AddCharacter_Button.Click += new System.EventHandler(this.NWO_AddCharacter_Button_Click);
            // 
            // NWO_Character_TextBox
            // 
            this.NWO_Character_TextBox.Location = new System.Drawing.Point(6, 165);
            this.NWO_Character_TextBox.Name = "NWO_Character_TextBox";
            this.NWO_Character_TextBox.Size = new System.Drawing.Size(241, 20);
            this.NWO_Character_TextBox.TabIndex = 2;
            this.NWO_Character_TextBox.Text = "Character Name";
            this.NWO_Character_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NWO_Character_TextBox.Enter += new System.EventHandler(this.NWO_Character_TextBox_Enter);
            this.NWO_Character_TextBox.Leave += new System.EventHandler(this.NWO_Character_TextBox_Leave);
            // 
            // NWO_Character_ListView
            // 
            this.NWO_Character_ListView.FormattingEnabled = true;
            this.NWO_Character_ListView.Location = new System.Drawing.Point(6, 41);
            this.NWO_Character_ListView.Name = "NWO_Character_ListView";
            this.NWO_Character_ListView.Size = new System.Drawing.Size(241, 121);
            this.NWO_Character_ListView.TabIndex = 1;
            // 
            // NWO_AccountNick_Textbox
            // 
            this.NWO_AccountNick_Textbox.Location = new System.Drawing.Point(6, 15);
            this.NWO_AccountNick_Textbox.Name = "NWO_AccountNick_Textbox";
            this.NWO_AccountNick_Textbox.Size = new System.Drawing.Size(241, 20);
            this.NWO_AccountNick_Textbox.TabIndex = 0;
            this.NWO_AccountNick_Textbox.Text = "@Account";
            this.NWO_AccountNick_Textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NWO_AccountNick_Textbox.TextChanged += new System.EventHandler(this.NWO_AccountNick_Textbox_TextChanged);
            this.NWO_AccountNick_Textbox.Enter += new System.EventHandler(this.NWO_AccountNick_Textbox_Enter);
            this.NWO_AccountNick_Textbox.Leave += new System.EventHandler(this.NWO_AccountNick_Textbox_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Max Timeouts Allowed";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.NWO_WaitTimePageLoading_TextBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.NWO_WaitTimeBetweenLoginAttemts_TextBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.NWO_MaxTimeoutsReached_ComboBox);
            this.groupBox3.Controls.Add(this.then);
            this.groupBox3.Controls.Add(this.NWO_MaxTimeoutsAllowed_Textbox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(1, 79);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(336, 195);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Misc.";
            // 
            // NWO_ForBotting_CheckBox
            // 
            this.NWO_ForBotting_CheckBox.AutoSize = true;
            this.NWO_ForBotting_CheckBox.Location = new System.Drawing.Point(24, 23);
            this.NWO_ForBotting_CheckBox.Name = "NWO_ForBotting_CheckBox";
            this.NWO_ForBotting_CheckBox.Size = new System.Drawing.Size(77, 17);
            this.NWO_ForBotting_CheckBox.TabIndex = 15;
            this.NWO_ForBotting_CheckBox.Text = "For Botting";
            this.NWO_ForBotting_CheckBox.UseVisualStyleBackColor = true;
            this.NWO_ForBotting_CheckBox.CheckedChanged += new System.EventHandler(this.NWO_ForBotting_CheckBox_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(212, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "(sec)";
            // 
            // NWO_WaitTimePageLoading_TextBox
            // 
            this.NWO_WaitTimePageLoading_TextBox.Location = new System.Drawing.Point(156, 78);
            this.NWO_WaitTimePageLoading_TextBox.Name = "NWO_WaitTimePageLoading_TextBox";
            this.NWO_WaitTimePageLoading_TextBox.Size = new System.Drawing.Size(50, 20);
            this.NWO_WaitTimePageLoading_TextBox.TabIndex = 13;
            this.NWO_WaitTimePageLoading_TextBox.Text = "12";
            this.NWO_WaitTimePageLoading_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Wait Time for Page Loading";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(270, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "(sec)";
            // 
            // NWO_WaitTimeBetweenLoginAttemts_TextBox
            // 
            this.NWO_WaitTimeBetweenLoginAttemts_TextBox.Location = new System.Drawing.Point(212, 50);
            this.NWO_WaitTimeBetweenLoginAttemts_TextBox.Name = "NWO_WaitTimeBetweenLoginAttemts_TextBox";
            this.NWO_WaitTimeBetweenLoginAttemts_TextBox.Size = new System.Drawing.Size(52, 20);
            this.NWO_WaitTimeBetweenLoginAttemts_TextBox.TabIndex = 11;
            this.NWO_WaitTimeBetweenLoginAttemts_TextBox.Text = "600";
            this.NWO_WaitTimeBetweenLoginAttemts_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Wait Time Between Each Login Attempt";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.NWO_DevLogging_CheckBox);
            this.groupBox6.Controls.Add(this.NWO_AppendLogFiles_CheckBox);
            this.groupBox6.Controls.Add(this.NWO_VerboseLogging_Checkbox);
            this.groupBox6.Location = new System.Drawing.Point(0, 147);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(336, 48);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            // 
            // NWO_DevLogging_CheckBox
            // 
            this.NWO_DevLogging_CheckBox.AutoSize = true;
            this.NWO_DevLogging_CheckBox.Location = new System.Drawing.Point(226, 19);
            this.NWO_DevLogging_CheckBox.Name = "NWO_DevLogging_CheckBox";
            this.NWO_DevLogging_CheckBox.Size = new System.Drawing.Size(87, 17);
            this.NWO_DevLogging_CheckBox.TabIndex = 2;
            this.NWO_DevLogging_CheckBox.Text = "Dev Logging";
            this.NWO_DevLogging_CheckBox.UseVisualStyleBackColor = true;
            // 
            // NWO_AppendLogFiles_CheckBox
            // 
            this.NWO_AppendLogFiles_CheckBox.AutoSize = true;
            this.NWO_AppendLogFiles_CheckBox.Location = new System.Drawing.Point(129, 19);
            this.NWO_AppendLogFiles_CheckBox.Name = "NWO_AppendLogFiles_CheckBox";
            this.NWO_AppendLogFiles_CheckBox.Size = new System.Drawing.Size(97, 17);
            this.NWO_AppendLogFiles_CheckBox.TabIndex = 1;
            this.NWO_AppendLogFiles_CheckBox.Text = "Apend Log File";
            this.NWO_AppendLogFiles_CheckBox.UseVisualStyleBackColor = true;
            // 
            // NWO_VerboseLogging_Checkbox
            // 
            this.NWO_VerboseLogging_Checkbox.AutoSize = true;
            this.NWO_VerboseLogging_Checkbox.Location = new System.Drawing.Point(23, 19);
            this.NWO_VerboseLogging_Checkbox.Name = "NWO_VerboseLogging_Checkbox";
            this.NWO_VerboseLogging_Checkbox.Size = new System.Drawing.Size(106, 17);
            this.NWO_VerboseLogging_Checkbox.TabIndex = 0;
            this.NWO_VerboseLogging_Checkbox.Text = "Verbose Logging";
            this.NWO_VerboseLogging_Checkbox.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.NWO_AutoCalculateTimeBetweenDelays_CheckBox);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.NWO_TimeBetweenDelay_TextBox);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(0, 110);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(336, 45);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            // 
            // NWO_AutoCalculateTimeBetweenDelays_CheckBox
            // 
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox.AutoSize = true;
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox.Location = new System.Drawing.Point(11, 17);
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox.Name = "NWO_AutoCalculateTimeBetweenDelays_CheckBox";
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox.Size = new System.Drawing.Size(101, 17);
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox.TabIndex = 10;
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox.Text = "Auto Calculate?";
            this.NWO_AutoCalculateTimeBetweenDelays_CheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(301, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "(sec)";
            // 
            // NWO_TimeBetweenDelay_TextBox
            // 
            this.NWO_TimeBetweenDelay_TextBox.Location = new System.Drawing.Point(246, 15);
            this.NWO_TimeBetweenDelay_TextBox.Name = "NWO_TimeBetweenDelay_TextBox";
            this.NWO_TimeBetweenDelay_TextBox.Size = new System.Drawing.Size(54, 20);
            this.NWO_TimeBetweenDelay_TextBox.TabIndex = 1;
            this.NWO_TimeBetweenDelay_TextBox.Text = "30";
            this.NWO_TimeBetweenDelay_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Time Between Each Delay";
            // 
            // NWO_MaxTimeoutsReached_ComboBox
            // 
            this.NWO_MaxTimeoutsReached_ComboBox.FormattingEnabled = true;
            this.NWO_MaxTimeoutsReached_ComboBox.Location = new System.Drawing.Point(214, 21);
            this.NWO_MaxTimeoutsReached_ComboBox.Name = "NWO_MaxTimeoutsReached_ComboBox";
            this.NWO_MaxTimeoutsReached_ComboBox.Size = new System.Drawing.Size(115, 21);
            this.NWO_MaxTimeoutsReached_ComboBox.TabIndex = 7;
            // 
            // then
            // 
            this.then.AutoSize = true;
            this.then.Location = new System.Drawing.Point(172, 25);
            this.then.Name = "then";
            this.then.Size = new System.Drawing.Size(28, 13);
            this.then.TabIndex = 6;
            this.then.Text = "then";
            // 
            // NWO_MaxTimeoutsAllowed_Textbox
            // 
            this.NWO_MaxTimeoutsAllowed_Textbox.Location = new System.Drawing.Point(129, 22);
            this.NWO_MaxTimeoutsAllowed_Textbox.Name = "NWO_MaxTimeoutsAllowed_Textbox";
            this.NWO_MaxTimeoutsAllowed_Textbox.Size = new System.Drawing.Size(37, 20);
            this.NWO_MaxTimeoutsAllowed_Textbox.TabIndex = 5;
            this.NWO_MaxTimeoutsAllowed_Textbox.Text = "5";
            this.NWO_MaxTimeoutsAllowed_Textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NWO_Close_Button
            // 
            this.NWO_Close_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.NWO_Close_Button.Location = new System.Drawing.Point(1, 280);
            this.NWO_Close_Button.Name = "NWO_Close_Button";
            this.NWO_Close_Button.Size = new System.Drawing.Size(595, 23);
            this.NWO_Close_Button.TabIndex = 4;
            this.NWO_Close_Button.Text = "Close";
            this.NWO_Close_Button.UseVisualStyleBackColor = true;
            // 
            // NWO_ForBottingCharacter_ComboBox
            // 
            this.NWO_ForBottingCharacter_ComboBox.FormattingEnabled = true;
            this.NWO_ForBottingCharacter_ComboBox.Location = new System.Drawing.Point(107, 21);
            this.NWO_ForBottingCharacter_ComboBox.Name = "NWO_ForBottingCharacter_ComboBox";
            this.NWO_ForBottingCharacter_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.NWO_ForBottingCharacter_ComboBox.TabIndex = 16;
            // 
            // NWO_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(601, 306);
            this.Controls.Add(this.NWO_Close_Button);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NWO_Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox NWO_AutoLogin_CheckBox;
        private System.Windows.Forms.TextBox NWO_Password_Textbox;
        private System.Windows.Forms.TextBox NWO_Username_Textbox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox NWO_AccountNick_Textbox;
        private System.Windows.Forms.TextBox NWO_Character_TextBox;
        private System.Windows.Forms.ListBox NWO_Character_ListView;
        private System.Windows.Forms.Button NWO_RemoveCharacter_Button;
        private System.Windows.Forms.Button NWO_AddCharacter_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button NWO_Close_Button;
        private System.Windows.Forms.ComboBox NWO_MaxTimeoutsReached_ComboBox;
        private System.Windows.Forms.Label then;
        private System.Windows.Forms.TextBox NWO_MaxTimeoutsAllowed_Textbox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NWO_TimeBetweenDelay_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox NWO_DevLogging_CheckBox;
        private System.Windows.Forms.CheckBox NWO_AppendLogFiles_CheckBox;
        private System.Windows.Forms.CheckBox NWO_VerboseLogging_Checkbox;
        private System.Windows.Forms.CheckBox NWO_AutoCalculateTimeBetweenDelays_CheckBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox NWO_WaitTimeBetweenLoginAttemts_TextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox NWO_WaitTimePageLoading_TextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox NWO_ForBotting_CheckBox;
        private System.Windows.Forms.ComboBox NWO_ForBottingCharacter_ComboBox;
    }
}