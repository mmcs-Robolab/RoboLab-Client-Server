﻿namespace RoboClient
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.authBtn = new System.Windows.Forms.Button();
            this.passText = new System.Windows.Forms.TextBox();
            this.passLabel = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.serverPanel = new System.Windows.Forms.Panel();
            this.pointNameText = new System.Windows.Forms.TextBox();
            this.pointNameLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.portText = new System.Windows.Forms.TextBox();
            this.ipText = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.msgField = new System.Windows.Forms.TextBox();
            this.logText = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.serverPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // authBtn
            // 
            this.authBtn.Location = new System.Drawing.Point(412, 9);
            this.authBtn.Name = "authBtn";
            this.authBtn.Size = new System.Drawing.Size(152, 20);
            this.authBtn.TabIndex = 18;
            this.authBtn.Text = "Авторизация";
            this.authBtn.UseVisualStyleBackColor = true;
            this.authBtn.Click += new System.EventHandler(this.authBtn_Click);
            // 
            // passText
            // 
            this.passText.Location = new System.Drawing.Point(278, 10);
            this.passText.Name = "passText";
            this.passText.PasswordChar = '*';
            this.passText.Size = new System.Drawing.Size(128, 20);
            this.passText.TabIndex = 17;
            // 
            // passLabel
            // 
            this.passLabel.AutoSize = true;
            this.passLabel.Location = new System.Drawing.Point(216, 13);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(56, 13);
            this.passLabel.TabIndex = 16;
            this.passLabel.Text = "Password:";
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(54, 10);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(141, 20);
            this.nameText.TabIndex = 15;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 13);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(36, 13);
            this.nameLabel.TabIndex = 14;
            this.nameLabel.Text = "Login:";
            // 
            // serverPanel
            // 
            this.serverPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serverPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverPanel.Controls.Add(this.pointNameText);
            this.serverPanel.Controls.Add(this.pointNameLabel);
            this.serverPanel.Controls.Add(this.label2);
            this.serverPanel.Controls.Add(this.label1);
            this.serverPanel.Controls.Add(this.portText);
            this.serverPanel.Controls.Add(this.ipText);
            this.serverPanel.Controls.Add(this.connectButton);
            this.serverPanel.Enabled = false;
            this.serverPanel.Location = new System.Drawing.Point(425, 43);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(209, 138);
            this.serverPanel.TabIndex = 13;
            // 
            // pointNameText
            // 
            this.pointNameText.Location = new System.Drawing.Point(106, 68);
            this.pointNameText.Name = "pointNameText";
            this.pointNameText.Size = new System.Drawing.Size(92, 20);
            this.pointNameText.TabIndex = 5;
            this.pointNameText.Text = "Точка 1";
            // 
            // pointNameLabel
            // 
            this.pointNameLabel.AutoSize = true;
            this.pointNameLabel.Location = new System.Drawing.Point(9, 71);
            this.pointNameLabel.Name = "pointNameLabel";
            this.pointNameLabel.Size = new System.Drawing.Size(91, 13);
            this.pointNameLabel.TabIndex = 4;
            this.pointNameLabel.Text = "Название точки:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Порт:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP:";
            // 
            // portText
            // 
            this.portText.Location = new System.Drawing.Point(106, 38);
            this.portText.Name = "portText";
            this.portText.Size = new System.Drawing.Size(92, 20);
            this.portText.TabIndex = 1;
            this.portText.Text = "3004";
            // 
            // ipText
            // 
            this.ipText.Location = new System.Drawing.Point(106, 6);
            this.ipText.Name = "ipText";
            this.ipText.Size = new System.Drawing.Size(92, 20);
            this.ipText.TabIndex = 0;
            this.ipText.Text = "127.0.0.1";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(12, 94);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(186, 30);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Соединение";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // msgField
            // 
            this.msgField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msgField.Location = new System.Drawing.Point(12, 440);
            this.msgField.Name = "msgField";
            this.msgField.Size = new System.Drawing.Size(407, 20);
            this.msgField.TabIndex = 12;
            this.msgField.Text = "Message";
            // 
            // logText
            // 
            this.logText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logText.Location = new System.Drawing.Point(12, 43);
            this.logText.Multiline = true;
            this.logText.Name = "logText";
            this.logText.ReadOnly = true;
            this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logText.Size = new System.Drawing.Size(407, 391);
            this.logText.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(425, 440);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(209, 20);
            this.button2.TabIndex = 10;
            this.button2.Text = "Отправить";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 472);
            this.Controls.Add(this.authBtn);
            this.Controls.Add(this.passText);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.serverPanel);
            this.Controls.Add(this.msgField);
            this.Controls.Add(this.logText);
            this.Controls.Add(this.button2);
            this.MinimumSize = new System.Drawing.Size(590, 250);
            this.Name = "Form1";
            this.Text = "RoboClient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.serverPanel.ResumeLayout(false);
            this.serverPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button authBtn;
        private System.Windows.Forms.TextBox passText;
        private System.Windows.Forms.Label passLabel;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Panel serverPanel;
        private System.Windows.Forms.TextBox pointNameText;
        private System.Windows.Forms.Label pointNameLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox portText;
        private System.Windows.Forms.TextBox ipText;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox msgField;
        private System.Windows.Forms.TextBox logText;
        private System.Windows.Forms.Button button2;
    }
}
