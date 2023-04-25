/*
Файл: Presenter.cs
Лабораторная робота № 5
Автор: Положий А. С.
Тема: Розробка та дослідження програми побудови графіків
Дата створення: 06.04.2023
*/

using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diagrams
{
    internal class Presenter
    {
        private readonly IView form;
        public Presenter(IView form)
        {
            this.form = form;
            this.form.OpenAboutFormAttempted += OpenAboutForm;
            this.form.GenerateAttempted += GenerateArray;
            this.form.DiagramPaint += PrintDiagram;
            this.form.SortAttempted += SortArray;
        }

        private void GenerateArray(object sender, EventArgs e)
        {
            if (this.form.Min > this.form.Max)
                throw new ArgumentException("Min value must be less than Max value.");

            this.form.Array = new int[this.form.N];
            Random rand = new Random();
            for (int i = 0; i < this.form.N; i++)
            {
                this.form.Array[i] = rand.Next(this.form.Min, this.form.Max);
            }

            Output(this.form.Array);
        }

        private void Output(int[] arr)
        {
            this.form.DataGrid.Rows.Clear();
            for (int i = 0; i < this.form.Array?.Length; i++)
            {
                this.form.DataGrid.Rows.Add(new string[] { this.form.Array[i].ToString() });
            }
        }

        private void OpenAboutForm(object sender, EventArgs e)
        {
            this.form.AboutForm = new AboutForm();
            this.form.AboutForm.ShowDialog();
        }

        private void SortArray(object sender, EventArgs e)
        {
            Array.Sort(this.form.Array);
            Output(this.form.Array);
        }

        private void PrintDiagram(object sender, PaintEventArgs e)
        {
            if (this.form.Array == null)
                throw new NullReferenceException("Array does not exist.");
            else if(this.form.Array.Length == 0)
                throw new ArgumentException("Length of an array must be greater than 0.");

            Graphics gr = e.Graphics;
            Brush[] brushes;
            int[] xAxis;
            switch (this.form.Tabs.SelectedTab.Name)
            {
                case "HorPrimTab":
                    int space = 3;
                    int height = (this.form.Tabs.SelectedTab.Height - space) / this.form.Array.Length;
                    double k = (this.form.Tabs.SelectedTab.Width - 25) / this.form.Array.Max();
                    brushes = typeof(Brushes).GetProperties(BindingFlags.Static | BindingFlags.Public)
                       .Select(p => p.GetValue(null))
                       .OfType<System.Drawing.Brush>()
                       .ToArray();
                    brushes[19] = brushes[120];
                    brushes[15] = brushes[122];

                    for (int i = 0; i < this.form.Array.Length; i++)
                    {
                        gr.FillRectangle(brushes[i + 12], 2, space * (i + 1) + i * (height - space), (int)(k * this.form.Array[i]), height - space);
                        gr.DrawString($"{this.form.Array[i]}", new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, (int)(k * this.form.Array[i]) + 5, (height / 2) - 5 + i * height);
                    }
                    break;
                case "RoundPrimTab":
                    brushes = typeof(Brushes).GetProperties(BindingFlags.Static | BindingFlags.Public)
                        .Select(p => p.GetValue(null))
                        .OfType<System.Drawing.Brush>()
                        .ToArray();
                    brushes[19] = brushes[120];
                    brushes[15] = brushes[122];

                    int sum = this.form.Array.Sum();
                    int start = 0, sweep;
                    for (int i = 0; i < this.form.Array.Length; i++)
                    {
                        if (i == this.form.Array.Length - 1)
                            sweep = 360 - start;
                        else
                            sweep = (int)(Math.Round((this.form.Array[i] / (float)sum) * 360));

                        gr.FillPie(brushes[i + 12], this.form.Tabs.SelectedTab.Width/2 - (this.form.Tabs.SelectedTab.Height-60)/2, 30, this.form.Tabs.SelectedTab.Height - 60, this.form.Tabs.SelectedTab.Height - 60, start, sweep);

                        int radius = (this.form.Tabs.SelectedTab.Height - 30) / 2;
                        int angle = (start + sweep / 2);
                        int x = (int)(radius * Math.Cos(DegreesToRadians(angle))) + (this.form.Tabs.SelectedTab.Width-20) / 2;
                        int y = (int)(radius * Math.Sin(DegreesToRadians(angle))) + (this.form.Tabs.SelectedTab.Height-15) / 2;
                        gr.DrawString($"{this.form.Array[i]}", new Font("Montserrat", 8, FontStyle.Bold), Brushes.Black, x, y);
                        start += sweep;
                    }
                    break;
                case "HorChartTab":
                    this.form.HorizontalChart.Series[0].ChartType = SeriesChartType.RangeBar;
                    this.form.HorizontalChart.Series[0].Name = "Array";

                    xAxis = Enumerable.Range(1, this.form.Array.Length).ToList().ToArray();
                    this.form.HorizontalChart.Series[0].Points.DataBindXY(xAxis, this.form.Array);
                    break;
                case "RoundChartTab":
                    this.form.RoundChart.Series[0].ChartType = SeriesChartType.Pie;
                    this.form.RoundChart.Series[0].IsValueShownAsLabel = true;

                    xAxis = Enumerable.Range(1, this.form.Array.Length).ToList().ToArray();
                    this.form.RoundChart.Series[0].Points.DataBindXY(xAxis, this.form.Array);
                    break;
                case "TwoDiagramsTab":                 
                    this.form.FirstChart.Series[0].ChartType = SeriesChartType.Spline;
                    this.form.FirstChart.Series[0].Name = "Array";
                    this.form.FirstChart.Series[0].Color = Color.Red;

                    this.form.SecondChart.Series[0].ChartType = SeriesChartType.Column;
                    this.form.SecondChart.Series[0].Name = "Array";
                    this.form.SecondChart.Series[0].Color = Color.CadetBlue;

                    xAxis = Enumerable.Range(1, this.form.Array.Length).ToList().ToArray();
                    this.form.FirstChart.Series[0].Points.DataBindXY(xAxis, this.form.Array);
                    this.form.SecondChart.Series[0].Points.DataBindXY(xAxis, this.form.Array);
                    break;
                case "OneChartTwoDiagramsTab":                 
                    this.form.TwoDiagramsChart.Series[0].Name = "Array";

                    xAxis = Enumerable.Range(1, this.form.Array.Length).ToList().ToArray();
                    this.form.TwoDiagramsChart.Series[0].ChartType = SeriesChartType.Bubble;
                    this.form.TwoDiagramsChart.Series[0].Points.DataBindXY(xAxis, this.form.Array);

                    int[] randArr = new int[this.form.Array.Length];
                    Random rand = new Random();
                    for (int i = 0; i < this.form.Array.Length; i++)
                    {
                        randArr[i] = rand.Next(this.form.Min, this.form.Max);
                    }

                    if(this.form.TwoDiagramsChart.ChartAreas.Count < 2)
                    {
                        ChartArea myChartArea = new ChartArea("ChartArea2");
                        this.form.TwoDiagramsChart.ChartAreas.Add(myChartArea);
                    }

                    this.form.TwoDiagramsChart.Series[1].ChartArea = "ChartArea2";
                    this.form.TwoDiagramsChart.Series[1].Name = "Random Array";
                    this.form.TwoDiagramsChart.Series[1].ChartType = SeriesChartType.Column;
                    this.form.TwoDiagramsChart.Series[1].Points.DataBindXY(xAxis, randArr);

                    break;
                case "OneChartSeveralDiagramsTab":
                    xAxis = Enumerable.Range(1, this.form.Array.Length).ToList().ToArray();
                    this.form.SeveralDiagramsChart.Series[0].Points.DataBindXY(xAxis, this.form.Array);

                    int[] randA = new int[this.form.Array.Length];
                    Random random = new Random();
                    for (int i = 0; i < this.form.Array.Length; i++)
                    {
                        randA[i] = random.Next(this.form.Min, this.form.Max);
                    }

                    if (this.form.SeveralDiagramsChart.ChartAreas.Count < 2)
                    {
                        ChartArea myChartArea = new ChartArea("ChartArea2");
                        this.form.SeveralDiagramsChart.ChartAreas.Add(myChartArea);
                    }

                    this.form.SeveralDiagramsChart.Series[0].Name = "Array";

                    this.form.SeveralDiagramsChart.Series[1].ChartType = SeriesChartType.Column;
                    this.form.SeveralDiagramsChart.Series[1].BackGradientStyle = GradientStyle.LeftRight;
                    this.form.SeveralDiagramsChart.Series[1].Name = "Random Array";

                    this.form.SeveralDiagramsChart.Series[1].Points.DataBindXY(xAxis, randA);
                    this.form.SeveralDiagramsChart.Series[0].Color = Color.Green;
                    this.form.SeveralDiagramsChart.Series[1].Color = Color.Red;

                    this.form.SeveralDiagramsChart.Series[2].ChartArea = "ChartArea2";
                    this.form.SeveralDiagramsChart.Series[2].Name = "Arr";
                    this.form.SeveralDiagramsChart.Series[2].ChartType = SeriesChartType.Spline;
                    this.form.SeveralDiagramsChart.Series[2].Points.DataBindXY(xAxis, this.form.Array);

                    this.form.SeveralDiagramsChart.Series[3].ChartArea = "ChartArea2";
                    this.form.SeveralDiagramsChart.Series[3].Name = "Random Arr";
                    this.form.SeveralDiagramsChart.Series[3].ChartType = SeriesChartType.Spline;
                    this.form.SeveralDiagramsChart.Series[3].Points.DataBindXY(xAxis, randA);
                    break;
            }
        }

        double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180f;
        }
    }
}
