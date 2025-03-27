
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace goruntuisleme
{
    internal class ImagePanel : ResizablePanel
    {
        System.Windows.Forms.PictureBox pictureBox;
        System.Windows.Forms.ComboBox filterComboBox;

        private IImageObserver observer;

        List<object> TempComponents = new List<object>();

        Bitmap image;
        int imgSize;

        public int Number { get; set; } = 0;

        public ImagePanel(Bitmap mainImage) : base()
        {
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            fillPanel();

            this.image = mainImage;

            if (this.image != null)
            {
                setImage(this.image);
            }


            this.pictureBox.Click += (sender, e) => openImage();
            this.tempSizeChanged += (sender, e) => updateComponents((ResizeEventArgs)e);
        }

        private void updateComponents(ResizeEventArgs e)
        {
            if (base.isResizing)
            {
                observer.OnPanelResized(e.Width, e.Height);
            }
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
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

           
            this.Controls.Add(pictureBox);

            filterComboBox = new System.Windows.Forms.ComboBox();
            filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            filterComboBox.Items.AddRange(new string[] { "None", "Gray", "Negative", "Sepia", "meangray", "binarize", "rotate"});
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
                case 4:
                    filtered = ImageFilters.MaenGrayscale(filtered);
                    break;
                case 5:
                    filtered = ImageFilters.Binarize(filtered);
                    break;
                case 6:
                    SetComboBoxForRotate();

                    break;

            }
            
            if (filterComboBox.SelectedIndex != 6)
            {
                DeleteTepmsFromControls();
            }

            pictureBox.Image = filtered;

        }


        private void SetComboBoxForRotate()
        {
            filterComboBox.Size = new System.Drawing.Size(imgSize / 2, 21);

            NumericUpDown numericUpDown = new NumericUpDown();
            numericUpDown.Location = new System.Drawing.Point(((imgSize / 2) +27), imgSize + 14);
            numericUpDown.Size = new System.Drawing.Size(40, 21);
            numericUpDown.Maximum = 360;
            numericUpDown.Minimum = 0;
            numericUpDown.Value = 0;
            TempComponents.Add(numericUpDown);
            this.Controls.Add(numericUpDown);

            Button button = new Button();
            button.Location = new System.Drawing.Point(((imgSize / 2) + 70), imgSize + 14);
            button.Size = new System.Drawing.Size(40, 21);
            button.Text = "Apply";
            button.Click += (sender, e) =>
            {
                int angle = (int)numericUpDown.Value;
                Bitmap rotated = ImageFilters.RotateImage(image, angle);
                pictureBox.Image = rotated;
            };
            TempComponents.Add(button);
            this.Controls.Add(button);
        }

        private void DeleteTepmsFromControls()
        {
            foreach (var item in TempComponents)
            {
                this.Controls.Remove((Control)item);
            }
            TempComponents.Clear();
            filterComboBox.Size = new System.Drawing.Size(imgSize, 21);

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

        public void UpdateSize(int[] location, int imgSize)
        {
            this.imgSize = imgSize;
            this.Location = new System.Drawing.Point(location[0], location[1]);
            pictureBox.Size = new System.Drawing.Size(imgSize, imgSize);
            filterComboBox.Location = new System.Drawing.Point(7, imgSize  + 14);
            filterComboBox.Size = new System.Drawing.Size(imgSize, 21);
            this.Size = new System.Drawing.Size(imgSize + 14, imgSize + 44);

        }

        

    }
}
