using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace goruntuisleme
{
    
    public class ResizeEventArgs: EventArgs
    {
        public readonly int Width;
        public readonly int Height;
        public ResizeEventArgs(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
    }

    public class ResizablePanel : Panel
    {
        public bool isResizing = false;
        private Point lastMousePosition;
        public  int resizeMargin;

        

        public int tempWidth;
        public int tempHeight;

       

        public event EventHandler tempSizeChanged;

        public ResizablePanel()
        {
            this.DoubleBuffered = true; // Flickering'i azaltır
            this.BorderStyle = BorderStyle.FixedSingle;

            resizeMargin = (Width + Height) /3;

            this.MouseDown += ResizablePanel_MouseDown;
            this.MouseMove += ResizablePanel_MouseMove;
            this.MouseUp += ResizablePanel_MouseUp;

        }

        private void ResizablePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && IsInResizeZone(e.Location))
            {
                isResizing = true;
                tempWidth = Width;
                tempHeight = Height;
                lastMousePosition = e.Location;
                this.Cursor = Cursors.SizeNWSE;
            }
        }

        private void ResizablePanel_MouseMove(object sender, MouseEventArgs e)
        {
            

            if (IsInResizeZone(e.Location))
            {
                this.Cursor = Cursors.SizeNWSE; // Sağ alt köşede imleç değişir
            }
            else
            {
                this.Cursor = Cursors.Default;
            }

            if (isResizing)
            {
                int widthChange = e.X - lastMousePosition.X;
                int heightChange = e.Y - lastMousePosition.Y;

                this.tempWidth = Math.Max(50, this.tempWidth + widthChange);
                this.tempHeight = Math.Max(50, this.tempHeight + heightChange);

                lastMousePosition = e.Location;

                tempSizeChanged?.Invoke(this, new ResizeEventArgs(tempWidth, tempHeight));
            }
        }

        private void ResizablePanel_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
            tempWidth = Width;
            tempHeight = Height;
            this.Cursor = Cursors.Default;
        }

        private bool IsInResizeZone(Point location)
        {
            return location.X >= this.Width - resizeMargin && location.Y >= this.Height - resizeMargin;
        }
    }

}
