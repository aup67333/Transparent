namespace CameraObjectROI
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
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.imageBox3 = new Emgu.CV.UI.ImageBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Stitch = new System.Windows.Forms.Button();
            this.testmode = new System.Windows.Forms.CheckBox();
            this.showcheck = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.tbEye = new System.Windows.Forms.TextBox();
            this.tbObj = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.came = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox1.Location = new System.Drawing.Point(12, 162);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(540, 351);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            this.imageBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseClick);
            this.imageBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseMove);
            // 
            // imageBox2
            // 
            this.imageBox2.Location = new System.Drawing.Point(587, 162);
            this.imageBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(531, 351);
            this.imageBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox2.TabIndex = 3;
            this.imageBox2.TabStop = false;
            // 
            // imageBox3
            // 
            this.imageBox3.Location = new System.Drawing.Point(12, 540);
            this.imageBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imageBox3.Name = "imageBox3";
            this.imageBox3.Size = new System.Drawing.Size(540, 352);
            this.imageBox3.TabIndex = 4;
            this.imageBox3.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 718);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(815, 692);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(815, 718);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "label2";
            // 
            // Stitch
            // 
            this.Stitch.Location = new System.Drawing.Point(12, 718);
            this.Stitch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Stitch.Name = "Stitch";
            this.Stitch.Size = new System.Drawing.Size(75, 22);
            this.Stitch.TabIndex = 8;
            this.Stitch.Text = "Stitch";
            this.Stitch.UseVisualStyleBackColor = true;
            this.Stitch.Click += new System.EventHandler(this.Stitch_Click);
            // 
            // testmode
            // 
            this.testmode.AutoSize = true;
            this.testmode.Location = new System.Drawing.Point(12, 12);
            this.testmode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.testmode.Name = "testmode";
            this.testmode.Size = new System.Drawing.Size(18, 17);
            this.testmode.TabIndex = 21;
            this.testmode.UseVisualStyleBackColor = true;
            this.testmode.CheckedChanged += new System.EventHandler(this.testmode_CheckedChanged);
            // 
            // showcheck
            // 
            this.showcheck.AutoSize = true;
            this.showcheck.Location = new System.Drawing.Point(12, 38);
            this.showcheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.showcheck.Name = "showcheck";
            this.showcheck.Size = new System.Drawing.Size(18, 17);
            this.showcheck.TabIndex = 22;
            this.showcheck.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(901, 692);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 20);
            this.label9.TabIndex = 24;
            this.label9.Text = "label9";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(901, 718);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 20);
            this.label10.TabIndex = 23;
            this.label10.Text = "label10";
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(815, 661);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 20);
            this.label11.TabIndex = 25;
            this.label11.Text = "label11";
            // 
            // tbEye
            // 
            this.tbEye.Location = new System.Drawing.Point(587, 592);
            this.tbEye.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbEye.Multiline = true;
            this.tbEye.Name = "tbEye";
            this.tbEye.Size = new System.Drawing.Size(204, 146);
            this.tbEye.TabIndex = 26;
            // 
            // tbObj
            // 
            this.tbObj.Location = new System.Drawing.Point(819, 592);
            this.tbObj.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbObj.Multiline = true;
            this.tbObj.Name = "tbObj";
            this.tbObj.Size = new System.Drawing.Size(204, 146);
            this.tbObj.TabIndex = 27;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(93, 718);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 22);
            this.button3.TabIndex = 28;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 748);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 22);
            this.button4.TabIndex = 29;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // came
            // 
            this.came.AutoSize = true;
            this.came.Location = new System.Drawing.Point(12, 67);
            this.came.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.came.Name = "came";
            this.came.Size = new System.Drawing.Size(18, 17);
            this.came.TabIndex = 30;
            this.came.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1149, 798);
            this.Controls.Add(this.came);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tbObj);
            this.Controls.Add(this.tbEye);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.showcheck);
            this.Controls.Add(this.testmode);
            this.Controls.Add(this.Stitch);
            this.Controls.Add(this.imageBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.imageBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private Emgu.CV.UI.ImageBox imageBox2;
        private Emgu.CV.UI.ImageBox imageBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Stitch;
        private System.Windows.Forms.CheckBox testmode;
        private System.Windows.Forms.CheckBox showcheck;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbEye;
        private System.Windows.Forms.TextBox tbObj;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox came;
    }
}

