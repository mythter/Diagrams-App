/*
Файл: IView.cs
Лабораторная робота № 5
Автор: Положий А. С.
Тема: Розробка та дослідження програми побудови графіків
Дата створення: 06.04.2023
*/

using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diagrams
{
    internal interface IView
    {
        // Numeric Up Down.
        int N { get; set; }
        int Min { get; set; }
        int Max { get; set; }

        // Data grid view.
        DataGridView DataGrid { get; set; }

        // Tab control.
        TabControl Tabs { get; set; }

        // Charts.
        Chart HorizontalChart { get; set; }
        Chart RoundChart { get; set; }
        Chart FirstChart { get; set; }
        Chart SecondChart { get; set; }
        Chart TwoDiagramsChart { get; set; }
        Chart SeveralDiagramsChart { get; set; }

        // Buttons.
        event EventHandler GenerateAttempted;
        event EventHandler SortAttempted;
        event EventHandler ShowDiagAttempted;
        event EventHandler OpenAboutFormAttempted;

        // Other events.
        event PaintEventHandler DiagramPaint;

        // Forms.
        AboutForm AboutForm { get; set; }

        // Data.
        int[] Array { get; set; }
    }
}
