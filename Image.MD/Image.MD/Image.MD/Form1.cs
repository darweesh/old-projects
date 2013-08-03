using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Threading;
using System.Drawing.Imaging;

namespace Image.MD
{

    public enum ImageBitsPerPixel { Eight, Sixteen };
    public partial class Form1 : Form
    {
        #region my variables

        DicomDecoder dd;       //
        List<byte> pixels8;   //
        List<ushort> pixels16;//
        int imageWidth;      //
        int imageHeight;    //
        int bitDepth;      //
        bool imageOpened;
        double winCentre;
        double winWidth;
        string[] files;
        Info myInfo;
        int ii = 0;
        myPic o;
        double brightness;
        int thresholding;
        bool processI;
        //int serisenum;
        List<lesion> mylesions;
        #endregion

        public Form1()
        {
            InitializeComponent();
            #region my init
            dd = new DicomDecoder();
            pixels8 = new List<byte>();
            pixels16 = new List<ushort>();
            imageOpened = false;
            myInfo = new Info();
            brightness = 0;
            thresholding = 128;
            processI = false;
            mylesions=new List<lesion>();
            #endregion

        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All DICOM Files(*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Length > 0)
                {
                    Cursor = Cursors.WaitCursor;
                    ReadAndDisplayDicomFile(ofd.FileName, ofd.SafeFileName);
                    imageOpened = true;
                    Cursor = Cursors.Default;
                }
                ofd.Dispose();

            }


        }
        private void ReadAndDisplayDicomFile(string fileName, string fileNameOnly)
        {


            dd.DicomFileName = fileName;
            bool result = dd.dicomFileReadSuccess;
            if (result == true)
            {
                imageWidth = dd.width;
                imageHeight = dd.height;
                bitDepth = dd.bitsAllocated;
                winCentre = dd.windowCentre;
                winWidth = dd.windowWidth;


                StatusLabel1.Text = fileName + ": " + imageWidth.ToString() + " X " + imageHeight.ToString();
                StatusLabel1.Text += "  " + bitDepth.ToString() + " bits per pixel";

                userControl11.NewImage = true;
                Text = "DICOM Image Viewer: " + fileNameOnly;
               

                if (bitDepth == 16)
                {
                    pixels16.Clear();
                    pixels8.Clear();
                    dd.GetPixels16(ref pixels16);
                    byte[] buffer = new byte[pixels16.Count * 2];
                    byte[] temp;
                    ByteConverter d = new ByteConverter();
                    int j = 0;
                    for (int i = 0; i < pixels16.Count; i++)
                    {
                        temp = System.BitConverter.GetBytes(pixels16[i]);
                        buffer[j++] = temp[0];
                        buffer[j++] = temp[1];

                    }

                    if (winCentre == 0 && winWidth == 0)
                    {
                        winWidth = 4095;
                        winCentre = 4095 / 2;
                    }
                    string index="";
                    foreach (string s in dd.dicomInfo)
                    {
                        if(s.Contains("Image Number"))
                        {
                            index=s;
                        }

                    }
                   
                    
                    
                    userControl11.SetParameters(ref pixels16, imageWidth, imageHeight, winWidth, winCentre, true, this);
                   
                    
                        if (processI&&int.Parse(index.Split(':')[1]) > 9)
                        {
                            AForge.Imaging.Filters.Grayscale g1 = new Grayscale(0.2125, 0.7154, 0.0721);
                            AForge.Imaging.Filters.BrightnessCorrection bC = new AForge.Imaging.Filters.BrightnessCorrection(brightness);
                            bC.ApplyInPlace(userControl11.bmp);
                            Bitmap image = g1.Apply(userControl11.bmp);
                            thresholding = (int)((dd.windowWidth - dd.windowCentre) * 255 / dd.windowWidth) - trackBar2.Value;
                            label1.Text = thresholding.ToString();
                            AForge.Imaging.Filters.Threshold thf = new AForge.Imaging.Filters.Threshold(thresholding);
                            Bitmap ther = thf.Apply(image);
                            BlobCounter blobCounter = new BlobCounter(ther);
                            Blob[] blobs = blobCounter.GetObjects(ther, false);
                            ImageStatistics img;
                            AForge.Imaging.Filters.GrayscaleToRGB d1 = new GrayscaleToRGB();
                            Bitmap bm = d1.Apply(image);
                            Edges s = new Edges();
                            Graphics gg = Graphics.FromImage(bm);
                            string ss = null;
                            Bitmap myImage = null;
                            Blob b;
                            int count = 0;
                            listView1.Items.Clear();
                            mylesions.Clear();
                            Crop cut;
                            Bitmap Ilesion = null;

                            //System.Threading.Tasks.Parallel.ForEach(blobs, blob =>
                            foreach (Blob blob in blobs)
                            {

                                img = new ImageStatistics(blob.Image);
                                double perc = ((double)img.PixelsCountWithoutBlack / (double)img.PixelsCount) * 100;
                                textBox2.Text = perc.ToString();
                                if (blob.Image.Size.Height > 20 && blob.Image.Size.Width > 20 && perc > 35)
                                {



                                    b = blob;
                                    cut = new Crop(b.Rectangle);
                                    Ilesion = g1.Apply(cut.Apply(userControl11.bmp));

                                    ImageStatistics st = new ImageStatistics(b.Image);

                                    Bitmap pp = s.Apply(b.Image);
                                    ChannelFiltering c = new ChannelFiltering(new IntRange(0, 255), new IntRange(0, 0), new IntRange(0, 0));

                                    Bitmap pp2 = d1.Apply(pp);
                                    c.ApplyInPlace(pp2);

                                    pp2.MakeTransparent(Color.Black);



                                    gg.DrawImage(pp2, b.Rectangle);
                                    gg.Flush();

                                    myImage = userControl11.bmp.Clone(b.Rectangle, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                                    ss = ((double)(st.PixelsCountWithoutBlack) * (double)dd.pixelHeight * dd.pixelWidth).ToString();
                                    ListViewItem lv = new ListViewItem(count.ToString());
                                    lv.SubItems.Add(ss);
                                    lv.SubItems.Add(b.Rectangle.Location.X.ToString() + "," + b.Rectangle.Location.Y.ToString());
                                    listView1.Items.Add(lv);

                                    Add adder = new Add(pp);
                                    Bitmap undashes = (Bitmap)Ilesion.Clone();
                                    adder.ApplyInPlace(Ilesion);
                                    string locc = (b.Rectangle.Location.X * dd.pixelWidth).ToString() + "mm," + (b.Rectangle.Location.Y * dd.pixelHeight).ToString() + "mm";
                                    mylesions.Add(new lesion((Bitmap)Ilesion.Clone(), ss, b.Rectangle.Location, locc, undashes));

                                    count++;
                                }
                            }
                            textBox1.Text = "tumor size= " + ss + " mm² *" + dd.pixelDepth.ToString() + "mm";

                            // host.NewDocument(bmp);

                            // pictureBox2.Image = myImage;
                            pictureBox1.Image = bm;
                            pictureBox2.Image = Ilesion;
                        }else pictureBox1.Image = userControl11.bmp;
                            pictureBox1.Invalidate();
                    }
                    
                    //userControl11.increasecontrast(200);
                        
               
            }
            else
            {
                if (dd.dicmFound == false)
                {
                    MessageBox.Show("This does not seem to be a DICOM 3.0 file. Sorry, I can't open this.");
                }
                else if (dd.dicomDir == true)
                {
                    MessageBox.Show("This seems to be a DICOMDIR file, and does not contain an image.");
                }
                else
                {
                    MessageBox.Show("Sorry, I can't read a DICOM file with this Transfer Syntax\n" +
                        "You may view the initial tags instead.");
                }

               
               
                //userControl11.SetParameters(ref pixels8, imageWidth, imageHeight,
                //    winWidth, winCentre, true, this);
               

            }

        }
        private Bitmap thumnail(string fileName)
        {

            dd.DicomFileName = fileName;
            bool result = dd.dicomFileReadSuccess;
            if (result == true)
            {
                imageWidth = dd.width;
                imageHeight = dd.height;
                bitDepth = dd.bitsAllocated;
                winCentre = dd.windowCentre;
                winWidth = dd.windowWidth;

                if (bitDepth == 8)
                {
                    //    pixels8.Clear();
                    //    pixels16.Clear();
                    //    dd.GetPixels8(ref pixels8);

                    //    if (winCentre == 0 && winWidth == 0)
                    //    {
                    //        winWidth = 256;
                    //        winCentre = 128;
                    //    }

                    //    //userControl11.SetParameters(ref pixels8, imageWidth, imageHeight,
                    //        winWidth, winCentre, true, this);
                }

                if (bitDepth == 16)
                {
                    pixels16.Clear();
                    pixels8.Clear();
                    dd.GetPixels16(ref pixels16);
                    byte[] buffer = new byte[pixels16.Count * 2];
                    byte[] temp;
                    ByteConverter d = new ByteConverter();
                    int j = 0;
                    for (int i = 0; i < pixels16.Count; i++)
                    {
                        temp = System.BitConverter.GetBytes(pixels16[i]);
                        buffer[j++] = temp[0];
                        buffer[j++] = temp[1];

                    }

                    if (winCentre == 0 && winWidth == 0)
                    {
                        winWidth = 4095;
                        winCentre = 4095 / 2;
                    }

                    userControl11.SetParameters(ref pixels16, imageWidth, imageHeight, winWidth, winCentre, true, this);

                }
            }
            else
            {
                if (dd.dicmFound == false)
                {
                    MessageBox.Show("This does not seem to be a DICOM 3.0 file. Sorry, I can't open this.");
                }
                else if (dd.dicomDir == true)
                {
                    MessageBox.Show("This seems to be a DICOMDIR file, and does not contain an image.");
                }
                else
                {
                    MessageBox.Show("Sorry, I can't read a DICOM file with this Transfer Syntax\n" +
                        "You may view the initial tags instead.");
                }

                // Show a plain grayscale image
                pixels8.Clear();
                pixels16.Clear();

                imageWidth = userControl11.Width - 25;   // 25 is a magic number
                imageHeight = userControl11.Height - 25; // Same magic number
                int iNoPix = imageWidth * imageHeight;

                for (int i = 0; i < iNoPix; ++i)
                {
                    pixels8.Add(224);// 224 is the grayvalue corresponding to the Control colour
                }
                winWidth = 256;
                winCentre = 128;
                //userControl11.SetParameters(ref pixels8, imageWidth, imageHeight,
                //    winWidth, winCentre, true, this);
                userControl11.Invalidate();


            }
            Bitmap reterned = (Bitmap)userControl11.bmp.Clone();
            return reterned;
        }
        public void UpdateWindowLevel(int winWidth, int winCentre, ImageBitsPerPixel bpp)
        {
            int winMin = Convert.ToInt32(winCentre - 0.5 * winWidth);
            int winMax = winMin + winWidth;
            //this.userControl11.SetWindowWidthCentre(winMin, winMax, winWidth, winCentre, bpp);
        }
        private void fileInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region     Add items to the List View
            // Add items to the List View Control 
            myInfo.listView1.Items.Clear();
            string s1, s2, s3, s4, s5, s11, s12;
            int ind;
            for (int i = 0; i < dd.dicomInfo.Count; ++i)
            {
                if (dd.dicomInfo[i].Contains("Private Tag")) continue;
                s1 = dd.dicomInfo[i];
                ind = s1.IndexOf("//");
                s2 = s1.Substring(0, ind);

                s11 = s1.Substring(0, 4);
                s12 = s1.Substring(4, 4);

                s3 = s1.Substring(ind + 2);
                ind = s3.IndexOf(":");
                s4 = s3.Substring(0, ind);
                s5 = s3.Substring(ind + 1);

                ListViewItem lvi = new ListViewItem(s4);
                lvi.SubItems.Add(s5);
                myInfo.listView1.Items.Add(lvi);
            }
            #endregion
            myInfo.ShowDialog();
        }
        private void openSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            panel1.Controls.Clear();
            if (of.ShowDialog() == DialogResult.OK)
            {

                vScrollBar1.Maximum = of.FileNames.Length - 1;
                files = of.FileNames;
                vScrollBar1.Enabled = true;
            }
            System.Threading.Thread t = new Thread(delegate()
             {
                 for (int i = 0; i < files.Length; i++)
                 {
                     myPic p = new myPic(i);
                     p.Image = thumnail(files[i]);
                     p.Click += new System.EventHandler(this.pic_clicked);
                     p.BorderStyle = BorderStyle.FixedSingle;
                     p.SetBounds(10, 120 * i + 10, 100, 100);
                     p.SizeMode = PictureBoxSizeMode.StretchImage;
                     this.BeginInvoke(new Action(() =>
                         panel1.Controls.Add(p)
                     ));

                     //panel1.Invalidate(); 
                 }
             });
            t.Start();
            
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {

        }
        private void pic_clicked(object sender, EventArgs e)
        {
            trackBar2.Value = 0;
            label3.Text = "0";
            myPic mys = (myPic)sender;
            string[] temp = files[mys.Index].Split('\\');
            ReadAndDisplayDicomFile(files[mys.Index], temp[temp.Length - 1]);
            vScrollBar1.Value = mys.Index;

        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            trackBar2.Value = 0;
            label3.Text = "0";
            string[] temp = files[vScrollBar1.Value].Split('\\');
            ReadAndDisplayDicomFile(files[vScrollBar1.Value], temp[temp.Length - 1]);


        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            brightness = (double)trackBar1.Value / 10;
            string[] temp = files[vScrollBar1.Value].Split('\\');
            ReadAndDisplayDicomFile(files[vScrollBar1.Value], temp[temp.Length - 1]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            //thresholding = trackBar2.Value;
            string[] temp = files[vScrollBar1.Value].Split('\\');
            ReadAndDisplayDicomFile(files[vScrollBar1.Value], temp[temp.Length - 1]);
            label3.Text = trackBar2.Value.ToString();
        }
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            vScrollBar1.Focus();
        }
        private void thresholdingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            processI = true;
            string[] temp = files[vScrollBar1.Value].Split('\\');
            ReadAndDisplayDicomFile(files[vScrollBar1.Value], temp[temp.Length - 1]);
            
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            processI = false;
            string[] temp = files[vScrollBar1.Value].Split('\\');
            ReadAndDisplayDicomFile(files[vScrollBar1.Value], temp[temp.Length - 1]);
        }
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                
                TumorStat t = new TumorStat(mylesions.ElementAt<lesion>(int.Parse(listView1.SelectedItems[0].Text)).TumorUndashed);
                t.tumorImg.Image = mylesions.ElementAt<lesion>(int.Parse(listView1.SelectedItems[0].Text)).Tumor;
                t.TumorSLable.Text += mylesions.ElementAt<lesion>(int.Parse(listView1.SelectedItems[0].Text)).Tumorsize + "mm²";
                t.location.Text +="x="+ mylesions.ElementAt<lesion>(int.Parse(listView1.SelectedItems[0].Text)).TumorPosition.X.ToString() + ",y=" + mylesions.ElementAt<lesion>(int.Parse(listView1.SelectedItems[0].Text)).TumorPosition.Y.ToString();
                t.pixel.Text += mylesions.ElementAt<lesion>(int.Parse(listView1.SelectedItems[0].Text)).TumorLocation;
                t.ShowDialog();
            }
           
        }
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            
        }
    }
}
