using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FuzzyLogic.Classes;

namespace FuzzyLogic.Forms
{
    public partial class Form1 : Form
    {
        private WorkWithDataGrid _workWithDataGrid;
        private WorkWithDataGrid _workWithDataGrid1;
        private List<string> _columnNamesList = new List<string>();
        private ExpertClass _expertClass = new ExpertClass("Expert1");
        private ExpertClass _expertClass1 = new ExpertClass("Expert2");
        private DataClass _dataClass;

        public void Init()
        {
            _expertClass.Load();
            _expertClass1.Load();
            var dict1 = new Dictionary<string, ExpertClass>
            {
                [_expertClass.Name] = _expertClass, [_expertClass1.Name] = _expertClass1
            };
            //_dataClass = new DataClass(dict1);
            _dataClass = new DataClass("Table1");
            _workWithDataGrid = new WorkWithDataGrid(dataGridView1, _dataClass);
        }

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var calc = _dataClass.Calculation();
            var tempDataClass = new DataClass("Table2", new Dictionary<string, ExpertClass>() { [calc.Name] = calc });
            _workWithDataGrid1 = new WorkWithDataGrid(
                dataGridView2,
                tempDataClass);
            chart1.Series.Clear();
            foreach (var termName in calc.TermNames)
            {
                chart1.Series.Add(termName);
                chart1.Series[termName].ChartType = SeriesChartType.Line;
                chart1.Series[termName].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[termName].BorderWidth = 5;
                chart1.Series[termName].MarkerSize = 12;
                var I = 0;
                foreach (var termValue in calc.TermValues)
                {
                    chart1.Series[termName].Points.AddXY(termValue, calc.ValuesDictionary[(termName, termValue)]);
                }
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var a = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var calc = _dataClass.Calculation();
            var tempDataClass = new DataClass("Table2", new Dictionary<string, ExpertClass>() {[calc.Name] = calc});
            _workWithDataGrid1 = new WorkWithDataGrid(
                dataGridView2, 
                tempDataClass);
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Points.AddXY(1, 2);
            chart1.Series[0].Points.AddXY(2, 2);
        }
    }
}
