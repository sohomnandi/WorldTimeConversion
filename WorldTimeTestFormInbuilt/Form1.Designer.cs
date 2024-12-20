using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WorldTimeTestFormInbuilt
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtUtcTime = new TextBox();
            comboBoxTimeZones = new ComboBox();
            btnConvertToLocal = new Button();
            txtLocalTime = new TextBox();
            btnConvertToUTC = new Button();
            lblUtcTimeResult = new Label();
            lblLocalTimeResult = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // txtUtcTime
            // 
            txtUtcTime.Location = new Point(641, 417);
            txtUtcTime.Name = "txtUtcTime";
            txtUtcTime.Size = new Size(200, 39);
            txtUtcTime.TabIndex = 0;
            // 
            // comboBoxTimeZones
            // 
            comboBoxTimeZones.FormattingEnabled = true;
            comboBoxTimeZones.Location = new Point(675, 85);
            comboBoxTimeZones.Name = "comboBoxTimeZones";
            comboBoxTimeZones.Size = new Size(343, 40);
            comboBoxTimeZones.TabIndex = 1;
            // 
            // btnConvertToLocal
            // 
            btnConvertToLocal.Location = new Point(895, 416);
            btnConvertToLocal.Name = "btnConvertToLocal";
            btnConvertToLocal.Size = new Size(264, 46);
            btnConvertToLocal.TabIndex = 2;
            btnConvertToLocal.Text = "Convert To Local Time";
            btnConvertToLocal.UseVisualStyleBackColor = true;
            btnConvertToLocal.Click += btnConvertToLocal_Click;
            // 
            // txtLocalTime
            // 
            txtLocalTime.Location = new Point(641, 220);
            txtLocalTime.Name = "txtLocalTime";
            txtLocalTime.Size = new Size(200, 39);
            txtLocalTime.TabIndex = 3;
            // 
            // btnConvertToUTC
            // 
            btnConvertToUTC.Location = new Point(895, 216);
            btnConvertToUTC.Name = "btnConvertToUTC";
            btnConvertToUTC.Size = new Size(264, 46);
            btnConvertToUTC.TabIndex = 4;
            btnConvertToUTC.Text = "Convert To UTC Time";
            btnConvertToUTC.UseVisualStyleBackColor = true;
            btnConvertToUTC.Click += btnConvertToUTC_Click;
            // 
            // lblUtcTimeResult
            // 
            lblUtcTimeResult.AutoSize = true;
            lblUtcTimeResult.Location = new Point(1224, 225);
            lblUtcTimeResult.Name = "lblUtcTimeResult";
            lblUtcTimeResult.Size = new Size(78, 32);
            lblUtcTimeResult.TabIndex = 5;
            lblUtcTimeResult.Text = "label1";
            // 
            // lblLocalTimeResult
            // 
            lblLocalTimeResult.AutoSize = true;
            lblLocalTimeResult.Location = new Point(1224, 425);
            lblLocalTimeResult.Name = "lblLocalTimeResult";
            lblLocalTimeResult.Size = new Size(78, 32);
            lblLocalTimeResult.TabIndex = 6;
            lblLocalTimeResult.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(410, 88);
            label1.Name = "label1";
            label1.Size = new Size(259, 32);
            label1.TabIndex = 7;
            label1.Text = "Select Your Time Zone:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 223);
            label2.Name = "label2";
            label2.Size = new Size(202, 32);
            label2.TabIndex = 8;
            label2.Text = "Enter Local Time: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 423);
            label3.Name = "label3";
            label3.Size = new Size(184, 32);
            label3.TabIndex = 9;
            label3.Text = "Enter UTC Time:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(570, 332);
            label4.Name = "label4";
            label4.Size = new Size(46, 32);
            label4.TabIndex = 10;
            label4.Text = "OR";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1723, 815);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblLocalTimeResult);
            Controls.Add(lblUtcTimeResult);
            Controls.Add(btnConvertToUTC);
            Controls.Add(txtLocalTime);
            Controls.Add(btnConvertToLocal);
            Controls.Add(comboBoxTimeZones);
            Controls.Add(txtUtcTime);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUtcTime;
        private ComboBox comboBoxTimeZones;
        private Button btnConvertToLocal;
        private TextBox txtLocalTime;
        private Button btnConvertToUTC;
        private Label lblUtcTimeResult;
        private Label lblLocalTimeResult;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
