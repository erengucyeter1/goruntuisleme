using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goruntuisleme
{
    internal class PanelHandler : IImageObserver
    {
        Panel mainPanel;

        Button btnAddPanel;

        private byte columnSize{ get;} = 3;
        private byte rowSize { get;} = 2;

        private byte maxPanelCount { get; set; }

        private List<ImagePanel> panels = new List<ImagePanel>();


        Bitmap mainImage;

        public void OnImageSelected(Bitmap selectedImage)
        {
            this.mainImage = selectedImage;

            foreach (ImagePanel panel in panels)
            {
                panel.setImage(mainImage);
            }

        }


        public PanelHandler(System.Windows.Forms.Panel mainPanel)
        {
            maxPanelCount = (byte)(columnSize * rowSize);

            this.mainPanel = mainPanel;
            this.btnAddPanel = new System.Windows.Forms.Button();
            this.btnAddPanel.Size = new System.Drawing.Size(270, 300);
            this.btnAddPanel.Text = "+";
            this.btnAddPanel.Font = new System.Drawing.Font("Microsoft Sans Serif",50);
            this.btnAddPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddPanel.Click += (sender, e) => AddPanel();
            this.mainPanel.Controls.Add(btnAddPanel);

            AddPanel();

        }


        private void newPanel()
        {
            int[] location = getLocation();
            ImagePanel panel = new ImagePanel(location[0], location[1], mainImage);
            panel.AddObserver(this);
            panels.Add(panel);
            mainPanel.Controls.Add(panel);

        }

        private void moveButton()
        {
            int[] location = getLocation();
            btnAddPanel.Location = new System.Drawing.Point(location[0], location[1]);

        }
        private int[] getLocation()
        {
            byte x = (byte)(this.panels.Count % columnSize);
            byte y = (byte)(this.panels.Count / columnSize);

            return new int[] { 12 + (x * 282), 12 + (y * 312) };
        }

        public void AddPanel()
        {


            if (this.panels.Count < maxPanelCount)
            {
                newPanel();
            }
            if (this.panels.Count < maxPanelCount)
            {
                moveButton();
            }
            else
            {
                btnAddPanel.Enabled = false;
                btnAddPanel.Visible = false;
            }


        }


    }
}
