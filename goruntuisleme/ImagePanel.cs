using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goruntuisleme
{
    internal class ImagePanel : System.Windows.Forms.Panel
    {
        System.Windows.Forms.PictureBox pictureBox;
        System.Windows.Forms.ComboBox filterComboBox;

        private IImageObserver observer;

        Bitmap image;

        public ImagePanel(int x, int y, Bitmap mainImage)
        {
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Location = new System.Drawing.Point(x, y);
            this.Size = new System.Drawing.Size(270, 300);
            fillPanel();

            this.image = mainImage;

            if (this.image != null)
            {
                setImage(this.image);
            }

            this.pictureBox.Click += (sender, e) => openImage();
        }

        private void openImage()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                Bitmap bitmap = new Bitmap(filePath);
                observer.OnImageSelected(bitmap);
            }

        }
        private void fillPanel()
        {
            pictureBox = new System.Windows.Forms.PictureBox();
            pictureBox.Location = new System.Drawing.Point(7, 7);
            pictureBox.Size = new System.Drawing.Size(256, 256);
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);

            filterComboBox = new System.Windows.Forms.ComboBox();
            filterComboBox.Location = new System.Drawing.Point(7, 270);
            filterComboBox.Size = new System.Drawing.Size(256, 21);
            filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; // Bu satırı ekleyin

            filterComboBox.Items.AddRange(new string[] { "None", "Gray", "Negative", "Sepia" });
            filterComboBox.SelectedIndex = 0;
            filterComboBox.SelectedIndexChanged += (sender, e) => applyFilter();
            this.Controls.Add(filterComboBox);


        }

        public void applyFilter()
        {
            Bitmap filtered = new Bitmap(this.image);

            switch (filterComboBox.SelectedIndex)
            {
                case 1:
                    filtered =  ImageFilters.Grayscale(filtered);
                    break;
                case 2:
                    filtered = ImageFilters.Negative(filtered);
                    break;
                case 3:
                    filtered = ImageFilters.Sepia(filtered);
                    break;

            }

            pictureBox.Image = filtered;

        }

        public void setImage(Bitmap image)
        {
            this.image = image;
            applyFilter();

        }


        public void AddObserver(IImageObserver observer)
        {
            this.observer = observer;
        }

    }
}
