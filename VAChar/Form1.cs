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
            Color[] colors = { Color.Blue, Color.Crimson, Color.Green, Color.Fuchsia, Color.Orange};

            this.Text = "Voltampérová charakteristika bipolárního tranzistoru";
            this.Size = new Size(1380, 780);

            // Vytvoření grafu a přidání dat
            Chart chart = new Chart();
            chart.Size = new Size(1280, 720);

            for (int i = 0; i < collectorCurrent.Length; i++)
            {
                chart.Series.Add("Výstupní charakteristika" + i);
                chart.Series["Výstupní charakteristika" + i].ChartType = SeriesChartType.Line;
                chart.Series["Výstupní charakteristika" + i].Color = colors[i];
            }

            chart.ChartAreas.Add(new ChartArea("Výstupní charakteristika"));
            
            // Nastavení rozsahu osy X na -10 až 10
            chart.ChartAreas["Výstupní charakteristika"].AxisX.Minimum = -10;
            chart.ChartAreas["Výstupní charakteristika"].AxisX.Maximum = 10;

            // Nastavení rozsahu osy Y na -10 až 10
            chart.ChartAreas["Výstupní charakteristika"].AxisY.Minimum = -10;
            chart.ChartAreas["Výstupní charakteristika"].AxisY.Maximum = 10;
            
            int currentId = 0;
            foreach (double[] current in collectorCurrent)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    var spline = CubicSpline.InterpolateNatural(collectorVoltage, current);
                    
                    double x = spline.Interpolate(collectorVoltage[i]);
                    
                    double y = spline.Interpolate(current[i]);
                    
                    //chart.Series["Výstupní charakteristika" + currentId].Points.AddXY(x, y);
                    chart.Series["Výstupní charakteristika" + currentId].Points.AddXY(collectorVoltage[i], current[i]);
                }

                currentId++;
            }
            
            
            // Přidání grafu do formuláře a zobrazení
            this.Controls.Add(chart);
        }
    }
}