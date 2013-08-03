namespace Image.MD
{
    partial class TumorStat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.LineAnnotation lineAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.LineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tumorImg = new System.Windows.Forms.PictureBox();
            this.TumorSLable = new System.Windows.Forms.Label();
            this.location = new System.Windows.Forms.Label();
            this.pixel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tumorImg)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            lineAnnotation1.Name = "LineAnnotation3";
            this.chart.Annotations.Add(lineAnnotation1);
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.LightStyle = System.Windows.Forms.DataVisualization.Charting.LightStyle.Realistic;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(307, 12);
            this.chart.Name = "chart";
            this.chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(782, 292);
            this.chart.TabIndex = 0;
            this.chart.Text = "Chart";
            this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_MouseMove);
            // 
            // tumorImg
            // 
            this.tumorImg.Location = new System.Drawing.Point(12, 12);
            this.tumorImg.Name = "tumorImg";
            this.tumorImg.Size = new System.Drawing.Size(289, 292);
            this.tumorImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.tumorImg.TabIndex = 1;
            this.tumorImg.TabStop = false;
            // 
            // TumorSLable
            // 
            this.TumorSLable.AutoSize = true;
            this.TumorSLable.Font = new System.Drawing.Font("Tempus Sans ITC", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TumorSLable.Location = new System.Drawing.Point(12, 323);
            this.TumorSLable.Name = "TumorSLable";
            this.TumorSLable.Size = new System.Drawing.Size(115, 24);
            this.TumorSLable.TabIndex = 2;
            this.TumorSLable.Text = "Tumor Size =";
            // 
            // location
            // 
            this.location.AutoSize = true;
            this.location.Font = new System.Drawing.Font("Tempus Sans ITC", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.location.Location = new System.Drawing.Point(13, 358);
            this.location.Name = "location";
            this.location.Size = new System.Drawing.Size(206, 24);
            this.location.TabIndex = 3;
            this.location.Text = "Tumor location In Pexl :";
            // 
            // pixel
            // 
            this.pixel.AutoSize = true;
            this.pixel.Font = new System.Drawing.Font("Tempus Sans ITC", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pixel.Location = new System.Drawing.Point(13, 399);
            this.pixel.Name = "pixel";
            this.pixel.Size = new System.Drawing.Size(146, 24);
            this.pixel.TabIndex = 4;
            this.pixel.Text = "Tumor position :";
            this.pixel.Click += new System.EventHandler(this.label2_Click);
            // 
            // TumorStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 484);
            this.Controls.Add(this.pixel);
            this.Controls.Add(this.location);
            this.Controls.Add(this.TumorSLable);
            this.Controls.Add(this.tumorImg);
            this.Controls.Add(this.chart);
            this.Name = "TumorStat";
            this.Text = "TumorStat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TumorStat_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tumorImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart chart;
        public System.Windows.Forms.PictureBox tumorImg;
        public System.Windows.Forms.Label TumorSLable;
        public System.Windows.Forms.Label location;
        public System.Windows.Forms.Label pixel;
    }
}