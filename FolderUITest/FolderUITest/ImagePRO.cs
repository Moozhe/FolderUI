using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FolderUI;

namespace FolderUITest
{
    public partial class ImagePRO : UserControl
    {
        private List<Category> categories = new List<Category>();
        private List<ClientImage> images = new List<ClientImage>();
        private int orgId = 1;
        private int userId = -1;
        private int clientId = 4037;
        private int catId = -1;

        private Category periapicalCat;
        private Category bitewingCat;
        private Category panoramicCat;
        private Category clinicalCat;

        public ImagePRO()
        {
            InitializeComponent();

            categories.AddRange(new Category[]
            {
                periapicalCat = new Category(1, "Periapical"),
                bitewingCat = new Category(2, "Bitewing"),
                panoramicCat = new Category(3, "Panoramic"),
                clinicalCat = new Category(4, "Clinical"),
            });

            images.AddRange(new ClientImage[]
            {
                
            });

            UpdateItems();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            templatedList.Select();
        }

        private void UpdateItems()
        {
            templatedList.Items.Clear();
            templatedList.Groups.Clear();

            foreach (ClientImage xClientImage in images)
            {
                if ((userId != -1 && xClientImage.UserId != userId) ||
                    (clientId != -1 && xClientImage.ClientId != clientId) ||
                    (catId != -1 && xClientImage.Category.Id != catId))
                {
                    continue;
                }

                string xCategoryKey = xClientImage.Category.Id.ToString();

                if (!templatedList.Groups.ContainsKey(xCategoryKey))
                {
                    templatedList.Groups.Add(new FolderTemplatedGroup(xClientImage.Category.Name, imagesSwitch.SelectedItem.SelectedIcon, xCategoryKey, xClientImage.Category));
                }

                templatedList.Items.Add(new ClientImageListItem(xClientImage, templatedList.Groups[xCategoryKey]));
            }
        }

        private void formsSwitch_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void folderTabs_SelectedItemChanged(object sender, EventArgs e)
        {
        }

        private void sourceSwitch_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void miscSwitch_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void templatedListStatus_ValueChanged(object sender, EventArgs e)
        {
            templatedList.ZoomPercent = templatedListStatus.Value;
        }
    }
}
