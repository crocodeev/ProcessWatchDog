namespace ProcessWatchDogConfigurator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SaveRestart = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Start = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.ADD = new System.Windows.Forms.Button();
            this.CLEAR = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.Status = new System.Windows.Forms.Button();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SELECT = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.helpProvider1.SetHelpString(this.textBox1, resources.GetString("textBox1.HelpString"));
            this.textBox1.Location = new System.Drawing.Point(18, 47);
            this.textBox1.Name = "textBox1";
            this.helpProvider1.SetShowHelp(this.textBox1, true);
            this.textBox1.Size = new System.Drawing.Size(584, 29);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "09:30,16:30";
            // 
            // SaveRestart
            // 
            this.helpProvider1.SetHelpKeyword(this.SaveRestart, "");
            this.helpProvider1.SetHelpString(this.SaveRestart, "Save schedule from text field below and restart CheshkaWatchDog");
            this.SaveRestart.Location = new System.Drawing.Point(218, 16);
            this.SaveRestart.Name = "SaveRestart";
            this.helpProvider1.SetShowHelp(this.SaveRestart, true);
            this.SaveRestart.Size = new System.Drawing.Size(113, 23);
            this.SaveRestart.TabIndex = 3;
            this.SaveRestart.Text = "Save and Restart";
            this.SaveRestart.UseVisualStyleBackColor = true;
            this.SaveRestart.Click += new System.EventHandler(this.SaveRestart_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(436, 16);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(142, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Visible = false;
            // 
            // Start
            // 
            this.helpProvider1.SetHelpString(this.Start, "Start CheshkaWatchDog service");
            this.Start.Location = new System.Drawing.Point(6, 16);
            this.Start.Name = "Start";
            this.helpProvider1.SetShowHelp(this.Start, true);
            this.Start.Size = new System.Drawing.Size(107, 23);
            this.Start.TabIndex = 5;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Stop
            // 
            this.helpProvider1.SetHelpKeyword(this.Stop, "");
            this.helpProvider1.SetHelpString(this.Stop, "Stop CheshkaWatchDog service");
            this.Stop.Location = new System.Drawing.Point(119, 16);
            this.Stop.Name = "Stop";
            this.helpProvider1.SetShowHelp(this.Stop, true);
            this.Stop.Size = new System.Drawing.Size(93, 23);
            this.Stop.TabIndex = 6;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker1.CustomFormat = "HH:mm";
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(69, 29);
            this.dateTimePicker1.TabIndex = 7;
            // 
            // ADD
            // 
            this.helpProvider1.SetHelpString(this.ADD, "Add time from left timepicker  to schedule text field below");
            this.ADD.Location = new System.Drawing.Point(93, 12);
            this.ADD.Name = "ADD";
            this.helpProvider1.SetShowHelp(this.ADD, true);
            this.ADD.Size = new System.Drawing.Size(89, 29);
            this.ADD.TabIndex = 8;
            this.ADD.Text = "ADD";
            this.ADD.UseVisualStyleBackColor = true;
            this.ADD.Click += new System.EventHandler(this.ADD_Click);
            // 
            // CLEAR
            // 
            this.helpProvider1.SetHelpString(this.CLEAR, "Clear schedule text field below");
            this.CLEAR.Location = new System.Drawing.Point(188, 12);
            this.CLEAR.Name = "CLEAR";
            this.helpProvider1.SetShowHelp(this.CLEAR, true);
            this.CLEAR.Size = new System.Drawing.Size(89, 29);
            this.CLEAR.TabIndex = 9;
            this.CLEAR.Text = "CLEAR";
            this.CLEAR.UseVisualStyleBackColor = true;
            this.CLEAR.Click += new System.EventHandler(this.CLEAR_Click);
            // 
            // Status
            // 
            this.helpProvider1.SetHelpKeyword(this.Status, "");
            this.helpProvider1.SetHelpString(this.Status, "Show status CheshkaWatchDog service");
            this.Status.Location = new System.Drawing.Point(337, 16);
            this.Status.Name = "Status";
            this.helpProvider1.SetShowHelp(this.Status, true);
            this.Status.Size = new System.Drawing.Size(93, 23);
            this.Status.TabIndex = 10;
            this.Status.Text = "Status";
            this.Status.UseVisualStyleBackColor = true;
            this.Status.Click += new System.EventHandler(this.Status_Click);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Start);
            this.groupBox1.Controls.Add(this.Status);
            this.groupBox1.Controls.Add(this.Stop);
            this.groupBox1.Controls.Add(this.SaveRestart);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Location = new System.Drawing.Point(18, 215);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(584, 48);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // SELECT
            // 
            this.SELECT.Location = new System.Drawing.Point(18, 82);
            this.SELECT.Name = "SELECT";
            this.SELECT.Size = new System.Drawing.Size(184, 29);
            this.SELECT.TabIndex = 12;
            this.SELECT.Text = "CHOOSE EXECUTABLE";
            this.SELECT.UseVisualStyleBackColor = true;
            this.SELECT.Click += new System.EventHandler(this.SELECT_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(18, 117);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 272);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.SELECT);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CLEAR);
            this.Controls.Add(this.ADD);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.textBox1);
            this.HelpButton = true;
            this.helpProvider1.SetHelpString(this, "If for some reason Start,Stop and etc. function didn\'t work, please use service.m" +
        "sc or sc.exe to control CheshkaWatchDog");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.helpProvider1.SetShowHelp(this, true);
            this.Text = "Configurator";
            this.Load += new System.EventHandler(this.FormLoad);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SaveRestart;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button ADD;
        private System.Windows.Forms.Button CLEAR;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.Button Status;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SELECT;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}

