namespace _3kursova_Archivator
{
    partial class FormArchiveExists
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormArchiveExists));
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(64, 64);
            label1.Name = "label1";
            label1.Size = new Size(695, 38);
            label1.TabIndex = 0;
            label1.Text = "Архів з таким іменем вже існує! Як продовжити?";
            // 
            // button1
            // 
            button1.DialogResult = DialogResult.Yes;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(23, 174);
            button1.Name = "button1";
            button1.Size = new Size(238, 87);
            button1.TabIndex = 2;
            button1.Text = "Додати файли до існуючого";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.DialogResult = DialogResult.No;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(292, 174);
            button2.Name = "button2";
            button2.Size = new Size(238, 87);
            button2.TabIndex = 3;
            button2.Text = "Замінити архів новим";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.DialogResult = DialogResult.Cancel;
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button3.Location = new Point(559, 174);
            button3.Name = "button3";
            button3.Size = new Size(238, 87);
            button3.TabIndex = 4;
            button3.Text = "Відмінити";
            button3.UseVisualStyleBackColor = true;
            // 
            // FormArchiveExists
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 317);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormArchiveExists";
            Text = "Архів з таким іменем вже існує!";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}