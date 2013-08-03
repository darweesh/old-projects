using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Image.MD
{
    class myPic : PictureBox
    {
        int index;
        public myPic(int i)
        {
            index = i;

        }

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }
    }
}
