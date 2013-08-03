using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using MRIlib;
using System.ComponentModel;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;
public partial class _Default : System.Web.UI.Page
{
    //
    List<ushort> pixels16;//
    int imageWidth;      //
    int imageHeight;    //
    int bitDepth;      //

    double winCentre;
    double winWidth;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["logout"] == "y")
        {
            Session["logged"] = null;
        }
        
        
        
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string filename = "C:\\Users\\darwesh\\Documents\\Visual Studio 2010\\WebSites\\WebSite1\\Images\\" + FileUpload1.FileName;
        string filedir = "C:\\Users\\darwesh\\Documents\\Visual Studio 2010\\WebSites\\WebSite1\\" ;

        FileUpload1.SaveAs("C:\\Users\\darwesh\\Documents\\Visual Studio 2010\\WebSites\\WebSite1\\Images\\" + FileUpload1.FileName);
        pixels16 = new List<ushort>();
        Imagemri im=new Imagemri();
        DicomDecoder dd = new DicomDecoder();
        dd.DicomFileName = filename;
        imageWidth = dd.width;
        imageHeight = dd.height;
        bitDepth = dd.bitsAllocated;
        winCentre = dd.windowCentre;
        winWidth = dd.windowWidth;
        
           
            bool result = dd.dicomFileReadSuccess;
            if (result == true)
            {


                im.NewImage = true;



                if (bitDepth == 16)
                {
                    pixels16.Clear();

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
                }

                im.SetParameters(ref pixels16, imageWidth, imageHeight, winWidth, winCentre, true);
                string index = "";
                foreach (string stt in dd.dicomInfo)
                {
                    if (stt.Contains("Patient's Weight"))
                    {
                        index = stt;
                    }

                }
                string wii = index.Split(':')[1];
                foreach (string stt in dd.dicomInfo)
                {
                    if (stt.Contains("Patient's Name"))
                    {
                        index = stt;
                    }

                }
                string pn = index.Split(':')[1]; ;
                AForge.Imaging.Filters.Grayscale g1 = new Grayscale(0.2125, 0.7154, 0.0721);
                
                Bitmap imagew = g1.Apply(im.bmp);
                int thresholding = (int)((dd.windowWidth - dd.windowCentre) * 255 / dd.windowWidth);
                AForge.Imaging.Filters.Threshold thf = new AForge.Imaging.Filters.Threshold(thresholding);
                Bitmap ther = thf.Apply(imagew);
                BlobCounter blobCounter = new BlobCounter(ther);
                Blob[] blobs = blobCounter.GetObjects(ther, false);
                ImageStatistics img;
                AForge.Imaging.Filters.GrayscaleToRGB d1 = new GrayscaleToRGB();
                Bitmap bm = d1.Apply(imagew);
                Edges s = new Edges();
                Graphics gg = Graphics.FromImage(bm);
                string ss = null;
                Bitmap myImage = null;
                Blob b;
                int count = 0;
                string locc="";
               

                

                foreach (Blob blob in blobs)
                {

                    img = new ImageStatistics(blob.Image);
                    double perc = ((double)img.PixelsCountWithoutBlack / (double)img.PixelsCount) * 100;

                    if (blob.Image.Size.Height > 20 && blob.Image.Size.Width > 20 && perc > 35)
                    {



                        b = blob;

                        ImageStatistics st = new ImageStatistics(b.Image);
                        Bitmap pp = s.Apply(b.Image);
                        ChannelFiltering c = new ChannelFiltering(new IntRange(0, 255), new IntRange(0, 0), new IntRange(0, 0));
                        Bitmap pp2 = d1.Apply(pp);
                        c.ApplyInPlace(pp2);
                        pp2.MakeTransparent(Color.Black);
                        gg.DrawImage(pp2, b.Rectangle);
                        gg.Flush();
                        myImage = im.bmp.Clone(b.Rectangle, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        ss = ((double)(st.PixelsCountWithoutBlack) * (double)dd.pixelHeight * dd.pixelWidth).ToString();
                        locc = (b.Rectangle.Location.X * dd.pixelWidth).ToString() + "mm," + (b.Rectangle.Location.Y * dd.pixelHeight).ToString() + "mm";

                        count++;
                    }
                }//end foreach
                    

                    bm.Save(filedir + FileUpload1.FileName + ".png", ImageFormat.Png);
                    records r = new records();
                    recordsTableAdapters.recordsTableAdapter ta = new recordsTableAdapters.recordsTableAdapter();
                    ta.InsertRecord(pn, wii, FileUpload1.FileName, FileUpload1.FileName + ".png", "", ss, locc);
            }

        }
    }
}