using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MRIlib
{ public enum ImageBitsPerPixel { Eight, Sixteen };
    public  class  Imagemri
    {
        List<byte> pix8;
        List<ushort> pix16;
        public Bitmap bmp;
        int hOffset;
        int vOffset;
        int hMax;
        int vMax;
        int imgWidth;
        int imgHeight;
        int panWidth;
        int panHeight;
        bool newImage;

        // For Window Level
        int winMin;
        int winMax;
        int winCentre;
        int winWidth;
        int winShr1;
        int deltaX;
        int deltaY;

        Point ptWLDown;
        double changeValWidth;
        double changeValCentre;
        bool rightMouseDown;
        bool imageAvailable;

        byte[] lut8;
        byte[] lut16;

        byte[] imagePixels8;
        byte[] imagePixels16;
        int sizeImg;
        int sizeImg3;


        ImageBitsPerPixel bpp;

        public Imagemri()
        {
           
          

            pix8 = new List<byte>();
            pix16 = new List<ushort>();

           

            winMin = 0;
            winMax = 65535;

            ptWLDown = new Point();
            changeValWidth = 0.5;
            changeValCentre = 0.5;
            rightMouseDown = false;
            imageAvailable = false;

            lut8 = new byte[256];
            lut16 = new byte[65536];

           
        }

        public bool NewImage
        {
            set
            {
                newImage = value;
            }
        }

        public void SetParameters(ref List<byte> arr, int wid, int hei, double windowWidth,
            double windowCentre, bool resetScroll)
        {
            bpp = ImageBitsPerPixel.Eight;
            imgWidth = wid;
            imgHeight = hei;
            winWidth = Convert.ToInt32(windowWidth);
            winCentre = Convert.ToInt32(windowCentre);
            changeValWidth = 0.1;
            changeValCentre = 0.1;
            sizeImg = imgWidth * imgHeight;
            sizeImg3 = sizeImg * 3;

            pix8 = arr;
            imagePixels8 = new byte[sizeImg3];

          
            imageAvailable = true;
            if (bmp != null)
                bmp.Dispose();
            ResetValues();
            ComputeLookUpTable8();
            bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CreateImage8();
            
            
        }

        public void SetParameters(ref List<ushort> arr, int wid, int hei, double windowWidth,
            double windowCentre, bool resetScroll)
        {
            
            bpp = ImageBitsPerPixel.Sixteen;
            imgWidth = wid;
            imgHeight = hei;
            winWidth = Convert.ToInt32(windowWidth);
            winCentre = Convert.ToInt32(windowCentre);

            sizeImg = imgWidth * imgHeight;
            sizeImg3 = sizeImg * 3;
            double sizeImg3By4 = sizeImg3 / 4.0;

            // Modify the 'sensitivity' of the mouse based on the original window width
            if (winWidth < 5000)
            {
                changeValWidth = 2;
                changeValCentre = 2;
            }
           
            else // it is inbetween 5000 and 40000
            {
                changeValWidth = 25;
                changeValCentre = 25;
            }

            pix16 = arr;
            imagePixels16 = new byte[sizeImg3];

           
            imageAvailable = true;
            if (bmp != null)
                bmp.Dispose();
            ResetValues();
            ComputeLookUpTable16();
            bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CreateImage16();
          
            
        }

        private void CreateImage8()
        {
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, imgWidth, imgHeight),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            unsafe
            {
                int pixelSize = 3;
                int i, j, j1, i1;
                byte b;

                for (i = 0; i < bmd.Height; ++i)
                {
                    byte* row = (byte*)bmd.Scan0 + (i * bmd.Stride);
                    i1 = i * bmd.Width;

                    for (j = 0; j < bmd.Width; ++j)
                    {
                        b = lut8[pix8[i * bmd.Width + j]];
                        j1 = j * pixelSize;
                        row[j1] = b;            // Red
                        row[j1 + 1] = b;        // Green
                        row[j1 + 2] = b;        // Blue
                    }
                }
            }

            IntPtr ptr = bmd.Scan0;
            bmp.UnlockBits(bmd);
        }

        private void CreateImage16()
        {
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, imgWidth, imgHeight),
               System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

            unsafe
            {
                int pixelSize = 3;
               

                System.Threading.Tasks.Parallel.For(0, bmd.Height, delegate(int i)
                { 
                    int j, j1, i1;
                    byte b;
                    byte* row = (byte*)bmd.Scan0 + (i * bmd.Stride);
                    i1 = i * bmd.Width;

                    for (j = 0; j < bmd.Width; ++j)
                    {
                        b = lut16[pix16[i * bmd.Width + j]];
                        j1 = j * pixelSize;
                        row[j1] = b;            // Red
                        row[j1 + 1] = b;        // Green
                        row[j1 + 2] = b;        // Blue
                    }
                });
            }

            IntPtr ptr = bmd.Scan0;
            bmp.UnlockBits(bmd);
        }

      
        

        private void ComputeLookUpTable8()
        {
            int range = winMax - winMin;
            if (range < 1) range = 1;
            double factor = 255.0 / range;

            for (int i = 0; i < 256; ++i)
            {
                if (i <= winMin)
                    lut8[i] = 0;
                else if (i >= winMax)
                    lut8[i] = 255;
                else
                {
                    lut8[i] = (byte)((i - winMin) * factor);
                }
            }
        }

        private void ComputeLookUpTable16()
        {
            int range = winMax - winMin;
            if (range < 1) range = 1;
            double factor = 255.0 / winWidth;
            

           System.Threading.Tasks.Parallel.For(0,65536,delegate(int i)
            {
                if (i <= winMin)
                    lut16[i] = 0;
                else if (i >= winWidth)
                    lut16[i] = 255;
                else 
                {
                    lut16[i] = (byte)((i - winMin) * factor);
                }
                
            });
        }

       

      

    
        public void ResetValues()
        {
            winMax = Convert.ToInt32(winCentre + 0.5 * winWidth);
            winMin = winMax - winWidth;
           
        }

       

       

       
    }
}
