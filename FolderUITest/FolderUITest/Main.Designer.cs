namespace FolderUITest
{
    partial class Main
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
            this.itemSwitch1 = new FolderUI.ButtonSwitch();
            this.formPRO1 = new FolderUITest.ImagePRO();
            this.SuspendLayout();
            // 
            // itemSwitch1
            // 
            this.itemSwitch1.GradientTop = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.itemSwitch1.HoverGradientTop = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.itemSwitch1.Location = new System.Drawing.Point(0, 0);
            this.itemSwitch1.Margin = new System.Windows.Forms.Padding(0);
            this.itemSwitch1.Name = "itemSwitch1";
            this.itemSwitch1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.itemSwitch1.Size = new System.Drawing.Size(150, 150);
            this.itemSwitch1.TabIndex = 0;
            // 
            // formPRO1
            // 
            this.formPRO1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formPRO1.Location = new System.Drawing.Point(0, 0);
            this.formPRO1.Name = "formPRO1";
            this.formPRO1.Size = new System.Drawing.Size(1051, 595);
            this.formPRO1.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 595);
            this.Controls.Add(this.formPRO1);
            this.Name = "Main";
            this.Text = "Folder UI Test";
            this.ResumeLayout(false);

        }

        #endregion

        private FolderUI.ButtonSwitch itemSwitch1;
        private ImagePRO formPRO1;

    }
}

