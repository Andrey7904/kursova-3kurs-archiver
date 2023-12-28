namespace _3kursova_Archivator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button_choose_folder = new Button();
            richTextBox1 = new RichTextBox();
            button_archivate = new Button();
            button_dearchivate = new Button();
            richTextBox2 = new RichTextBox();
            label2 = new Label();
            label1 = new Label();
            richTextBox3 = new RichTextBox();
            button_choose_archive = new Button();
            button_choose_files = new Button();
            label3 = new Label();
            label5 = new Label();
            richTextBox4 = new RichTextBox();
            label6 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            checkBox1 = new CheckBox();
            comboBox1 = new ComboBox();
            checkBox2 = new CheckBox();
            richTextBox5 = new RichTextBox();
            button_choose_volumes = new Button();
            SuspendLayout();
            // 
            // button_choose_folder
            // 
            button_choose_folder.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_choose_folder.Location = new Point(391, 143);
            button_choose_folder.Margin = new Padding(2);
            button_choose_folder.Name = "button_choose_folder";
            button_choose_folder.Size = new Size(127, 30);
            button_choose_folder.TabIndex = 0;
            button_choose_folder.Text = "Обрати папку";
            button_choose_folder.UseVisualStyleBackColor = true;
            button_choose_folder.Click += button_choose_folder_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox1.Location = new Point(20, 143);
            richTextBox1.Margin = new Padding(2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(357, 72);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // button_archivate
            // 
            button_archivate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_archivate.Location = new Point(380, 236);
            button_archivate.Margin = new Padding(2);
            button_archivate.Name = "button_archivate";
            button_archivate.Size = new Size(145, 38);
            button_archivate.TabIndex = 3;
            button_archivate.Text = "Заархівувати";
            button_archivate.TextImageRelation = TextImageRelation.ImageAboveText;
            button_archivate.UseVisualStyleBackColor = true;
            button_archivate.Click += button_archivate_Click;
            // 
            // button_dearchivate
            // 
            button_dearchivate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_dearchivate.Location = new Point(967, 261);
            button_dearchivate.Margin = new Padding(2);
            button_dearchivate.Name = "button_dearchivate";
            button_dearchivate.Size = new Size(168, 38);
            button_dearchivate.TabIndex = 4;
            button_dearchivate.Text = "Розархівувати архів";
            button_dearchivate.UseVisualStyleBackColor = true;
            button_dearchivate.Click += button_dearchivate_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(274, 373);
            richTextBox2.Margin = new Padding(2);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(650, 93);
            richTextBox2.TabIndex = 5;
            richTextBox2.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(326, 343);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(109, 21);
            label2.TabIndex = 6;
            label2.Text = "Результат log:";
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(33, 22);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(1057, 60);
            label1.TabIndex = 8;
            label1.Text = resources.GetString("label1.Text");
            // 
            // richTextBox3
            // 
            richTextBox3.Font = new Font("Segoe UI", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox3.Location = new Point(563, 140);
            richTextBox3.Margin = new Padding(2);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.Size = new Size(430, 40);
            richTextBox3.TabIndex = 11;
            richTextBox3.Text = "";
            // 
            // button_choose_archive
            // 
            button_choose_archive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_choose_archive.Location = new Point(1007, 140);
            button_choose_archive.Margin = new Padding(2);
            button_choose_archive.Name = "button_choose_archive";
            button_choose_archive.Size = new Size(127, 30);
            button_choose_archive.TabIndex = 12;
            button_choose_archive.Text = "Обрати архів";
            button_choose_archive.UseVisualStyleBackColor = true;
            button_choose_archive.Click += button_choose_archive_Click;
            // 
            // button_choose_files
            // 
            button_choose_files.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_choose_files.Location = new Point(391, 184);
            button_choose_files.Margin = new Padding(2);
            button_choose_files.Name = "button_choose_files";
            button_choose_files.Size = new Size(127, 30);
            button_choose_files.TabIndex = 13;
            button_choose_files.Text = "Обрати файли";
            button_choose_files.UseVisualStyleBackColor = true;
            button_choose_files.Click += button_choose_files_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(146, 105);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(87, 21);
            label3.TabIndex = 14;
            label3.Text = "Архівація";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(714, 105);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(113, 21);
            label5.TabIndex = 15;
            label5.Text = "Розархівація";
            // 
            // richTextBox4
            // 
            richTextBox4.Font = new Font("Segoe UI", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox4.Location = new Point(20, 247);
            richTextBox4.Margin = new Padding(2);
            richTextBox4.Name = "richTextBox4";
            richTextBox4.Size = new Size(306, 28);
            richTextBox4.TabIndex = 16;
            richTextBox4.Text = "";
            // 
            // label6
            // 
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(20, 225);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(153, 20);
            label6.TabIndex = 17;
            label6.Text = "Введіть ім'я архіву:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(25, 18);
            flowLayoutPanel1.Margin = new Padding(2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1070, 74);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox1.Location = new Point(20, 290);
            checkBox1.Margin = new Padding(2);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(237, 25);
            checkBox1.TabIndex = 19;
            checkBox1.Text = "Розділити на томи розміром:";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "100 KB", "5 MB", "100 МB", "700 МB", "1 GB", "1.5 GB", "4095 МB" });
            comboBox1.Location = new Point(264, 292);
            comboBox1.Margin = new Padding(2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(125, 23);
            comboBox1.TabIndex = 20;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox2.Location = new Point(563, 196);
            checkBox2.Margin = new Padding(2);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(320, 25);
            checkBox2.TabIndex = 21;
            checkBox2.Text = "Розархівація архіву розділеного на томи";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // richTextBox5
            // 
            richTextBox5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBox5.Location = new Point(563, 220);
            richTextBox5.Margin = new Padding(2);
            richTextBox5.Name = "richTextBox5";
            richTextBox5.Size = new Size(387, 80);
            richTextBox5.TabIndex = 22;
            richTextBox5.Text = "";
            // 
            // button_choose_volumes
            // 
            button_choose_volumes.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_choose_volumes.Location = new Point(967, 220);
            button_choose_volumes.Margin = new Padding(2);
            button_choose_volumes.Name = "button_choose_volumes";
            button_choose_volumes.Size = new Size(127, 30);
            button_choose_volumes.TabIndex = 23;
            button_choose_volumes.Text = "Обрати томи";
            button_choose_volumes.UseVisualStyleBackColor = true;
            button_choose_volumes.Click += button_choose_volumes_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1155, 489);
            Controls.Add(button_choose_volumes);
            Controls.Add(richTextBox5);
            Controls.Add(checkBox2);
            Controls.Add(comboBox1);
            Controls.Add(checkBox1);
            Controls.Add(label1);
            Controls.Add(label6);
            Controls.Add(richTextBox4);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(button_choose_files);
            Controls.Add(button_choose_archive);
            Controls.Add(richTextBox3);
            Controls.Add(label2);
            Controls.Add(richTextBox2);
            Controls.Add(button_dearchivate);
            Controls.Add(button_archivate);
            Controls.Add(richTextBox1);
            Controls.Add(button_choose_folder);
            Controls.Add(flowLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Архіватор";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_choose_folder;
        private RichTextBox richTextBox1;
        private Label label1;
        private Button button_archivate;
        private Button button_dearchivate;
        private RichTextBox richTextBox2;
        private Label label2;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label4;
        private RichTextBox richTextBox3;
        private Button button_choose_archive;
        private Button button_choose_files;
        private Label label3;
        private Label label5;
        private RichTextBox richTextBox4;
        private Label label6;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
        private CheckBox checkBox2;
        private RichTextBox richTextBox5;
        private Button button_choose_volumes;
    }
}