using System;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using MathNet.Numerics.Interpolation;

namespace VAChar
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            // Vstupní hodnoty pro charakteristiku tranzistoru
            double[][] collectorCurrent =
            {
                new double[] { 0.0, 1.4, 2.22, 2.24, 2.27, 2.29, 2.31, 2.33, 2.37 },
                new double[] { 0.0, 1.71, 3.16, 5.01, 5.08, 5.14, 5.24, 5.24, 5.38 },
                new double[] { 0.0, 1.84, 3.35, 6.5, 7.96, 8.09, 8.25, 8.35, 8.53 },
            };
            double[] collectorVoltage = { 0.0, 0.3, 0.5, 1.0, 2.0, 4.0, 6.0, 8.0, 10.0 };

            double[] inputCurrent = { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            double[][] inputVoltage =
            {
                new double[] { 0.0, 0.64, 0.67, 0.68, 0.69, 0.71, 0.72, 0.73, 0.74 }
            };
            
            Color[] colors = { Color.Blue, Color.Crimson, Color.Green, Color.Fuchsia, Color.Orange, Color.Purple};

            this.Text = "Voltampérová charakteristika bipolárního tranzistoru";
            this.Size = new Size(1380, 780);

            // Vytvoření grafu a přidání dat
            Chart chart = new Chart();
            chart.Size = new Size(1280, 720);
            chart.Legends.Add("Legendy");

            for (int i = 0; i < collectorCurrent.Length; i++)
            {
                chart.Series.Add("Výstupní charakteristika " + i);
                chart.Series["Výstupní charakteristika " + i].ChartType = SeriesChartType.Spline;
                chart.Series["Výstupní charakteristika " + i].Color = colors[i];
            }
            
            for (int i = 0; i < inputVoltage.Length; i++)
            {
                chart.Series.Add("Vstupní charakteristika " + i);
                chart.Series["Vstupní charakteristika " + i].ChartType = SeriesChartType.Spline;
                chart.Series["Vstupní charakteristika " + i].Color = colors[colors.Length -1 - i];
            }
            
            chart.Series.Add("Převodní charakteristika");
            chart.Series["Převodní charakteristika"].ChartType = SeriesChartType.Line;
            chart.Series["Převodní charakteristika"].Color = Color.Black;

            chart.ChartAreas.Add(new ChartArea("Výstupní charakteristika"));

            // Nastavení rozsahu osy X na -10 až 10
            chart.ChartAreas["Výstupní charakteristika"].AxisX.Minimum = -11;
            chart.ChartAreas["Výstupní charakteristika"].AxisX.Maximum = 11;

            // Nastavení rozsahu osy Y na -10 až 10
            chart.ChartAreas["Výstupní charakteristika"].AxisY.Minimum = -11;
            chart.ChartAreas["Výstupní charakteristika"].AxisY.Maximum = 11;
            
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.Interval = 1;  
            
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
            
            chart.ChartAreas[0].AxisX.Title = "Ib   |   Uce";
            chart.ChartAreas[0].AxisY.Title = "Ube  |   Ic";
            


            double yy = 0.0;
            int currentId = 0;
            foreach (double[] current in collectorCurrent)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    var spline = CubicSpline.InterpolateNatural(collectorVoltage, current);
                    
                    double x = spline.Interpolate(collectorVoltage[i]);
                    
                    double y = spline.Interpolate(current[i]);
                    
                    //chart.Series["Výstupní charakteristika" + currentId].Points.AddXY(x, y);
                    chart.Series["Výstupní charakteristika " + currentId].Points.AddXY(collectorVoltage[i], current[i]);
                    yy = y;
                }

                currentId++;
            }

            double xx = 0.0;
            
            int voltageId = 0;
            foreach (double[] voltage in inputVoltage)
            {
                for (int i = 0; i < voltage.Length; i++)
                {
                    //var spline = CubicSpline.InterpolateNatural(collectorVoltage, voltage);
                    
                    /*
                    double x = spline.Interpolate(collectorVoltage[i]);
                    
                    double y = spline.Interpolate(voltage[i]);
                    */

                    double x = inputCurrent[i] * -1;
                    double y = voltage[i] * -1;
                    
                    //chart.Series["Výstupní charakteristika" + currentId].Points.AddXY(x, y);
                    chart.Series["Vstupní charakteristika " + voltageId].Points.AddXY(x/10, y);

                    xx = x;
                }

                voltageId++;
            }

            chart.Series["Převodní charakteristika"].Points.AddXY(0, 0);
            
            double[] highestCurrent = collectorCurrent[collectorCurrent.Length - 1];
            chart.Series["Převodní charakteristika"].Points.AddXY(xx/10, yy);
            


            // Přidání grafu do formuláře a zobrazení
            this.Controls.Add(chart);
        }
    }
}