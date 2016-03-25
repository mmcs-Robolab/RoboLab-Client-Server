namespace RoboServer
{
    partial class MainForm
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
            this.socketGroupBox = new System.Windows.Forms.GroupBox();
            this.sockIpText = new System.Windows.Forms.TextBox();
            this.sockPortText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.socketPortLabel = new System.Windows.Forms.Label();
            this.logSocketText = new System.Windows.Forms.TextBox();
            this.webSocketGroupBox = new System.Windows.Forms.GroupBox();
            this.webSockIpText = new System.Windows.Forms.TextBox();
            this.webSockPortText = new System.Windows.Forms.TextBox();
            this.webSockIpLabel = new System.Windows.Forms.Label();
            this.webSockPortLabel = new System.Windows.Forms.Label();
            this.logWebSocketText = new System.Windows.Forms.TextBox();
            this.createSockServerBtn = new System.Windows.Forms.Button();
            this.socketGroupBox.SuspendLayout();
            this.webSocketGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // socketGroupBox
            // 
            this.socketGroupBox.Controls.Add(this.sockIpText);
            this.socketGroupBox.Controls.Add(this.sockPortText);
            this.socketGroupBox.Controls.Add(this.label1);
            this.socketGroupBox.Controls.Add(this.socketPortLabel);
            this.socketGroupBox.Location = new System.Drawing.Point(12, 305);
            this.socketGroupBox.Name = "socketGroupBox";
            this.socketGroupBox.Size = new System.Drawing.Size(315, 93);
            this.socketGroupBox.TabIndex = 12;
            this.socketGroupBox.TabStop = false;
            this.socketGroupBox.Text = "Socket - сервер";
            // 
            // sockIpText
            // 
            this.sockIpText.Enabled = false;
            this.sockIpText.Location = new System.Drawing.Point(53, 57);
            this.sockIpText.Name = "sockIpText";
            this.sockIpText.Size = new System.Drawing.Size(100, 20);
            this.sockIpText.TabIndex = 8;
            // 
            // sockPortText
            // 
            this.sockPortText.Location = new System.Drawing.Point(53, 29);
            this.sockPortText.Name = "sockPortText";
            this.sockPortText.Size = new System.Drawing.Size(100, 20);
            this.sockPortText.TabIndex = 7;
            this.sockPortText.Text = "3004";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(6, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "IP";
            // 
            // socketPortLabel
            // 
            this.socketPortLabel.AutoSize = true;
            this.socketPortLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.socketPortLabel.Location = new System.Drawing.Point(6, 29);
            this.socketPortLabel.Name = "socketPortLabel";
            this.socketPortLabel.Size = new System.Drawing.Size(41, 16);
            this.socketPortLabel.TabIndex = 5;
            this.socketPortLabel.Text = "Порт";
            // 
            // logSocketText
            // 
            this.logSocketText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logSocketText.BackColor = System.Drawing.SystemColors.Window;
            this.logSocketText.Location = new System.Drawing.Point(340, 305);
            this.logSocketText.Multiline = true;
            this.logSocketText.Name = "logSocketText";
            this.logSocketText.ReadOnly = true;
            this.logSocketText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logSocketText.Size = new System.Drawing.Size(438, 174);
            this.logSocketText.TabIndex = 11;
            // 
            // webSocketGroupBox
            // 
            this.webSocketGroupBox.Controls.Add(this.webSockIpText);
            this.webSocketGroupBox.Controls.Add(this.webSockPortText);
            this.webSocketGroupBox.Controls.Add(this.webSockIpLabel);
            this.webSocketGroupBox.Controls.Add(this.webSockPortLabel);
            this.webSocketGroupBox.Location = new System.Drawing.Point(12, 12);
            this.webSocketGroupBox.Name = "webSocketGroupBox";
            this.webSocketGroupBox.Size = new System.Drawing.Size(315, 93);
            this.webSocketGroupBox.TabIndex = 10;
            this.webSocketGroupBox.TabStop = false;
            this.webSocketGroupBox.Text = "WebSocket - сервер";
            // 
            // webSockIpText
            // 
            this.webSockIpText.Enabled = false;
            this.webSockIpText.Location = new System.Drawing.Point(53, 57);
            this.webSockIpText.Name = "webSockIpText";
            this.webSockIpText.Size = new System.Drawing.Size(100, 20);
            this.webSockIpText.TabIndex = 8;
            // 
            // webSockPortText
            // 
            this.webSockPortText.Location = new System.Drawing.Point(53, 29);
            this.webSockPortText.Name = "webSockPortText";
            this.webSockPortText.Size = new System.Drawing.Size(100, 20);
            this.webSockPortText.TabIndex = 7;
            this.webSockPortText.Text = "3003";
            // 
            // webSockIpLabel
            // 
            this.webSockIpLabel.AutoSize = true;
            this.webSockIpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.webSockIpLabel.Location = new System.Drawing.Point(6, 58);
            this.webSockIpLabel.Name = "webSockIpLabel";
            this.webSockIpLabel.Size = new System.Drawing.Size(20, 16);
            this.webSockIpLabel.TabIndex = 6;
            this.webSockIpLabel.Text = "IP";
            // 
            // webSockPortLabel
            // 
            this.webSockPortLabel.AutoSize = true;
            this.webSockPortLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.webSockPortLabel.Location = new System.Drawing.Point(6, 29);
            this.webSockPortLabel.Name = "webSockPortLabel";
            this.webSockPortLabel.Size = new System.Drawing.Size(41, 16);
            this.webSockPortLabel.TabIndex = 5;
            this.webSockPortLabel.Text = "Порт";
            // 
            // logWebSocketText
            // 
            this.logWebSocketText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logWebSocketText.BackColor = System.Drawing.SystemColors.Window;
            this.logWebSocketText.Location = new System.Drawing.Point(340, 12);
            this.logWebSocketText.Multiline = true;
            this.logWebSocketText.Name = "logWebSocketText";
            this.logWebSocketText.ReadOnly = true;
            this.logWebSocketText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logWebSocketText.Size = new System.Drawing.Size(438, 274);
            this.logWebSocketText.TabIndex = 8;
            // 
            // createSockServerBtn
            // 
            this.createSockServerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createSockServerBtn.Location = new System.Drawing.Point(12, 438);
            this.createSockServerBtn.Name = "createSockServerBtn";
            this.createSockServerBtn.Size = new System.Drawing.Size(315, 41);
            this.createSockServerBtn.TabIndex = 9;
            this.createSockServerBtn.Text = "Создать";
            this.createSockServerBtn.UseVisualStyleBackColor = true;
            this.createSockServerBtn.Click += new System.EventHandler(this.createSockServerBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 491);
            this.Controls.Add(this.socketGroupBox);
            this.Controls.Add(this.logSocketText);
            this.Controls.Add(this.webSocketGroupBox);
            this.Controls.Add(this.logWebSocketText);
            this.Controls.Add(this.createSockServerBtn);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.socketGroupBox.ResumeLayout(false);
            this.socketGroupBox.PerformLayout();
            this.webSocketGroupBox.ResumeLayout(false);
            this.webSocketGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox socketGroupBox;
        private System.Windows.Forms.TextBox sockIpText;
        private System.Windows.Forms.TextBox sockPortText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label socketPortLabel;
        private System.Windows.Forms.TextBox logSocketText;
        private System.Windows.Forms.GroupBox webSocketGroupBox;
        private System.Windows.Forms.TextBox webSockIpText;
        private System.Windows.Forms.TextBox webSockPortText;
        private System.Windows.Forms.Label webSockIpLabel;
        private System.Windows.Forms.Label webSockPortLabel;
        private System.Windows.Forms.TextBox logWebSocketText;
        private System.Windows.Forms.Button createSockServerBtn;
    }
}

