using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goruntuisleme
{
    public interface IImageObserver
    {
        void OnImageSelected(Bitmap selectedImage);
        void OnPanelResized( int Width, int Height);


    }

}
