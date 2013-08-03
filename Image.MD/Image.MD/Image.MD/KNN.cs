using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Imaging;
using System.Drawing;
using System.Data.Sql;

namespace Image.MD
{
    class KNN
    {
        int size;
        Point position;
        double intensity;
        ImageStatistics img;
        private int n;
        private int k;
        private int knn;
        private bool classification;
        private double dist;
        public KNN(Blob blob)
        {
            img = new ImageStatistics(blob.Image);
            intensity = ((double)img.PixelsCountWithoutBlack / (double)img.PixelsCount) * 100;
            size = blob.Area;
            position = blob.Rectangle.Location;


        }
        public bool clasifiy()
        {
            Database1DataSet ds = new Database1DataSet();
            Database1DataSetTableAdapters.datasetTableAdapter ta = new Database1DataSetTableAdapters.datasetTableAdapter();
            int []  distances=new int[knn];
            System.Data.DataRow[] samples = ds.Tables["dataset"].Select();
            Random rand = new Random();//for ties
            // for each point, classify it with kNN.
            int error = 0; // number of misclassified points
            for (int x = 0; x < n; x++)
                for (int y = 0; y < n; y++)
                {
                    // find the knn
                    for (int i = 0; i < k; i++)
                    {
                      //  double dist = (samples[i].x - x) * (samples[i].x - x) + (samples[i].y - y) * (samples[i].y - y);
                        if (i < knn)
                        {
                           // distances[i] = (int)dist;
                            distances[i] = int.Parse(samples[i].ItemArray.ToString());
                        }
                        else
                        {// go through the knn list and replace the biggest one if possible
                            double biggestd = distances[0];
                            int biggestindex = 0;
                            for (int a = 1; a < knn; a++)
                                if (distances[a] > biggestd)
                                {
                                    biggestd = distances[a];
                                    biggestindex = a;
                                }
                            if (dist < biggestd)
                            {
                                distances[biggestindex] =(int) dist;
                                distances[biggestindex] = int.Parse(samples[i].ItemArray.ToString());
                            }
                        }
                    }
                    // count which label in knn occurs most, this is the classification of (x,y)
                    int nT = 0, nF = 0;
                   
                    for (int i = 0; i < knn; i++)
                        if (distances[i] !=0)
                            nT++;
                        else
                            nF++;
                    if (nT < nF)
                        classification = false;
                    else if (nT > nF)
                        classification = true;
                    else
                        classification = rand.Next() < 0.5; // if tie, randomly break it

                    
                }


            return classification;
        }
    }
}
