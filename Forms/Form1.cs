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
            DataClass.Form1 = this;
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

        public void Init(int expertCount, List<string> termNames, List<string> termValues)
        {
            DataClass.Form1 = this;
            var dict1 = new Dictionary<string, ExpertClass>();
            for (var i = 0; i < expertCount; i++)
            {
                var tempExpertClass = new ExpertClass($"Эксперт{i+1}");
                tempExpertClass.Load(termNames, termValues);
                dict1[tempExpertClass.Name] = tempExpertClass;
            }
            _dataClass = new DataClass("Table1", dict1);
            _workWithDataGrid = new WorkWithDataGrid(dataGridView1, _dataClass);
        }

        public Form1()
        {
            InitializeComponent();
            //Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _dataClass.Calculation();
            var tempDataClass = new DataClass("Table2", _dataClass.CalculatedExpertClass);
            _workWithDataGrid1 = new WorkWithDataGrid(dataGridView2, tempDataClass);
            WorkWithCharts.DrawChart(chart1, _dataClass.CalculatedExpertClass);
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        public void RefreshChartAndDataGrid()
        {
            var tempDataClass = new DataClass("Table2", _dataClass.CalculatedExpertClass);
            _workWithDataGrid1 = new WorkWithDataGrid(dataGridView2, tempDataClass);
            WorkWithCharts.DrawChart(chart1, _dataClass.CalculatedExpertClass);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
