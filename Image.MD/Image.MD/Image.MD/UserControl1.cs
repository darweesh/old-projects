using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Image.MD
{
    public partial class UserControl1 : UserControl
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
        Form1 mf;

        ImageBitsPerPixel bpp;

        public UserControl1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            pix8 = new List<byte>();
            pix16 = new List<ushort>();

            this.hScrollBar.Visible = false;
            this.vScrollBar.Visible = false;

            winMin = 0;
            winMax = 65535;

            ptWLDown = new Point();
            changeValWidth = 0.5;
            changeValCentre = 0.5;
            rightMouseDown = false;
            imageAvailable = false;

            lut8 = new byte[256];
            lut16 = new byte[65536];

            PerformResize();
        }

        public bool NewImage
        {
            set
            {
                newImage = value;
            }
        }

        public void SetParameters(ref List<byte> arr, int wid, int hei, double windowWidth,
            double windowCentre, bool resetScroll, Form1 mainFrm)
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

            mf = mainFrm;
            imageAvailable = true;
            if (bmp != null)
                bmp.Dispose();
            ResetValues();
            ComputeLookUpTable8();
            bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CreateImage8();
            if (resetScroll == true) ComputeScrollBarParameters();
            Invalidate();
        }

        public void SetParameters(ref List<ushort> arr, int wid, int hei, double windowWidth,
            double windowCentre, bool resetScroll, Form1 mainFrm)
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
            else if (Width > 40000)
            {
                changeValWidth = 50;
                changeValCentre = 50;
            }
            else // it is inbetween 5000 and 40000
            {
                changeValWidth = 25;
                changeValCentre = 25;
            }

            pix16 = arr;
            imagePixels16 = new byte[sizeImg3];

            mf = mainFrm;
            imageAvailable = true;
            if (bmp != null)
                bmp.Dispose();
            ResetValues();
            ComputeLookUpTable16();
            bmp = new Bitmap(imgWidth, imgHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            CreateImage16();
            if (resetScroll == true) ComputeScrollBarParameters();
            Invalidate();
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

        private void ComputeScrollBarParameters()
        {
            //panWidth = panel.Width;
            //panHeight = panel.Height;

            //hOffset = (panWidth - imgWidth) / 2;
            //vOffset = (panHeight - imgHeight) / 2;

            //if (imgWidth < panWidth)
            //{
            //    hScrollBar.Visible = false;
            //}
            //else
            //{
            //    hScrollBar.Visible = true;
            //    hScrollBar.Value = (hScrollBar.Maximum + 1 -
            //        hScrollBar.LargeChange - hScrollBar.Minimum) / 2;
            //}

            //if (imgHeight < panHeight)
            //{
            //    vScrollBar.Visible = false;
            //}
            //else
            //{
            //    vScrollBar.Visible = true;
            //    vScrollBar.Value = (vScrollBar.Maximum + 1 -
            //        vScrollBar.LargeChange - vScrollBar.Minimum) / 2;
            //}
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int val = vScrollBar.Value;
            vOffset = (panHeight - imgHeight) * (val - vScrollBar.Minimum) /
                    (vMax - vScrollBar.Minimum);
            Invalidate();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int val = hScrollBar.Value;
            hOffset = (panWidth - imgWidth) * (val - hScrollBar.Minimum) /
                (hMax - hScrollBar.Minimum);
            Invalidate();
        }

        private void ImagePanel_Paint(object sender, PaintEventArgs e)
        {
            if (bpp == ImageBitsPerPixel.Eight)
            {
                if (pix8.Count > 0)
                {
                    //Graphics g = Graphics.FromHwnd(panel.Handle);
                    //if (newImage == true)
                    //{
                    //    g.Clear(SystemColors.Control);
                    //    newImage = false;
                    //}

                    //g.DrawImage(bmp, hOffset, vOffset);
                    //g.Dispose();
                }
            }
            else //if (bpp == ImageBitsPerPixel.Sixteen)
            {
                //if (pix16.Count > 0)
                //{
                //    Graphics g = Graphics.FromHwnd(panel.Handle);
                //    if (newImage == true)
                //    {
                //        g.Clear(SystemColors.Control);
                //        newImage = false;
                //    }

                //    g.DrawImage(bmp, 0, 0, panel.Width, panel.Height);
                //    //panel.BackgroundImage = bmp;
                //    g.Dispose();
                //}
            }
        }

        public void SaveImage(String fileName)
        {
            if (bmp != null)
                bmp.Save(fileName, ImageFormat.Png);
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

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            //if (imageAvailable == true)
            //{
            //    if (e.Button == MouseButtons.Right)
            //    {
            //        ptWLDown.X = e.X;
            //        ptWLDown.Y = e.Y;
            //        rightMouseDown = true;
            //        Cursor = Cursors.Hand;
            //    }
            //}
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (rightMouseDown == true)
            {
                winShr1 = winWidth >> 1;
                winWidth = winMax - winMin;
                winCentre = winMin + winShr1;

                deltaX = Convert.ToInt32((ptWLDown.X - e.X) * changeValWidth);
                deltaY = Convert.ToInt32((ptWLDown.Y - e.Y) * changeValCentre);

                winCentre -= deltaY;
                winWidth -= deltaX;

                if (winWidth < 2) winWidth = 2;
                if (winWidth > 5000) winWidth = 5000;
                if (winCentre < -2500) winCentre = -2500;
                if (winCentre > 2500) winCentre = 2500;

                winMin = winCentre - winShr1;
                winMax = winCentre + winShr1;

                if (winMin >= winMax) winMin = winMax - 1;
                if (winMax <= winMin) winMax = winMin + 1;

                ptWLDown.X = e.X;
                ptWLDown.Y = e.Y;

                UpdateMainForm();
                if (bpp == ImageBitsPerPixel.Eight)
                {
                    ComputeLookUpTable8();
                    CreateImage8();
                }
                if (bpp == ImageBitsPerPixel.Sixteen)
                {
                    ComputeLookUpTable16();
                    CreateImage16();
                }
                Invalidate();
            }
        }
      

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (rightMouseDown == true)
            {
                rightMouseDown = false;
                Cursor = Cursors.Default;
            }
        }

        private void UpdateMainForm()
        {
           mf.UpdateWindowLevel(winWidth, winCentre, bpp);
        }

        public void ResetValues()
        {
            winMax = Convert.ToInt32(winCentre + 0.5 * winWidth);
            winMin = winMax - winWidth;
            UpdateMainForm();
        }

        private void ImagePanelControl_Resize(object sender, EventArgs e)
        {
            PerformResize();
        }

        private void PerformResize()
        {
            panel.Location = new Point(3, 3);
            panel.Width = ClientRectangle.Width-5 ;
            panel.Height = ClientRectangle.Height-5 ;

            vScrollBar.Location = new Point(ClientRectangle.Width - 19, 3);
            vScrollBar.Height = panel.Height;

            hScrollBar.Location = new Point(3, ClientRectangle.Height - 19);
            hScrollBar.Width = panel.Width;

            hMax = hScrollBar.Maximum - hScrollBar.LargeChange + hScrollBar.SmallChange;
            vMax = vScrollBar.Maximum - vScrollBar.LargeChange + vScrollBar.SmallChange;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
