using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Image.MD
{
    class lesion
    {
        #region lesions field
        Bitmap tumor;
        Bitmap tumorUndashed;

        public Bitmap TumorUndashed
        {
            get { return tumorUndashed; }
            set { tumorUndashed = value; }
        }

        public Bitmap Tumor
        {
            get { return tumor; }
            set { tumor = value; }
        }
        string tumorsize;

        public string Tumorsize
        {
            get { return tumorsize; }
            set { tumorsize = value; }
        }
        Point tumorPosition;

        public Point TumorPosition
        {
            get { return tumorPosition; }
            set { tumorPosition = value; }
        }
        string tumorLocation;

        public string TumorLocation
        {
            get { return tumorLocation; }
            set { tumorLocation = value; }
        }
        #endregion


        /// <summary>
        /// intialize the lesion
        /// </summary>
        /// <param name="timg">tumor Image</param>
        /// <param name="tsize">Tumor Size</param>
        /// <param name="tpos">Tumor Position</param>
        /// <param name="tlocation">Tumor Location</param>
        /// <param name="undashed">clean tumor image</param>
        /// 

        public lesion(Bitmap timg,string tsize,Point tpos,string tlocation,Bitmap undashed)
        {
            tumor = timg;
            tumorsize = tsize;
            tumorLocation = tlocation;
            tumorPosition = tpos;
            tumorUndashed = undashed;
        }
    }
}
