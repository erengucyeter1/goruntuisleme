using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goruntuisleme
{
    internal class ImageFilters
    {

        public static Bitmap Grayscale(Bitmap image)
        {
            Bitmap grayScaleImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            BitmapData bmData = grayScaleImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                         ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                 ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(bmData.Stride) * image.Height;
            byte[] buffer = new byte[bytes];
            byte[] srcBuffer = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);
            image.UnlockBits(srcData);

            for (int i = 0; i < srcBuffer.Length; i += 4)
            {
                int gray = (int)(0.3 * srcBuffer[i + 2] + 0.59 * srcBuffer[i + 1] + 0.11 * srcBuffer[i]);
                buffer[i] = buffer[i + 1] = buffer[i + 2] = (byte)gray; // R=G=B=Gray
                buffer[i + 3] = srcBuffer[i + 3]; // Alpha değeri korunur
            }

            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmData.Scan0, bytes);
            grayScaleImage.UnlockBits(bmData);

            return grayScaleImage;
        }

        public static Bitmap Sepia(Bitmap image)
        {
            BitmapData bmData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(bmData.Stride) * image.Height;
            byte[] buffer = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(bmData.Scan0, buffer, 0, bytes);

            for (int i = 0; i < buffer.Length; i += 4)
            {
                byte blue = buffer[i];
                byte green = buffer[i + 1];
                byte red = buffer[i + 2];

                byte sepiaRed = (byte)Math.Min(255, (0.393 * red + 0.769 * green + 0.189 * blue));
                byte sepiaGreen = (byte)Math.Min(255, (0.349 * red + 0.686 * green + 0.168 * blue));
                byte sepiaBlue = (byte)Math.Min(255, (0.272 * red + 0.534 * green + 0.131 * blue));

                buffer[i] = sepiaBlue;
                buffer[i + 1] = sepiaGreen;
                buffer[i + 2] = sepiaRed;
            }

            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmData.Scan0, bytes);
            image.UnlockBits(bmData);

            return image;
        }

        public static Bitmap Negative(Bitmap image)
        {
            BitmapData bmData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(bmData.Stride) * image.Height;
            byte[] buffer = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(bmData.Scan0, buffer, 0, bytes);

            for (int i = 0; i < buffer.Length; i += 4)
            {
                buffer[i] = (byte)(255 - buffer[i]);       // Mavi (B)
                buffer[i + 1] = (byte)(255 - buffer[i + 1]); // Yeşil (G)
                buffer[i + 2] = (byte)(255 - buffer[i + 2]); // Kırmızı (R)
            }

            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmData.Scan0, bytes);
            image.UnlockBits(bmData);

            return image;
        }


    }
}
