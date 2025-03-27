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

        private byte columnSize{ get;} = 3;
        private byte rowSize { get;} = 2;

        private byte maxPanelCount { get => ((byte)(columnSize * rowSize)); }

        private List<ImagePanel> panels = new List<ImagePanel>();

        private int[] sizeList = { 800,600, 375, 375, 375, 375 };

        Bitmap mainImage;

        public void OnImageSelected(Bitmap selectedImage)
        {
            this.mainImage = selectedImage;

            foreach (ImagePanel panel in panels)
            {
                panel.setImage(mainImage);
            }

        }

        public void  OnPanelResized( int Width, int Height)
        {
            


            int size = ((Width + Height) / 2);
            if (panels.Count < maxPanelCount && size < sizeList[panels.Count])
            {
                AddPanel();

            }
            else if(size > sizeList[panels.Count - 1])
            {
                if (panels.Count > 1)
                {
                    panels[panels.Count - 1].Dispose();
                    panels.RemoveAt(panels.Count - 1);
                    updatePanels();
                }
            }
        }



        public PanelHandler(System.Windows.Forms.Panel mainPanel)
        {

            this.mainPanel = mainPanel;
            mainPanel.DoubleClick += (sender, e) => AddPanel();
            AddPanel();

        }


        private void updatePanels()
        {
            int imageSize = sizeList[panels.Count- 1];
            foreach (ImagePanel panel in panels)
            {
                panel.UpdateSize(getLocation(panel.Number, imageSize), imageSize);
                panel.resizeMargin = (panel.Width + panel.Height) / 3;

            }


        }

        private void newPanel()
        {
            ImagePanel panel = new ImagePanel(mainImage);
            panel.Number = panels.Count;
            panel.AddObserver(this);
            panels.Add(panel);
            updatePanels();
            mainPanel.Controls.Add(panel);

        }

        
        private int[] getLocation(int number, int imgSize)
        {
            byte x = (byte)(number % columnSize);
            byte y = (byte)(number / columnSize);
            if(number > 2)
            {
                y = 1;
                x = (byte)(number - 3);

            }
            return new int[] { 12 + (x * (imgSize + 26)), 12 + (y * (imgSize + 56)) };
        }

        public void AddPanel()
        {


            if (this.panels.Count < maxPanelCount)
            {
                newPanel();
            }

        }


    }
}
