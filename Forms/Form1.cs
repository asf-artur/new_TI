using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var a = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var calc = _dataClass.Calculation();
            _workWithDataGrid1 = new WorkWithDataGrid(
                dataGridView2, 
                new DataClass("Table2",  new Dictionary<string, ExpertClass>(){ [calc.Name] = calc }));
        }
    }
}
