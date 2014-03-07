namespace FolderUITest
{
    partial class ImagePRO
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderTabs = new FolderUI.FolderTabs();
            this.clinical_tab = new FolderUI.FolderTabItem();
            this.patient_tab = new FolderUI.FolderTabItem();
            this.xrays_tab = new FolderUI.FolderTabItem();
            this.all_tab = new FolderUI.FolderTabItem();
            this.imagesSwitch = new FolderUI.ButtonSwitch();
            this.images_btn = new FolderUI.ButtonSwitchItem();
            this.layouts_btn = new FolderUI.ButtonSwitchItem();
            this.workspace = new FolderUI.Workspace();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.templatedListStatus = new FolderUI.TemplatedListStatus();
            this.templatedList = new FolderUI.TemplatedList();
            this.barPanel = new System.Windows.Forms.Panel();
            this.sourceSwitch = new FolderUI.ButtonSwitch();
            this.patient_btn = new FolderUI.ButtonSwitchItem();
            this.user_btn = new FolderUI.ButtonSwitchItem();
            this.miscSwitch = new FolderUI.ButtonSwitch();
            this.workspace.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderTabs
            // 
            this.folderTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.folderTabs.Items.Add(this.clinical_tab);
            this.folderTabs.Items.Add(this.patient_tab);
            this.folderTabs.Items.Add(this.xrays_tab);
            this.folderTabs.Items.Add(this.all_tab);
            this.folderTabs.Location = new System.Drawing.Point(212, 0);
            this.folderTabs.Margin = new System.Windows.Forms.Padding(0);
            this.folderTabs.Name = "folderTabs";
            this.folderTabs.Size = new System.Drawing.Size(776, 50);
            this.folderTabs.TabIndex = 7;
            this.folderTabs.SelectedItemChanged += new System.EventHandler(this.folderTabs_SelectedItemChanged);
            // 
            // clinical_tab
            // 
            this.clinical_tab.Name = "clinical_tab";
            this.clinical_tab.Text = "Clinical";
            // 
            // patient_tab
            // 
            this.patient_tab.Name = "patient_tab";
            this.patient_tab.Text = "Patient";
            // 
            // xrays_tab
            // 
            this.xrays_tab.Name = "xrays_tab";
            this.xrays_tab.Text = "X-Rays";
            // 
            // all_tab
            // 
            this.all_tab.Name = "all_tab";
            this.all_tab.Text = "All";
            // 
            // imagesSwitch
            // 
            this.imagesSwitch.Items.Add(this.images_btn);
            this.imagesSwitch.Items.Add(this.layouts_btn);
            this.imagesSwitch.Location = new System.Drawing.Point(0, 0);
            this.imagesSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.imagesSwitch.Name = "imagesSwitch";
            this.imagesSwitch.Size = new System.Drawing.Size(212, 50);
            this.imagesSwitch.TabIndex = 6;
            this.imagesSwitch.SelectedItemChanged += new System.EventHandler(this.formsSwitch_SelectedItemChanged);
            // 
            // images_btn
            // 
            this.images_btn.Icon = global::FolderUITest.Properties.Resources.icon_template_off;
            this.images_btn.Name = "images_btn";
            this.images_btn.SelectedIcon = global::FolderUITest.Properties.Resources.icon_template_on;
            this.images_btn.Text = "Images";
            this.images_btn.ToolTipText = "Images";
            // 
            // layouts_btn
            // 
            this.layouts_btn.Icon = global::FolderUITest.Properties.Resources.icon_form_off;
            this.layouts_btn.Name = "layouts_btn";
            this.layouts_btn.SelectedIcon = global::FolderUITest.Properties.Resources.icon_form_on;
            this.layouts_btn.Text = "Layouts";
            this.layouts_btn.ToolTipText = "Layouts";
            // 
            // workspace
            // 
            this.workspace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.workspace.Controls.Add(this.mainPanel);
            this.workspace.Controls.Add(this.sourceSwitch);
            this.workspace.Controls.Add(this.miscSwitch);
            this.workspace.Location = new System.Drawing.Point(0, 50);
            this.workspace.Margin = new System.Windows.Forms.Padding(0);
            this.workspace.Name = "workspace";
            this.workspace.Size = new System.Drawing.Size(988, 606);
            this.workspace.TabIndex = 5;
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Controls.Add(this.templatedListStatus);
            this.mainPanel.Controls.Add(this.templatedList);
            this.mainPanel.Controls.Add(this.barPanel);
            this.mainPanel.Location = new System.Drawing.Point(56, 10);
            this.mainPanel.MinimumSize = new System.Drawing.Size(0, 254);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(922, 586);
            this.mainPanel.TabIndex = 4;
            // 
            // templatedListStatus
            // 
            this.templatedListStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.templatedListStatus.Location = new System.Drawing.Point(0, 561);
            this.templatedListStatus.Margin = new System.Windows.Forms.Padding(0);
            this.templatedListStatus.Name = "templatedListStatus";
            this.templatedListStatus.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.templatedListStatus.Size = new System.Drawing.Size(922, 25);
            this.templatedListStatus.StatusIcon = null;
            this.templatedListStatus.StatusText = null;
            this.templatedListStatus.TabIndex = 2;
            this.templatedListStatus.ValueChanged += new System.EventHandler(this.templatedListStatus_ValueChanged);
            // 
            // templatedList
            // 
            this.templatedList.AllowDrop = true;
            this.templatedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.templatedList.Location = new System.Drawing.Point(6, 0);
            this.templatedList.Margin = new System.Windows.Forms.Padding(0);
            this.templatedList.Name = "templatedList";
            this.templatedList.Size = new System.Drawing.Size(916, 561);
            this.templatedList.TabIndex = 1;
            this.templatedList.TileSize = new System.Drawing.Size(112, 156);
            // 
            // barPanel
            // 
            this.barPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.barPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(187)))), ((int)(((byte)(254)))));
            this.barPanel.Location = new System.Drawing.Point(0, 0);
            this.barPanel.Name = "barPanel";
            this.barPanel.Size = new System.Drawing.Size(6, 561);
            this.barPanel.TabIndex = 0;
            // 
            // sourceSwitch
            // 
            this.sourceSwitch.BarSize = 0;
            this.sourceSwitch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sourceSwitch.Items.Add(this.patient_btn);
            this.sourceSwitch.Items.Add(this.user_btn);
            this.sourceSwitch.Location = new System.Drawing.Point(12, 17);
            this.sourceSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.sourceSwitch.Name = "sourceSwitch";
            this.sourceSwitch.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.sourceSwitch.Size = new System.Drawing.Size(44, 68);
            this.sourceSwitch.TabIndex = 3;
            this.sourceSwitch.SelectedItemChanged += new System.EventHandler(this.sourceSwitch_SelectedItemChanged);
            // 
            // patient_btn
            // 
            this.patient_btn.Icon = global::FolderUITest.Properties.Resources.icon_form_off;
            this.patient_btn.Name = "patient_btn";
            this.patient_btn.SelectedIcon = global::FolderUITest.Properties.Resources.icon_form_on;
            this.patient_btn.Text = "Patient";
            this.patient_btn.ToolTipText = "Patient";
            // 
            // user_btn
            // 
            this.user_btn.Icon = global::FolderUITest.Properties.Resources.icon_template_off;
            this.user_btn.Name = "user_btn";
            this.user_btn.SelectedIcon = global::FolderUITest.Properties.Resources.icon_template_on;
            this.user_btn.Text = "User";
            this.user_btn.ToolTipText = "User";
            // 
            // miscSwitch
            // 
            this.miscSwitch.BarSize = 0;
            this.miscSwitch.Location = new System.Drawing.Point(12, 94);
            this.miscSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.miscSwitch.Name = "miscSwitch";
            this.miscSwitch.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.miscSwitch.Size = new System.Drawing.Size(44, 170);
            this.miscSwitch.TabIndex = 2;
            this.miscSwitch.SelectedItemChanged += new System.EventHandler(this.miscSwitch_SelectedItemChanged);
            // 
            // ImagePRO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.folderTabs);
            this.Controls.Add(this.imagesSwitch);
            this.Controls.Add(this.workspace);
            this.Name = "ImagePRO";
            this.Size = new System.Drawing.Size(988, 656);
            this.workspace.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FolderUI.Workspace workspace;
        private FolderUI.ButtonSwitch miscSwitch;
        private FolderUI.ButtonSwitch sourceSwitch;
        private FolderUI.ButtonSwitch imagesSwitch;
        private FolderUI.FolderTabs folderTabs;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel barPanel;
        private FolderUI.TemplatedList templatedList;
        private FolderUI.FolderTabItem clinical_tab;
        private FolderUI.FolderTabItem patient_tab;
        private FolderUI.FolderTabItem xrays_tab;
        private FolderUI.ButtonSwitchItem images_btn;
        private FolderUI.ButtonSwitchItem layouts_btn;
        private FolderUI.ButtonSwitchItem patient_btn;
        private FolderUI.ButtonSwitchItem user_btn;
        private FolderUI.FolderTabItem all_tab;
        private FolderUI.TemplatedListStatus templatedListStatus;
    }
}
