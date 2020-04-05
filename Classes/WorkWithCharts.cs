using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace FuzzyLogic.Classes
{
    public class WorkWithCharts
    {
        private static readonly Dictionary<Chart, ExpertClass> _chartsDictionary = new Dictionary<Chart, ExpertClass>();
        public static void DrawChart(Chart chart, ExpertClass expertClass)
        {
            if (!_chartsDictionary.ContainsKey(chart))
            {
                DrawPrivate(chart, expertClass);
            }
            else
            {
                if (_chartsDictionary[chart] != expertClass)
                {
                    DrawPrivate(chart, expertClass);
                }
            }
        }

        private static void DrawPrivate(Chart chart, ExpertClass expertClass)
        {
            _chartsDictionary[chart] = expertClass;
            chart.Series.Clear();
            foreach (var termName in expertClass.TermNames)
            {
                chart.Series.Add(termName);
                chart.Series[termName].ChartType = SeriesChartType.Line;
                chart.Series[termName].MarkerStyle = MarkerStyle.Circle;
                chart.Series[termName].BorderWidth = 5;
                chart.Series[termName].MarkerSize = 12;
                var I = 0;
                foreach (var termValue in expertClass.TermValues)
                {
                    chart.Series[termName].Points.AddXY(termValue, expertClass.ValuesDictionary[(termName, termValue)]);
                }
            }
        }
    }
}
