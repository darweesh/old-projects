using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRIlib;
using System.ComponentModel;
using System.Drawing.Imaging;

public partial class readImage : System.Web.UI.Page
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
        pixels16 = new List<ushort>();
        Imagemri im=new Imagemri();
        DicomDecoder dd = new DicomDecoder();
        dd.DicomFileName = "C:\\Users\\darwesh\\Documents\\Visual Studio 2010\\WebSites\\WebSite1\\Images\\IM_0076";
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
                Response.ContentType = "image/png";
                im.bmp.Save("C:\\Users\\darwesh\\Documents\\Visual Studio 2010\\WebSites\\WebSite1\\Images\\m.jpeg", ImageFormat.Png);
                Response.WriteFile("C:\\Users\\darwesh\\Documents\\Visual Studio 2010\\WebSites\\WebSite1\\Images\\m.jpeg");
            }
    }
}