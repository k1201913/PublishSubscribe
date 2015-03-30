namespace SocketPublisher
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.txtTopicName = new System.Windows.Forms.TextBox();
            this.txtEventData = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFireAuto = new System.Windows.Forms.Button();
            this.btnFireAutoStop = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEventCount = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tmrEvent = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTopicName
            // 
            this.txtTopicName.Location = new System.Drawing.Point(184, 85);
            this.txtTopicName.Margin = new System.Windows.Forms.Padding(4);
            this.txtTopicName.Name = "txtTopicName";
            this.txtTopicName.Size = new System.Drawing.Size(111, 22);
            this.txtTopicName.TabIndex = 1;
            // 
            // txtEventData
            // 
            this.txtEventData.Location = new System.Drawing.Point(184, 137);
            this.txtEventData.Margin = new System.Windows.Forms.Padding(4);
            this.txtEventData.Multiline = true;
            this.txtEventData.Name = "txtEventData";
            this.txtEventData.Size = new System.Drawing.Size(195, 100);
            this.txtEventData.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 89);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 38;
            this.label3.Text = "Topic Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 178);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 39;
            this.label1.Text = "Event Data";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFireAuto);
            this.groupBox1.Controls.Add(this.btnFireAutoStop);
            this.groupBox1.Location = new System.Drawing.Point(484, 82);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(225, 156);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Event";
            // 
            // btnFireAuto
            // 
            this.btnFireAuto.Location = new System.Drawing.Point(8, 52);
            this.btnFireAuto.Margin = new System.Windows.Forms.Padding(4);
            this.btnFireAuto.Name = "btnFireAuto";
            this.btnFireAuto.Size = new System.Drawing.Size(191, 28);
            this.btnFireAuto.TabIndex = 3;
            this.btnFireAuto.Text = "Fire Auto Event";
            this.btnFireAuto.UseVisualStyleBackColor = true;
            this.btnFireAuto.Click += new System.EventHandler(this.btnFireAuto_Click);
            // 
            // btnFireAutoStop
            // 
            this.btnFireAutoStop.Location = new System.Drawing.Point(49, 96);
            this.btnFireAutoStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnFireAutoStop.Name = "btnFireAutoStop";
            this.btnFireAutoStop.Size = new System.Drawing.Size(116, 28);
            this.btnFireAutoStop.TabIndex = 5;
            this.btnFireAutoStop.Text = "Stop";
            this.btnFireAutoStop.UseVisualStyleBackColor = true;
            this.btnFireAutoStop.Click += new System.EventHandler(this.btnFireAutoStop_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(341, 580);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 28);
            this.button2.TabIndex = 43;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(439, 270);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 17);
            this.label4.TabIndex = 42;
            this.label4.Text = "Total Events  Fired :";
            // 
            // txtEventCount
            // 
            this.txtEventCount.Location = new System.Drawing.Point(655, 270);
            this.txtEventCount.Margin = new System.Windows.Forms.Padding(4);
            this.txtEventCount.Name = "txtEventCount";
            this.txtEventCount.Size = new System.Drawing.Size(53, 22);
            this.txtEventCount.TabIndex = 41;
            this.txtEventCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(443, 32);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(300, 28);
            this.button3.TabIndex = 40;
            this.button3.Text = "Fire a single  Event";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tmrEvent
            // 
            this.tmrEvent.Tick += new System.EventHandler(this.tmrEvent_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 17);
            this.label2.TabIndex = 45;
            this.label2.Text = "Ip";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(184, 32);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(111, 22);
            this.txtIp.TabIndex = 46;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 352);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtEventCount);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEventData);
            this.Controls.Add(this.txtTopicName);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Publisher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTopicName;
        private System.Windows.Forms.TextBox txtEventData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFireAuto;
        private System.Windows.Forms.Button btnFireAutoStop;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEventCount;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer tmrEvent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIp;
    }
}

