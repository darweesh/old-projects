using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Imaging;
using System.Windows.Forms.DataVisualization.Charting;
namespace Image.MD
{
    public partial class TumorStat : Form
    {
        Bitmap tumorimage;
        public TumorStat(Bitmap mytumor)
        {
            InitializeComponent();
            tumorimage = mytumor;
            ImageStatistics st = new ImageStatistics(mytumor);
            for (int i = 0; i < st.Gray.Values.Length; i++)
            {
                DataPoint dt=new DataPoint(i,st.Gray.Values[i]);
                chart.Series[0].Points.Add(dt);
                
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TumorStat_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}
