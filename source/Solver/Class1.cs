using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace Solver
{
    class Class1 : Form
    {
        private System.ComponentModel.IContainer components = null;
        System.Windows.Forms.DataVisualization.Charting.Chart chart1;

        public Class1()
        {
            InitializeComponent();
        }

        private double f(int i)
        {
            var f1 = 59894 - (8128 * i) + (262 * i * i) - (1.6 * i * i * i);
            return f1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            this.chart1.Series.Add(series1);

            for (int i = 0; i < 100; i++)
            {
                series1.Points.AddXY(i, f(i));
            }
            chart1.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            //
            // chart1
            //
            prepare3dChart(chart1, chartArea1);
           chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 50);
            this.chart1.Name = "chart1";
            // this.chart1.Size = new System.Drawing.Size(284, 212);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.Text = "FakeChart";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
        }


        public void printChart()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form()); // or whatever

        }

        void prepare3dChart(Chart chart, ChartArea ca)
        {
            ca.Area3DStyle.Enable3D = true;  // set the chartarea to 3D!
            ca.AxisX.Minimum = -250;
            ca.AxisY.Minimum = -250;
            ca.AxisX.Maximum = 250;
            ca.AxisY.Maximum = 250;
            ca.AxisX.Interval = 50;
            ca.AxisY.Interval = 50;
            ca.AxisX.Title = "X-Achse";
            ca.AxisY.Title = "Y-Achse";
            ca.AxisX.MajorGrid.Interval = 250;
            ca.AxisY.MajorGrid.Interval = 250;
            ca.AxisX.MinorGrid.Enabled = true;
            ca.AxisY.MinorGrid.Enabled = true;
            ca.AxisX.MinorGrid.Interval = 50;
            ca.AxisY.MinorGrid.Interval = 50;
            ca.AxisX.MinorGrid.LineColor = Color.LightSlateGray;
            ca.AxisY.MinorGrid.LineColor = Color.LightSlateGray;

            // we add two series:
            chart.Series.Clear();
            for (int i = 0; i < 2; i++)
            {
                Series s = chart.Series.Add("S" + i.ToString("00"));
                s.ChartType = SeriesChartType.Bubble;   // this ChartType has a YValue array
                s.MarkerStyle = MarkerStyle.Circle;
                s["PixelPointWidth"] = "100";
                s["PixelPointGapDepth"] = "1";
            }
            chart.ApplyPaletteColors();

            addTestData(chart);
        }

        void addTestData(Chart chart)
        {
            Random rnd = new Random(9);
            for (int i = 0; i < 100; i++)
            {
                double x = Math.Cos(i / 10f) * 88 + rnd.Next(5);
                double y = Math.Sin(i / 11f) * 88 + rnd.Next(5);
                double z = Math.Sqrt(i * 2f) * 88 + rnd.Next(5);

                AddXY3d(chart.Series[0], x, y, z);
                AddXY3d(chart.Series[1], x - 111, y - 222, z);
            }
        }

        int AddXY3d(Series s, double xVal, double yVal, double zVal)
        {
            int p = s.Points.AddXY(xVal, yVal, zVal);
            // the DataPoint are transparent to the regular chart drawing:
            s.Points[p].Color = Color.Transparent;
            return p;
        }

        private void chart1_PostPaint(object sender, ChartPaintEventArgs e)
        {
            Chart chart = sender as Chart;

            if (chart.Series.Count < 1) return;
            if (chart.Series[0].Points.Count < 1) return;

            ChartArea ca = chart.ChartAreas[0];
            //e.ChartGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            List<List<PointF>> data = new List<List<PointF>>();
            foreach (Series s in chart.Series)
                data.Add(GetPointsFrom3D(ca, s, s.Points.ToList(), e.ChartGraphics));

            renderLines(data, e.ChartGraphics.Graphics, chart, true);  // pick one!
            renderPoints(data, e.ChartGraphics.Graphics, chart, 6);   // pick one!
        }

        List<PointF> GetPointsFrom3D(ChartArea ca, Series s,
                             List<DataPoint> dPoints, ChartGraphics cg)
        {
            var p3t = dPoints.Select(x => new Point3D((float)ca.AxisX.ValueToPosition(x.XValue),
                (float)ca.AxisY.ValueToPosition(x.YValues[0]),
                (float)ca.AxisY.ValueToPosition(x.YValues[1]))).ToArray();
            ca.TransformPoints(p3t.ToArray());

            return p3t.Select(x => cg.GetAbsolutePoint(new PointF(x.X, x.Y))).ToList();
        }

        void renderLines(List<List<PointF>> data, Graphics graphics, Chart chart, bool curves)
        {
            for (int i = 0; i < chart.Series.Count; i++)
            {
                if (data[i].Count > 1)
                    using (Pen pen = new Pen(Color.FromArgb(64, chart.Series[i].Color), 2.5f))
                        if (curves) graphics.DrawCurve(pen, data[i].ToArray());
                        else graphics.DrawLines(pen, data[i].ToArray());
            }
        }

        void renderPoints(List<List<PointF>> data, Graphics graphics, Chart chart, float width)
        {
            for (int s = 0; s < chart.Series.Count; s++)
            {
                Series S = chart.Series[s];
                for (int p = 0; p < S.Points.Count; p++)
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(64, S.Color)))
                        graphics.FillEllipse(brush, data[s][p].X - width / 2,
                                             data[s][p].Y - width / 2, width, width);
            }
        }
    }
}
