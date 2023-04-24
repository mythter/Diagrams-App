/*
Файл: Form1.cs
Лабораторная робота № 5
Автор: Положий А. С.
Тема: Розробка та дослідження програми побудови графіків
Дата створення: 06.04.2023
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diagrams
{
    public partial class Form1 : Form, IView
    {
        Presenter _presenter;
        AboutForm _aboutForm;
        int[] _arr;
        bool isClicked = false;
        public Form1()
        {
            InitializeComponent();
            _presenter = new Presenter(this);

            ArrDataGrid.DataSource = Array;
            ArrDataGrid.Columns[0].Width = 96;

            HorizontalChart.Titles.Add("Horizontal diagram with chart control");
            HorizontalChart.Titles[0].Font = new Font("Montserrat", 12f, FontStyle.Bold);

            RoundChart.Titles.Add("Round diagram with chart control");
            RoundChart.Titles[0].Font = new Font("Montserrat", 12f, FontStyle.Bold);

            TwoDiagramsChart.Series.Add(new Series());

            SeveralDiagramsChart.Series.Add(new Series());
            SeveralDiagramsChart.Series.Add(new Series());
            SeveralDiagramsChart.Series.Add(new Series());

            FirstChart.Titles.Add(SeriesChartType.Spline.ToString());
            FirstChart.Titles[0].Font = new Font("Montserrat", 12f, FontStyle.Bold);

            SecondChart.Titles.Add(SeriesChartType.Column.ToString());
            SecondChart.Titles[0].Font = new Font("Montserrat", 12f, FontStyle.Bold);

            TwoDiagramsChart.Titles.Add("First Area");
            TwoDiagramsChart.Titles.Add("Second Area");
            TwoDiagramsChart.Titles[0].Font = new Font("Montserrat", 12f, FontStyle.Bold);
            TwoDiagramsChart.Titles[1].Font = new Font("Montserrat", 12f, FontStyle.Bold);
            TwoDiagramsChart.Titles[1].Position.X = 50;
            TwoDiagramsChart.Titles[1].Position.Y = 56;

            SeveralDiagramsChart.Titles.Add("First Area");
            SeveralDiagramsChart.Titles.Add("Second Area");
            SeveralDiagramsChart.Titles[0].Font = new Font("Montserrat", 12f, FontStyle.Bold);
            SeveralDiagramsChart.Titles[1].Font = new Font("Montserrat", 12f, FontStyle.Bold);
            SeveralDiagramsChart.Titles[1].Position.X = 50;
            SeveralDiagramsChart.Titles[1].Position.Y = 56;
        }

        public int N { get => (int)NUpDown.Value; set => NUpDown.Value = value; }
        public int Min { get => (int)MinUpDown.Value; set => MinUpDown.Value = value; }
        public int Max { get => (int)MaxUpDown.Value; set => MaxUpDown.Value = value; }
        public DataGridView DataGrid { get => ArrDataGrid; set => ArrDataGrid = value; }
        public AboutForm AboutForm { get => _aboutForm; set => _aboutForm = value; }
        public TabControl Tabs { get => TabControl; set => TabControl = value; }
        public int[] Array { get => _arr; set => _arr = value; }
        public Chart HorizontalChart { get => HorChart; set => HorChart = value; }
        public Chart RoundChart { get => RChart; set => RChart = value; }
        public Chart FirstChart { get => Chart1; set => Chart1 = value; }
        public Chart SecondChart { get => Chart2; set => Chart2 = value; }
        public Chart TwoDiagramsChart { get => Chart3; set => Chart3 = value; }
        public Chart SeveralDiagramsChart { get => Chart4; set => Chart4 = value; }

        public event EventHandler GenerateAttempted;
        public event EventHandler SortAttempted;
        public event EventHandler ShowDiagAttempted;
        public event EventHandler OpenAboutFormAttempted;
        public event PaintEventHandler DiagramPaint;

        private void AboutBtn_Click(object sender, EventArgs e)
        {
            OpenAboutFormAttempted?.Invoke(sender, e);
        }

        private void GenBtn_Click(object sender, EventArgs e)
        {
            isClicked = false;
            try
            {
                GenerateAttempted?.Invoke(sender, e);
                Graphics g = Tabs.SelectedTab.CreateGraphics();
                g.Clear(BackColor);
                HorChart.Series[0].Points.Clear();
                RChart.Series[0].Points.Clear();
                FirstChart.Series[0].Points.Clear();
                SecondChart.Series[0].Points.Clear();
                TwoDiagramsChart.Series[0].Points.Clear();
                TwoDiagramsChart.Series[1].Points.Clear();
                SeveralDiagramsChart.Series[0].Points.Clear();
                SeveralDiagramsChart.Series[1].Points.Clear();
                SeveralDiagramsChart.Series[2].Points.Clear();
                SeveralDiagramsChart.Series[3].Points.Clear();

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ShowDiagBtn_Click(object sender, EventArgs e)
        {
            isClicked = true;
            Graphics g = Tabs.SelectedTab.CreateGraphics();
            g.Clear(BackColor);
            PaintEventHandler peh = new PaintEventHandler(Tabs_Paint);
            peh?.Invoke(Tabs.SelectedTab, new PaintEventArgs(g, new Rectangle(0, 0, 562, 395)));
        }

        private void Tabs_Paint(object sender, PaintEventArgs e)
        {
            if (isClicked)
            {
                try
                {
                    DiagramPaint?.Invoke(sender, e);
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        private void SortBtn_Click(object sender, EventArgs e)
        {
            SortAttempted?.Invoke(sender, e);

            if (isClicked)
            {
                Graphics g = Tabs.SelectedTab.CreateGraphics();
                g.Clear(BackColor);
                PaintEventHandler peh = new PaintEventHandler(Tabs_Paint);
                peh?.Invoke(Tabs.SelectedTab, new PaintEventArgs(g, new Rectangle(0, 0, 562, 395)));
            }
        }

        private void SelectedIndex_Changed(object sender, EventArgs e)
        {
            if (Tabs.SelectedIndex > 3)
            {
                Graphics g = Tabs.SelectedTab.CreateGraphics();
                g.Clear(BackColor);
                PaintEventHandler peh = new PaintEventHandler(Tabs_Paint);
                peh?.Invoke(Tabs.SelectedTab, new PaintEventArgs(g, new Rectangle(0, 0, 562, 395)));
            }            
        }
    }
}
